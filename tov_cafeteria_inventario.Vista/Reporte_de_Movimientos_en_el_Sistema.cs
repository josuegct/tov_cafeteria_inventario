using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Reporte_de_Movimientos_en_el_Sistema : Form
    {
        private readonly Reporte_Movimientos_SistemaController controller = new Reporte_Movimientos_SistemaController();
        private readonly int usuarioID;

        // -------------------- INICIALIZACIÓN --------------------
        public Reporte_de_Movimientos_en_el_Sistema(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;
            this.FormClosed += Reporte_de_Movimientos_FormClosed;
        }

        private void Reporte_de_Movimientos_en_el_Sistema_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
            CargarMovimientos();
        }

        private void Reporte_de_Movimientos_FormClosed(object sender, FormClosedEventArgs e)
        {
            var bitacora = new Bitacora(usuarioID);
            bitacora.MdiParent = this.MdiParent;
            bitacora.StartPosition = FormStartPosition.CenterScreen;
            bitacora.Show();
        }

        // -------------------- CARGA DE DATOS --------------------
        private void CargarUsuarios()
        {
            try
            {
                var tabla = controller.ObtenerUsuarios(); // debe retornar DataTable
                DataRow filaVacia = tabla.NewRow();
                filaVacia["UsuarioID"] = 0;
                filaVacia["Nombre"] = "";
                tabla.Rows.InsertAt(filaVacia, 0); // ✅ Insertar fila vacía correctamente

                cmbUsuarios.DataSource = tabla;
                cmbUsuarios.DisplayMember = "Nombre";
                cmbUsuarios.ValueMember = "UsuarioID";
                cmbUsuarios.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarMovimientos()
        {
            try
            {
                List<MovimientoRegistro> lista = controller.ObtenerMovimientos();
                MostrarDatos(lista);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar movimientos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarDatos(List<MovimientoRegistro> lista)
        {
            DataTable table = new DataTable();
            table.Columns.Add("BitacoraID", typeof(int));
            table.Columns.Add("NombreUsuario", typeof(string));
            table.Columns.Add("FechaRegistro", typeof(DateTime));
            table.Columns.Add("Accion", typeof(string));

            foreach (var m in lista)
            {
                table.Rows.Add(m.BitacoraID, m.NombreUsuario, m.FechaRegistro, m.Accion);
            }

            dgvMovimientos.DataSource = table;
            dgvMovimientos.ReadOnly = true;
            dgvMovimientos.AllowUserToAddRows = false;
            lblTotalRegistros.Text = $"Total de movimientos: {lista.Count}";
        }

        // -------------------- EVENTOS DE BOTÓN --------------------
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            dgvMovimientos.DataSource = null;
            dgvMovimientos.Rows.Clear();
            dgvMovimientos.Columns.Clear();
            CargarMovimientos();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            int? usuarioId = null;

            if (cmbUsuarios.SelectedValue != null && cmbUsuarios.SelectedValue != DBNull.Value)
            {
                int valor = Convert.ToInt32(cmbUsuarios.SelectedValue);
                usuarioId = (valor == 0) ? null : (int?)valor;
            }

            DateTime desde = dtpDesde.Value.Date;
            DateTime hasta = dtpHasta.Value.Date.AddDays(1).AddSeconds(-1);

            var lista = controller.ObtenerMovimientos(usuarioId, desde, hasta);
            MostrarDatos(lista);
        }

        private void btnExportartxt_Click(object sender, EventArgs e)
        {
            if (dgvMovimientos.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo PDF (*.pdf)|*.pdf|Archivo de texto (*.txt)|*.txt",
                FileName = "Reporte_Movimientos"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string extension = Path.GetExtension(saveFileDialog.FileName).ToLower();

                if (extension == ".pdf")
                    ExportarAPdf(saveFileDialog.FileName);
                else if (extension == ".txt")
                    ExportarATxt(saveFileDialog.FileName);
                else
                    MessageBox.Show("Formato no soportado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // -------------------- EXPORTACIÓN TXT/PDF --------------------
        private void ExportarATxt(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("REPORTE DE MOVIMIENTOS DEL SISTEMA - Cafetería TOV");
                sw.WriteLine("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                sw.WriteLine(new string('-', 80));

                foreach (DataGridViewColumn col in dgvMovimientos.Columns)
                    sw.Write(col.HeaderText.PadRight(20));
                sw.WriteLine();
                sw.WriteLine(new string('-', 80));

                foreach (DataGridViewRow row in dgvMovimientos.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                            sw.Write((cell.Value?.ToString() ?? "").PadRight(20));
                        sw.WriteLine();
                    }
                }

                sw.WriteLine(new string('-', 80));
                sw.WriteLine("*** Fin del Reporte ***");
                sw.WriteLine("Generado automáticamente por el sistema Cafetería TOV");
            }

            MessageBox.Show("📄 TXT exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportarAPdf(string path)
        {
            Document doc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 20f);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                doc.Open();

                Font fontTitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Paragraph title = new Paragraph("REPORTE DE MOVIMIENTOS DEL SISTEMA - Cafetería TOV", fontTitle)
                {
                    Alignment = Element.ALIGN_CENTER
                };

                doc.Add(title);
                doc.Add(new Paragraph("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                doc.Add(new Chunk(new LineSeparator()));

                PdfPTable table = new PdfPTable(dgvMovimientos.Columns.Count)
                {
                    WidthPercentage = 100
                };

                foreach (DataGridViewColumn col in dgvMovimientos.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                }

                foreach (DataGridViewRow row in dgvMovimientos.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                            table.AddCell(cell.Value?.ToString());
                    }
                }

                doc.Add(table);
                doc.Add(new Paragraph("\n*** Fin del Reporte ***"));
                doc.Add(new Paragraph("Generado automáticamente por el sistema Cafetería TOV"));

                doc.Close();
                stream.Close();
            }

            MessageBox.Show("📄 PDF exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarMovimientos();
        }
    }
}
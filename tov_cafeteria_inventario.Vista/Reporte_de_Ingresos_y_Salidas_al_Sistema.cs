using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Reporte_de_Ingresos_y_Salidas_al_Sistema : Form
    {
        private readonly Reporte_Ingreso_SalidaController reporteController = new Reporte_Ingreso_SalidaController();
        private readonly UsuarioController usuarioController = new UsuarioController();
        private readonly int usuarioID;


        public Reporte_de_Ingresos_y_Salidas_al_Sistema(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;
            this.FormClosed += Reporte_de_Ingresos_FormClosed;

            // Validar si el usuario es administrador
            int roleID = usuarioController.ObtenerRoleID(usuarioID);
            if (roleID != 1)
            {
                MessageBox.Show("No tiene permisos para acceder a este reporte.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void Reporte_de_Movimientos_FormClosed(object sender, FormClosedEventArgs e)
        {
            var bitacora = new Bitacora(usuarioID);
            bitacora.MdiParent = this.MdiParent;
            bitacora.StartPosition = FormStartPosition.CenterScreen;
            bitacora.Show();
        }

        private void Reporte_de_Ingresos_FormClosed(object sender, FormClosedEventArgs e)
        {
            var bitacora = new Bitacora(usuarioID);
            bitacora.MdiParent = this.MdiParent;
            bitacora.StartPosition = FormStartPosition.CenterScreen;
            bitacora.Show();
        }

        private void Reporte_de_Ingresos_y_Salidas_al_Sistema_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
            CargarIngresosSalidas();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            dataGridViewIngresosSalidas.DataSource = null;
            dataGridViewIngresosSalidas.Rows.Clear();
            dataGridViewIngresosSalidas.Columns.Clear();
            CargarIngresosSalidas();
        }

        private void CargarIngresosSalidas()
        {
            try
            {
                List<IngresoSalidaRegistro> lista = reporteController.ObtenerIngresosYSalidas();

                DataTable table = new DataTable();
                table.Columns.Add("BitacoraID", typeof(int));
                table.Columns.Add("NombreUsuario", typeof(string));
                table.Columns.Add("FechaRegistro", typeof(DateTime));
                table.Columns.Add("Accion", typeof(string));

                foreach (var item in lista)
                {
                    table.Rows.Add(item.BitacoraID, item.NombreUsuario, item.FechaRegistro, item.Accion);
                }

                dataGridViewIngresosSalidas.DataSource = table;

                dataGridViewIngresosSalidas.ReadOnly = true;
                dataGridViewIngresosSalidas.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportartxt_Click(object sender, EventArgs e)
        {
            if (dataGridViewIngresosSalidas.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo PDF (*.pdf)|*.pdf|Archivo de texto (*.txt)|*.txt",
                FileName = "Reporte_Ingresos_Salidas"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string extension = Path.GetExtension(saveFileDialog.FileName).ToLower();

                if (extension == ".pdf")
                {
                    ExportarAPdf(saveFileDialog.FileName);
                }
                else if (extension == ".txt")
                {
                    ExportarATxt(saveFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show("Formato no soportado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ExportarATxt(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("REPORTE DE INGRESOS Y SALIDAS - Cafetería TOV");
                sw.WriteLine("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                sw.WriteLine(new string('-', 80));

                for (int i = 0; i < dataGridViewIngresosSalidas.Columns.Count; i++)
                {
                    sw.Write(dataGridViewIngresosSalidas.Columns[i].HeaderText.PadRight(20));
                }
                sw.WriteLine();
                sw.WriteLine(new string('-', 80));

                foreach (DataGridViewRow row in dataGridViewIngresosSalidas.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            sw.Write((row.Cells[i].Value?.ToString() ?? "").PadRight(20));
                        }
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
                Paragraph title = new Paragraph("REPORTE DE INGRESOS Y SALIDAS - Cafetería TOV", fontTitle);
                title.Alignment = Element.ALIGN_CENTER;
                doc.Add(title);
                doc.Add(new Paragraph("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                doc.Add(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator()));

                PdfPTable table = new PdfPTable(dataGridViewIngresosSalidas.Columns.Count);
                table.WidthPercentage = 100;

                foreach (DataGridViewColumn col in dataGridViewIngresosSalidas.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(cell);
                }

                foreach (DataGridViewRow row in dataGridViewIngresosSalidas.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            table.AddCell(cell.Value?.ToString());
                        }
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


        private void CargarUsuarios()
        {
            var usuarios = usuarioController.ObtenerUsuarios();
            usuarios.Insert(0, new UsuarioSistema { UsuarioID = 0, Nombre = "" }); // Opción vacía

            cmbUsuarios.DataSource = usuarios;
            cmbUsuarios.DisplayMember = "Nombre";
            cmbUsuarios.ValueMember = "UsuarioID";
            cmbUsuarios.SelectedIndex = 0;
        }



        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            try
            {
                int? usuarioId = cmbUsuarios.SelectedIndex >= 0 ? (int?)cmbUsuarios.SelectedValue : null;
                DateTime fechaInicio = dtpInicio.Value.Date;
                DateTime fechaFin = dtpFin.Value.Date.AddDays(1).AddSeconds(-1);

                var datos = reporteController.ObtenerIngresosYSalidasFiltrado(usuarioId, fechaInicio, fechaFin);

                DataTable table = new DataTable();
                table.Columns.Add("BitacoraID", typeof(int));
                table.Columns.Add("NombreUsuario", typeof(string));
                table.Columns.Add("FechaRegistro", typeof(DateTime));
                table.Columns.Add("Accion", typeof(string));

                foreach (var item in datos)
                {
                    table.Rows.Add(item.BitacoraID, item.NombreUsuario, item.FechaRegistro, item.Accion);
                }

                dataGridViewIngresosSalidas.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

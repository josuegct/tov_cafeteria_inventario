using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;
using iTextSharp.text.pdf.draw;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Reporte_de_Usuarios : Form
    {
        private readonly Reporte_UsuariosController controller = new Reporte_UsuariosController();
        private readonly int usuarioID;

        public Reporte_de_Usuarios(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;
            this.FormClosed += Reporte_de_Usuarios_FormClosed;
        }

        private void CargarComboRoles()
        {
            var listaRoles = controller.ObtenerRoles();

            listaRoles.Insert(0, new Rol { RoleID = 0, NombreRol = "" });

            cmbRol.DataSource = listaRoles;
            cmbRol.DisplayMember = "NombreRol";
            cmbRol.ValueMember = "RoleID";
            cmbRol.SelectedIndex = 0;
        }

        private void Reporte_de_Usuarios_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
            CargarComboUsuarios();
            CargarComboRoles();
        }

        private void CargarComboUsuarios()
        {
            var listaUsuarios = controller.ObtenerUsuarios()
                                          .GroupBy(u => u.Nombre)
                                          .Select(g => g.First())
                                          .ToList();

            // Insertar opción vacía al inicio
            listaUsuarios.Insert(0, new UsuarioReporte { UsuarioID = 0, Nombre = "" });

            cmbUsuario.DataSource = listaUsuarios;
            cmbUsuario.DisplayMember = "Nombre";
            cmbUsuario.ValueMember = "UsuarioID";
            cmbUsuario.SelectedIndex = 0; // Mostrar vacío al iniciar
        }


        private void Reporte_de_Usuarios_FormClosed(object sender, FormClosedEventArgs e)
        {
            var menu = new Reportes(usuarioID);
            menu.MdiParent = this.MdiParent;
            menu.StartPosition = FormStartPosition.CenterScreen;
            menu.Show();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            try
            {
                List<UsuarioReporte> lista = controller.ObtenerUsuarios();

                DataTable table = new DataTable();
                table.Columns.Add("UsuarioID", typeof(int));
                table.Columns.Add("Nombre", typeof(string));
                table.Columns.Add("Usuario", typeof(string));
                table.Columns.Add("Correo", typeof(string));
                table.Columns.Add("Rol", typeof(string));
                table.Columns.Add("Estado", typeof(string));

                foreach (var u in lista)
                {
                    table.Rows.Add(u.UsuarioID, u.Nombre, u.Usuario, u.Correo, u.Rol, u.Estado);
                }

                dgvUsuarios.DataSource = table;

                dgvUsuarios.ReadOnly = true;

                dgvUsuarios.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los usuarios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ExportarAPdf(string path)
        {
            Document doc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 20f);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                doc.Open();

                Font encabezadoFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Paragraph encabezado = new Paragraph("REPORTE DE USUARIOS DEL SISTEMA - Cafetería TOV", encabezadoFont);
                encabezado.Alignment = Element.ALIGN_CENTER;
                doc.Add(encabezado);


                // TABLA
                PdfPTable tabla = new PdfPTable(dgvUsuarios.Columns.Count);
                tabla.WidthPercentage = 100;

                // Columnas
                foreach (DataGridViewColumn col in dgvUsuarios.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    tabla.AddCell(cell);
                }

                // Filas
                foreach (DataGridViewRow row in dgvUsuarios.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            tabla.AddCell(cell.Value?.ToString());
                        }
                    }
                }

                doc.Add(tabla);

                // TOTAL
                doc.Add(new Paragraph("\nTOTAL DE USUARIOS ENCONTRADOS: " + dgvUsuarios.Rows.Count));

                // PIE DE PÁGINA
                doc.Add(new Paragraph("\n*** Fin del Reporte ***"));
                doc.Add(new Paragraph("Generado automáticamente por el sistema Cafetería TOV"));

                doc.Close();
                stream.Close();
            }

            MessageBox.Show("📄 PDF exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnExportartxt_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo PDF (*.pdf)|*.pdf|Archivo de texto (*.txt)|*.txt",
                FileName = "Reporte_Usuarios"
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
                sw.WriteLine("REPORTE DE USUARIOS DEL SISTEMA - Cafetería TOV");
                sw.WriteLine("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                sw.WriteLine(new string('-', 80));

                for (int i = 0; i < dgvUsuarios.Columns.Count; i++)
                {
                    sw.Write(dgvUsuarios.Columns[i].HeaderText.PadRight(20));
                }
                sw.WriteLine();
                sw.WriteLine(new string('-', 80));

                int totalUsuarios = 0;
                foreach (DataGridViewRow row in dgvUsuarios.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            string valor = row.Cells[i].Value?.ToString() ?? "";
                            sw.Write(valor.PadRight(20));
                        }
                        sw.WriteLine();
                        totalUsuarios++;
                    }
                }

                sw.WriteLine(new string('-', 80));
                sw.WriteLine($"TOTAL DE USUARIOS ENCONTRADOS: {totalUsuarios}");
                sw.WriteLine();
                sw.WriteLine("*** Fin del Reporte ***");
                sw.WriteLine("Generado automáticamente por el sistema Cafetería TOV");
            }

            MessageBox.Show("📄 TXT exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            string usuario = cmbUsuario.Text.Trim();
            string rol = cmbRol.Text.Trim();
            DateTime desde = dtpDesde.Value.Date;
            DateTime hasta = dtpHasta.Value.Date.AddDays(1).AddTicks(-1); // incluir toda la fecha

            List<UsuarioReporte> lista = controller.ObtenerUsuariosFiltrados(usuario, rol, desde, hasta);

            DataTable table = new DataTable();
            table.Columns.Add("UsuarioID", typeof(int));
            table.Columns.Add("Nombre", typeof(string));
            table.Columns.Add("Usuario", typeof(string));
            table.Columns.Add("Correo", typeof(string));
            table.Columns.Add("Rol", typeof(string));
            table.Columns.Add("Estado", typeof(string));
            table.Columns.Add("FechaRegistro", typeof(string));

            foreach (var u in lista)
            {
                table.Rows.Add(u.UsuarioID, u.Nombre, u.Usuario, u.Correo, u.Rol, u.Estado, u.FechaRegistro.ToString("dd/MM/yyyy"));
            }

            dgvUsuarios.DataSource = table;

            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToAddRows = false;
            lblTotal.Text = $"Total usuarios encontrados: {lista.Count}";

        }

    }
}

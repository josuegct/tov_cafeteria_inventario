using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;

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

        private void Reporte_de_Usuarios_FormClosed(object sender, FormClosedEventArgs e)
        {
            var menu = new Reportes(usuarioID);
            menu.MdiParent = this.MdiParent;
            menu.StartPosition = FormStartPosition.CenterScreen;
            menu.Show();
        }

        private void Reporte_de_Usuarios_Load(object sender, EventArgs e)
        {
            CargarUsuarios();
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

                // Hacer que el DataGridView sea de solo lectura
                dgvUsuarios.ReadOnly = true;

                // Evitar que el usuario agregue nuevas filas
                dgvUsuarios.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los usuarios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                Filter = "Archivo de texto (*.txt)|*.txt",
                FileName = "Reporte_Usuarios.txt"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        // Escribir encabezados
                        for (int i = 0; i < dgvUsuarios.Columns.Count; i++)
                        {
                            sw.Write(dgvUsuarios.Columns[i].HeaderText);
                            if (i < dgvUsuarios.Columns.Count - 1)
                                sw.Write("\t");
                        }
                        sw.WriteLine();

                        // Escribir filas
                        foreach (DataGridViewRow row in dgvUsuarios.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                for (int i = 0; i < row.Cells.Count; i++)
                                {
                                    sw.Write(row.Cells[i].Value?.ToString());
                                    if (i < row.Cells.Count - 1)
                                        sw.Write("\t");
                                }
                                sw.WriteLine();
                            }
                        }
                    }

                    MessageBox.Show("📄 Reporte exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al exportar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

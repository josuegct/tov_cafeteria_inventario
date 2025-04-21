using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                Filter = "Archivo de texto (*.txt)|*.txt",
                FileName = "Reporte_Ingresos_Salidas.txt"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        for (int i = 0; i < dataGridViewIngresosSalidas.Columns.Count; i++)
                        {
                            sw.Write(dataGridViewIngresosSalidas.Columns[i].HeaderText);
                            if (i < dataGridViewIngresosSalidas.Columns.Count - 1)
                                sw.Write("\t");
                        }
                        sw.WriteLine();

                        foreach (DataGridViewRow row in dataGridViewIngresosSalidas.Rows)
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

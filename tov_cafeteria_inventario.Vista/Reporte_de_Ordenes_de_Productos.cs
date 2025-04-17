using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Reporte_de_Ordenes_de_Productos : Form
    {
        private readonly Reporte_Ordenes_ProductosController controller = new Reporte_Ordenes_ProductosController();
        private readonly int usuarioID;

        public Reporte_de_Ordenes_de_Productos(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;
            this.FormClosed += Reporte_de_Ordenes_FormClosed;
        }

        private void Reporte_de_Ordenes_FormClosed(object sender, FormClosedEventArgs e)
        {
            var menu = new Reportes(usuarioID);
            menu.MdiParent = this.MdiParent;
            menu.StartPosition = FormStartPosition.CenterScreen;
            menu.Show();
        }

        private void Reporte_de_Ordenes_de_Productos_Load(object sender, EventArgs e)
        {
            CargarOrdenes();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            dgvOrdenes.DataSource = null;
            dgvOrdenes.Rows.Clear();
            dgvOrdenes.Columns.Clear();
            CargarOrdenes();
        }

        private void CargarOrdenes()
        {
            try
            {
                List<OrdenProducto> lista = controller.ObtenerOrdenes();

                DataTable table = new DataTable();
                table.Columns.Add("OrdenID", typeof(int));
                table.Columns.Add("Producto", typeof(string));
                table.Columns.Add("Proveedor", typeof(string));
                table.Columns.Add("Usuario", typeof(string));
                table.Columns.Add("Estado", typeof(string));
                table.Columns.Add("FechaOrden", typeof(DateTime));

                foreach (var item in lista)
                {
                    table.Rows.Add(item.OrdenID, item.Producto, item.Proveedor, item.Usuario, item.Estado, item.FechaOrden);
                }

                dgvOrdenes.DataSource = table;

                dgvOrdenes.ReadOnly = true;
                dgvOrdenes.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las órdenes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportartxt_Click(object sender, EventArgs e)
        {
            if (dgvOrdenes.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo de texto (*.txt)|*.txt",
                FileName = "Reporte_Ordenes.txt"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        for (int i = 0; i < dgvOrdenes.Columns.Count; i++)
                        {
                            sw.Write(dgvOrdenes.Columns[i].HeaderText);
                            if (i < dgvOrdenes.Columns.Count - 1)
                                sw.Write("\t");
                        }
                        sw.WriteLine();

                        foreach (DataGridViewRow row in dgvOrdenes.Rows)
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

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using ModeloInventario = tov_cafeteria_inventario.Modelo.Inventario;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Reporte_de_Inventario : Form
    {
        private readonly InventarioController controller = new InventarioController();
        private readonly int usuarioID;

        public Reporte_de_Inventario(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;
            this.Load += Reporte_de_Inventario_Load;
            this.FormClosed += Reporte_de_Inventario_FormClosed;
        }

        private void Reporte_de_Inventario_Load(object sender, EventArgs e)
        {
            CargarStockActual();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            dgvInventario.DataSource = null;
            dgvInventario.Rows.Clear();
            dgvInventario.Columns.Clear();
            CargarStockActual();
        }

        private void CargarStockActual()
        {
            try
            {
                List<ModeloInventario> lista = controller.ObtenerStockActual();

                DataTable dt = new DataTable();
                dt.Columns.Add("ProductoID", typeof(int));
                dt.Columns.Add("Producto", typeof(string));
                dt.Columns.Add("Cantidad", typeof(int));
                dt.Columns.Add("Última Fecha", typeof(DateTime));
                dt.Columns.Add("Usuario", typeof(string));

                foreach (var item in lista)
                {
                    dt.Rows.Add(item.ProductoID, item.Producto, item.Cantidad, item.Fecha, item.Usuario);
                }

                dgvInventario.DataSource = dt;

                dgvInventario.ReadOnly = true;
                dgvInventario.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el inventario actual: " + ex.Message);
            }
        }

        private void Reporte_de_Inventario_FormClosed(object sender, FormClosedEventArgs e)
        {
            var volver = new Reportes(usuarioID);
            volver.MdiParent = this.MdiParent;
            volver.StartPosition = FormStartPosition.CenterScreen;
            volver.Show();
        }

        private void btnExportartxt_Click(object sender, EventArgs e)
        {
            if (dgvInventario.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo de texto (*.txt)|*.txt",
                FileName = "Reporte_Inventario.txt"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        for (int i = 0; i < dgvInventario.Columns.Count; i++)
                        {
                            sw.Write(dgvInventario.Columns[i].HeaderText);
                            if (i < dgvInventario.Columns.Count - 1)
                                sw.Write("\t");
                        }
                        sw.WriteLine();

                        foreach (DataGridViewRow row in dgvInventario.Rows)
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

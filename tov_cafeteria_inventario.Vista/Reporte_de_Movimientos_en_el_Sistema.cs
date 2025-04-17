using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Reporte_de_Movimientos_en_el_Sistema : Form
    {
        private readonly Reporte_Movimientos_SistemaController controller = new Reporte_Movimientos_SistemaController();
        private readonly int usuarioID;

        public Reporte_de_Movimientos_en_el_Sistema(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;
            this.FormClosed += Reporte_de_Movimientos_FormClosed;
        }

        private void Reporte_de_Movimientos_FormClosed(object sender, FormClosedEventArgs e)
        {
            var bitacora = new Bitacora(usuarioID);
            bitacora.MdiParent = this.MdiParent;
            bitacora.StartPosition = FormStartPosition.CenterScreen;
            bitacora.Show();
        }

        private void Reporte_de_Movimientos_en_el_Sistema_Load(object sender, EventArgs e)
        {
            CargarMovimientos();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            dgvMovimientos.DataSource = null;
            dgvMovimientos.Rows.Clear();
            dgvMovimientos.Columns.Clear();
            CargarMovimientos();
        }

        private void CargarMovimientos()
        {
            try
            {
                List<MovimientoRegistro> lista = controller.ObtenerMovimientos();

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar movimientos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                Filter = "Archivo de texto (*.txt)|*.txt",
                FileName = "Reporte_Movimientos.txt"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        for (int i = 0; i < dgvMovimientos.Columns.Count; i++)
                        {
                            sw.Write(dgvMovimientos.Columns[i].HeaderText);
                            if (i < dgvMovimientos.Columns.Count - 1)
                                sw.Write("\t");
                        }
                        sw.WriteLine();

                        foreach (DataGridViewRow row in dgvMovimientos.Rows)
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

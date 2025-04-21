using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Existencias : Form
    {
        private readonly ExistenciasController existenciasController = new ExistenciasController();
        private readonly int roleID;

        public Existencias(int roleID)
        {
            InitializeComponent();
            this.roleID = roleID;

            CargarExistencias();

            if (roleID != 1)
            {
                dgvExistencias.ReadOnly = true;
                dgvExistencias.AllowUserToAddRows = false;
                dgvExistencias.AllowUserToDeleteRows = false;
                dgvExistencias.DefaultCellStyle.BackColor = Color.LightGray;
            }
        }

        public Existencias()
        {
            InitializeComponent();
            CargarExistencias();
        }

        private void CargarExistencias()
        {
            try
            {
                var lista = existenciasController.ObtenerExistenciasActuales();
                dgvExistencias.DataSource = lista;

                if (dgvExistencias.Columns.Contains("ProductoID"))
                    dgvExistencias.Columns["ProductoID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar existencias: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarExistencias();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvExistencias.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivo de texto (*.txt)|*.txt";
            saveFileDialog.Title = "Guardar existencias como texto";
            saveFileDialog.FileName = "existencias.txt";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                    {
                        for (int i = 0; i < dgvExistencias.Columns.Count; i++)
                        {
                            if (dgvExistencias.Columns[i].Visible)
                                writer.Write(dgvExistencias.Columns[i].HeaderText + (i < dgvExistencias.Columns.Count - 1 ? "\t" : ""));
                        }
                        writer.WriteLine();

                        foreach (DataGridViewRow row in dgvExistencias.Rows)
                        {
                            for (int i = 0; i < dgvExistencias.Columns.Count; i++)
                            {
                                if (dgvExistencias.Columns[i].Visible)
                                    writer.Write(row.Cells[i].Value?.ToString() + (i < dgvExistencias.Columns.Count - 1 ? "\t" : ""));
                            }
                            writer.WriteLine();
                        }
                    }

                    MessageBox.Show("Exportación completada con éxito.", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al exportar: " + ex.Message, "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

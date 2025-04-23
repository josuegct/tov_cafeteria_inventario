using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        private void ExportarAPdf(string path)
        {
            Document doc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 20f);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                doc.Open();

                // ENCABEZADO
                Font fontEncabezado = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Paragraph encabezado = new Paragraph("REPORTE DE ÓRDENES DE PRODUCTOS - Cafetería TOV", fontEncabezado);
                encabezado.Alignment = Element.ALIGN_CENTER;
                doc.Add(encabezado);
                doc.Add(new Paragraph("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                doc.Add(new Chunk(new LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                // TABLA
                PdfPTable tabla = new PdfPTable(dgvOrdenes.Columns.Count);
                tabla.WidthPercentage = 100;

                foreach (DataGridViewColumn col in dgvOrdenes.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    tabla.AddCell(cell);
                }

                foreach (DataGridViewRow row in dgvOrdenes.Rows)
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
                doc.Add(new Paragraph("\nTOTAL DE ÓRDENES ENCONTRADAS: " + dgvOrdenes.Rows.Count));
                doc.Add(new Paragraph("\n*** Fin del Reporte ***"));
                doc.Add(new Paragraph("Generado automáticamente por el sistema Cafetería TOV"));

                doc.Close();
                stream.Close();
            }

            MessageBox.Show("📄 PDF exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ExportarATxt(string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("REPORTE DE ÓRDENES DE PRODUCTOS - Cafetería TOV");
                sw.WriteLine("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                sw.WriteLine(new string('-', 80));

                for (int i = 0; i < dgvOrdenes.Columns.Count; i++)
                {
                    sw.Write(dgvOrdenes.Columns[i].HeaderText.PadRight(20));
                }
                sw.WriteLine();
                sw.WriteLine(new string('-', 80));

                int total = 0;
                foreach (DataGridViewRow row in dgvOrdenes.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            string valor = row.Cells[i].Value?.ToString() ?? "";
                            sw.Write(valor.PadRight(20));
                        }
                        sw.WriteLine();
                        total++;
                    }
                }

                sw.WriteLine(new string('-', 80));
                sw.WriteLine($"TOTAL DE ÓRDENES ENCONTRADAS: {total}");
                sw.WriteLine();
                sw.WriteLine("*** Fin del Reporte ***");
                sw.WriteLine("Generado automáticamente por el sistema Cafetería TOV");
            }

            MessageBox.Show("📄 TXT exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Reporte_de_Ordenes_de_Productos_Load(object sender, EventArgs e)
        {
            cmbProducto.DataSource = controller.ObtenerProductos();
            cmbProducto.DisplayMember = "Nombre";
            cmbProducto.ValueMember = "ProductoID";
            cmbProducto.SelectedIndex = 0;

            cmbProveedor.DataSource = controller.ObtenerProveedores();
            cmbProveedor.DisplayMember = "Nombre";
            cmbProveedor.ValueMember = "ProveedorID";
            cmbProveedor.SelectedIndex = 0;

            dtpDesde.Value = DateTime.Today.AddMonths(-1);
            dtpHasta.Value = DateTime.Today;

            CargarOrdenes();
        }



        private void Reporte_de_Ordenes_FormClosed(object sender, FormClosedEventArgs e)
        {
            var menu = new Reportes(usuarioID);
            menu.MdiParent = this.MdiParent;
            menu.StartPosition = FormStartPosition.CenterScreen;
            menu.Show();
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
                Filter = "Archivo PDF (*.pdf)|*.pdf|Archivo de texto (*.txt)|*.txt",
                FileName = "Reporte_Ordenes"
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

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            int? productoID = cmbProducto.SelectedValue as int?;
            int? proveedorID = cmbProveedor.SelectedValue as int?;
            DateTime desde = dtpDesde.Value.Date;
            DateTime hasta = dtpHasta.Value.Date;

            List<OrdenProducto> lista = controller.ObtenerOrdenesFiltradas(productoID, proveedorID, desde, hasta);

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


    }
}

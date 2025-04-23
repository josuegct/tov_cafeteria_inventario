using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.Linq;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;
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
            CargarCombos();
        }

        private void CargarCombos()
        {
            var productos = controller.ObtenerProductos();
            productos.Insert(0, new Producto { ProductoID = 0, Nombre = "" }); // Agrega vacío
            cmbProducto.DataSource = productos;
            cmbProducto.DisplayMember = "Nombre";
            cmbProducto.ValueMember = "ProductoID";
            cmbProducto.SelectedIndex = 0;

            var proveedores = controller.ObtenerProveedores();
            proveedores.Insert(0, new Proveedor { ProveedorID = 0, Nombre = "" }); // Agrega vacío
            cmbProveedor.DataSource = proveedores;
            cmbProveedor.DisplayMember = "Nombre";
            cmbProveedor.ValueMember = "ProveedorID";
            cmbProveedor.SelectedIndex = 0;
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            dgvInventario.DataSource = null;
            dgvInventario.Rows.Clear();
            dgvInventario.Columns.Clear();
            CargarStockActual();
        }

        private void ExportarAPdf(string path)
        {
            Document doc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 20f);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                doc.Open();

                Font encabezadoFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Paragraph encabezado = new Paragraph("REPORTE DE INVENTARIO - Cafetería TOV", encabezadoFont);
                encabezado.Alignment = Element.ALIGN_CENTER;
                doc.Add(encabezado);
                doc.Add(new Paragraph("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                doc.Add(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator()));

                PdfPTable tabla = new PdfPTable(dgvInventario.Columns.Count);
                tabla.WidthPercentage = 100;

                foreach (DataGridViewColumn col in dgvInventario.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    tabla.AddCell(cell);
                }

                foreach (DataGridViewRow row in dgvInventario.Rows)
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
                sw.WriteLine("REPORTE DE INVENTARIO - Cafetería TOV");
                sw.WriteLine("Fecha de generación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                sw.WriteLine(new string('-', 100));

                for (int i = 0; i < dgvInventario.Columns.Count; i++)
                {
                    sw.Write(dgvInventario.Columns[i].HeaderText.PadRight(20));
                }
                sw.WriteLine();
                sw.WriteLine(new string('-', 100));

                foreach (DataGridViewRow row in dgvInventario.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            string valor = row.Cells[i].Value?.ToString() ?? "";
                            sw.Write(valor.PadRight(20));
                        }
                        sw.WriteLine();
                    }
                }

                sw.WriteLine(new string('-', 100));
                sw.WriteLine("*** Fin del Reporte ***");
                sw.WriteLine("Generado automáticamente por el sistema Cafetería TOV");
            }

            MessageBox.Show("📄 TXT exportado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CargarStockActual()
        {
            try
            {
                DateTime desde = new DateTime(1753, 1, 1); // ✅ Fecha mínima válida para SQL Server
                DateTime hasta = DateTime.MaxValue;

                var lista = controller.ObtenerStockFiltrado(null, null, desde, hasta);

                DataTable dt = new DataTable();
                dt.Columns.Add("MovimientoID", typeof(int));
                dt.Columns.Add("ProductoID", typeof(int));
                dt.Columns.Add("Producto", typeof(string));
                dt.Columns.Add("ProveedorID", typeof(int));
                dt.Columns.Add("Proveedor", typeof(string));
                dt.Columns.Add("UnidadMedida", typeof(string));
                dt.Columns.Add("TipoMovimiento", typeof(string));
                dt.Columns.Add("Cantidad", typeof(int));
                dt.Columns.Add("PrecioUnitario", typeof(decimal));
                dt.Columns.Add("PrecioTotal", typeof(decimal));
                dt.Columns.Add("FechaMovimiento", typeof(DateTime));
                dt.Columns.Add("Usuario", typeof(string));

                foreach (var item in lista)
                {
                    dt.Rows.Add(
                        item.MovimientoID,
                        item.ProductoID,
                        item.Producto,
                        item.ProveedorID,
                        item.Proveedor,
                        item.UnidadMedida,
                        item.TipoMovimiento,
                        item.Cantidad,
                        item.PrecioUnitario,
                        item.PrecioTotal,
                        item.Fecha,
                        item.Usuario
                    );
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
                Filter = "Archivo PDF (*.pdf)|*.pdf|Archivo de texto (*.txt)|*.txt",
                FileName = "Reporte_Inventario"
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
            int? productoID = cmbProducto.SelectedIndex >= 0 ? (int?)cmbProducto.SelectedValue : null;
            int? proveedorID = cmbProveedor.SelectedIndex >= 0 ? (int?)cmbProveedor.SelectedValue : null;
            DateTime desde = dtpDesde.Value.Date;
            DateTime hasta = dtpHasta.Value.Date;

            try
            {
                var lista = controller.ObtenerStockFiltrado(productoID, proveedorID, desde, hasta);

                DataTable dt = new DataTable();
                dt.Columns.Add("MovimientoID", typeof(int));
                dt.Columns.Add("ProductoID", typeof(int));
                dt.Columns.Add("Producto", typeof(string));
                dt.Columns.Add("ProveedorID", typeof(int));
                dt.Columns.Add("Proveedor", typeof(string));
                dt.Columns.Add("UnidadMedida", typeof(string));
                dt.Columns.Add("TipoMovimiento", typeof(string));
                dt.Columns.Add("Cantidad", typeof(int));
                dt.Columns.Add("PrecioUnitario", typeof(decimal));
                dt.Columns.Add("PrecioTotal", typeof(decimal));
                dt.Columns.Add("FechaMovimiento", typeof(DateTime));
                dt.Columns.Add("Usuario", typeof(string));

                foreach (var item in lista)
                {
                    dt.Rows.Add(
                        item.MovimientoID,
                        item.ProductoID,
                        item.Producto,
                        item.ProveedorID,
                        item.Proveedor,
                        item.UnidadMedida,
                        item.TipoMovimiento,
                        item.Cantidad,
                        item.PrecioUnitario,
                        item.PrecioTotal,
                        item.Fecha,
                        item.Usuario
                    );
                }

                dgvInventario.DataSource = dt;

                int totalCantidad = lista.Sum(x => x.Cantidad);
                decimal totalPrecio = lista.Sum(x => x.PrecioTotal);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar: " + ex.Message);
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Ordenes : Form
    {
        private readonly OrdenController ordenController = new OrdenController();
        private int usuarioID;

        public Ordenes(int usuarioID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.usuarioID = usuarioID;
            txtUsuarioID.Text = usuarioID.ToString();
            txtUsuarioID.Enabled = false;
            dgvOrdenes.SelectionChanged += dgvOrdenes_SelectionChanged;
            cmbEstado.Items.AddRange(new string[] { "Pendiente", "En proceso", "Completada" });
            CargarProveedores();
            CargarOrdenes();
        }

        private void CargarOrdenes()
        {
            try
            {
                var ordenes = ordenController.ObtenerOrdenes();
                dgvOrdenes.DataSource = ordenes;

                dgvOrdenes.Columns["OrdenID"].HeaderText = "ID";
                dgvOrdenes.Columns["FechaOrden"].HeaderText = "Fecha";
                dgvOrdenes.Columns["Estado"].HeaderText = "Estado";
                dgvOrdenes.Columns["NombreProducto"].HeaderText = "Producto";
                dgvOrdenes.Columns["NombreProveedor"].HeaderText = "Proveedor";
                dgvOrdenes.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";
                dgvOrdenes.Columns["PrecioTotal"].HeaderText = "Precio Total";

                if (dgvOrdenes.Columns.Contains("UsuarioID"))
                    dgvOrdenes.Columns["UsuarioID"].Visible = false;

                if (dgvOrdenes.Columns.Contains("ProductoID"))
                    dgvOrdenes.Columns["ProductoID"].Visible = false;

                if (dgvOrdenes.Columns.Contains("ProveedorID"))
                    dgvOrdenes.Columns["ProveedorID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar órdenes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProveedores()
        {
            try
            {
                var proveedores = ordenController.ObtenerProveedores();
                cmbProveedor.DataSource = proveedores;
                cmbProveedor.DisplayMember = "Nombre";
                cmbProveedor.ValueMember = "ProveedorID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proveedores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarProductosPorProveedor(int proveedorID)
        {
            try
            {
                var productos = ordenController.ObtenerProductosPorProveedor(proveedorID);
                if (productos.Count > 0)
                {
                    cmbProducto.DataSource = productos;
                    cmbProducto.DisplayMember = "Nombre";
                    cmbProducto.ValueMember = "ProductoID";
                }
                else
                {
                    MessageBox.Show("No hay productos disponibles para este proveedor.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProveedor.SelectedValue is int proveedorID)
            {
                CargarProductosPorProveedor(proveedorID);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProducto.SelectedValue == null || cmbProveedor.SelectedValue == null || cmbEstado.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, complete todos los campos antes de agregar la orden.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int productoID = Convert.ToInt32(cmbProducto.SelectedValue);
                int proveedorID = Convert.ToInt32(cmbProveedor.SelectedValue);
                string estado = cmbEstado.SelectedItem.ToString();
                DateTime fecha = dtpFechaOrden.Value;

                foreach (DataGridViewRow row in dgvOrdenes.Rows)
                {
                    if (row.Cells["ProductoID"].Value != null &&
                        row.Cells["ProveedorID"].Value != null &&
                        row.Cells["UsuarioID"].Value != null)
                    {
                        int prod = Convert.ToInt32(row.Cells["ProductoID"].Value);
                        int prov = Convert.ToInt32(row.Cells["ProveedorID"].Value);
                        int user = Convert.ToInt32(row.Cells["UsuarioID"].Value);
                        string estadoExistente = row.Cells["Estado"].Value.ToString();

                        if (prod == productoID && prov == proveedorID && user == usuarioID && estadoExistente != "Cancelada")
                        {
                            MessageBox.Show("Ya existe una orden activa para este producto y proveedor.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                var nuevaOrden = new Orden
                {
                    FechaOrden = fecha,
                    Estado = estado,
                    UsuarioID = usuarioID,
                    ProductoID = productoID,
                    ProveedorID = proveedorID
                };

                ordenController.AgregarOrden(nuevaOrden);
                CargarOrdenes();
                MessageBox.Show("Orden agregada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar orden: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvOrdenes.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione una orden para modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbProducto.SelectedValue == null || cmbProveedor.SelectedValue == null || cmbEstado.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, complete todos los campos antes de modificar la orden.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int ordenID = Convert.ToInt32(dgvOrdenes.SelectedRows[0].Cells["OrdenID"].Value);
                string estadoAnterior = dgvOrdenes.SelectedRows[0].Cells["Estado"].Value.ToString();

                var orden = new Orden
                {
                    OrdenID = ordenID,
                    FechaOrden = dtpFechaOrden.Value,
                    Estado = cmbEstado.SelectedItem.ToString(),
                    UsuarioID = usuarioID,
                    ProductoID = Convert.ToInt32(cmbProducto.SelectedValue),
                    ProveedorID = Convert.ToInt32(cmbProveedor.SelectedValue)
                };

                ordenController.ModificarOrden(orden);

                if (estadoAnterior != "Completada" && orden.Estado == "Completada")
                {
                    using (SqlConnection conn = new SqlConnection("Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;"))
                    {
                        conn.Open();
                        string query = "EXEC sp_RegistrarMovimiento @ProductoID, @TipoMovimiento, @Cantidad, @UsuarioID, @PrecioUnitario, @PrecioTotal";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ProductoID", orden.ProductoID);
                            cmd.Parameters.AddWithValue("@TipoMovimiento", "Ingreso");
                            cmd.Parameters.AddWithValue("@Cantidad", 1);
                            cmd.Parameters.AddWithValue("@UsuarioID", orden.UsuarioID);

                            decimal precioUnitario = Convert.ToDecimal(dgvOrdenes.SelectedRows[0].Cells["PrecioUnitario"].Value);
                            decimal precioTotal = Convert.ToDecimal(dgvOrdenes.SelectedRows[0].Cells["PrecioTotal"].Value);

                            cmd.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                            cmd.Parameters.AddWithValue("@PrecioTotal", precioTotal);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }


                CargarOrdenes();
                MessageBox.Show("Orden modificada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar orden: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvOrdenes.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione una orden para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int ordenID = Convert.ToInt32(dgvOrdenes.SelectedRows[0].Cells["OrdenID"].Value);
                var confirm = MessageBox.Show("¿Está seguro de que desea eliminar esta orden?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm == DialogResult.Yes)
                {
                    ordenController.EliminarOrden(ordenID);
                    CargarOrdenes();
                    MessageBox.Show("Orden eliminada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar orden: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVerDetalles_Click(object sender, EventArgs e)
        {
            if (dgvOrdenes.SelectedRows.Count > 0)
            {
                try
                {
                    int ordenID = Convert.ToInt32(dgvOrdenes.SelectedRows[0].Cells["OrdenID"].Value);
                    string productoNombre = dgvOrdenes.SelectedRows[0].Cells["NombreProducto"].Value.ToString();

                    DetalleOrden detalleForm = new DetalleOrden(ordenID, productoNombre);
                    detalleForm.ShowDialog();

                    CargarOrdenes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al intentar abrir los detalles: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Seleccione una orden para ver/modificar sus detalles.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dgvOrdenes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOrdenes.SelectedRows.Count > 0)
            {
                var fila = dgvOrdenes.SelectedRows[0];
                dtpFechaOrden.Value = Convert.ToDateTime(fila.Cells["FechaOrden"].Value);
                cmbEstado.SelectedItem = fila.Cells["Estado"].Value.ToString();
                txtUsuarioID.Text = fila.Cells["UsuarioID"].Value.ToString();
                string proveedorNombre = fila.Cells["NombreProveedor"].Value.ToString();
                cmbProveedor.SelectedIndex = cmbProveedor.FindStringExact(proveedorNombre);
                string productoNombre = fila.Cells["NombreProducto"].Value.ToString();
                if (cmbProducto.Items.Count > 0)
                {
                    cmbProducto.SelectedIndex = cmbProducto.FindStringExact(productoNombre);
                }
            }
        }
    }
}

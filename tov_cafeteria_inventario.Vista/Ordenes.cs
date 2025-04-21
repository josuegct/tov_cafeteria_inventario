using System;
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
        private int RoleID;

        public Ordenes(int usuarioID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.usuarioID = usuarioID;

            UsuarioController usuarioController = new UsuarioController();
            this.RoleID = usuarioController.ObtenerRoleID(usuarioID);

            txtUsuarioID.Text = usuarioID.ToString();
            txtUsuarioID.Enabled = false;

            dgvOrdenes.SelectionChanged += dgvOrdenes_SelectionChanged;
            dgvOrdenes.AllowUserToAddRows = false;
            dgvOrdenes.ReadOnly = true;

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
                dgvOrdenes.Columns["UsuarioID"].HeaderText = "Usuario";
                dgvOrdenes.Columns["FechaOrden"].HeaderText = "Fecha";
                dgvOrdenes.Columns["Estado"].HeaderText = "Estado";
                dgvOrdenes.Columns["ProveedorID"].HeaderText = "ProveedorID";
                dgvOrdenes.Columns["NombreProveedor"].HeaderText = "Proveedor";
                dgvOrdenes.Columns["ProductoID"].HeaderText = "ProductoID";
                dgvOrdenes.Columns["NombreProducto"].HeaderText = "Producto";
                dgvOrdenes.Columns["UnidadMedida"].HeaderText = "Unidad";
                dgvOrdenes.Columns["Cantidad"].HeaderText = "Cantidad";
                dgvOrdenes.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";
                dgvOrdenes.Columns["PrecioTotal"].HeaderText = "Precio Total";

                dgvOrdenes.Columns["OrdenID"].DisplayIndex = 0;
                dgvOrdenes.Columns["UsuarioID"].DisplayIndex = 1;
                dgvOrdenes.Columns["FechaOrden"].DisplayIndex = 2;
                dgvOrdenes.Columns["Estado"].DisplayIndex = 3;
                dgvOrdenes.Columns["ProveedorID"].DisplayIndex = 4;
                dgvOrdenes.Columns["NombreProveedor"].DisplayIndex = 5;
                dgvOrdenes.Columns["ProductoID"].DisplayIndex = 6;
                dgvOrdenes.Columns["NombreProducto"].DisplayIndex = 7;
                dgvOrdenes.Columns["UnidadMedida"].DisplayIndex = 8;
                dgvOrdenes.Columns["Cantidad"].DisplayIndex = 9;
                dgvOrdenes.Columns["PrecioUnitario"].DisplayIndex = 10;
                dgvOrdenes.Columns["PrecioTotal"].DisplayIndex = 11;
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
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;"))
                {
                    conn.Open();
                    string query = @"SELECT ProductoID, Nombre, PrecioUnitario 
                                     FROM Productos 
                                     WHERE ProveedorID = @ProveedorID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProveedorID", proveedorID);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable productosTable = new DataTable();
                        adapter.Fill(productosTable);

                        cmbProducto.DataSource = productosTable;
                        cmbProducto.DisplayMember = "Nombre";
                        cmbProducto.ValueMember = "ProductoID";
                    }
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
            if (cmbEstado.SelectedItem != null && cmbEstado.SelectedItem.ToString() == "Completada")
            {
                if (RoleID != 1)
                {
                    MessageBox.Show("Solo los administradores pueden registrar órdenes como 'Completada'. Debe de agregar la orden atraves de Modificar", "Permiso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

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

                var nuevaOrden = new Orden
                {
                    FechaOrden = fecha,
                    Estado = estado,
                    UsuarioID = usuarioID,
                    ProductoID = productoID,
                    ProveedorID = proveedorID
                };

                ordenController.AgregarOrden(nuevaOrden);

                if (estado == "Completada")
                {
                    DataRowView productoSeleccionado = cmbProducto.SelectedItem as DataRowView;
                    if (productoSeleccionado != null)
                    {
                        decimal precioUnitario = Convert.ToDecimal(productoSeleccionado["PrecioUnitario"]);
                        decimal precioTotal = precioUnitario * 1; // Cantidad 1 por defecto

                        RegistrarMovimientoInventario(nuevaOrden, precioUnitario, precioTotal);
                    }
                }

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
                string nuevoEstado = cmbEstado.SelectedItem.ToString();

                var orden = new Orden
                {
                    OrdenID = ordenID,
                    FechaOrden = dtpFechaOrden.Value,
                    Estado = nuevoEstado,
                    UsuarioID = usuarioID,
                    ProductoID = Convert.ToInt32(cmbProducto.SelectedValue),
                    ProveedorID = Convert.ToInt32(cmbProveedor.SelectedValue)
                };

                ordenController.ModificarOrden(orden);

                if (nuevoEstado == "Completada")
                {
                    decimal precioUnitario = Convert.ToDecimal(dgvOrdenes.SelectedRows[0].Cells["PrecioUnitario"].Value);
                    decimal precioTotal = Convert.ToDecimal(dgvOrdenes.SelectedRows[0].Cells["PrecioTotal"].Value);

                    RegistrarMovimientoInventario(orden, precioUnitario, precioTotal);
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

                // Asignar directamente Usuario y Fecha
                txtUsuarioID.Text = fila.Cells["UsuarioID"].Value.ToString();
                dtpFechaOrden.Value = Convert.ToDateTime(fila.Cells["FechaOrden"].Value);
                cmbEstado.SelectedItem = fila.Cells["Estado"].Value.ToString();

                // Obtener ProveedorID y ProductoID
                int proveedorID = Convert.ToInt32(fila.Cells["ProveedorID"].Value);
                int productoID = Convert.ToInt32(fila.Cells["ProductoID"].Value);

                // Establecer el proveedor y cargar sus productos
                cmbProveedor.SelectedValue = proveedorID;
                CargarProductosPorProveedor(proveedorID);

                // Buscar el producto en el combo (después de cargarlo)
                if (cmbProducto.DataSource != null)
                {
                    cmbProducto.SelectedValue = productoID;
                }
            }
        }


        private void RegistrarMovimientoInventario(Orden orden, decimal precioUnitario, decimal precioTotal)
        {
            using (SqlConnection conn = new SqlConnection("Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;"))
            {
                conn.Open();
                string query = "EXEC sp_RegistrarMovimiento @ProductoID, @TipoMovimiento, @Cantidad, @UsuarioID, @PrecioUnitario, @PrecioTotal";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    int cantidad = Convert.ToInt32(dgvOrdenes.SelectedRows[0].Cells["Cantidad"].Value);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@ProductoID", orden.ProductoID);
                    cmd.Parameters.AddWithValue("@TipoMovimiento", "Ingreso");
                    cmd.Parameters.AddWithValue("@UsuarioID", orden.UsuarioID);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                    cmd.Parameters.AddWithValue("@PrecioTotal", precioTotal);
 
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
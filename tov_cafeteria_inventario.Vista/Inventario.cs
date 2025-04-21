using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Inventario : Form
    {
        private readonly int usuarioID;
        private int movimientoSeleccionadoID = -1;
        private bool cargandoDesdeGrid = false;

        private readonly ProveedorController proveedorController = new ProveedorController();
        private readonly ProductoController productoController = new ProductoController();
        private readonly InventarioController inventarioController = new InventarioController();

        public Inventario(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;

            UsuarioController usuarioController = new UsuarioController();
            int roleID = usuarioController.ObtenerRoleID(usuarioID);
            if (roleID != 1)
            {
                MessageBox.Show("No tiene permisos para acceder al módulo de Inventario.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            dgvInventario.AllowUserToAddRows = false;
            dgvInventario.AutoGenerateColumns = true;

            txtMovimientoID.ReadOnly = true;
            txtMovimientoID.Enabled = false;
            txtPrecioTotal.ReadOnly = true;

            CargarMovimientos();
            CargarProveedores();

            cmbProveedor.SelectedIndexChanged += CmbProveedor_SelectedIndexChanged;
            dgvInventario.CellClick += dgvInventario_CellClick;
            txtCantidad.TextChanged += CalcularPrecioTotal;
            txtPrecioUnitario.TextChanged += CalcularPrecioTotal;
        }

        private void CargarMovimientos()
        {
            try
            {
                var tabla = inventarioController.ObtenerMovimientosIndividuales();

                if (tabla.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron movimientos en la base de datos.", "Inventario vacío", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                dgvInventario.AutoGenerateColumns = true;
                dgvInventario.DataSource = tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar movimientos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RegistrarMovimientoInventario(Orden orden, int cantidad, decimal precioUnitario, decimal precioTotal)
        {
            string unidadMedida = cmbUnidadMedida.SelectedItem?.ToString() ?? "N/A";

            using (SqlConnection conn = new SqlConnection("Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;"))
            {
                conn.Open();
                string query = "EXEC sp_RegistrarMovimiento @ProductoID, @TipoMovimiento, @Cantidad, @UsuarioID, @PrecioUnitario, @PrecioTotal, @UnidadMedida";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductoID", orden.ProductoID);
                    cmd.Parameters.AddWithValue("@TipoMovimiento", "Ingreso");
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@UsuarioID", orden.UsuarioID);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                    cmd.Parameters.AddWithValue("@PrecioTotal", precioTotal);
                    cmd.Parameters.AddWithValue("@UnidadMedida", unidadMedida);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void LimpiarFormulario()
        {
            txtMovimientoID.Text = "";
            txtCantidad.Text = "";
            txtPrecioUnitario.Text = "";
            txtPrecioTotal.Text = "";
            cmbProveedor.SelectedIndex = 0;
            cmbProducto.DataSource = null;
            cmbTipoMovimiento.SelectedIndex = -1;
            movimientoSeleccionadoID = -1;
        }

        private void CargarProveedores()
        {
            var proveedores = proveedorController.ObtenerProveedores();

            DataTable tabla = new DataTable();
            tabla.Columns.Add("ProveedorID", typeof(int));
            tabla.Columns.Add("Nombre", typeof(string));
            tabla.Rows.Add(0, ""); // opción en blanco

            foreach (var proveedor in proveedores)
            {
                tabla.Rows.Add(proveedor.ProveedorID, proveedor.Nombre);
            }

            cmbProveedor.DataSource = tabla;
            cmbProveedor.DisplayMember = "Nombre";
            cmbProveedor.ValueMember = "ProveedorID";
            cmbProveedor.SelectedIndex = 0;
        }

        private void CmbProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargandoDesdeGrid) return;

            if (cmbProveedor.SelectedValue != null &&
                int.TryParse(cmbProveedor.SelectedValue.ToString(), out int proveedorID) &&
                proveedorID != 0)
            {
                CargarProductosPorProveedor(proveedorID);
            }
            else
            {
                cmbProducto.DataSource = null;
            }
        }

        private void CargarProductosPorProveedor(int proveedorID)
        {
            var productos = productoController.ObtenerProductosPorProveedor(proveedorID);
            cmbProducto.DataSource = productos;
            cmbProducto.DisplayMember = "Nombre";
            cmbProducto.ValueMember = "ProductoID";
        }

        private void dgvInventario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            cargandoDesdeGrid = true;

            DataGridViewRow row = dgvInventario.Rows[e.RowIndex];

            try
            {
                movimientoSeleccionadoID = Convert.ToInt32(row.Cells["MovimientoID"].Value ?? -1);
                txtMovimientoID.Text = movimientoSeleccionadoID.ToString();
                txtCantidad.Text = row.Cells["Cantidad"].Value?.ToString();
                txtPrecioUnitario.Text = row.Cells["PrecioUnitario"].Value?.ToString();
                txtPrecioTotal.Text = row.Cells["PrecioTotal"].Value?.ToString();
                cmbTipoMovimiento.SelectedItem = row.Cells["TipoMovimiento"].Value?.ToString();

                int proveedorID = Convert.ToInt32(row.Cells["ProveedorID"].Value ?? 0);
                int productoID = Convert.ToInt32(row.Cells["ProductoID"].Value ?? 0);

                cmbProveedor.SelectedValue = proveedorID;
                CargarProductosPorProveedor(proveedorID);
                cmbProducto.SelectedValue = productoID;

                string unidad = row.Cells["UnidadMedida"].Value?.ToString();

                if (!string.IsNullOrEmpty(unidad))
                {
                    int index = cmbUnidadMedida.FindStringExact(unidad);
                    cmbUnidadMedida.SelectedIndex = index >= 0 ? index : -1;
                }
                else
                {
                    cmbUnidadMedida.SelectedIndex = -1;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al seleccionar el movimiento: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cargandoDesdeGrid = false;
            }
        }


        private void CalcularPrecioTotal(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtCantidad.Text, out decimal cantidad) &&
                decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario))
            {
                txtPrecioTotal.Text = (cantidad * precioUnitario).ToString("0.00");
            }
            else
            {
                txtPrecioTotal.Text = "";
            }
        }

        private void btnEliminar2_Click(object sender, EventArgs e)
        {
            if (movimientoSeleccionadoID <= 0)
            {
                MessageBox.Show("Por favor, seleccione un movimiento válido.", "Movimiento no seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmar = MessageBox.Show("¿Está seguro de que desea eliminar este movimiento?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar == DialogResult.Yes)
            {
                try
                {
                    bool eliminado = inventarioController.EliminarMovimiento(movimientoSeleccionadoID);
                    if (eliminado)
                    {
                        MessageBox.Show("Movimiento eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CargarMovimientos();
                        LimpiarFormulario();
                    }
                    else
                    {
                        MessageBox.Show("No se pudo eliminar el movimiento.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el movimiento: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarMovimientos();
            LimpiarFormulario();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {

            if (movimientoSeleccionadoID <= 0)
            {
                MessageBox.Show("Por favor, seleccione un movimiento para modificar.", "Movimiento no seleccionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) ||
                !decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario) ||
                !decimal.TryParse(txtPrecioTotal.Text, out decimal precioTotal) ||
                cmbTipoMovimiento.SelectedItem == null ||
                cmbProducto.SelectedValue == null)
            {
                MessageBox.Show("Por favor, complete todos los campos correctamente.", "Campos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tipoMovimiento = cmbTipoMovimiento.SelectedItem.ToString();
            int productoID = Convert.ToInt32(cmbProducto.SelectedValue);

            string unidadMedida = cmbUnidadMedida.SelectedItem?.ToString() ?? "N/A";

            try
            {
                using (SqlConnection conn = new SqlConnection("Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;"))
                {
                    conn.Open();
                    string query = @"UPDATE Inventario 
                             SET ProductoID = @ProductoID,
                                 TipoMovimiento = @TipoMovimiento,
                                 Cantidad = @Cantidad,
                                 UsuarioID = @UsuarioID,
                                 PrecioUnitario = @PrecioUnitario,
                                 PrecioTotal = @PrecioTotal,
                                 UnidadMedida = @UnidadMedida
                             WHERE MovimientoID = @MovimientoID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductoID", productoID);
                        cmd.Parameters.AddWithValue("@TipoMovimiento", tipoMovimiento);
                        cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                        cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                        cmd.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                        cmd.Parameters.AddWithValue("@PrecioTotal", precioTotal);
                        cmd.Parameters.AddWithValue("@UnidadMedida", unidadMedida);
                        cmd.Parameters.AddWithValue("@MovimientoID", movimientoSeleccionadoID);

                        int filasAfectadas = cmd.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Movimiento actualizado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CargarMovimientos();
                            LimpiarFormulario();
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el movimiento para actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar el movimiento: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string unidadMedida = cmbUnidadMedida.SelectedItem?.ToString() ?? "N/A";

            if (cmbProducto.SelectedValue == null || cmbTipoMovimiento.SelectedItem == null || !decimal.TryParse(txtCantidad.Text, out decimal cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Por favor, complete correctamente todos los campos antes de agregar.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int productoID = Convert.ToInt32(cmbProducto.SelectedValue);
                string tipoMovimiento = cmbTipoMovimiento.SelectedItem.ToString();
                decimal precioUnitario = decimal.TryParse(txtPrecioUnitario.Text, out decimal pu) ? pu : 0;
                decimal precioTotal = cantidad * precioUnitario;

                var orden = new Orden
                {
                    ProductoID = productoID,
                    UsuarioID = usuarioID
                };

                RegistrarMovimientoInventario(orden, (int)cantidad, precioUnitario, precioTotal);
                MessageBox.Show("Movimiento registrado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarMovimientos();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar movimiento: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbUnidadMedida_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
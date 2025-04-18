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
        private string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";
        private int usuarioID;
        private int movimientoSeleccionadoID = -1;

        private readonly ProveedorController proveedorController = new ProveedorController();
        private readonly ProductoController productoController = new ProductoController();
        private readonly InventarioController inventarioController = new InventarioController();

        public Inventario(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;
            dgvInventario.AllowUserToAddRows = false;

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

        private void CargarProveedores()
        {
            try
            {
                var listaProveedores = proveedorController.ObtenerProveedores();
                cmbProveedor.DataSource = listaProveedores;
                cmbProveedor.DisplayMember = "Nombre";
                cmbProveedor.ValueMember = "ProveedorID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proveedores: " + ex.Message);
            }
        }

        private void CmbProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProveedor.SelectedValue is int proveedorID)
            {
                CargarProductosPorProveedor(proveedorID);
            }
        }

        private void CargarProductosPorProveedor(int proveedorID)
        {
            try
            {
                var productos = productoController.ObtenerProductosPorProveedor(proveedorID);
                cmbProducto.DataSource = productos;
                cmbProducto.DisplayMember = "Nombre";
                cmbProducto.ValueMember = "ProductoID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
        }

        private void CargarMovimientos()
        {
            try
            {
                DataTable tabla = inventarioController.ObtenerMovimientosIndividuales();
                dgvInventario.DataSource = tabla;

                if (tabla.Columns.Contains("MovimientoID"))
                    dgvInventario.Columns["MovimientoID"].HeaderText = "ID";

                dgvInventario.Columns["NombreProducto"].HeaderText = "Producto";
                dgvInventario.Columns["TipoMovimiento"].HeaderText = "Tipo";
                dgvInventario.Columns["Cantidad"].HeaderText = "Cantidad";
                dgvInventario.Columns["PrecioUnitario"].HeaderText = "Precio Unitario";
                dgvInventario.Columns["PrecioTotal"].HeaderText = "Precio Total";
                dgvInventario.Columns["FechaMovimiento"].HeaderText = "Fecha";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar movimientos: " + ex.Message);
            }
        }

        private void dgvInventario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvInventario.Rows[e.RowIndex];

                if (int.TryParse(fila.Cells["MovimientoID"].Value?.ToString(), out movimientoSeleccionadoID))
                {
                    txtMovimientoID.Text = movimientoSeleccionadoID.ToString();
                }

                // Solo si las columnas existen
                if (dgvInventario.Columns.Contains("ProveedorID"))
                {
                    int proveedorID = Convert.ToInt32(fila.Cells["ProveedorID"].Value);
                    cmbProveedor.SelectedValue = proveedorID;
                    CargarProductosPorProveedor(proveedorID);
                }

                if (dgvInventario.Columns.Contains("ProductoID"))
                {
                    int productoID = Convert.ToInt32(fila.Cells["ProductoID"].Value);
                    cmbProducto.SelectedValue = productoID;
                }

                cmbTipoMovimiento.Text = fila.Cells["TipoMovimiento"].Value.ToString();
                txtCantidad.Text = fila.Cells["Cantidad"].Value.ToString();
                txtPrecioUnitario.Text = fila.Cells["PrecioUnitario"].Value.ToString();
                txtPrecioTotal.Text = fila.Cells["PrecioTotal"].Value.ToString();
            }
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0 ||
                !decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario) || precioUnitario <= 0)
            {
                MessageBox.Show("Datos inválidos.");
                return;
            }

            if (cmbTipoMovimiento.Text != "Ingreso")
            {
                MessageBox.Show("Solo se permiten ingresos.");
                return;
            }

            decimal precioTotal = cantidad * precioUnitario;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_RegistrarMovimiento", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductoID", Convert.ToInt32(cmbProducto.SelectedValue));
                    cmd.Parameters.AddWithValue("@TipoMovimiento", cmbTipoMovimiento.Text);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                    cmd.Parameters.AddWithValue("@PrecioTotal", precioTotal);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Movimiento registrado.");
                LimpiarCampos();
                CargarMovimientos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnTestID_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMovimientoID.Text, out int id)) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Inventario WHERE MovimientoID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", id);
                int count = (int)cmd.ExecuteScalar();
                MessageBox.Show("Existe: " + (count > 0));
            }
        }

        private void btnVerDatosID_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMovimientoID.Text, out int id)) return;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Inventario WHERE MovimientoID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string data = $"ProductoID: {reader["ProductoID"]}\nTipo: {reader["TipoMovimiento"]}\nCantidad: {reader["Cantidad"]}";
                    MessageBox.Show(data, "Datos del Movimiento");
                }
                else
                {
                    MessageBox.Show("No encontrado.");
                }
            }
        }

        private void CalcularPrecioTotal(object sender, EventArgs e)
        {
            if (int.TryParse(txtCantidad.Text, out int cantidad) && decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario))
            {
                txtPrecioTotal.Text = (cantidad * precioUnitario).ToString("0.00");
            }
        }

        private void LimpiarCampos()
        {
            txtCantidad.Clear();
            txtPrecioUnitario.Clear();
            txtPrecioTotal.Clear();
            txtMovimientoID.Clear();
            cmbTipoMovimiento.SelectedIndex = -1;
            movimientoSeleccionadoID = -1;
        }

        private void btnEliminar2_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtMovimientoID.Text, out int movimientoID))
            {
                MessageBox.Show("ID no válido.");
                return;
            }

            if (!cmbTipoMovimiento.Text.Equals("Correccion", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Solo se pueden eliminar correcciones.");
                return;
            }

            DialogResult confirm = MessageBox.Show("¿Eliminar movimiento?", "Confirmar", MessageBoxButtons.YesNo);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Inventario WHERE MovimientoID = @ID", conn);
                    cmd.Parameters.AddWithValue("@ID", movimientoID);
                    int result = cmd.ExecuteNonQuery();
                    MessageBox.Show("Eliminados: " + result);
                    CargarMovimientos();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error SQL al eliminar: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            
            if (movimientoSeleccionadoID == -1)
            {
                MessageBox.Show("Seleccione un movimiento primero.");
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0 ||
                !decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario) || precioUnitario <= 0)
            {
                MessageBox.Show("Datos inválidos.");
                return;
            }

            int productoID = Convert.ToInt32(cmbProducto.SelectedValue);
            string tipoMovimiento = cmbTipoMovimiento.Text;
            decimal precioTotal = cantidad * precioUnitario;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                UPDATE Inventario
                SET 
                    ProductoID = @ProductoID,
                    TipoMovimiento = @TipoMovimiento,
                    Cantidad = @Cantidad,
                    PrecioUnitario = @PrecioUnitario,
                    PrecioTotal = @PrecioTotal
                WHERE MovimientoID = @MovimientoID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductoID", productoID);
                    cmd.Parameters.AddWithValue("@TipoMovimiento", tipoMovimiento);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                    cmd.Parameters.AddWithValue("@PrecioTotal", precioTotal);
                    cmd.Parameters.AddWithValue("@MovimientoID", movimientoSeleccionadoID);

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Movimiento actualizado correctamente.");
                        LimpiarCampos();
                        CargarMovimientos();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el movimiento para actualizar.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar movimiento: " + ex.Message);
            }
        }

    }
}


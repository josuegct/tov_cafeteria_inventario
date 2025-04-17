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
            dgvInventario.AllowUserToAddRows = false;
            CargarMovimientos();
            CargarProveedores();
            this.usuarioID = usuarioID;        
            cmbProveedor.SelectedIndexChanged += CmbProveedor_SelectedIndexChanged;
            dgvInventario.CellClick += dgvInventario_CellClick;
            txtPrecioTotal.ReadOnly = true;
            txtCantidad.TextChanged += CalcularPrecioTotal;
            txtPrecioUnitario.TextChanged += CalcularPrecioTotal;
        }

        // ==================== CARGA DE DATOS =========================

        private void CargarProveedores()
        {
            try
            {
                var listaProveedores = proveedorController.ObtenerProveedores();
                if (listaProveedores.Count == 0)
                {
                    MessageBox.Show("⚠️ No hay proveedores disponibles.");
                    return;
                }

                cmbProveedor.DataSource = null;
                cmbProveedor.DataSource = listaProveedores;
                cmbProveedor.DisplayMember = "Nombre";
                cmbProveedor.ValueMember = "ProveedorID";
                cmbProveedor.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar proveedores: " + ex.Message);
            }
        }

        private void CmbProveedor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProveedor.SelectedValue == null || !(cmbProveedor.SelectedValue is int))
                return;

            int proveedorID = (int)cmbProveedor.SelectedValue;
            CargarProductosPorProveedor(proveedorID);
        }

        private void CargarProductosPorProveedor(int proveedorID)
        {
            try
            {
                var productos = productoController.ObtenerProductosPorProveedor(proveedorID);
                cmbProducto.DataSource = null;
                cmbProducto.DataSource = productos;
                cmbProducto.DisplayMember = "Nombre";
                cmbProducto.ValueMember = "ProductoID";
                if (productos.Count == 0)
                    MessageBox.Show("⚠️ Este proveedor no tiene productos.");
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

                // 🔥 Limpia por completo el DataGridView antes de asignar datos nuevos
                dgvInventario.DataSource = null;
                dgvInventario.Rows.Clear();
                dgvInventario.Columns.Clear();

                if (tabla.Rows.Count == 0)
                {
                    MessageBox.Show("⚠️ No hay movimientos en la base de datos.");
                    return;
                }

                dgvInventario.DataSource = tabla;

                if (dgvInventario.Columns.Contains("MovimientoID"))
                    dgvInventario.Columns["MovimientoID"].Visible = false;

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



        // ==================== EVENTOS =========================

        private void dgvInventario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var fila = dgvInventario.Rows[e.RowIndex];
                movimientoSeleccionadoID = Convert.ToInt32(fila.Cells["MovimientoID"].Value);
                cmbProducto.Text = fila.Cells["NombreProducto"].Value.ToString();
                cmbTipoMovimiento.Text = fila.Cells["TipoMovimiento"].Value.ToString();
                txtCantidad.Text = fila.Cells["Cantidad"].Value.ToString();
                txtPrecioUnitario.Text = fila.Cells["PrecioUnitario"].Value.ToString();
                txtPrecioTotal.Text = fila.Cells["PrecioTotal"].Value.ToString();
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarMovimientos();
            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (movimientoSeleccionadoID == -1)
            {
                MessageBox.Show("Seleccione un movimiento para eliminar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmar = MessageBox.Show("¿Desea eliminar el movimiento seleccionado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes) return;

            try
            {
                bool eliminado = inventarioController.EliminarMovimiento(movimientoSeleccionadoID);
                if (eliminado)
                {
                    MessageBox.Show("Movimiento eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarMovimientos();
                }
                else
                {
                    MessageBox.Show("⚠️ No se encontró el movimiento para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (cmbProducto.SelectedValue == null || string.IsNullOrWhiteSpace(cmbTipoMovimiento.Text)
                || string.IsNullOrWhiteSpace(txtCantidad.Text) || string.IsNullOrWhiteSpace(txtPrecioUnitario.Text))
            {
                MessageBox.Show("Complete todos los campos requeridos.", "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCantidad.Text, out int cantidad) || cantidad <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida.", "Cantidad inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario) || precioUnitario <= 0)
            {
                MessageBox.Show("Ingrese un precio unitario válido.", "Precio inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal precioTotal = cantidad * precioUnitario;
            txtPrecioTotal.Text = precioTotal.ToString("0.00");

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd;

                    if (movimientoSeleccionadoID == -1)
                    {
                        cmd = new SqlCommand("sp_RegistrarMovimiento", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                    }
                    else
                    {
                        string updateQuery = "UPDATE Inventario SET ProductoID=@ProductoID, TipoMovimiento=@TipoMovimiento, Cantidad=@Cantidad, UsuarioID=@UsuarioID, PrecioUnitario=@PrecioUnitario, PrecioTotal=@PrecioTotal WHERE MovimientoID=@MovimientoID";
                        cmd = new SqlCommand(updateQuery, conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@MovimientoID", movimientoSeleccionadoID);
                    }

                    cmd.Parameters.AddWithValue("@ProductoID", Convert.ToInt32(cmbProducto.SelectedValue));
                    cmd.Parameters.AddWithValue("@TipoMovimiento", cmbTipoMovimiento.Text);
                    cmd.Parameters.AddWithValue("@Cantidad", cantidad);
                    cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                    cmd.Parameters.AddWithValue("@PrecioUnitario", precioUnitario);
                    cmd.Parameters.AddWithValue("@PrecioTotal", precioTotal);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Movimiento guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarMovimientos();
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error SQL:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== UTILIDADES =========================

        private void LimpiarCampos()
        {
            txtCantidad.Clear();
            txtPrecioUnitario.Clear();
            txtPrecioTotal.Clear();
            cmbTipoMovimiento.SelectedIndex = -1;
            movimientoSeleccionadoID = -1;
        }

        private void CalcularPrecioTotal(object sender, EventArgs e)
        {
            if (int.TryParse(txtCantidad.Text, out int cantidad) && decimal.TryParse(txtPrecioUnitario.Text, out decimal precioUnitario))
            {
                decimal total = cantidad * precioUnitario;
                txtPrecioTotal.Text = total.ToString("0.00");
            }
            else
            {
                txtPrecioTotal.Text = "";
            }
        }
    }
}

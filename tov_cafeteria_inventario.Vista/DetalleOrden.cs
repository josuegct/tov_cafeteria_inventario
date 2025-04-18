using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace tov_cafeteria_inventario.Vista
{
    public partial class DetalleOrden : Form
    {
        private readonly string connectionString = "Server=DESKTOP-M9AEQR3\\SQLEXPRESS;Database=CafeteriaDB;Integrated Security=True;";
        private int ordenID;
        private string productoNombre;
        private int usuarioID;

        public DetalleOrden(int ordenID, string productoNombre)
        {
            InitializeComponent();
            this.ordenID = ordenID;
            this.productoNombre = productoNombre;

            this.StartPosition = FormStartPosition.CenterScreen;

            txtProducto.Text = productoNombre;
            txtProducto.ReadOnly = true;
            txtProducto.Enabled = false;

            this.FormClosed += DetalleOrden_FormClosed;
            usuarioID = ObtenerUsuarioIDDeOrden(ordenID);
            CerrarOrdenesAbierta();

            CargarCabeceraDeOrden();
            CargarDetalles();

            dgvOrdenDetalles.ReadOnly = true;
            dgvOrdenDetalles.AllowUserToAddRows = false;
            dgvOrdenDetalles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void CerrarOrdenesAbierta()
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm is Ordenes)
                {
                    frm.Close();
                    break;
                }
            }
        }

        private void DetalleOrden_FormClosed(object sender, FormClosedEventArgs e)
        {
            var ordenesForm = new Ordenes(usuarioID);
            ordenesForm.MdiParent = this.MdiParent;
            ordenesForm.StartPosition = FormStartPosition.CenterScreen;
            ordenesForm.Show();
            ordenesForm.BringToFront();
        }

        private int ObtenerUsuarioIDDeOrden(int ordenID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT UsuarioID FROM Ordenes WHERE OrdenID = @OrdenID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@OrdenID", ordenID);
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }

        private void CargarCabeceraDeOrden()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT o.OrdenID, o.FechaOrden, pr.Nombre AS NombreProveedor
                        FROM Ordenes o
                        INNER JOIN Proveedores pr ON o.ProveedorID = pr.ProveedorID
                        WHERE o.OrdenID = @OrdenID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrdenID", ordenID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtOrdenID.Text = reader["OrdenID"].ToString();
                                txtFechaOrden.Text = Convert.ToDateTime(reader["FechaOrden"]).ToString("yyyy-MM-dd");
                                txtProveedor.Text = reader["NombreProveedor"].ToString();

                                txtOrdenID.Enabled = false;
                                txtFechaOrden.Enabled = false;
                                txtProveedor.Enabled = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos de cabecera: " + ex.Message);
            }
        }

        private void CargarDetalles()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT 
                            od.OrdenDetalleID,
                            p.Nombre AS Producto,
                            od.Cantidad,
                            od.Precio
                        FROM OrdenDetalles od
                        INNER JOIN Productos p ON od.ProductoID = p.ProductoID
                        WHERE od.OrdenID = @OrdenID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrdenID", ordenID);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dgvOrdenDetalles.DataSource = dt;

                            if (dgvOrdenDetalles.Columns.Contains("OrdenDetalleID"))
                                dgvOrdenDetalles.Columns["OrdenDetalleID"].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar detalles: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDetalles();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCantidad.Text) || string.IsNullOrWhiteSpace(txtPrecio.Text))
                {
                    MessageBox.Show("Complete los campos de cantidad y precio.");
                    return;
                }

                int cantidad = Convert.ToInt32(txtCantidad.Text);
                decimal precioUnitario = Convert.ToDecimal(txtPrecio.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                INSERT INTO OrdenDetalles (OrdenID, ProductoID, Cantidad, Precio)
                VALUES (@OrdenID, @ProductoID, @Cantidad, @Precio)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrdenID", ordenID);
                        command.Parameters.AddWithValue("@ProductoID", GetProductoIDFromName(productoNombre));
                        command.Parameters.AddWithValue("@Cantidad", cantidad);
                        command.Parameters.AddWithValue("@Precio", precioUnitario); // Guardamos precio UNITARIO aquí
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Detalle agregado.");
                CargarDetalles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar: " + ex.Message);
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvOrdenDetalles.SelectedRows.Count > 0)
                {
                    int detalleID = Convert.ToInt32(dgvOrdenDetalles.SelectedRows[0].Cells["OrdenDetalleID"].Value);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM OrdenDetalles WHERE OrdenDetalleID = @OrdenDetalleID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@OrdenDetalleID", detalleID);
                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Detalle eliminado.");
                    CargarDetalles();
                }
                else
                {
                    MessageBox.Show("Seleccione una fila.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvOrdenDetalles.SelectedRows.Count > 0)
                {
                    int detalleID = Convert.ToInt32(dgvOrdenDetalles.SelectedRows[0].Cells["OrdenDetalleID"].Value);
                    int cantidad = Convert.ToInt32(txtCantidad.Text);
                    decimal precioUnitario = Convert.ToDecimal(txtPrecio.Text);
                    decimal precioTotal = cantidad * precioUnitario;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = @"
                            UPDATE OrdenDetalles
                            SET Cantidad = @Cantidad, Precio = @Precio
                            WHERE OrdenDetalleID = @OrdenDetalleID";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Cantidad", cantidad);
                            command.Parameters.AddWithValue("@Precio", precioUnitario);
                            command.Parameters.AddWithValue("@OrdenDetalleID", detalleID);
                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Detalle modificado.");
                    CargarDetalles();
                }
                else
                {
                    MessageBox.Show("Seleccione una fila.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar: " + ex.Message);
            }
        }

        private int GetProductoIDFromName(string productoNombre)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProductoID FROM Productos WHERE Nombre = @Nombre";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", productoNombre);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
        }
    }
}

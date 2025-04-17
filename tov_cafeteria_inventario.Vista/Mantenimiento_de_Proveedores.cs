using System;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;
using System.Linq;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Mantenimiento_de_Proveedores : Form
    {
        private readonly ProductoController productoController = new ProductoController();
        private readonly ProveedorController proveedorController = new ProveedorController();
        private int proveedorIDSeleccionado = -1;
        private readonly int usuarioID;

        public Mantenimiento_de_Proveedores(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;
            this.FormClosed += Mantenimiento_de_Proveedores_FormClosed;
        }

        private void Mantenimiento_de_Proveedores_FormClosed(object sender, FormClosedEventArgs e)
        {
            Mantenimientos menu = new Mantenimientos(usuarioID);
            menu.MdiParent = this.MdiParent;
            menu.StartPosition = FormStartPosition.CenterScreen;
            menu.Show();
        }

        private void Mantenimiento_de_Proveedores_Load(object sender, EventArgs e)
        {
            LimpiarCampos();
            CargarProveedores();
        }

        private void CargarProveedores()
        {
            try
            {
                var proveedores = proveedorController.ObtenerProveedores();
                dataGridViewProveedores.DataSource = proveedores;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los proveedores: " + ex.Message);
            }
        }

        private void btn_agregar_Click(object sender, EventArgs e)
        {
            if (CamposVacios())
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            Proveedor proveedor = new Proveedor
            {
                Nombre = txt_nombre.Text,
                Cedula = txt_cedula.Text,
                Telefono = txt_telefono.Text,
                Correo = txt_correo.Text,
                Direccion = txt_direccion.Text
            };

            try
            {
                proveedorController.AgregarProveedorConProductos(proveedor, txtProductList.Lines.ToList());
                MessageBox.Show("Proveedor y productos agregados correctamente.");
                LimpiarCampos();
                CargarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar proveedor: " + ex.Message);
            }
        }

        private void btn_modificar_Click(object sender, EventArgs e)
        {
            if (proveedorIDSeleccionado == -1)
            {
                MessageBox.Show("Seleccione un proveedor a modificar.");
                return;
            }

            if (CamposVacios())
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            Proveedor proveedor = new Proveedor
            {
                ProveedorID = proveedorIDSeleccionado,
                Nombre = txt_nombre.Text,
                Cedula = txt_cedula.Text,
                Telefono = txt_telefono.Text,
                Correo = txt_correo.Text,
                Direccion = txt_direccion.Text
            };

            try
            {
                proveedorController.ModificarProveedorConProductos(proveedor, txtProductList.Lines.ToList());
                MessageBox.Show("Proveedor y productos modificados correctamente.");
                LimpiarCampos();
                CargarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar proveedor: " + ex.Message);
            }
        }

        private void btn_eliminar_Click(object sender, EventArgs e)
        {
            if (proveedorIDSeleccionado == -1)
            {
                MessageBox.Show("Seleccione un proveedor a eliminar.");
                return;
            }

            DialogResult result = MessageBox.Show("¿Está seguro que desea eliminar este proveedor?", "Confirmar", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) return;

            try
            {
                proveedorController.EliminarProveedor(proveedorIDSeleccionado);
                MessageBox.Show("Proveedor eliminado correctamente.");
                LimpiarCampos();
                CargarProveedores();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar proveedor: " + ex.Message);
            }
        }

        private void btn_limpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txt_nombre.Clear();
            txt_cedula.Clear();
            txt_telefono.Clear();
            txt_correo.Clear();
            txt_direccion.Clear();
            txtProductList.Clear();
            proveedorIDSeleccionado = -1;
        }

        private bool CamposVacios()
        {
            return string.IsNullOrWhiteSpace(txt_nombre.Text) ||
                   string.IsNullOrWhiteSpace(txt_cedula.Text) ||
                   string.IsNullOrWhiteSpace(txt_telefono.Text) ||
                   string.IsNullOrWhiteSpace(txt_correo.Text) ||
                   string.IsNullOrWhiteSpace(txt_direccion.Text);
        }

        private void dataGridViewProveedores_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewProveedores.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridViewProveedores.SelectedRows[0];
                proveedorIDSeleccionado = Convert.ToInt32(row.Cells["ProveedorID"].Value);
                txt_nombre.Text = row.Cells["Nombre"].Value.ToString();
                txt_cedula.Text = row.Cells["Cedula"].Value.ToString();
                txt_telefono.Text = row.Cells["Telefono"].Value.ToString();
                txt_correo.Text = row.Cells["Correo"].Value.ToString();
                txt_direccion.Text = row.Cells["Direccion"].Value.ToString();

                var productos = productoController.ObtenerProductosPorProveedor(proveedorIDSeleccionado);
                if (productos.Count > 0)
                {
                    txtProductList.Text = string.Join(Environment.NewLine, productos.Select(p => p.Nombre));
                }
                else
                {
                    txtProductList.Text = string.Empty;
                }
            }
        }
    }
}

using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Bitacora : Form
    {
        private readonly BitacoraController bitacoraController = new BitacoraController();
        private readonly UsuarioController usuarioController = new UsuarioController();
        private int usuarioID;

        public Bitacora(int usuarioID)
        {
            InitializeComponent();
            this.usuarioID = usuarioID;

            int roleID = usuarioController.ObtenerRoleID(usuarioID);
            if (roleID != 1)
            {
                MessageBox.Show("No tiene permisos para acceder a la Bitácora.", "Acceso denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
            }
        }

        private void Bitacora_Load(object sender, EventArgs e)
        {
            CargarBitacora();
        }

        private void btn_reporteIngresoSalida_Click(object sender, EventArgs e)
        {
            var form = new Reporte_de_Ingresos_y_Salidas_al_Sistema(usuarioID);
            form.MdiParent = this.MdiParent;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
            this.Close();
        }

        private void btn_movimientosSistema_Click(object sender, EventArgs e)
        {
            var form = new Reporte_de_Movimientos_en_el_Sistema(usuarioID);
            form.MdiParent = this.MdiParent;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.Show();
            this.Close();
        }

        private void CargarBitacora(string filtro = "")
        {
            try
            {
                List<BitacoraRegistro> registros = bitacoraController.ObtenerBitacora(filtro);

                DataTable table = new DataTable();
                table.Columns.Add("BitacoraID", typeof(int));
                table.Columns.Add("UsuarioID", typeof(int));
                table.Columns.Add("FechaRegistro", typeof(DateTime));
                table.Columns.Add("Accion", typeof(string));

                foreach (var registro in registros)
                {
                    table.Rows.Add(registro.BitacoraID, registro.UsuarioID, registro.FechaRegistro, registro.Accion);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la bitácora: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

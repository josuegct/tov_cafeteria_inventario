﻿using System;
using System.Linq;
using System.Windows.Forms;
using tov_cafeteria_inventario.Controlador;
using tov_cafeteria_inventario.Modelo;
using tov_cafeteria_inventario.Vista;

namespace tov_cafeteria_inventario.Vista
{
    public partial class Pantalla_Principal : Form
    {
        private int usuarioID;
        private int roleID;
        private PantallaPrincipalController controller;
        private Ordenes ordenesForm;
        private Inventario inventarioForm;
        private Mantenimientos mantenimientosForm;
        private Reportes reportesForm;
        private Bitacora bitacoraForm;
        private AcercaDe acercaDeForm;
        private Ayuda ayudaForm;

        
        public Pantalla_Principal(int usuarioID)
        {
            InitializeComponent();
            this.IsMdiContainer = true;
            this.usuarioID = usuarioID;
            controller = new PantallaPrincipalController();
            this.roleID = controller.ObtenerRoleID(usuarioID);
        }

        private void ordenesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.ordenesForm = new Ordenes(this.usuarioID);
            this.ordenesForm.MdiParent = this;
            this.ordenesForm.Show();
        }

        private void Pantalla_Principal_Load(object sender, EventArgs e)
        {

        }

        private void inventarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.inventarioForm = new Inventario(this.usuarioID);
            this.inventarioForm.MdiParent = this;
            this.inventarioForm.StartPosition = FormStartPosition.CenterScreen; // <- Esta línea es clave
            this.inventarioForm.Show();
        }

        private void mantenimientoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.mantenimientosForm = new Mantenimientos(this.usuarioID);
            this.mantenimientosForm.MdiParent = this;
            this.mantenimientosForm.Show();
        }

        private void reportesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.reportesForm = new Reportes(usuarioID);
            this.reportesForm.MdiParent = this; // Pantalla_Principal es el contenedor MDI
            this.reportesForm.Show();
        }

        private void bitacorasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.bitacoraForm = new Bitacora(this.usuarioID);
            this.bitacoraForm.MdiParent = this;
            this.bitacoraForm.Show();
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.acercaDeForm = new AcercaDe();
            this.acercaDeForm.MdiParent = this;
            this.acercaDeForm.Show();
        }

        private void ayudaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ayudaForm = new Ayuda();
            this.ayudaForm.MdiParent = this;
            this.ayudaForm.Show();
        }
    }

    internal class InventarioForm : Inventario
    {
        public InventarioForm(int usuarioID) : base(usuarioID)
        {
        }
    }
}
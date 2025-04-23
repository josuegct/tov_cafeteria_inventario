namespace tov_cafeteria_inventario.Modelo
{
    public class UsuarioSistema
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public string Correo { get; set; }
        public int RoleID { get; set; }
        public string RolNombre { get; set; }
        public string Estado { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}

namespace BE
{
    public class PermisoSimple : Permiso
    {
        public PermisoSimple(uint id, string nombre) : base(id, nombre) { }

        public override bool ValidarPermiso(string nombrePermiso)
        {
            return Nombre.Equals(nombrePermiso, StringComparison.OrdinalIgnoreCase);
        }
    }
}

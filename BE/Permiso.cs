namespace BE
{
    public abstract class Permiso
    {
        public uint ID { get; private set; }
        public string Nombre { get; private set; }

        protected Permiso(uint id, string nombre)
        {
            ID = id;
            Nombre = nombre;
        }

        public abstract bool ValidarPermiso(string nombrePermiso);
    }
}

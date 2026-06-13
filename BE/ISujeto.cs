using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public interface ISujeto
    {
        public abstract void Attach(IObserver observer);
        public abstract void Detach (IObserver observer);
        public abstract void Notificar(Usuario usuarioInvolucrado, string accion);
    }
}

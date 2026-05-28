using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public interface IObserver
    {
        public abstract void Update(string username, string action);
    }
}

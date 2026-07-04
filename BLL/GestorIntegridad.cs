using BE;
using DAL;
using servicios;
using System.Collections.Generic;

namespace BLL
{
    public static class GestorIntegridad
    {
        public static List<Usuario> VerificarIntegridadDVH()
        {
            List<Usuario> usuariosCorruptos = new List<Usuario>();
            List<Usuario> usuarios = RepositorioUsuarios.GetInstance.ObtenerListadoTotalUsuarios();

            foreach (var u in usuarios)
            {
                if (u.DVH != IntegridadService.CalcularDVH(u))
                {
                    usuariosCorruptos.Add(u);
                }
            }
            return usuariosCorruptos;
        }
        /*
         1. Factory Method (Patrón Creacional)
Objetivo: Delegar la creación de instancias a una clase fábrica, para que tu sistema dependa de abstracciones (interfaces) y no de clases concretas.

C#


// 1. La abstracción (Lo que el sistema conoce)
public interface INotificador
{
    void EnviarAlerta(string mensaje);
}

// 2. Los productos concretos
public class NotificadorEmail : INotificador
{
    public void EnviarAlerta(string mensaje) => Console.WriteLine($"Enviando Email: {mensaje}");
}

public class NotificadorSMS : INotificador
{
    public void EnviarAlerta(string mensaje) => Console.WriteLine($"Enviando SMS: {mensaje}");
}

// 3. El Factory Method (El creador)
public static class NotificadorFactory
{
    public static INotificador CrearNotificador(string tipo)
    {
        if (tipo == "EMAIL") return new NotificadorEmail();
        if (tipo == "SMS") return new NotificadorSMS();
        
        throw new ArgumentException("Tipo de notificador no válido");
    }
}
2. Adapter (Patrón Estructural)
Objetivo: Permitir que dos interfaces incompatibles trabajen juntas. En este caso, tu sistema espera un formato específico para consultar el stock a los proveedores, pero una API externa usa métodos y nombres distintos.

C#


// 1. El Target (La interfaz que TU sistema espera usar)
public interface IProveedorSistema
{
    int ConsultarStockBebidas();
}

// 2. El Adaptee (La clase externa, legacy o de terceros que no puedes modificar)
public class ApiExternaCerveceria
{
    public int GetBeerInventoryData() 
    {
        return 500; // Lógica compleja externa
    }
}

// 3. El Adapter (El traductor)
public class AdaptadorCerveceria : IProveedorSistema
{
    private readonly ApiExternaCerveceria _apiExterna;

    public AdaptadorCerveceria()
    {
        _apiExterna = new ApiExternaCerveceria();
    }

    public int ConsultarStockBebidas()
    {
        // Traduce el llamado de tu sistema al llamado que la API entiende
        return _apiExterna.GetBeerInventoryData(); 
    }
}
3. Decorator (Patrón Estructural)
Objetivo: Agregar responsabilidades o comportamientos a un objeto de forma dinámica sin modificar su código original, envolviéndolo en clases decoradoras.

C#


// 1. El componente base
public interface IReporteDistribuidora
{
    string Generar();
}

// 2. El componente concreto (La funcionalidad básica)
public class ReporteBasico : IReporteDistribuidora
{
    public string Generar() => "Reporte de ventas de bebidas...";
}

// 3. El Decorator base
public abstract class ReporteDecorator : IReporteDistribuidora
{
    protected IReporteDistribuidora _reporteEnvoltura;

    public ReporteDecorator(IReporteDistribuidora reporte)
    {
        _reporteEnvoltura = reporte;
    }

    public virtual string Generar() => _reporteEnvoltura.Generar();
}

// 4. El Decorator concreto (Agrega nueva funcionalidad)
public class ReporteConFirmaDecorator : ReporteDecorator
{
    public ReporteConFirmaDecorator(IReporteDistribuidora reporte) : base(reporte) { }

    public override string Generar()
    {
        string reporteOriginal = base.Generar();
        return reporteOriginal + "\n-- FIRMADO POR GERENCIA --";
    }
}
4. Memento (Patrón de Comportamiento)
Objetivo: Capturar y externalizar el estado interno de un objeto para que pueda ser restaurado más tarde, sin violar su encapsulamiento (ideal para un botón de "Deshacer").

C#


// 1. El Memento (La cápsula del tiempo, propiedades de solo lectura)
public class MementoProducto
{
    public decimal PrecioGuardado { get; private set; }
    
    public MementoProducto(decimal precio)
    {
        PrecioGuardado = precio;
    }
}

// 2. El Originator (El objeto del negocio que tiene estado)
public class ProductoBebida
{
    public string Nombre { get; set; }
    public decimal Precio { get; set; }

    public MementoProducto GuardarEstado()
    {
        return new MementoProducto(Precio);
    }

    public void RestaurarEstado(MementoProducto memento)
    {
        Precio = memento.PrecioGuardado;
    }
}

// 3. El Caretaker (El gestor que guarda el historial de mementos)
public class HistorialPrecios
{
    private Stack<MementoProducto> _historial = new Stack<MementoProducto>();

    public void Guardar(ProductoBebida producto)
    {
        _historial.Push(producto.GuardarEstado());
    }

    public void Deshacer(ProductoBebida producto)
    {
        if (_historial.Count > 0)
        {
            producto.RestaurarEstado(_historial.Pop());
        }
    }
}
         
         
         
         */

        public static bool VerificarIntegridadDVV()
        {
            List<Usuario> usuarios = RepositorioUsuarios.GetInstance.ObtenerListadoTotalUsuarios();

            string dvvCalculado = IntegridadService.CalcularDVV(usuarios);
            string dvvAlmacenado = RepositorioIntegridad.GetInstance.ObtenerDVV("Usuario");

            if (string.IsNullOrEmpty(dvvAlmacenado)) return true;

            return dvvCalculado == dvvAlmacenado;
        }
    }
}
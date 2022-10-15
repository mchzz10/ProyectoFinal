using Mauro_Chazarreta.Models;
using Mauro_Chazarreta.ViewModels;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        // Menu para elegir la opcion que se va a ejecutar.
        
        Console.WriteLine("Seleccione una opcion para ejecutar");
        Console.WriteLine("Opcion A: Traer Usuario\r\nOpcion B: Traer Producto\r\nOpcion C: Traer Productos Vendidos\r\nOpcion D: Traer Ventas\r\nOpcion E: Iniciar Sesion\r\nOpcion F: SALIR");
        Console.WriteLine("Eleccion: ");
        string eleccion = Console.ReadLine();

        while (eleccion.ToUpper() != "F")
        {

            switch (eleccion.ToUpper())
            {
                case "A":
                    Console.WriteLine("Ingrese el nombre de Usuario:");
                    var nombre = Console.ReadLine();
                    var resultado = GetUsuarioByNombre(nombre);

                    Console.WriteLine("Id: " + resultado.id.ToString());
                    Console.WriteLine("Nombre: " + resultado.nombre);
                    Console.WriteLine("Apellido: " + resultado.apellido);
                    Console.WriteLine("Nombre de Usuario: " + resultado.nombreUsuario);
                    Console.WriteLine($"Contraseña: {resultado.contrasena}");
                    Console.WriteLine("Mail: " + resultado.mail);
                    Console.WriteLine();
                    break;

                case "B":
                    Console.WriteLine("Ingrese el Id de Usuario:");
                    var id = Console.ReadLine();
                    GetProductosByUsuario(Convert.ToInt32(id));
                    break;

                case "C":
                    Console.WriteLine("Ingrese el Id de Usuario del cual quiere saber los producctos vendidos");
                    var idu = Console.ReadLine();
                    GetProductosVendidosByIdUsuario(Convert.ToInt32(idu));
                    break;

                case "D":
                    Console.WriteLine("Ingrese el Id de Usuario del que quiere saber sus ventas:");
                    var idv = Console.ReadLine();
                    GetVentasByIdUsuario(Convert.ToInt32(idv));
                    break;

                case "E":
                    Console.WriteLine("Ingrese su nombre de usuario:");
                    var nombreUsuario = Console.ReadLine();
                    Console.WriteLine("Ingrese su nombre de contraseña:");
                    var contrasena = Console.ReadLine();
                    GetLoginByUsuario(nombreUsuario, contrasena);
                    break;



                default:
                    Console.WriteLine("*****OPCION INVALIDA INTENTE NUEVAMENTE*****");
                    break;
            }
           
            Console.WriteLine("Seleccione una opcion para ejecutar");
            Console.WriteLine("Opcion A: Traer Usuario\r\nOpcion B: Traer Producto\r\nOpcion C: Traer Productos Vendidos\r\nOpcion D: Traer Ventas\r\nOpcion E: Iniciar Sesion\r\nOpcion F: SALIR");
            Console.WriteLine("Eleccion: ");

            eleccion = Console.ReadLine();
            Console.Clear();
        }
    }

    // A_ Metodo para traer usuario a travez del nombre.

    public static Usuario GetUsuarioByNombre(string nombre)
    {
        SqlConnectionStringBuilder connectionStringBuilder = new();
        connectionStringBuilder.DataSource = "DESKTOP-O2006PP";
        connectionStringBuilder.InitialCatalog = "SistemaGestion";
        connectionStringBuilder.IntegratedSecurity = true;
        var cs = connectionStringBuilder.ConnectionString;

        var usuario = new Usuario();

        using (SqlConnection connection = new SqlConnection(cs))
        {
            connection.Open();

            string sql = $"Select * from usuario where NombreUsuario = '{nombre}'";

            SqlCommand cmd = new SqlCommand(sql, connection);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                usuario.id = Convert.ToInt32(reader["Id"]);
                usuario.nombre = reader["Nombre"].ToString();
                usuario.apellido = reader["Apellido"].ToString();
                usuario.nombreUsuario = reader["NombreUsuario"].ToString();
                usuario.contrasena = reader["Contraseña"].ToString();
                usuario.mail = reader["Mail"].ToString();


            }
            reader.Close();
        }
        return usuario;
    }

    // B_ Metodo para traer los productos cargados por un usuario en particular.

    public static void GetProductosByUsuario(int id)
    {
        SqlConnectionStringBuilder connectionStringBuilder = new();
        connectionStringBuilder.DataSource = "DESKTOP-O2006PP";
        connectionStringBuilder.InitialCatalog = "SistemaGestion";
        connectionStringBuilder.IntegratedSecurity = true;
        var cs = connectionStringBuilder.ConnectionString;

        var productos = new List<Producto>();

        using (SqlConnection connection = new SqlConnection(cs))
        {
            connection.Open();

            string sql = $"Select * from Producto where IdUsuario = {id}";

            SqlCommand cmd = new SqlCommand(sql, connection);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var producto = new Producto();
                producto.id = Convert.ToInt32(reader.GetValue(0));
                producto.descripciones = reader.GetValue(1).ToString();
                producto.costo = Convert.ToDouble(reader.GetValue(2));
                producto.precioVenta = Convert.ToDouble(reader.GetValue(3));
                producto.stock = Convert.ToInt32(reader.GetValue(4));
                producto.idUsuario = reader.GetValue(5).ToString();

                productos.Add(producto);

            }

            reader.Close();
        }

        Console.WriteLine("-----Productos-----");
        foreach (var produc in productos)
        {
            Console.WriteLine("*Id = " + produc.id);
            Console.WriteLine("*descripciones = " + produc.descripciones);
            Console.WriteLine("*costo = " + produc.costo);
            Console.WriteLine("*precioVenta = " + produc.precioVenta);
            Console.WriteLine("*stock = " + produc.stock);
            Console.WriteLine("*idUsuario = " + produc.idUsuario);

            Console.WriteLine("________________");
        }

    }

    // C_ Metodo para traer los productos vendidos por un usuario.
    public static void GetProductosVendidosByIdUsuario(int id)
    {
        SqlConnectionStringBuilder connectionStringBuilder = new();
        connectionStringBuilder.DataSource = "DESKTOP-O2006PP";
        connectionStringBuilder.InitialCatalog = "SistemaGestion";
        connectionStringBuilder.IntegratedSecurity = true;
        var cs = connectionStringBuilder.ConnectionString;

        var productos = new List<ProductoDto>();

        using (SqlConnection connection = new SqlConnection(cs))
        {
            connection.Open();

            string sql = $"select p.Id,p.Descripciones,v.Stock from Producto p inner join ProductoVendido v on v.IdProducto = p.Id where v.IdVenta = {id}";

            SqlCommand cmd = new SqlCommand(sql, connection);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var producto = new ProductoDto();
                producto.Id = Convert.ToInt32(reader.GetValue(0));
                producto.Descripciones = reader.GetValue(1).ToString();
                producto.Stock = Convert.ToInt32(reader.GetValue(2));

                productos.Add(producto);

            }

            reader.Close();
        }
        Console.WriteLine("-----Productos Vendidos por el usuario-----");
        foreach (var produc in productos)
        {
            Console.WriteLine("*Id = " + produc.Id);
            Console.WriteLine("*Descripciones = " + produc.Descripciones);
            Console.WriteLine("*Stock = " + produc.Stock);

            Console.WriteLine("________________");
        }
    }

    // D_ Metodo para traer ventas filtrando por la Id de un usuario.

    public static void GetVentasByIdUsuario(int id)
    {
        SqlConnectionStringBuilder connectionStringBuilder = new();
        connectionStringBuilder.DataSource = "DESKTOP-O2006PP";
        connectionStringBuilder.InitialCatalog = "SistemaGestion";
        connectionStringBuilder.IntegratedSecurity = true;
        var cs = connectionStringBuilder.ConnectionString;

        var ventas = new List<Venta>();

        using (SqlConnection connection = new SqlConnection(cs))
        {
            connection.Open();

            string sql = $"Select * from Venta where IdUsuario = {id}";

            SqlCommand cmd = new SqlCommand(sql, connection);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var venta = new Venta();
                venta.id = Convert.ToInt32(reader.GetValue(0));
                venta.comentarios = reader.GetValue(1).ToString();
                venta.idUsuario = Convert.ToInt32(reader.GetValue(2));

                ventas.Add(venta);

            }

            reader.Close();
        }

        Console.WriteLine("-----VENTAS-----");
        foreach (var vent in ventas)
        {
            Console.WriteLine("*Id = " + vent.id);
            Console.WriteLine("*descripciones = " + vent.comentarios);
            Console.WriteLine("*idUsuario = " + vent.idUsuario);
            Console.WriteLine("________________");
        }

    }

    // E_ Metodo para Ininio de Sesion.

    public static Usuario GetLoginByUsuario(string nombreUsuario, string contrasena)
    {
        SqlConnectionStringBuilder connectionStringBuilder = new();
        connectionStringBuilder.DataSource = "DESKTOP-O2006PP";
        connectionStringBuilder.InitialCatalog = "SistemaGestion";
        connectionStringBuilder.IntegratedSecurity = true;
        var cs = connectionStringBuilder.ConnectionString;

        var usuario = new Usuario();

        using (SqlConnection connection = new SqlConnection(cs))
        {
            connection.Open();

            string sql = $"Select * from usuario where NombreUsuario = '{nombreUsuario}' and Contraseña = '{contrasena}'";

            SqlCommand cmd = new SqlCommand(sql, connection);

            var reader = cmd.ExecuteReader();
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("***Ha Iniciado Sesion Correctamente!!!***");
            Console.WriteLine();

            reader.Close();
        }
        return usuario;
    }
}
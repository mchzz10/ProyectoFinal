using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mauro_Chazarreta.Models
{
    public class Producto
    {
        public int id { get; set; }
        public string descripciones { get; set; }
        public double costo { get; set; }
        public double precioVenta { get; set; }
        public int stock { get; set; }
        public string idUsuario { get; set; }

        public Producto()
        {
            id = 0;
            descripciones = string.Empty;
            costo = 0;
            precioVenta = 0;
            stock = 0;
            idUsuario = string.Empty;
        }
    }
}

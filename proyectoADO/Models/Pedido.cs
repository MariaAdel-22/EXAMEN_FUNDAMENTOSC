using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoADO.Models
{
    public class Pedido
    {
        public string Codigo_Pedido { get; set; }
        public string Codigo_Cliente { get; set; }
        public DateTime Fecha_entrega { get; set; }
        public string FormaEnvio { get; set; }
        public int Importe { get; set; }
    }
}

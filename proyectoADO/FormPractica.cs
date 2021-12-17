using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using proyectoADO.Models;
using proyectoADO.Context;

namespace ProyectoADO
{
    public partial class FormPractica : Form
    {
        PracticaContext context;

        public FormPractica()
        {
            InitializeComponent();
            this.context = new PracticaContext();
            this.CargarClientes();
        }

        private void CargarClientes() {

            List<Cliente> Clientes = this.context.CargarClientes();

            foreach (Cliente cl in Clientes) {

                this.cmbclientes.Items.Add(cl.Empresa);
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Nombre = this.cmbclientes.SelectedItem.ToString();

            List<Object> Datos = new List<object>();

            Datos = this.context.BuscarCliente(Nombre);

            foreach (Object dat in Datos) {

                if (dat is Cliente) {

                    Cliente cl = (Cliente)dat;

                    this.txtcargo.Text = cl.Cargo;
                    this.txtciudad.Text = cl.ciudad;
                    this.txtcontacto.Text = cl.Contacto;
                    this.txtempresa.Text = cl.Empresa;
                    this.txttelefono.Text = cl.Telefono.ToString();
                }

                else if (dat is Pedido) {

                    Pedido ped = (Pedido)dat;

                    this.lstpedidos.Items.Add(ped.Codigo_Pedido);

                }
            }

        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            Pedido ped = new Pedido();

            ped.Codigo_Cliente = this.context.BuscarCodigoCliente(this.cmbclientes.SelectedItem.ToString());
            ped.Codigo_Pedido = this.txtcodigopedido.Text;
            ped.Fecha_entrega = DateTime.Parse(this.txtfechaentrega.Text);
            ped.FormaEnvio = this.txtformaenvio.Text;
            ped.Importe = int.Parse(this.txtimporte.Text);

            this.context.InsertarPedido(ped);
        }

        private void btnmodificarcliente_Click(object sender, EventArgs e)
        {
  
            Cliente cl = new Cliente();

             cl.Cargo= this.txtcargo.Text;
             cl.ciudad= this.txtciudad.Text;
             cl.Contacto= this.txtcontacto.Text;
             cl.Empresa= this.txtempresa.Text;
             cl.Telefono = int.Parse(this.txttelefono.Text);
             cl.Codigo_Cliente= this.context.BuscarCodigoCliente(this.cmbclientes.SelectedItem.ToString());

            this.context.ModificarCliente(cl);
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cod=this.lstpedidos.SelectedItem.ToString();

            Pedido ped = this.context.BuscarPedido(cod);

            this.txtcodigopedido.Text = ped.Codigo_Pedido;
            this.txtfechaentrega.Text = ped.Fecha_entrega.ToString();
            this.txtformaenvio.Text = ped.FormaEnvio;
            this.txtimporte.Text = ped.Importe.ToString();
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            string cod = this.lstpedidos.SelectedItem.ToString();

            this.context.EliminarPedido(cod);
        }
    }
}

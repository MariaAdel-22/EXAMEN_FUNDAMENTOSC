using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using proyectoADO.Models;
using Microsoft.Extensions.Configuration;

#region PROCEDIMIENTOS

/*


 CREATE PROCEDURE CARGAR_CLIENTES
AS
	SELECT * FROM clientes GROUP BY CodigoCliente, Empresa,Contacto,Cargo,Ciudad,Telefono
GO
 */

/*

CREATE PROCEDURE BUSCAR_CLIENTE(@NOMBRE NVARCHAR(200))
AS
DECLARE @COD_CLI NVARCHAR(200)
	
	SELECT a.Empresa,a.Contacto,a.Cargo,a.Ciudad,a.Telefono,b.CodigoPedido,b.FechaEntrega,b.FormaEnvio,b.Importe FROM clientes AS a INNER JOIN pedidos AS b ON a.CodigoCliente=b.CodigoCliente 
WHERE a.Empresa=@NOMBRE
GROUP BY a.CodigoCliente,b.CodigoCliente,a.Empresa,a.Contacto,a.Cargo,a.Ciudad,a.Telefono,b.CodigoPedido,b.FechaEntrega,b.FormaEnvio,b.Importe 

	
GO

 */

/*
 
CREATE PROCEDURE BUSCAR_CODIGO_CLIENTE(@NOMBRE NVARCHAR(200))
AS

SELECT CodigoCliente from clientes where Empresa=@NOMBRE
GO
 */
#endregion

namespace proyectoADO.Context
{
    public class PracticaContext
    {
        SqlConnection connect;
        SqlCommand com;
        SqlDataReader reader;

        public PracticaContext() {

            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("config.json");
            IConfigurationRoot config = builder.Build();
            String cadena = config["Practica"];

            this.connect = new SqlConnection(cadena);
            this.com = new SqlCommand();
            this.com.Connection = this.connect;
        }

        public List<Cliente> CargarClientes() {

            List<Cliente> clientes = new List<Cliente>();

            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = "CARGAR_CLIENTES";

            this.connect.Open();
            this.reader = this.com.ExecuteReader();

            while (this.reader.Read()) {

                Cliente cl = new Cliente();

                cl.Cargo = this.reader["Cargo"].ToString();
                cl.ciudad = this.reader["Ciudad"].ToString();
                cl.Codigo_Cliente = this.reader["CodigoCliente"].ToString();
                cl.Contacto = this.reader["Contacto"].ToString();
                cl.Empresa = this.reader["Empresa"].ToString();
                cl.Telefono = int.Parse(this.reader["Telefono"].ToString());

                clientes.Add(cl);
            }

            this.reader.Close();
            this.connect.Close();

            return clientes;
        }

        public List<Object> BuscarCliente(string nombre) {

            List<Object> datos = new List<Object>();

            Cliente cl = new Cliente();
            Pedido pl = new Pedido();

            this.com.Parameters.AddWithValue("@NOMBRE", nombre);
            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = "BUSCAR_CLIENTE";

            this.connect.Open();
            this.reader = this.com.ExecuteReader();

            while (this.reader.Read())
            {
                cl.Cargo = this.reader["Cargo"].ToString();
                cl.ciudad = this.reader["Ciudad"].ToString();
                cl.Contacto = this.reader["Contacto"].ToString();
                cl.Empresa = this.reader["Empresa"].ToString();
                cl.Telefono = int.Parse(this.reader["Telefono"].ToString());

                pl.Codigo_Pedido = this.reader["CodigoPedido"].ToString();
                pl.Fecha_entrega = DateTime.Parse(this.reader["FechaEntrega"].ToString());
                pl.FormaEnvio = this.reader["FormaEnvio"].ToString();
                pl.Importe = int.Parse(this.reader["Importe"].ToString());

                datos.Add(cl);
                datos.Add(pl);
            }

            this.reader.Close();
            this.connect.Close();
            this.com.Parameters.Clear();

            return datos;

        }

        public String BuscarCodigoCliente(string nombre) {

            String codigo="";

            this.com.Parameters.AddWithValue("@NOMBRE", nombre);
            this.com.CommandType = System.Data.CommandType.StoredProcedure;
            this.com.CommandText = "BUSCAR_CODIGO_CLIENTE";

            this.connect.Open();
            this.reader = this.com.ExecuteReader();

            while (this.reader.Read())
            {
    
                codigo = this.reader["CodigoCliente"].ToString();
               
            }

            this.reader.Close();
            this.connect.Close();
            this.com.Parameters.Clear();

            return codigo;
        }

        public void InsertarPedido(Pedido ped) {

            this.com.Parameters.AddWithValue("@CODIGO_CLIENTE",ped.Codigo_Cliente);
            this.com.Parameters.AddWithValue("@CODIGO_PEDIDO",ped.Codigo_Pedido);
            this.com.Parameters.AddWithValue("@FECHA_ENTREGA",ped.Fecha_entrega);
            this.com.Parameters.AddWithValue("@FORMAENVIO",ped.FormaEnvio);
            this.com.Parameters.AddWithValue("@IMPORTE",ped.Importe);

            string sql = "INSERT INTO PEDIDOS VALUES(@CODIGO_CLIENTE,@CODIGO_PEDIDO,@FECHA_ENTREGA,@FORMAENVIO,@IMPORTE)";
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;

            this.connect.Open();
            this.com.ExecuteNonQuery();
            this.com.Parameters.Clear();
        }

        public void ModificarCliente(Cliente cl) {

            this.com.Parameters.AddWithValue("@CARGO",cl.Cargo);
            this.com.Parameters.AddWithValue("@CIUDAD",cl.ciudad);
            this.com.Parameters.AddWithValue("@CONTACTO",cl.Contacto);
            this.com.Parameters.AddWithValue("@EMPRESA",cl.Empresa);
            this.com.Parameters.AddWithValue("@TELEFONO",cl.Telefono);
            this.com.Parameters.AddWithValue("@CODIGO_CLIENTE", cl.Codigo_Cliente);

            string sql = "UPDATE CLIENTES SET Cargo=@CARGO,Ciudad=@CIUDAD,Contacto=@CONTACTO,Empresa=@EMPRESA,Telefono=@TELEFONO WHERE CodigoCliente=@CODIGO_CLIENTE";

            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;

            this.connect.Open();
            this.com.ExecuteNonQuery();
            this.com.Parameters.Clear();
        }

        public Pedido BuscarPedido(string codigoPedido) {

            Pedido ped = new Pedido();

            this.com.Parameters.AddWithValue("@CODIGO", codigoPedido);

            string sql = "SELECT * FROM pedidos WHERE CodigoPedido=@CODIGO";
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;

            this.connect.Open();
            this.reader = this.com.ExecuteReader();

            while (this.reader.Read()) {

                ped.Codigo_Cliente = this.reader["CodigoCliente"].ToString();
                ped.Codigo_Pedido = this.reader["CodigoPedido"].ToString();
                ped.Fecha_entrega = DateTime.Parse(this.reader["FechaEntrega"].ToString());
                ped.FormaEnvio = this.reader["FormaEnvio"].ToString();
                ped.Importe = int.Parse(this.reader["Importe"].ToString());
            
            }

            this.reader.Close();
            this.connect.Close();
            this.com.Parameters.Clear();

            return ped;

        }

        public void EliminarPedido(string cod) {

            this.com.Parameters.AddWithValue("@CODIGO", cod);

            string sql = "DELETE FROM pedidos WHERE CodigoPedido=@CODIGO";
            this.com.CommandType = System.Data.CommandType.Text;
            this.com.CommandText = sql;

            this.connect.Open();
            this.com.ExecuteNonQuery();

            this.connect.Close();
            this.com.Parameters.Clear();
        }
    }
}

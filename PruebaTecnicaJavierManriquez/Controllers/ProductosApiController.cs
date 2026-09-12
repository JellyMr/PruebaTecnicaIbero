using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PruebaTecnicaJavierManriquez.Models;
using System.Data;

namespace PruebaTecnicaJavierManriquez.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly string _connectionString;

        public ProductoController(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("conexion");
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Producto> productos = new List<Producto>();

                using (SqlConnection connection =
                       new SqlConnection(_connectionString))
                {
                    using (SqlCommand command =
                           new SqlCommand(
                               "SP_Productos_Seleccionar",
                               connection))
                    {
                        command.CommandType =
                            CommandType.StoredProcedure;

                        connection.Open();

                        using (SqlDataReader reader =
                               command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                productos.Add(new Producto
                                {
                                    IdProducto =
                                        Convert.ToInt32(reader["IdProducto"]),

                                    Nombre =
                                        reader["Nombre"].ToString(),

                                    Descripcion =
                                        reader["Descripcion"] == DBNull.Value
                                            ? ""
                                            : reader["Descripcion"].ToString(),

                                    Precio =
                                        Convert.ToDecimal(reader["Precio"]),

                                    Existencia =
                                        Convert.ToInt32(reader["Existencia"]),

                                    Activo =
                                        Convert.ToBoolean(reader["Activo"]),

                                    FechaRegistro =
                                        Convert.ToDateTime(
                                            reader["FechaRegistro"])
                                });
                            }
                        }
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = "Consulta exitosa",
                    data = productos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Ocurrió un error al consultar los productos.",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProductoDto producto)
        {
            try
            {
                using (SqlConnection connection =
                       new SqlConnection(_connectionString))
                {
                    using (SqlCommand command =
                           new SqlCommand(
                               "SP_Productos_Insertar",
                               connection))
                    {
                        command.CommandType =
                            CommandType.StoredProcedure;

                        command.Parameters.AddWithValue(
                            "@Nombre", producto.Nombre);

                        command.Parameters.AddWithValue(
                            "@Descripcion",
                            string.IsNullOrWhiteSpace(producto.Descripcion)
                                ? DBNull.Value
                                : producto.Descripcion);

                        command.Parameters.AddWithValue(
                            "@Precio", producto.Precio);

                        command.Parameters.AddWithValue(
                            "@Existencia", producto.Existencia);

                        connection.Open();

                        command.ExecuteNonQuery();
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = "Producto registrado correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Ocurrió un error al registrar el producto.",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "El identificador del producto no es válido."
                    });
                }

                using (SqlConnection connection =
                       new SqlConnection(_connectionString))
                {
                    using (SqlCommand command =
                           new SqlCommand(
                               "SP_Productos_Eliminar",
                               connection))
                    {
                        command.CommandType =
                            CommandType.StoredProcedure;

                        command.Parameters.AddWithValue(
                            "@IdProducto", id);

                        connection.Open();

                        command.ExecuteNonQuery();
                    }
                }

                return Ok(new
                {
                    success = true,
                    message = "Producto dado de baja correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Ocurrió un error al eliminar el producto.",
                    error = ex.Message
                });
            }
        }
    }
}

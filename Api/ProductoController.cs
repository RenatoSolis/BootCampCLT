using BootcampCLT.Api.Request;
using BootcampCLT.Api.Response;
using BootcampCLT.Application.Command;
using BootcampCLT.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NpgsqlTypes;

namespace BootcampCLT.Api
{
    [ApiController]
    public class ProductoController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(IMediator mediator, ILogger<ProductoController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        /// <summary>
        /// Obtiene la lista completa de productos.
        /// </summary>
        /// <returns>Producto encontrado.</returns>
        [HttpGet("v1/api/productos")]
        [ProducesResponseType(typeof(IEnumerable<ProductoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductosAsync()
        {
            try
            {
                _logger.LogInformation("Consultando lista completa de productos");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var result = await _mediator.Send(new GetProductosQuery());
                watch.Stop();
                _logger.LogInformation("Consulta de productos completada en {ElapsedMilliseconds}ms", watch.ElapsedMilliseconds);

                if (result == null || !result.Any())
                {
                    _logger.LogWarning("La consulta de productos no devolvió resultados");
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de productos");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno al procesar la lista");
            }
        }

        /// <summary>
        /// Obtiene el detalle de un producto por su identificador.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>Producto encontrado.</returns>
        [HttpGet("v1/api/productos/{id:int}", Name = "GetProductoById")]
        [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductoResponse>> GetProductoByIdAsync([FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Inicia búsqueda de ProductoId={ProductoID}", id);
                var result = await _mediator.Send(new GetProductoByIdQuery(id));

                if (result == null)
                {
                    _logger.LogWarning("ProductoId={ProductoID} no encontrado", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar ProductoId={ProductoID}", id);
                return StatusCode(500, "Error al consultar el producto");
            }
        }

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        /// <param name="request">Datos del producto a crear.</param>
        /// <returns>Producto creado.</returns>
        [HttpPost("v1/api/productos")]
        [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductoResponse>> CreateProductoAsync([FromBody] CreateProductoRequest request)
        {
            try
            {
                _logger.LogInformation("Intentando crear producto con Código={Codigo}", request.Codigo);
                var command = new CreateProductoCommand(
                    request.Codigo, request.Nombre, request.Descripcion ?? string.Empty,
                    request.Precio, request.Activo, request.CategoriaId
                );

                var result = await _mediator.Send(command);
                _logger.LogInformation("Producto creado exitosamente con ID={ID}", result.Id);

                return CreatedAtRoute("GetProductoById", new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto con Código={Codigo}", request.Codigo);
                return StatusCode(500, "Error al crear el producto. Verifique si el código ya existe.");
            }
        }

        /// Actualiza completamente un producto existente.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <param name="request">Datos completos a actualizar.</param>
        [HttpPut("v1/api/productos/{id:int}")]
        [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductoResponse>> UpdateProductoAsync(
            [FromRoute] int id,
            [FromBody] UpdateProductoRequest request)
        {
            try
            {
                _logger.LogInformation("Actualizando ProductoId={ID}", id);
                var command = new UpdateProductoCommand(
                    id, request.Codigo, request.Nombre, request.Descripcion ?? string.Empty,
                    request.Precio, request.Activo, request.CategoriaId
                );

                var result = await _mediator.Send(command);

                if (result == null)
                {
                    _logger.LogWarning("No se pudo actualizar: ProductoId={ID} no existe", id);
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Update para ProductoId={ID}", id);
                return StatusCode(500, "Error al actualizar el producto");
            }
        }

        /// <summary>
        /// Actualiza parcialmente un producto existente.
        /// Solo se modificarán los campos enviados.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <param name="request">Campos a actualizar.</param>
        [HttpPatch("v1/api/productos/{id:int}")]
        [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProductoResponse>> PatchProducto(
            [FromRoute] int id,
            [FromBody] PatchProductoRequest request)
        {
            try
            {
                _logger.LogInformation("Aplicando Patch a ProductoId={ID}", id);
                var command = new PatchProductoCommand(
                    id, request.Codigo, request.Nombre, request.Descripcion,
                    request.Precio, request.Activo, request.CategoriaId
                );

                var result = await _mediator.Send(command);

                if (result == null) return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Patch para ProductoId={ID}", id);
                return StatusCode(500, "Error parcial al actualizar");
            }
        }

        /// <summary>
        /// Elimina un producto existente.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        [HttpDelete("v1/api/productos/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteProducto([FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Eliminando ProductoId={ID}", id);
                var success = await _mediator.Send(new DeleteProductoCommand(id));

                if (!success)
                {
                    _logger.LogWarning("Intento de eliminación fallido: ID={ID} no encontrado", id);
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar ProductoId={ID}", id);
                return StatusCode(500, "Error al eliminar el producto");
            }
        }
    }

    //public class Transferencia
    //{
    //    public int CuentaOrdenante { get; set; }
    //    public int NumeroDocumentoOrdenante { get; set; }
    //    public int CuentaBeneficiario {  get; set; }
    //    public int NumeroDocumentoBeneficiario { get; set; }
    //    public int Monto {  get; set; }
    //    public int Concepto { get; set; }

    //    public bool EnviarTransferencia() { return true; }
    //}

    //public class TransferenciaInterna : Transferencia
    //{

    //}

    //public class TransferenciaSipap : Transferencia
    //{

    //}
    
    ////Transferencia transferencia = new TransferenciaSipap();

    //    //transferencia.enviarTransferencia();
   
}

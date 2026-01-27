using BootcampCLT.Api.Response;
using BootcampCLT.Application.Command;
using BootcampCLT.Domain.Entity;
using BootcampCLT.Infrastructure.Context;
using MediatR;

namespace BootcampCLT.Application.Handler
{
    public class CreateProductoHandler : IRequestHandler<CreateProductoCommand, ProductoResponse>
    {
        private readonly PostgresDbContext _postgresDbContext;

        public CreateProductoHandler(PostgresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<ProductoResponse> Handle(CreateProductoCommand request, CancellationToken cancellationToken)
        {
            // 1. Crear la instancia de la Entidad de Dominio
            var nuevoProducto = new Producto
            {
                Codigo = request.Codigo,
                Nombre = request.Nombre,
                Descripcion = request.Descripcion ?? string.Empty,
                Precio = request.Precio,
                Activo = request.Activo,
                CategoriaId = request.CategoriaId,
                FechaCreacion = DateTime.UtcNow, 
                CantidadStock = 0 
            };

            // 2. Persistir en la base de datos
            _postgresDbContext.Productos.Add(nuevoProducto);
            await _postgresDbContext.SaveChangesAsync(cancellationToken);

            // 3. Mapear la entidad creada al Response para devolverlo al controlador
            return new ProductoResponse(
                nuevoProducto.Id,
                nuevoProducto.Codigo,
                nuevoProducto.Nombre,
                nuevoProducto.Descripcion ?? string.Empty,
                (double)nuevoProducto.Precio,              
                nuevoProducto.Activo,
                nuevoProducto.CategoriaId,
                nuevoProducto.FechaCreacion,
                nuevoProducto.FechaActualizacion,         
                nuevoProducto.CantidadStock
    );
        }
    }
}

using BootcampCLT.Api.Response;
using BootcampCLT.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Command
{
    public class UpdateProductoHandler : IRequestHandler<UpdateProductoCommand, ProductoResponse>
    {
        private readonly PostgresDbContext _context;

        public UpdateProductoHandler(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<ProductoResponse> Handle(UpdateProductoCommand request, CancellationToken cancellationToken)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (producto == null) return null!;

            // Actualizamos los campos
            producto.Codigo = request.Codigo;
            producto.Nombre = request.Nombre;
            producto.Descripcion = request.Descripcion;
            producto.Precio = request.Precio;
            producto.Activo = request.Activo;
            producto.CategoriaId = request.CategoriaId;
            producto.FechaActualizacion = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new ProductoResponse(
                producto.Id,
                producto.Codigo,
                producto.Nombre,
                producto.Descripcion ?? string.Empty,
                (double)producto.Precio,
                producto.Activo,
                producto.CategoriaId,
                producto.FechaCreacion,
                producto.FechaActualizacion,
                producto.CantidadStock
            );
        }
    }
}

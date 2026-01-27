using BootcampCLT.Api.Response;
using BootcampCLT.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Command
{
    public class PatchProductoHandler : IRequestHandler<PatchProductoCommand, ProductoResponse>
    {
        private readonly PostgresDbContext _context;

        public PatchProductoHandler(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<ProductoResponse> Handle(PatchProductoCommand request, CancellationToken cancellationToken)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (producto == null) return null;

            // Solo actualizamos si el campo viene en el request
            if (request.Codigo != null) producto.Codigo = request.Codigo;
            if (request.Nombre != null) producto.Nombre = request.Nombre;
            if (request.Descripcion != null) producto.Descripcion = request.Descripcion;
            if (request.Precio.HasValue) producto.Precio = request.Precio.Value;
            if (request.Activo.HasValue) producto.Activo = request.Activo.Value;
            if (request.CategoriaId.HasValue) producto.CategoriaId = request.CategoriaId.Value;

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

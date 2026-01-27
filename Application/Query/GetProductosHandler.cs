using BootcampCLT.Api.Response;
using BootcampCLT.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Query
{
    public class GetProductosHandler : IRequestHandler<GetProductosQuery, IEnumerable<ProductoResponse>>
    {
        private readonly PostgresDbContext _context;

        public GetProductosHandler(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoResponse>> Handle(GetProductosQuery request, CancellationToken cancellationToken)
        {
            var productos = await _context.Productos.ToListAsync(cancellationToken);

            return productos.Select(p => new ProductoResponse(
                p.Id,
                p.Codigo,
                p.Nombre,
                p.Descripcion ?? string.Empty, 
                (double)p.Precio,              
                p.Activo,
                p.CategoriaId,
                p.FechaCreacion,
                p.FechaActualizacion,
                p.CantidadStock
            ));
        }
    }
}
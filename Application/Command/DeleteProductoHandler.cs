using BootcampCLT.Infrastructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Command
{
    public class DeleteProductoHandler : IRequestHandler<DeleteProductoCommand, bool>
    {
        private readonly PostgresDbContext _context;

        public DeleteProductoHandler(PostgresDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteProductoCommand request, CancellationToken cancellationToken)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (producto == null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

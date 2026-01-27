using MediatR;

namespace BootcampCLT.Application.Command
{
    // Solo necesitamos el ID para saber qué borrar
    public record DeleteProductoCommand(int Id) : IRequest<bool>;
}

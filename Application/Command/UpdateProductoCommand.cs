using BootcampCLT.Api.Response;
using MediatR;

namespace BootcampCLT.Application.Command
{
    public record UpdateProductoCommand(
        int Id,
        string Codigo,
        string Nombre,
        string Descripcion,
        double Precio,
        bool Activo,
        int CategoriaId
    ) : IRequest<ProductoResponse>;
}

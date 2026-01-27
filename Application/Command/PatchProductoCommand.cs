using BootcampCLT.Api.Response;
using MediatR;

namespace BootcampCLT.Application.Command
{
    public record PatchProductoCommand(
        int Id,
        string? Codigo = null,
        string? Nombre = null,
        string? Descripcion = null,
        double? Precio = null,
        bool? Activo = null,
        int? CategoriaId = null
    ) : IRequest<ProductoResponse>;
}

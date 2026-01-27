using BootcampCLT.Api.Response;
using MediatR;

namespace BootcampCLT.Application.Query
{
    public record GetProductoByIdQuery(int Id) : IRequest<ProductoResponse?>
    {}
}

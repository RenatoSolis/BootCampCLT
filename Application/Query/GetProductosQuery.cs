using BootcampCLT.Api.Response;
using MediatR;

namespace BootcampCLT.Application.Query
{
    public record GetProductosQuery() : IRequest<IEnumerable<ProductoResponse>>;
}

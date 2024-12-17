using Domain.Common;
using Domain.Comparison;
using MediatR;

namespace Application.Use_Cases.Actions
{
    public class CompareTwoPropertiesCommand : IRequest<Result<PropertyComparison>>
    {
        public Guid Initial { get; set; }
        public Guid Secondary { get; set; }
    }
}
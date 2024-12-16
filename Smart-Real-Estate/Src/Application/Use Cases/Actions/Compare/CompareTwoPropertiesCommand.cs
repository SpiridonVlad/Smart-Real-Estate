using Domain.Common;
using MediatR;

namespace Application.Use_Cases.Actions
{
    public class CompareTwoPropertiesCommand : IRequest<Result<ComparisonResult>>
    {
        public Guid Initial { get; set; }
        public Guid Secondary { get; set; }
    }
}
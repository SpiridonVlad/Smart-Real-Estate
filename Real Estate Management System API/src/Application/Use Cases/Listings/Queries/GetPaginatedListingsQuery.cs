using Application.DTOs;
using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Use_Cases.Queries
{
    public class GetPaginatedListingsQuery: IRequest<Result<IEnumerable<ListingDto>>>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Application.Use_Cases.Wrappers;
using Application.Filters;
using Domain.Types;

namespace Real_Estate_Management_System.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RecordController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator mediator = mediator;


        [HttpGet("Paginated")]
        public async Task<ActionResult<IEnumerable<RecordDto>>> GetPaginatedRecords(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] PropertyType? propertyType = null,
            [FromQuery] int? minPrice = null,
            [FromQuery] int? maxPrice = null,
            [FromQuery] DateTime? minPublicationDate = null,
            [FromQuery] DateTime? maxPublicationDate = null,
            [FromQuery] string? descriptionContains = null,
            [FromQuery(Name = "propertyMinFeatures")] Dictionary<PropertyFeatureType, int>? propertyMinFeatures = null,
            [FromQuery(Name = "propertyMaxFeatures")] Dictionary<PropertyFeatureType, int>? propertyMaxFeatures = null,
            [FromQuery(Name = "listingMinFeatures")] Dictionary<ListingAssetss, int>? listingMinFeatures = null)
        {

            var filter = new RecordFilter
            {
                PropertyFilter = new PropertyFilter
                {
                    PropertyType = propertyType,
                    PropertyFeaturesMinValues = propertyMinFeatures,
                    PropertyFeaturesMaxValues = propertyMaxFeatures
                },
                ListingFilter = new ListingFilter
                {
                    MinPrice = minPrice,
                    MaxPrice = maxPrice,
                    MinPublicationDate = minPublicationDate,
                    MaxPublicationDate = maxPublicationDate,
                    ListingDescriptionContains = descriptionContains,
                    ListingFeatures = listingMinFeatures,
                },
                AddressFilter = new AddressFilter(),
                UserFilters = new UserFilter()
            };

            var query = new GetRecordQuery
            {
                Page = page,
                PageSize = pageSize,
                Filter = filter
            };

            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RecordDto>> GetRecordsById(Guid id)
        {
            var query = new GetRecordByIdQuery { Id = id };
            var result = await mediator.Send(query);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);
        }

    }
}

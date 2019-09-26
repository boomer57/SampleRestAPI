using AutoMapper;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Domain.Models.Queries;
using SampleRestAPI.API.Extensions;
using SampleRestAPI.API.Resources;

namespace SampleRestAPI.API.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Movie, MovieResource>();

            CreateMap<User, UserResource>()
                .ForMember(src => src.UnitOfMeasurement,
                           opt => opt.MapFrom(src => src.UnitOfMeasurement.ToDescriptionString()));

            CreateMap<QueryResult<User>, QueryResultResource<User>>();
        }
    }
}
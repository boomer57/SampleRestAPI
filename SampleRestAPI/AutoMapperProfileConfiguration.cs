using AutoMapper;
using SampleRestAPI.API.Domain.Models;
using SampleRestAPI.API.Resources;

namespace SampleRestAPI
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration() : this("MyProfile")
        {
        }

        protected AutoMapperProfileConfiguration(string profileName)
        : base(profileName)
        {
            CreateMap<Movie, MovieResource>();
        }
    }
}
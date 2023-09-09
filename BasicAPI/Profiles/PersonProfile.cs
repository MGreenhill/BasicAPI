using AutoMapper;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.JsonPatch;
using System.Security.Principal;
using BasicAPI.Models;

namespace BasicAPI.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile() {
            CreateMap<Entities.Person, Models.PersonDto>();
            CreateMap<Models.PersonCreateDto, Entities.Person>();
            CreateMap<Models.PersonUpdateDto, Entities.Person>();
            CreateMap<JsonPatchDocument<PersonUpdateDto>, JsonPatchDocument>();
        }
    }
}

using AssuredBid.DTOs;
using AssuredBid.Models;
using AutoMapper;

namespace AssuredBid.Mappings
{
    public class Map : Profile
    {
        public static ModelResult<T> Result<T>(List<T> entity, bool status, string message, Metadata metaData = null)
        {
            var r = new ModelResult<T>()
            {
                Data = entity,
                Succeeded = status,
                Message = message,
                MetaData = metaData
            };

            return r;
        }

        public Map()
        {
            CreateMap<CreateTenders, CreateTenderDTO>().ReverseMap();
            CreateMap<UsersPage, UsersDTO>().ReverseMap();
        }

    }
}

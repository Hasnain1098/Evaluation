using AutoMapper;
using Evaluation.DTOs.Sales;
using Evaluation.Models;
using System.Data;

namespace Evaluation.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            //For Sales
            CreateMap<Sale, CreateSaleDto>().ReverseMap();
            CreateMap<Sale, GetSaleDto>().ReverseMap();
            CreateMap<Sale, UpdateSaleDto>().ReverseMap();
        }
    }
}


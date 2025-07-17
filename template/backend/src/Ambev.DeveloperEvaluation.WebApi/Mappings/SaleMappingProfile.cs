using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.SearchSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.SearchSale;
using AutoMapper;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Sales.MappingProfiles;

public class SaleMappingProfile : Profile
{
    public SaleMappingProfile()
    {
        CreateMap<Sale, CreateSaleResult>();

        CreateMap<CreateSaleRequest, CreateSaleCommand>();

        CreateMap<CreateSaleCommand, Sale>()
             .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
                 src.Items != null ? src.Items.Sum(i => i.Total) : 0m))
             .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
             .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(_ => false));

        CreateMap<CreateSaleItemDto, Ambev.DeveloperEvaluation.Domain.Entities.SaleItem>();

        CreateMap<Ambev.DeveloperEvaluation.Domain.Entities.SaleItem, CreateSaleItemDto>();

        CreateMap<CreateSaleItemDto, SaleItemResponse>();

        CreateMap<SaleItemResponse, CreateSaleItemDto>();

        CreateMap<CreateSaleResult, CreateSaleResponse>();

        CreateMap<CreateSaleResponse, CreateSaleResult>();

        CreateMap<Ambev.DeveloperEvaluation.Domain.Entities.SaleItem, SaleItemResponse>();

        CreateMap<CreateSaleItemDto, SaleItemResponse>();

        //List
        CreateMap<ListSalesRequest, ListSaleCommand>();
        
        CreateMap<Sale, ListSaleResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<ListSaleResponse, ListSaleResult>();

        CreateMap<ListSaleResult, ListSaleResponse>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        //Get
        CreateMap<Sale, GetSaleResult>();

        CreateMap<GetSaleResult, GetSaleResponse>();

        //Search
        CreateMap<SearchSalesRequest, SearchSaleCommand>();

        CreateMap<Sale, SearchSaleResult>();
    }
}


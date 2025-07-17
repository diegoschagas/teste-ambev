using Ambev.DeveloperEvaluation.Application.SaleItem.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.Cancel;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.Modify;
using Ambev.DeveloperEvaluation.Application.Sales.SearchSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.CancelSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Modify;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ModifySale;
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
             .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => SaleStatus.Active));

        CreateMap<CreateSaleItemDto, Ambev.DeveloperEvaluation.Domain.Entities.SaleItem>();

        CreateMap<Ambev.DeveloperEvaluation.Domain.Entities.SaleItem, CreateSaleItemDto>();

        CreateMap<CreateSaleResult, CreateSaleResponse>();

        CreateMap<CreateSaleResponse, CreateSaleResult>();

        CreateMap<Ambev.DeveloperEvaluation.Domain.Entities.SaleItem, CreateSaleItemDto>();

        //List
        CreateMap<ListSalesRequest, ListSaleCommand>();

        CreateMap<Sale, ListSaleResult>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<ListSaleResponse, ListSaleResult>();

        CreateMap<ListSaleResult, ListSaleResponse>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => Enum.GetName(typeof(SaleStatus), src.Status)));
        //Get
        CreateMap<Sale, GetSaleResult>();

        CreateMap<GetSaleResult, GetSaleResponse>()
            .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => Enum.GetName(typeof(SaleStatus), src.Status)));

        //Search
        CreateMap<SearchSalesRequest, SearchSaleCommand>();

        CreateMap<Sale, SearchSaleResult>()
            .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => Enum.GetName(typeof(SaleStatus), src.Status)));

        //Cancel
        CreateMap<CancelSaleResponse, CancelSaleResult>();

        CreateMap<CancelSaleResult, CancelSaleResponse>()
            .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => Enum.GetName(typeof(SaleStatus), src.Status)));

        //Modify
        CreateMap<ModifySaleCommand, ModifySaleRequest>();

        CreateMap<ModifySaleRequest, ModifySaleCommand>();

        CreateMap<ModifySaleCommand, Sale>();

        CreateMap<Sale, ModifySaleResult>();

        CreateMap<ModifySaleResult, ModifySaleResponse>()
            .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => Enum.GetName(typeof(SaleStatus), src.Status)));

        //CancelSaleItem
        CreateMap<CancelSaleItemResponse, CancelSaleItemResult>();

        CreateMap<CancelSaleItemResult, CancelSaleItemResponse>();
    }
}


using AutoMapper;
using Jumia_Api.Application.Dtos.OrderDtos;
using Jumia_Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia_Api.Application.MappingProfiles
{
    public class OrderMapping:Profile
    {
        public OrderMapping()
        {
            CreateMap<CreateOrderDTO, Order>()
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => "pending"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest=>dest.AffiliateCode,opt=> opt.MapFrom(src => src.AffiliateCode ?? "N/A"))

                ;

            CreateMap<UpdateOrderDTO, Order>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                //.ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<Order, OrderDTO>()
                .ForMember(dest=>dest.SubOrders,opt=>opt.MapFrom(src=>src.SubOrders));

            CreateMap<CancelOrderDTO, Order>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "cancelled"))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<SubOrder, SubOrderDTO>()
                .ForMember(dest=>dest.ID,opt=>opt.MapFrom(src => src.SubOrderId))
                .ForMember(dest=>dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ReverseMap();


            CreateMap<SubOrderDTO, SubOrder>()
                .ForMember(dest => dest.SubOrderId, opt => opt.MapFrom(src => src.ID));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));
           
            
            
            
            CreateMap<OrderItemDTO, OrderItem>()
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest=>dest.ProductVariant, opt => opt.Ignore())
                .ForMember(dest => dest.SubOrder, opt => opt.Ignore());



            CreateMap<OrderDTO, Order>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) 
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
        }
    }
}

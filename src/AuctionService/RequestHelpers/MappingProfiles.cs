using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;

namespace AuctionService.RequestHelpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item); //AuctionDto can be populated from both Auction and its Item property
            CreateMap<Item, AuctionDto>();
            CreateMap<CreateAuctionDto, Auction>().ForMember(d => d.Item, o => o.MapFrom(x => x)); 
            // d => d.Item: Specifies the destination member (Item in Auction)
            // ForMember(d => d.DestinationProperty, options => options.MapFrom(s => s.SourceProperty));
            // so, ForMember => s.SourceProperty -----> d.DestinationProperty

            CreateMap<CreateAuctionDto, Item>();

        }
    }
}
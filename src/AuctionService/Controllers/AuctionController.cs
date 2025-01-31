using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionController : ControllerBase
    {
        private readonly AuctionDBContext _context;
        private readonly IMapper _mapper;
        // dependency injection need to use services added in the Program.cs
        // when request comes, every time it is handled by controller during creating instance, see that there is params in the constructor, and it then make available to the controller that params(dbContext, mapper)
        public AuctionController(AuctionDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
        {
            var auctions = await _context
                                  .Auctions
                                  .Include(x => x.Item)
                                  .OrderBy(x => x.Item.Make)
                                  .ToListAsync();
            return _mapper.Map<List<AuctionDto>>(auctions);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction = await _context
                         .Auctions
                         .Include(x => x.Item)
                         .FirstOrDefaultAsync(x => x.Id == id);
            if (auction == null) return NotFound();
            return _mapper.Map<AuctionDto>(auction);
        }

        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);
            // TODO: Add current user as a seller
            auction.Seller = "Test1";
            _context.Auctions.Add(auction);

            var result = await _context.SaveChangesAsync() > 0;
            if (!result) return BadRequest();
            return CreatedAtAction(
              nameof(GetAuctionById),
              new { auction.Id },
              _mapper.Map<AuctionDto>(auction)
            );
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
        {
            var auction = await _context.Auctions
                                        .Include(x => x.Item)
                                        .FirstOrDefaultAsync(
                                           x => x.Id == id
                                        );
            if (auction == null) return NotFound();
            // TODO: check seller = username
            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;


            var result = await _context.SaveChangesAsync() > 0;
            if (result) return Ok();

            return BadRequest("Problem saving changes");

        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteAuction(Guid id){
           var auction =await _context.Auctions.FindAsync(id);
           if (auction == null) return NotFound();

           //TODO: check if seller == username
           _context.Auctions.Remove(auction);
           var result = await _context.SaveChangesAsync() > 0;
           if (!result) return BadRequest("Could not delete this auction");
           return Ok();
        }


    }

}
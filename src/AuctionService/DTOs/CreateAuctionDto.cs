using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuctionService.DTOs
{
    public class CreateAuctionDto
    {
        [Required(ErrorMessage = "make is required")]
        public string Make { get; set; }
        [Required(ErrorMessage = "model is required")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Year  is required")]
        public int Year { get; set; }
        [Required(ErrorMessage = "color is required")]
        public string Color { get; set; }
        [Required(ErrorMessage = "mileage is required")]
        public int Mileage { get; set; }
        [Required(ErrorMessage = "image url is required")]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "reserve price is required")]
        public int ReservePrice { get; set; }
        [Required(ErrorMessage = "auction end is required")]
        public DateTime AuctionEnd {get; set;}

    }
}
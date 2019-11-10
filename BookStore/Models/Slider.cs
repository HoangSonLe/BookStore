using System;
using System.Collections.Generic;

namespace BookStore.Models
{
    public partial class Slider
    {
        public int SlideId { get; set; }
        public string SliderImage { get; set; }
        public int? Display { get; set; }
        public string Description { get; set; }
    }
}

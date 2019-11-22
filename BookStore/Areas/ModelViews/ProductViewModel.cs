using BookStore.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Areas.ModelViews
{
    public class ProductViewModel
    {
        public Product product { get; set; }
        public List<IFormFile> productImages { get; set; }
        public List<IFormFile> productAddImages { get; set; }
    }
}

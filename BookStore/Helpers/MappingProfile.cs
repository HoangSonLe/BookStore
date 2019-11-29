using AutoMapper;
using BookStore.Models;
using BookStore.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Helpers
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, UserRegister>().ReverseMap();
        }
    }
}

﻿using AutoMapper;
using Mango.Services.CouponAPI.Models.Dto;
using Mango.Services.CouponAPI.Models;

namespace Mango.Services.CouponAPI
{
    public class MapperConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}

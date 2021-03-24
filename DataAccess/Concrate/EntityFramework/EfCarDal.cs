﻿using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Concrate.EntityFramework
{
    public class EfCarDal : EfEntityRepositoryBase<Car, RentACarContext>, ICarDal
    {
        public List<CarDetailDto> GetCarDetails(Expression<Func<Car, bool>> filter = null)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = (from p in filter == null ? context.Cars : context.Cars.Where(filter)
                              join c in context.Colors on p.ColorId equals c.ColorId
                              join d in context.Brands on p.BrandId equals d.BrandId
                              join im in context.CarImages on p.CarId equals im.CarId
                              select new CarDetailDto
                              {
                                  CarId = p.CarId,
                                  CarName = p.CarName,
                                  BrandName = d.BrandName,
                                  ColorName = c.ColorName,
                                  DailyPrice = p.DailyPrice,
                                  Description = p.Description,
                                  ModelYear = p.ModelYear,
                                  Date = im.Date,
                                  ImagePath = im.ImagePath,
                                  ImageId = im.Id
                              }).ToList();
                return result.GroupBy(p => p.CarId).Select(p => p.FirstOrDefault()).ToList();
            }
        }

        public List<CarDetailDto> GetCarDetailById(int carId)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from p in context.Cars
                             join c in context.Colors on p.ColorId equals c.ColorId
                             join d in context.Brands on p.BrandId equals d.BrandId
                             join im in context.CarImages on p.CarId equals im.CarId
                             where p.CarId == carId
                             select new CarDetailDto
                             {
                                 CarId = p.CarId,
                                 CarName = p.CarName,
                                 BrandId = p.BrandId,
                                 BrandName = d.BrandName,
                                 ColorId = p.ColorId,
                                 ColorName = c.ColorName,
                                 DailyPrice = p.DailyPrice,
                                 Description = p.Description,
                                 ModelYear = p.ModelYear,
                                 Date = im.Date,
                                 ImagePath = im.ImagePath,
                                 ImageId = im.Id
                             };
                return result.ToList();
            }
        }

        public List<CarDetailDto> GetCarDetailsByBrandAndColor(int brandId, int colorId)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from car in context.Cars.Where
                        (car => car.BrandId == brandId && car.ColorId == colorId)
                             join brand in context.Brands on car.BrandId equals brand.BrandId
                             join color in context.Colors on car.ColorId equals color.ColorId

                             select new CarDetailDto
                             {
                                 CarId = car.CarId,
                                 BrandName = brand.BrandName,
                                 ColorName = color.ColorName,
                                 DailyPrice = car.DailyPrice,
                                 ModelYear = car.ModelYear,
                                 ImagePath = (from carImage in context.CarImages
                                              where (carImage.CarId == car.CarId)
                                              select carImage).FirstOrDefault().ImagePath
                             };
                return result.ToList();
            }
        }
    }
}


/*
public List<CarDetailDto> GetCarDetails(Expression<Func<Car, bool>> filter = null)
{
    using (RentACarContext context = new RentACarContext())
    {
        var result = from car in context.Cars
                     join b in context.Brands on car.BrandId equals b.BrandId
                     join c in context.Colors on car.ColorId equals c.ColorId
                     //join image in context.CarImages on car.CarId equals image.CarId
                     select new CarDetailDto
                     {
                         CarId = car.CarId,
                         CarName = car.CarName,
                         BrandId=car.BrandId,
                         BrandName = b.BrandName,
                         ColorId = car.ColorId,
                         ColorName = c.ColorName,
                         DailyPrice = car.DailyPrice,
                         ModelYear = car.ModelYear,
                         Description = car.Description,
                         //CarImageDate= image.Date,
                         //ImagePath = image.ImagePath
                     };
        return filter == null
            ? result.ToList()
            : result.Where(filter).ToList();
    }
}
}
}
*/
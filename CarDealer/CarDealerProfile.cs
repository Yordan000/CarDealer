using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CarDealer.DTO;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<ImportSuppliersDto, Supplier>();
            this.CreateMap<ImportPartsDto, Part>();
            this.CreateMap<ImportCarsDto, Car>();
            this.CreateMap<ImportCustomersDto, Customer>();
            this.CreateMap<ImportSalesDto, Sale>();

            this.CreateMap<Customer, ExportOrderedCustomersDto>()
                .ForMember( dest => dest.BirthDate , sc => sc.MapFrom( s=> $"{s.BirthDate:dd/MM/yyyy}"));

            this.CreateMap<Car, ExportCarsFromMakeToyotaDto>();

            this.CreateMap<Supplier, ExportLocalSuppliersDto>()
                .ForMember(dest => dest.PartsCount, sc => sc.MapFrom(s => s.Parts.Count));
            this.CreateMap<Car, ExportCarsCarInfoDto>();

            //this.CreateMap<Car, ExportCarsAndPartsDto>()
            //    .ForMember(dest => dest.Parts, sc => sc.MapFrom(s => s.PartCars));
            this.CreateMap<Part, ExportPartsFromCarsDto>()
                .ForMember(dest => dest.Name, sc=> sc.MapFrom( s=> s.Name))
                .ForMember(dest => dest.Price, sc=> sc.MapFrom( s=> $"{s.Price:F2}"));
        }
    }
}

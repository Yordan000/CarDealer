using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTO;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(CarDealerProfile)));
            CarDealerContext dbContext = new CarDealerContext();
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //Console.WriteLine("Database was created !!!");

            //string inputJson = File.ReadAllText("../../../Datasets/sales.json");

            //string result = ImportSales(dbContext, inputJson);

            //Console.WriteLine(result);

            string json = GetTotalSalesByCustomer(dbContext);
            File.WriteAllText("../../../Datasets/customers-total-sales.json", json);
        }

        //Problem 09
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            ImportSuppliersDto[] suppliersDtos = JsonConvert.DeserializeObject<ImportSuppliersDto[]>(inputJson);

            ICollection<Supplier> suppliers = new List<Supplier>();

            foreach (var sDto in suppliersDtos)
            {
                if (!IsValid(sDto))
                {
                    continue;
                }

                Supplier supplier = Mapper.Map<Supplier>(sDto);
                suppliers.Add(supplier);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}.";
        }

        //Problem 10
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            ImportPartsDto[] partsDtos = JsonConvert.DeserializeObject<ImportPartsDto[]>(inputJson);

            ICollection<Part> parts = new List<Part>();

            foreach (var pDto in partsDtos)
            {
                if (!context.Suppliers.Any( s=> s.Id == pDto.SupplierId)) 
                {
                    continue;
                }

                Part part = Mapper.Map<Part>(pDto);
                parts.Add(part);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}.";
        }

        //Problem 11
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            ImportCarsDto[] carsDtos = JsonConvert.DeserializeObject<ImportCarsDto[]>(inputJson);

            List<Car> cars = new List<Car>();

            foreach (var dtoCar in carsDtos)
            {
                Car newCar = new Car
                {
                    Make = dtoCar.Make,
                    Model = dtoCar.Model,
                    TravelledDistance = dtoCar.TravelledDistance,
                };
                foreach (int partId in dtoCar.PartsIdInfo.Distinct())
                {
                    newCar.PartCars.Add(new PartCar
                    {
                        PartId = partId
                    });
                }

                cars.Add(newCar);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();


            //ImportCarsDto[] carsDtos = JsonConvert.DeserializeObject<ImportCarsDto[]>(inputJson);

            //ICollection<Car> cars = new List<Car>();
            //foreach (var cDto in carsDtos)
            //{
            //    Car car = new Car()
            //    {
            //        Make = cDto.Make,
            //        Model = cDto.Model,
            //        TravelledDistance = cDto.TravelledDistance
            //    };
            //    ICollection<PartCar> currPart = new List<PartCar>();

            //    foreach (var part in cDto.PartsIdInfo.Distinct())
            //    {
            //        currPart.Add(new PartCar()
            //        {
            //            PartId = part
            //        });
            //    }

            //    car.PartCars = currPart;
            //    cars.Add(car);
            //}
            //context.Cars.AddRange(cars);
            //context.SaveChanges();

            return $"Successfully imported {cars.Count}.";
        }

        //Problem 12
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            ImportCustomersDto[] customersDtos = JsonConvert.DeserializeObject<ImportCustomersDto[]>(inputJson);

            ICollection<Customer> customers = new List<Customer>();

            foreach (var cuDto in customersDtos)
            {
                if (!IsValid(cuDto))
                {
                    continue;
                }

                Customer customer = Mapper.Map<Customer>(cuDto);
                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}.";
        }

        //Problem 13
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            ImportSalesDto[] salesDtos = JsonConvert.DeserializeObject<ImportSalesDto[]>(inputJson);

            ICollection<Sale> sales = new List<Sale>();

            foreach (var sDto in salesDtos)
            {
                if (!IsValid(sDto))
                {
                    continue;
                }

                Sale sale = Mapper.Map<Sale>(sDto);
                sales.Add(sale);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}.";
        }

        //Problem 14
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            ExportOrderedCustomersDto[] customersDtos = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver ? 1:0)
                .ProjectTo<ExportOrderedCustomersDto>()
                .ToArray();

            string json = JsonConvert.SerializeObject(customersDtos, Formatting.Indented);
            return json;
        }

        //Problem 15
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            ExportCarsFromMakeToyotaDto[] carsToyotaDtos = context.Cars
                .Where( c=> c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ProjectTo<ExportCarsFromMakeToyotaDto>()
                .ToArray();

            string json = JsonConvert.SerializeObject(carsToyotaDtos, Formatting.Indented);
            return json;
        }

        //Problem 16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            ExportLocalSuppliersDto[] suppliersDtos = context.Suppliers
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportLocalSuppliersDto>()
                .ToArray();

            string json = JsonConvert.SerializeObject(suppliersDtos, Formatting.Indented);
            return json;
        }

        //Problem 17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsDtos = context.Cars
                .Select(x => new
                {
                    car = new
                    {
                        x.Make,
                        x.Model,
                        x.TravelledDistance
                    },
                    parts = x.PartCars.Select(y => new
                    {
                        y.Part.Name,
                        Price = y.Part.Price.ToString("F2")
                    })
                })
                .ToArray();
 
            string json = JsonConvert.SerializeObject(carsDtos, Formatting.Indented);
            return json;
        }

        //Problem 18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customersDtos = context.Customers
                .Where(s => s.Sales.Count > 0)
                .Select(s => new 
                {
                    fullName = s.Name,
                    boughtCars = s.Sales.Count,
                    spentMoney = s.Sales.Sum(x => x.Car.PartCars.Sum( v=> v.Part.Price))
                })
                .OrderByDescending( x=> x.spentMoney)
                .ThenByDescending( x=> x.boughtCars)
                .ToArray();

            string json = JsonConvert.SerializeObject(customersDtos, Formatting.Indented);
            return json;
        }

        private static bool IsValid(object obj)
        {
            var validateContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validateResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validateContext, validateResult);

            return isValid;
        }
    }
}
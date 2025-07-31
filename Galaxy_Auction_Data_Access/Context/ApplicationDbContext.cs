using Galaxy_Auction_Data_Access.Domain;
using Galaxy_Auction_Data_Access.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Galaxy_Auction_Data_Access.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Vehicle>().HasData(
                new Vehicle
                {
                    VehicleId = 1,
                    BrandAndModel = "Toyota Camry",
                    ManufacturingYear = 2020,
                    Color = "Silver",
                    EngineCapacity = 2.5m,
                    Price = 25000.00m,
                    Mileage = 15000,
                    PlateNumber = "34AA21",
                    AuctionPrice = 200.0,
                    AdditionalInformation = "Excellent condition, single owner",
                    StartTime = new DateTime(2025, 01, 01, 00, 00, 00),
                    EndTime = new DateTime(2025, 12, 31, 23, 59, 59),
                    IsActive = true,
                    Image = "https://i.gaw.to/content/photos/39/21/392165_2020_Toyota_Camry.jpg",
                    SellerId = "756997f4-c894-419f-ac98-020d1ed6c75b"
                },
                new Vehicle
                {
                    VehicleId = 2,
                    BrandAndModel = "Honda Civic",
                    ManufacturingYear = 2019,
                    Color = "Blue",
                    EngineCapacity = 1.8m,
                    Price = 18000.00m,
                    Mileage = 20000,
                    PlateNumber = "34AA21",
                    AuctionPrice = 200.0,
                    AdditionalInformation = "Good condition, one previous owner",
                    StartTime = new DateTime(2025, 01, 01, 00, 00, 00),
                    EndTime = new DateTime(2025, 12, 31, 23, 59, 59),
                    IsActive = true,
                    Image = "https://i.pinimg.com/originals/4f/b7/96/4fb796d99758f4889338c69efc74dbfe.jpg",
                    SellerId = "756997f4-c894-419f-ac98-020d1ed6c75b"
                },
                new Vehicle
                {
                    VehicleId = 3,
                    BrandAndModel = "Ford F-150",
                    ManufacturingYear = 2018,
                    Color = "Red",
                    EngineCapacity = 5.0m,
                    Price = 28000.00m,
                    Mileage = 25000,
                    PlateNumber = "34AA21",
                    AuctionPrice = 200.0,
                    AdditionalInformation = "Low mileage, well-maintained",
                    StartTime = new DateTime(2025, 01, 01, 00, 00, 00),
                    EndTime = new DateTime(2025, 12, 31, 23, 59, 59),
                    IsActive = true,
                    Image = "https://www.autopartmax.com/images/cUpload/FORD%20Truck-F150%20Raptor.jpg",
                    SellerId = "756997f4-c894-419f-ac98-020d1ed6c75b"
                },
                new Vehicle
                {
                    VehicleId = 4,
                    BrandAndModel = "Nissan Altima",
                    ManufacturingYear = 2017,
                    Color = "Black",
                    EngineCapacity = 2.5m,
                    Price = 16000.00m,
                    Mileage = 30000,
                    PlateNumber = "34AA21",
                    AuctionPrice = 200.0,
                    AdditionalInformation = "Great condition, low mileage",
                    StartTime = new DateTime(2025, 01, 01, 00, 00, 00),
                    EndTime = new DateTime(2025, 12, 31, 23, 59, 59),
                    IsActive = true,
                    Image = "https://www.jonathanmotorcars.com/imagetag/631/3/l/Used-2017-Nissan-Altima-25-SV-Premium.jpg",
                    SellerId = "756997f4-c894-419f-ac98-020d1ed6c75b"
                },
                new Vehicle
                {
                    VehicleId = 5,
                    BrandAndModel = "Chevrolet Malibu",
                    ManufacturingYear = 2017,
                    Color = "Silver",
                    EngineCapacity = 2.4m,
                    Price = 15500.00m,
                    Mileage = 28000,
                    AuctionPrice = 200.0,
                    PlateNumber = "34AA21",
                    AdditionalInformation = "Well-maintained, single owner",
                    StartTime = new DateTime(2025, 01, 01, 00, 00, 00),
                    EndTime = new DateTime(2025, 12, 31, 23, 59, 59),
                    IsActive = true,
                    Image = "https://cdn.carbuzz.com/gallery-images/2016-chevrolet-malibu-carbuzz-489817-1600.jpg",
                    SellerId = "756997f4-c894-419f-ac98-020d1ed6c75b"
                }
            );
        }
    }
}
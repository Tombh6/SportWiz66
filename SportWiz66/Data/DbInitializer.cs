using SportWiz66.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportWiz66.Data
{
    public static class DbInitializer
    {
        //section changed in program.cs in order to extract SportWiz66Context and initalize!


        public static void Initialize(SportWiz66Context _context)
        {
            //this will delete existing database
            //_context.Database.EnsureDeleted();

            //Add Accounts
            if (!(_context.Account.Any(i => i.Type == UserType.Admin)))
            {
                Account TomBh = new Account()
                {
                    Username = "Admin",
                    FirstName = "Tom",
                    LastName = "BenHamo",
                    Email = "Tombh@gmail.com",
                    Password = "TomBenHamo",
                    Type = UserType.Admin,
                    RegistrationDate = DateTime.Now
                };

                _context.Account.Add(TomBh);
                _context.SaveChanges();
            }


            //Addin Brands
            if (!(_context.Brand.Any()))
            {
                Brand notFound = new Brand()
                {
                    Name = "Not Found",
                    Description = "Brand Not Found",
                };
                _context.Brand.Add(notFound);/*In case the brand is'nt found'*/

                Brand Gasp = new Brand()
                {
                    Name = "Gasp",
                    Description = "GASP products are not made for everybody at the gym – It’s only for the most dedicated athletes!",
                    ImageUrl = "/images/gasp/logo.jpg"
                };
                _context.Brand.Add(Gasp);

                Brand Nike = new Brand()
                {
                    Name = "Nike",
                    Description = "Just Do It.",
                    ImageUrl = "/images/nike/logo.jpg"
                };
                _context.Brand.Add(Nike);

                Brand BetterBodies = new Brand()
                {
                    Name = "Better Bodies",
                    Description = "Quality fitness apparel since 1982.",
                    ImageUrl = "/images/betterbodies/logo.png"
                };
                _context.Brand.Add(BetterBodies);

                _context.SaveChanges();
            }


            //Addin Categories
            if (!(_context.Category.Any()))
            {
                Category notFound = new Category() { Name = "Not Found" };
                _context.Category.Add(notFound);

                Category category1 = new Category() { Name = "Hoodies & Long-Sleeves" };
                _context.Category.Add(category1);

                Category category2 = new Category() { Name = "Joggers" };
                _context.Category.Add(category2);

                Category category3 = new Category() { Name = "Pants" };
                _context.Category.Add(category3);

                Category category4 = new Category() { Name = "T-Shirts" };
                _context.Category.Add(category4);

                Category category5 = new Category() { Name = "Trainers" };
                _context.Category.Add(category5);

                Category category6 = new Category() { Name = "Accessories" };
                _context.Category.Add(category6);

                _context.SaveChanges();
            }






            //Addin Products
            if (!(_context.Product.Any()))
            {
                Product notFound = new Product()
                {
                    Name = "Not Found",
                    Price = 0,
                    Description = "Product Not Found",
                    ImageUrl = "/images/Default.png",
                    FrontImageUrl = "/images/Default.png",
                    BackImageUrl = "/images/Default.png",
                    InStock = false,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Not Found"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Not Found"),
                };
                _context.Product.Add(notFound);

                Product hoodie1 = new Product()
                {
                    Name = "Gasp ORIGINAL HOODIE",
                    Price = 350,
                    Description = "A grab and go hoodie designed for your life both inside and out of the gym. " +
                                  "Designed in such a way where the hoodie sits perfect in all the right places.",
                    ImageUrl = "/images/gasp/hoodie/index.png",
                    FrontImageUrl = "/images/gasp/hoodie/front.png",
                    BackImageUrl = "/images/gasp/hoodie/back.png",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Gasp"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Hoodies & Long-Sleeves"),
                };
                _context.Product.Add(hoodie1);

                Product hoodie2 = new Product()
                {
                    Name = "1.2 Ibs hoodie",
                    Price = 700,
                    Description = "This hoodie is, as you can tell from its name, heavy! It’s made out of " +
                                   "American fleece in 80% cotton and 20% polyester and has a weight of 520 grams/sqm!",
                    ImageUrl = "/images/gasp/lbsHoodieBlack/index.png",
                    FrontImageUrl = "/images/gasp/lbsHoodieBlack/front.png",
                    BackImageUrl = "/images/gasp/lbsHoodieBlack/back.png",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Gasp"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Hoodies & Long-Sleeves"),
                };
                _context.Product.Add(hoodie2);

                Product cargoPants = new Product()
                {
                    Name = "Ops edition cargos",
                    Price = 630,
                    Description = "The comfort and versatility of these pants make them perfect for not " +
                                   "only tactical training but any situation where a durable and functional " +
                                   "pair of pants that is designed for the GASP physique is needed.",
                    ImageUrl = "/images/gasp/cargoBlack/index.png",
                    FrontImageUrl = "/images/gasp/cargoBlack/front.png",
                    BackImageUrl = "/images/gasp/cargoBlack/back.png",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Gasp"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Pants"),
                };
                _context.Product.Add(cargoPants);

                Product joggers1 = new Product()
                {
                    Name = "No 89 mesh pant",
                    Price = 630,
                    Description = "Our highly appreciated NO 89 Mesh pants is equipped with huge " +
                                   "logos on the front and back. They are produced in our flexible mesh " +
                                   "fabric and features front pockets with zippers and adjustable drawstrings at the waist.",
                    ImageUrl = "/images/gasp/no90mesh/index.png",
                    FrontImageUrl = "/images/gasp/no90mesh/front.png",
                    BackImageUrl = "/images/gasp/no90mesh/back.png",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Gasp"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Joggers"),
                };
                _context.Product.Add(joggers1);

                Product opsShirt = new Product()
                {
                    Name = "Ops edition tee",
                    Price = 200,
                    Description = "The Ops Edition Tee is made out of 100% polyester with hydropolic " +
                   "finish which is profiled to favor the escape of moisture.",
                    ImageUrl = "/images/gasp/OpsShirt/index.png",
                    FrontImageUrl = "/images/gasp/OpsShirt/front.png",
                    BackImageUrl = "/images/gasp/OpsShirt/back.png",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Gasp"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "T-Shirts"),
                };
                _context.Product.Add(opsShirt);

                Product Duffelbag = new Product()
                {
                    Name = "GASP Duffel bag",
                    Price = 560,
                    Description = "This is not just any duffel bag!" +
                                  " Big GASP logo print on top and orange GASP symbole print on both sides to give you the feeling of roughness.",
                    ImageUrl = "/images/gasp/Duffelbag/index.png",
                    FrontImageUrl = "/images/gasp/Duffelbag/front.png",
                    BackImageUrl = "/images/gasp/Duffelbag/back.png",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Gasp"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Accessories"),
                };
                _context.Product.Add(Duffelbag);

                Product airForce = new Product()
                {
                    Name = "Nike Air Force 1 High '07",
                    Price = 620,
                    Description = "The legend lives on in the Nike Air Force 1 High '07, " +
                                  "an update on the iconic AF-1 that offers classic court style and premium cushioning.",
                    ImageUrl = "/images/nike/airForce/index.jpg",
                    FrontImageUrl = "/images/nike/airForce/front.jpg",
                    BackImageUrl = "/images/nike/airForce/back.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Nike"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Trainers"),
                };
                _context.Product.Add(airForce);

                Product EssentialTee = new Product()
                {
                    Name = "BB Essential Tee",
                    Price = 130,
                    Description = "Essential Tee comes a new fit to the Better Bodies Men’s line. This tee sits just right - slightly fitted around the chest and shoulder," + "" +
                                  " with a looser fit from there on down.",
                    ImageUrl = "/images/betterbodies/EssentialTee/index.png",
                    FrontImageUrl = "/images/betterbodies/EssentialTee/front.png",
                    BackImageUrl = "/images/betterbodies/EssentialTee/back.png",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Better Bodies"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "T-Shirts"),
                };
                _context.Product.Add(EssentialTee);

                Product LogoHoodie = new Product()
                {
                    Name = "BB Logo Hoodie",
                    Price = 230,
                    Description = "This is our first unisex hoodie that we can all wear to the gym, at the gym and casually.",
                    ImageUrl = "/images/betterbodies/LogoHoodie/index.png",
                    FrontImageUrl = "/images/betterbodies/LogoHoodie/front.png",
                    BackImageUrl = "/images/betterbodies/LogoHoodie/back.png",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Better Bodies"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Hoodies & Long-Sleeves"),
                };
                _context.Product.Add(LogoHoodie);

                Product GymBag = new Product()
                {
                    Name = "BB Gym Bag",
                    Price = 215,
                    Description = "BB Gym Bag is the perfect bag for carrying your gym gear or to use when you’re travelling around." +
                                  " It’s equipped with a zipped pocket on the front side and an extra pocket on the inside for your training equipment and your personal belongings.",
                    ImageUrl = "/images/betterbodies/GymBag/index.png",
                    FrontImageUrl = "/images/betterbodies/GymBag/front.png",
                    BackImageUrl = "/images/betterbodies/GymBag/back.png",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Better Bodies"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Accessories"),
                };
                _context.Product.Add(GymBag);
                _context.SaveChanges();
            }




            //Addin Locations
            if (!(_context.Location.Any()))
            {
                Location Home = new Location()
                {
                    Name = "Home",
                    Adress = "Hashunit St 2, Herzliya",
                };

                _context.Location.Add(Home);

                Location College = new Location()
                {
                    Name = "Colman",
                    Adress = "Elie Wiesel St 2, Rishon LeZion",
                };

                _context.Location.Add(College);

                Location Branch = new Location()
                {
                    Name = "Branch",
                    Adress = "TLV Fashion Mall, Tel-Aviv",
                };

                _context.Location.Add(Branch);



                _context.SaveChanges();
            }
        }
    }
}



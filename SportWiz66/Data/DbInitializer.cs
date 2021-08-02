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

                Brand Man = new Brand()
                {
                    Name = "Man",
                    Description = "Man collection of shirts, pants and shoes",
                    ImageUrl = "/images/Man/logo.jpg"
                };
                _context.Brand.Add(Man);

                Brand Woman = new Brand()
                {
                    Name = "Woman",
                    Description = "Woman collection of tops, pants and shoes",
                    ImageUrl = "/images/Woman/logo.jpg"
                };
                _context.Brand.Add(Woman);

                Brand Accessories = new Brand()
                {
                    Name = "Accessories",
                    Description = "Accessories for woman and man.",
                    ImageUrl = "/images/Accessories/acc-logo.jpg"
                };
                _context.Brand.Add(Accessories);

                _context.SaveChanges();
            }


            //Addin Categories
            if (!(_context.Category.Any()))
            {
                Category notFound = new Category() { Name = "Not Found" };
                _context.Category.Add(notFound);

                Category category1 = new Category() { Name = "Tops" };
                _context.Category.Add(category1);

                Category category2 = new Category() { Name = "Pants" };
                _context.Category.Add(category2);

                Category category3 = new Category() { Name = "Shoes" };
                _context.Category.Add(category3);

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

                Product menTop1 = new Product()
                {
                    Name = "Under Armour Sportstyle LC Logo T-Shirt",
                    Price = 100,
                    Description = "60% Cotton, 40% Polyester",
                    ImageUrl = "/images/Man/tops/top1.jpg",
                    FrontImageUrl = "/images/Man/tops/top1.jpg",
                    BackImageUrl = "/images/Man/tops/top1-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Tops"),
                };
                _context.Product.Add(menTop1);

                Product menTop2 = new Product()
                {
                    Name = "Calvin Klein Golf Black Embossed Half Zip top",
                    Price = 200,
                    Description = "High performance meets dynamic design in Calvin Klein Golf’s Embossed half-zip top. " +
                                  "Easy to pack away, this garment provides a comfortable, " +
                                  "insulating layer allowing you to play your best whatever the weather. " +
                                  "Smooth jersey fabric has a fleece backing and plenty of stretch. Machine washable. " ,
                    ImageUrl = "/images/Man/tops/top2.jpg",
                    FrontImageUrl = "/images/Man/tops/top2.jpg",
                    BackImageUrl = "/images/Man/tops/top2-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Tops"),
                };
                _context.Product.Add(menTop2);

                Product menTop3 = new Product()
                {
                    Name = "The North Face® Simple Dome T-Shirt",
                    Price = 200,
                    Description = "The Simple Dome T-shirt is just that," +
                                  " simple and easy to wear." +
                                  " Made from lightweight cotton with a standard fit and crew neck, " +
                                  "it's a timeless classic for low-intensity activities. 100% Cotton.",
                    ImageUrl = "/images/Man/tops/top3.jpg",
                    FrontImageUrl = "/images/Man/tops/top3.jpg",
                    BackImageUrl = "/images/Man/tops/top3-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Tops"),
                };
                _context.Product.Add(menTop3);

                Product menTop4 = new Product()
                {
                    Name = "adidas Navy Tiro 21 Track Top",
                    Price = 120,
                    Description = "You can't train for real life. " +
                                  "But you can look good while you live it." +
                                  "This adidas Tiro 21 Training Top was born from football culture. " +
                                  "Made from recycled materials, it features moisture-absorbing " +
                                  "AEROREADY to keep you dry and confident. " +
                                  "The stand-up collar and cuff thumbholes add on-demand coverage. " +
                                  "It fits slim for when you need to move. This product is made with Primegreen, " +
                                  "a series of high-performance recycled materials. " +
                                  "100% Recycled polyester. ",
                    ImageUrl = "/images/Man/tops/top4.jpg",
                    FrontImageUrl = "/images/Man/tops/top4.jpg",
                    BackImageUrl = "/images/Man/tops/top4-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Tops"),
                };
                _context.Product.Add(menTop4);

                Product menTop5 = new Product()
                {
                    Name = "Nike Club T-Shirt",
                    Price = 150,
                    Description = "Classic comfort. The Nike Sportswear men's t-shirt " +
                                  "sets you up with a soft cotton jersey feel and a Nike corporate" +
                                  " logo embroidered on the chest. " +
                                  "Cotton fabric is soft and comfortable.",
                    ImageUrl = "/images/Man/tops/top5.jpg",
                    FrontImageUrl = "/images/Man/tops/top5.jpg",
                    BackImageUrl = "/images/Man/tops/top5-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Tops"),
                };
                _context.Product.Add(menTop5);


                Product manPants1 = new Product()
                {
                    Name = "Nike Club Shorts",
                    Price = 180,
                    Description = "CLASSIC COMFORT. IMPROVED FIT. " +
                                  "Made from soft cotton jersey, " +
                                  "the Nike Sportswear Club Shorts are perfect " +
                                  "for all-day wear. And with an adjusted rise " +
                                  "and a shorter inseam than previous versions, " +
                                  "they're more comfortable than ever. Soft Feel Jersey fabric is " +
                                  "soft and comfortable for all-day wear. Improved Fit An adjusted rise " +
                                  "and a slightly shorter inseam improve the fit with more room to move. " +
                                  "Adjustable Comfort An elastic drawcord waistband provides adjustable comfort.",
                    ImageUrl = "/images/Man/pants/pants1.jpg",
                    FrontImageUrl = "/images/Man/pants/pants1.jpg",
                    BackImageUrl = "/images/Man/pants/pants1-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Pants"),
                };
                _context.Product.Add(manPants1);


                Product manPants2 = new Product()
                {
                    Name = "Nike Club Joggers black",
                    Price = 250,
                    Description = "CLASSIC COMFORT. A closet staple, the Nike Sportswear Club Fleece " +
                                  "Joggers combine classic style with the soft comfort of " +
                                  "fleece for an elevated, everyday look that you really" +
                                  " can wear every day. Soft comfort – brushed-back fleece " +
                                  "fabric feels soft and smooth. Joggers style – " +
                                  "ribbed cuffs give you that classic joggers look and show " +
                                  "off your kicks. Personalised fit – elastic waistband " +
                                  "with an adjustable drawcord lets you personalise the fit.",
                    ImageUrl = "/images/Man/pants/pants2.jpg",
                    FrontImageUrl = "/images/Man/pants/pants2.jpg",
                    BackImageUrl = "/images/Man/pants/pants2-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Pants"),
                };
                _context.Product.Add(manPants2);


                Product manPants3 = new Product()
                {
                    Name = "Nike Club Joggers blue",
                    Price = 250,
                    Description = "CLASSIC COMFORT. A closet staple, the Nike Sportswear" +
                                  " Club Fleece Joggers combine classic style with the" +
                                  " soft comfort of fleece for an elevated, " +
                                  "everyday look that you really can wear every day. " +
                                  "Soft comfort – brushed-back fleece fabric feels" +
                                  " soft and smooth. Joggers style – ribbed cuffs give you " +
                                  "that classic joggers look and show off your kicks. " +
                                  "Personalised fit – elastic waistband with an adjustable " +
                                  "drawcord lets you personalise the fit.",
                    ImageUrl = "/images/Man/pants/pants3.jpg",
                    FrontImageUrl = "/images/Man/pants/pants3.jpg",
                    BackImageUrl = "/images/Man/pants/pants3-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Pants"),
                };
                _context.Product.Add(manPants3);

                Product manPants4 = new Product()
                {
                    Name = "Adidas sport pants",
                    Price = 230,
                    Description = "Casual Fridays at the office, nights out with " +
                                  "your friends, off-duty days relaxing at home. " +
                                  "When you've got these adidas essential pants," +
                                  " you're ready for anything. Classic 3-Stripes meet " +
                                  "ribbed cuffs with retro style. Moisture-absorbing " +
                                  "AEROREADY provides dry comfort when you start to move. " +
                                  "This product is made with Primegreen, a series " +
                                  "of high-performance recycled materials.",
                    ImageUrl = "/images/Man/pants/pants4.jpg",
                    FrontImageUrl = "/images/Man/pants/pants4.jpg",
                    BackImageUrl = "/images/Man/pants/pants4-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Pants"),
                };
                _context.Product.Add(manPants4);

                Product manPants5 = new Product()
                {
                    Name = "Under Armour Challenger 3 Joggers",
                    Price = 215,
                    Description = "The UA Challenger collection has all your " +
                                  "go-to soccer gear that's built to keep you staying " +
                                  "light and feeling fast on the field. Now on its third" +
                                  " evolution, this gear just keeps getting better every season.",
                    ImageUrl = "/images/Man/pants/pants5.jpg",
                    FrontImageUrl = "/images/Man/pants/pants5.jpg",
                    BackImageUrl = "/images/Man/pants/pants5-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Pants"),
                };
                _context.Product.Add(manPants5);

                Product womanTop1 = new Product()
                {
                    Name = "Nike DriFIT Icon Clash Training Vest",
                    Price = 200,
                    Description = "COMFORT IN THE FORECAST. The Nike Dri-FIT Tank " +
                                  "takes the classic Swoosh up a notch with a " +
                                  "sky-dye print. Soft, sweat-wicking fabric helps " +
                                  "you stay dry and comfortable whether your workout " +
                                  "takes you indoors or outdoors. Soft fabric with Dri-FIT" +
                                  " technology helps you stay dry and comfortable. " +
                                  "Longer openings at the arms let you move freely. " +
                                  "Standard fit for a relaxed, easy feel. 58% Cotton, 42% Polyester.",
                    ImageUrl = "/images/Woman/tops/top1.jpg",
                    FrontImageUrl = "/images/Woman/tops/top1.jpg",
                    BackImageUrl = "/images/Woman/tops/top1-1.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Woman"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Top"),
                };
                _context.Product.Add(womanTop1);

                Product womanTop3 = new Product()
                {
                    Name = "Nike Dri-FIT Colourblock Stripe Training Vest ",
                    Price = 230,
                    Description = "SOFT COMFORT TO KEEP YOU DRY. " +
                                  "The Nike Dri-FIT Tank has sweat-wicking power " +
                                  "to help keep you dry from warmups through cool downs. " +
                                  "Soft fabric is made with at least 75% sustainable materials, " +
                                  "using a blend of both recycled polyester and organic cotton" +
                                  " fibres. The blend is at least 10% recycled fibres or at " +
                                  "least 10% organic cotton fibres. Sweat-Wicking Power: " +
                                  "Lightweight, supersoft fabric with Dri-FIT technology moves " +
                                  "sweat away from your skin for quicker evaporation to help" +
                                  " you stay dry and comfortable. Focused Coverage: The high neckline " +
                                  "helps keep you covered as you bend and stretch. " +
                                  "Longer openings at the arms let you move freely. " +
                                  "Standard fit for a relaxed, easy feel. Colourblock stripes. " +
                                  "Outlined rubber Swoosh design on chest. " +
                                  "75% Polyester, 13% Cotton, 12% Viscose",
                    ImageUrl = "/images/Woman/tops/top3.jpg",
                    FrontImageUrl = "/images/Woman/tops/top3.jpg",
                    BackImageUrl = "/images/Woman/tops/top3-3.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Woman"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Top"),
                };
                _context.Product.Add(womanTop3);

                Product womanTop4 = new Product()
                {
                    Name = "Nike DriFIT Icon Clash Training Vest",
                    Price = 150,
                    Description = "UNTAMED COMFORT. The Nike Yoga Dri-FIT Tank " +
                                  "has a loose, comfortable fit with sweat-wicking " +
                                  "power to keep you dry and moving naturally from your " +
                                  "first stretch to your last set. Zebra print in a Swoosh " +
                                  "design gives them a taste of your wild side. " +
                                  "Dri-FIT technology helps you stay dry and comfortable. " +
                                  "Soft jersey fabric has curved hems to keep you covered " +
                                  "and comfortable. 57% Cotton, 43% Polyester",
                    ImageUrl = "/images/Woman/tops/top4.jpg",
                    FrontImageUrl = "/images/Woman/tops/top4.jpg",
                    BackImageUrl = "/images/Woman/tops/top4-4.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Woman"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Top"),
                };
                _context.Product.Add(womanTop4);

                Product womanTop5 = new Product()
                {
                    Name = "Short Sleeve V-Neck Sports Top ",
                    Price = 165,
                    Description = "Reach your goals in style with this comfy sports top, " +
                                  "designed with sweat-wicking fabric to help you stay cool " +
                                  "and comfortable all the way through your workout. " +
                                  "Styled in a relaxed shape with short sleeves, " +
                                  "it's finished with a flattering V-neck and stepped hem. " +
                                  "Other colours available. Machine washable. 100% Polyester.",
                    ImageUrl = "/images/Woman/tops/top5.jpg",
                    FrontImageUrl = "/images/Woman/tops/top5.jpg",
                    BackImageUrl = "/images/Woman/tops/top5-5.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Woman"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Top"),
                };
                _context.Product.Add(womanTop5);

                Product womanPants1 = new Product()
                {
                    Name = "Nike Black Favourite Leggings ",
                    Price = 200,
                    Description = " SOFT AND STRETCHY. For the comfort you love, " +
                                  "there's the Nike sportswear favourites leggings. " +
                                  "Move, dance and play with ease. This fabric feels soft " +
                                  "and stretchy. Complete with elastic waistband brings a snug" +
                                  " fit. Complete with tight fit for a body-hugging feel. " +
                                  "92% Cotton, 8% Elastane.",
                    ImageUrl = "/images/Woman/Pants/Pants1.jpg",
                    FrontImageUrl = "/images/Woman/Pants/Pants1.jpg",
                    BackImageUrl = "/images/Woman/Pants/Pants1-1.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Woman"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Pants"),
                };
                _context.Product.Add(womanPants1);

                Product womanPants2 = new Product()
                {
                    Name = "Nike Black/Pink Swoosh Leggings ",
                    Price = 250,
                    Description = "Fish for compliments! This baby ivory organic " +
                                  "cotton bodysuit from Mini Rodini features an all over " +
                                  "repeating fish print. 92% Cotton, 8% Elastane.",
                    ImageUrl = "/images/Woman/Pants/Pants2.jpg",
                    FrontImageUrl = "/images/Woman/Pants/Pants2.jpg",
                    BackImageUrl = "/images/Woman/Pants/Pants2-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Woman"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Pants"),
                };
                _context.Product.Add(womanPants2);

                Product womanPants3 = new Product()
                {
                    Name = "Sculpting Sports Leggings",
                    Price = 170,
                    Description = "Jump higher, run faster, work harder; " +
                                  "our Ultimate Leggings have got you covered. " +
                                  "Designed for all types of fitness, they feature extra " +
                                  "stretch to lift and shape the bum, waist and hips, " +
                                  "staying opaque at all times no matter how hard " +
                                  "you bend and stretch. With a high-waisted fit that won't " +
                                  "roll down or dig in, they feature a hidden front pocket to " +
                                  "keep small valuables safe. And don't worry about breaking a " +
                                  "sweat – our moisture-wicking fabric will keep you cool and comfortable. " +
                                  "Tested and approved by Women's Health Lab for performance, fit, " +
                                  "breathability and opacity during exercise. " +
                                  "Regular to fit inside leg 29 / 74cm.Machine washable. 70 % Recycled polyester,30 % Elastane.",
                    ImageUrl = "/images/Woman/Pants/Pants3.jpg",
                    FrontImageUrl = "/images/Woman/Pants/Pants3.jpg",
                    BackImageUrl = "/images/Woman/Pants/Pants3-3.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Woman"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Pants"),
                };
                _context.Product.Add(womanPants3);


                Product womanPants4 = new Product()
                {
                    Name = "adidas Originals 3 Stripe Leggings",
                    Price = 230,
                    Description = "adidas reaches back into the archives to bring iconic " +
                                  "designs to the modern streets. Translating the " +
                                  "look of classic track pants, these leggings have " +
                                  "three-stripes down the sides. The stretchy tights have " +
                                  "a long, lean silhouette and a small Trefoil logo on the leg. " +
                                  "92% Cotton, 8% Elastane.",
                    ImageUrl = "/images/Woman/Pants/Pants4.jpg",
                    FrontImageUrl = "/images/Woman/Pants/Pants4.jpg",
                    BackImageUrl = "/images/Woman/Pants/Pants4-4.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Woman"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Pants"),
                };
                _context.Product.Add(womanPants4);


                Product womanPants5 = new Product()
                {
                    Name = "Sculpting Sports Capri Leggings ",
                    Price = 200,
                    Description = "Stretch further and work harder; " +
                                  "our capri-length technical leggings have got you covered. " +
                                  "They're designed using a moisture-wicking fabric, " +
                                  "with a high-waisted fit that won't roll down or dig in. " +
                                  "Regular to fit inside leg 29 / 74cm.Machine washable. " +
                                  "70 % Recycled polyester, 30 % Elastane.",
                    ImageUrl = "/images/Woman/Pants/Pants5.jpg",
                    FrontImageUrl = "/images/Woman/Pants/Pants5.jpg",
                    BackImageUrl = "/images/Woman/Pants/Pants5-5.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Woman"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Pants"),
                };
                _context.Product.Add(womanPants5);


                Product manShoes1 = new Product()
                {
                    Name = "Nike Air Max VGR Trainers",
                    Price = 300,
                    Description = "Time happens—keep up in your Nike Air Max VG-R, " +
                                  "a new gesture of speed and air that lets you occupy the now. " +
                                  "Inspired by the Air Max 95, the new design quickens the shape " +
                                  "with hybrid lines evoking GT race cars and the driver's " +
                                  "fast-paced breathing. Monofilament mesh on the upper adds " +
                                  "rich texture and ventilation while Air cushioning in the heel " +
                                  "softens the ride. Winning is just too easy when the shoes " +
                                  "are an extension of your body.",
                    ImageUrl = "/images/Man/shoes/shoes1.jpg",
                    FrontImageUrl = "/images/Man/shoes/shoes1.jpg",
                    BackImageUrl = "/images/Man/shoes/shoes1-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Shoes"),
                };
                _context.Product.Add(manShoes1);

                Product manShoes2 = new Product()
                {
                    Name = "adidas Originals Gazelle Trainers",
                    Price = 250,
                    Description = "These shoes are a faithful reissue of the 1991 Gazelle. " +
                                  "They retain the design lines and graceful silhouette of " +
                                  "the original. The soft nubuck upper is done in archival " +
                                  "colours with contrast 3-Stripes and heel tab.(shoes2)",
                    ImageUrl = "/images/Man/shoes/shoes2.jpg",
                    FrontImageUrl = "/images/Man/shoes/shoes2.jpg",
                    BackImageUrl = "/images/Man/shoes/shoes2-.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Shoes"),
                };
                _context.Product.Add(manShoes2);

                Product manShoes3 = new Product()
                {
                    Name = "Nike Run Swift 2 Trainers",
                    Price = 270,
                    Description = "DURABLE COMFORT FOR THE CASUAL RUNNER. " +
                                  "The Nike Run Swift 2 is designed with shorter miles in mind. " +
                                  "An updated yet familiar shape delivers breathable stability. " +
                                  "A durable outsole keeps you supported no matter your pace. " +
                                  "Classic Look and Feel: the upper features all the classic " +
                                  "touch points at the collar and tongue for easy on-and-off. " +
                                  "Mesh throughout provides breathability. Webbing along the " +
                                  "midfoot connects to your laces for a secure, comfortable fit. " +
                                  "Step With Comfort: foam cushioning delivers a soft underfoot " +
                                  "feel. A higher foam height gives you a plush sensation with " +
                                  "every step. A cushioned collar provides support for your heel. " +
                                  "A rubber outsole delivers durable traction.",
                    ImageUrl = "/images/Man/shoes/shoes3.jpg",
                    FrontImageUrl = "/images/Man/shoes/shoes3.jpg",
                    BackImageUrl = "/images/Man/shoes/shoes3-.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Man"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Shoes"),
                };
                _context.Product.Add(manShoes3);

                Product womanShoes1 = new Product()
                {
                    Name = "Nike Run Revolution 5 Trainers",
                    Price = 250,
                    Description = "The Nike Revolution 5 cushions your stride with " +
                                  "soft foam to keep you running in comfort. " +
                                  "Lightweight knit material wraps your foot in breathable " +
                                  "support, while a minimalist design fits in just about " +
                                  "anywhere your day takes you. Breathable Support Lightweight " +
                                  "knit textile wraps your foot in breathable comfort. " +
                                  "Reinforced heel and no-sew overlays lend support and " +
                                  "durability. Lightweight Cushioning Soft foam midsole " +
                                  "delivers a smooth, stable ride. Its textured outer wall " +
                                  "helps reduce weight and hide creases. Durable, Flexible " +
                                  "Traction Rubber outsole offers durable traction on a " +
                                  "variety of surfaces. Spaces in the tread let your foot " +
                                  "flex naturally. Upper - Textile, Lining & Sock - Textile, " +
                                  "Sole - Other Materials.",
                    ImageUrl = "/images/Woman/shoes/shoes1.jpg",
                    FrontImageUrl = "/images/Woman/shoes/shoes1.jpg",
                    BackImageUrl = "/images/Woman/shoes/shoes1-1.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Moman"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Shoes"),
                };
                _context.Product.Add(womanShoes1);

                Product womanShoes2 = new Product()
                {
                    Name = "Nike Run Revolution 5 Trainers ",
                    Price = 250,
                    Description = "The Nike Revolution 5 cushions your stride with " +
                                  "soft foam to keep you running in comfort. " +
                                  "Lightweight knit material wraps your foot in breathable " +
                                  "support, while a minimalist design fits in just about " +
                                  "anywhere your day takes you. Breathable Support Lightweight " +
                                  "knit textile wraps your foot in breathable comfort. " +
                                  "Reinforced heel and no-sew overlays lend support and " +
                                  "durability. Lightweight Cushioning Soft foam midsole " +
                                  "delivers a smooth, stable ride. Its textured outer wall " +
                                  "helps reduce weight and hide creases. Durable, Flexible " +
                                  "Traction Rubber outsole offers durable traction on a" +
                                  " variety of surfaces. Spaces in the tread let your foot " +
                                  "flex naturally. Upper - Textile, Lining & Sock - Textile, " +
                                  "Sole - Other Materials.",
                    ImageUrl = "/images/Woman/shoes/shoes2.jpg",
                    FrontImageUrl = "/images/Woman/shoes/shoes2.jpg",
                    BackImageUrl = "/images/Woman/shoes/shoes2-2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Moman"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Shoes"),
                };
                _context.Product.Add(womanShoes2);


                Product womanShoes3 = new Product()
                {
                    Name = "Nike Run Revolution 5 Trainers ",
                    Price = 250,
                    Description = "The Nike Revolution 5 cushions your stride with " +
                                  "soft foam to keep you running in comfort. " +
                                  "Lightweight knit material wraps your foot in breathable " +
                                  "support, while a minimalist design fits in just about " +
                                  "anywhere your day takes you. Breathable Support Lightweight " +
                                  "knit textile wraps your foot in breathable comfort. " +
                                  "Reinforced heel and no-sew overlays lend support and " +
                                  "durability. Lightweight Cushioning Soft foam midsole " +
                                  "delivers a smooth, stable ride. Its textured outer wall " +
                                  "helps reduce weight and hide creases. Durable, Flexible " +
                                  "Traction Rubber outsole offers durable traction on a" +
                                  " variety of surfaces. Spaces in the tread let your foot " +
                                  "flex naturally. Upper - Textile, Lining & Sock - Textile, " +
                                  "Sole - Other Materials.",
                    ImageUrl = "/images/Woman/shoes/shoes3.jpg",
                    FrontImageUrl = "/images/Woman/shoes/shoes3.jpg",
                    BackImageUrl = "/images/Woman/shoes/shoes3-3.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Moman"),
                    Category = _context.Category.FirstOrDefault(c => c.Name == "Shoes"),
                };
                _context.Product.Add(womanShoes3);

                Product Accessories1 = new Product()
                {
                    Name = "Black Next Active Protein Shaker ",
                    Price = 100,
                    Description = "Protein shaker for mixing supplements throughout the day. " +
                                  "With unique screw-on storage. Stay on top of your nutrition, " +
                                  "wherever you are. 100% Rubber.",
                    ImageUrl = "/images/Accessories/acc1.jpg",
                    FrontImageUrl = "/images/Accessories/acc1.jpg",
                    BackImageUrl = "/images/Accessories/acc1.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Accessories"),
                };
                _context.Product.Add(Accessories1);

                Product Accessories2 = new Product()
                {
                    Name = "Nike Adult Swoosh Cap ",
                    Price = 150,
                    Description = " Unisex Nike Sportswear Heritage86 Cap is a classic, " +
                                  "six-panel design with sweat-wicking support. " +
                                  "It has a metal Swoosh design trademark ingot in the " +
                                  "front and an adjustable closure for the perfect fit. " +
                                  "Dri-FIT technology helps you stay dry and comfortable. " +
                                  "Embroidered eyelets enhance ventilation. " +
                                  "Adjustable back closure for a custom fit. " +
                                  "Metal Swoosh design trademark ingot on the front. 100% Polyester.",
                    ImageUrl = "/images/Accessories/acc2.jpg",
                    FrontImageUrl = "/images/Accessories/acc2.jpg",
                    BackImageUrl = "/images/Accessories/acc2.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Accessories"),
                };
                _context.Product.Add(Accessories2);

                Product Accessories3 = new Product()
                {
                    Name = "Regatta Grey Cassian Washed Cap",
                    Price = 180,
                    Description = "The men's Cassian Cap from Regatta is made " +
                                  "from naturally breathable Coolweave Cotton twill. " +
                                  "With embroidered eyelets for a light and breathable " +
                                  "fit and an adjustable fastening at the back. " +
                                  "Machine washable. 100% Cotton.",
                    ImageUrl = "/images/Accessories/acc3.jpg",
                    FrontImageUrl = "/images/Accessories/acc3.jpg",
                    BackImageUrl = "/images/Accessories/acc3.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Accessories"),
                };
                _context.Product.Add(Accessories3);

                Product Accessories4 = new Product()
                {
                    Name = "Nike Black Elemental Backpack ",
                    Price = 200,
                    Description = "CLASSIC DESIGN. DURABLE STORAGE. " +
                                  "The Nike Elemental 2.0 Backpack is a " +
                                  "new spin on an old classic. Its durable " +
                                  "design features two large compartments and two external " +
                                  "pockets for small item storage, while the padded shoulder " +
                                  "straps offer supportive comfort. Dual-zippered main " +
                                  "compartment for spacious, secure storage. Multiple " +
                                  "easy-access pockets for convenient, organized storage. " +
                                  "Padded shoulder straps and back panel offer support. " +
                                  "Haul loop offers a convenient carrying option. " +
                                  "Height 48cm Width 30.5cm Depth 15cm 100% Polyester.",
                    ImageUrl = "/images/Accessories/acc5.jpg",
                    FrontImageUrl = "/images/Accessories/acc5.jpg",
                    BackImageUrl = "/images/Accessories/acc5-5.jpg",
                    InStock = true,
                    Brand = _context.Brand.FirstOrDefault(b => b.Name == "Accessories"),
                };
                _context.Product.Add(Accessories4);
                _context.SaveChanges();
            }


            //Addin Locations
            if (!(_context.Location.Any()))
            {
                Location Home = new Location() /*Home addres*/
                {
                    Name = "Home",
                    Adress = "Hashunit St 2, Herzliya",
                };

                _context.Location.Add(Home);

                Location College = new Location() /*college addres*/
                {
                    Name = "Colman",
                    Adress = "Elie Wiesel St 2, Rishon LeZion",
                };

                _context.Location.Add(College);

                Location Branch = new Location() /*Branch addres*/
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



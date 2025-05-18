using Microsoft.AspNetCore.Identity;
using Quick_Shop.Models;
using Quick_Shop.Models.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quick_Shop.Data
{
    public class AppDbInitializer
    {
        public static async Task Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<Users>>();

                context.Database.EnsureCreated();

                // Seed Roles
                string[] roles = new[] { "Customer", "Admin" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Seed Admin User
                var adminEmail = "admin@quickshop.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new Users
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FullName = "Admin User"
                    };
                    var adminPassword = "Admin@123"; // كلمة مرور قوية
                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                    else
                    {
                        // لو حصل خطأ أثناء إنشاء المستخدم، ممكن تسجل الخطأ
                        throw new System.Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }

                // Seed Products
                if (!context.Products.Any())
                {
                    context.Products.AddRange(new List<Product>
                    {
                        // Electronics
                        new Product()
                        {
                            Name = "iPhone 15",
                            Price = 799.00m,
                            OldPrice = 899.00m,
                            Category = "electronics",
                            Image = "../img/iphone.png",
                            Images = "[\"../img/iphone1.png\", \"../img/iphone2.png\"]",
                            Description = "The Apple iPhone 15 features a 6.1-inch Super Retina XDR display, A16 Bionic chip for blazing-fast performance, and a dual 48MP camera system with advanced computational photography. Supports 5G, USB-C charging, and iOS 18 for a seamless user experience."
                        },
                        new Product()
                        {
                            Name = "Galaxy S23",
                            Price = 799.99m,
                            OldPrice = 899.99m,
                            Category = "electronics",
                            Image = "../img/samsung.png",
                            Images = "[\"../img/samsung1.png\", \"../img/samsung2.png\"]",
                            Description = "Samsung Galaxy S23 offers a 6.1-inch Dynamic AMOLED 2X display with 120Hz refresh rate, powered by Snapdragon 8 Gen 2. Includes a facet 50MP camera system, 5G connectivity, and 3900mAh battery with fast charging, perfect for power users."
                        },
                        new Product()
                        {
                            Name = "Dell XPS 13",
                            Price = 1399.99m,
                            OldPrice = null,
                            Category = "electronics",
                            Image = "../img/lapdell.png",
                            Images = "[\"../img/lapdell1.png\", \"../img/lapdell2.png\"]",
                            Description = "Dell XPS 13 Plus 9320 is a premium ultrabook with a 13.4-inch 4K+ touchscreen, Intel Core i7-1260P, 16GB RAM, and 512GB SSD. Its sleek design, vibrant display, and long battery life make it ideal for professionals and students."
                        },
                        new Product()
                        {
                            Name = "HP Spectre x360",
                            Price = 1599.99m,
                            OldPrice = 1799.99m,
                            Category = "electronics",
                            Image = "../img/laphp.png",
                            Images = "[\"../img/laphp1.png\", \"../img/laphp2.png\"]",
                            Description = "HP Spectre x360 is a 2-in-1 convertible laptop with a 13.5-inch OLED display, Intel Core i7-1255U, 16GB RAM, and 1TB SSD. Features a 360-degree hinge, stylus support, and up to 15 hours of battery life for versatile use."
                        },
                        new Product()
                        {
                            Name = "Sony Headphones",
                            Price = 349.99m,
                            OldPrice = 399.99m,
                            Category = "electronics",
                            Image = "../img/head.png",
                            Images = "[\"../img/head1.png\", \"../img/head2.png\"]",
                            Description = "Sony WH-1000XM5 headphones deliver industry-leading noise cancellation, 40mm drivers for rich audio, and up to 30 hours of battery life. With Bluetooth 5.2, multipoint connection, and touch controls, they’re perfect for music and calls."
                        },
                        new Product()
                        {
                            Name = "Wireless Mouse",
                            Price = 59.99m,
                            OldPrice = null,
                            Category = "electronics",
                            Image = "../img/mouse.png",
                            Images = "[\"../img/mouse1.png\", \"../img/mouse2.png\"]",
                            Description = "Logitech MX Master 3 is an advanced wireless mouse with ergonomic design, 4000 DPI sensor, and customizable buttons. Supports multi-device pairing, USB-C charging, and works seamlessly on glass surfaces for productivity and gaming."
                        },

                        // Fashion
                        new Product()
                        {
                            Name = "Levi's Jeans",
                            Price = 69.50m,
                            OldPrice = 89.50m,
                            Category = "fashion",
                            Image = "../img/Levi.png",
                            Images = "[\"../img/Levi1.png\", \"../img/Levi2.png\"]",
                            Description = "Levi’s 501 Original Jeans offer a classic straight-fit design with 100% cotton denim for durability and comfort. Features a button fly closure, five-pocket styling, and a timeless look perfect for casual or semi-formal occasions."
                        },
                        new Product()
                        {
                            Name = "Adidas Sneakers",
                            Price = 180.00m,
                            OldPrice = 200.00m,
                            Category = "fashion",
                            Image = "../img/Running.png",
                            Images = "[\"../img/Running1.png\", \"../img/Running2.png\"]",
                            Description = "Adidas Ultraboost sneakers combine style and performance with Boost cushioning for superior comfort. Features a breathable Primeknit upper, Continental rubber outsole, and a sleek design ideal for running, gym workouts, or casual wear."
                        },
                        new Product()
                        {
                            Name = "Classic Jeans",
                            Price = 49.99m,
                            OldPrice = null,
                            Category = "fashion",
                            Image = "../img/Classic.png",
                            Images = "[\"../img/Classic1.png\", \"../img/Classic2.png\"]",
                            Description = "Starter Flower Classic Jeans feature a straight-fit cut with durable denim construction. Designed for all-day comfort with a relaxed waist and versatile style, these jeans are perfect for casual outings or everyday wear."
                        },
                        new Product()
                        {
                            Name = "Ravin Jacket",
                            Price = 129.99m,
                            OldPrice = 149.99m,
                            Category = "fashion",
                            Image = "../img/Ravin.png",
                            Images = "[\"../img/Ravin1.png\", \"../img/Ravin2.png\"]",
                            Description = "Ravin Men’s Black Jacket is a lightweight, water-resistant fleece ideal for outdoor adventures. With zippered pockets, adjustable hood, and a modern fit, it provides warmth and style for hiking, camping, or urban exploration."
                        },
                        new Product()
                        {
                            Name = "Andora Cap",
                            Price = 29.99m,
                            OldPrice = null,
                            Category = "fashion",
                            Image = "../img/Andora.png",
                            Images = "[\"../img/Andora1.png\", \"../img/Andora2.png\"]",
                            Description = "Andora Men’s Classic Wool Cap is an adjustable baseball cap with a premium wool blend and iconic logo. Its curved brim and breathable design make it a stylish accessory for casual outings or sports activities."
                        },
                        new Product()
                        {
                            Name = "Ray-Ban Sunglasses",
                            Price = 161.00m,
                            OldPrice = 181.00m,
                            Category = "fashion",
                            Image = "../img/Ray-Ban.png",
                            Images = "[\"../img/Ray-Ban1.png\", \"../img/Ray-Ban2.png\"]",
                            Description = "Ray-Ban Wayfarer Sunglasses feature a timeless square design with polarized lenses for UV protection and glare reduction. Crafted with durable acetate frames, they’re perfect for driving, outdoor activities, or everyday style."
                        },
                        new Product()
                        {
                            Name = "Crocs Clogs",
                            Price = 54.99m,
                            OldPrice = null,
                            Category = "fashion",
                            Image = "../img/Crocs.png",
                            Images = "[\"../img/Crocs1.png\", \"../img/Crocs2.png\"]",
                            Description = "Crocs Classic Clogs offer lightweight comfort with Croslite cushioning and a ventilated upper for breathability. Ideal for casual wear, beach trips, or lounging, these unisex clogs are durable and easy to clean."
                        },

                        // Books
                        new Product()
                        {
                            Name = "Atomic Habits",
                            Price = 27.00m,
                            OldPrice = 30.00m,
                            Category = "books",
                            Image = "../img/atomic.png",
                            Images = "[\"../img/atomic1.png\", \"../img/atomic2.png\"]",
                            Description = "Atomic Habits by James Clear is a bestselling guide to building good habits and breaking bad ones. With practical strategies and real-world examples, it teaches how small changes can lead to remarkable results in personal and professional life."
                        },
                        new Product()
                        {
                            Name = "Subtle Art",
                            Price = 24.99m,
                            OldPrice = null,
                            Category = "books",
                            Image = "../img/HarperCollins.jpg",
                            Images = "[\"../img/HarperCollins.jpg\", \"../img/HarperCollins.jpg\"]",
                            Description = "The Subtle Art of Not Giving a F*ck by Mark Manson offers a counterintuitive approach to living a meaningful life. This bold self-help book emphasizes focusing on what truly matters and embracing life’s challenges with honesty and humor."
                        },
                        new Product()
                        {
                            Name = "Can't Hurt Me",
                            Price = 28.00m,
                            OldPrice = 32.00m,
                            Category = "books",
                            Image = "../img/cant hurt me.jpg",
                            Images = "[\"../img/cant hurt me.jpg\", \"../img/cant hurt me.jpg\"]",
                            Description = "Can’t Hurt Me by David Goggins is an inspiring memoir of a Navy SEAL who overcame adversity through mental toughness. Packed with lessons on resilience and self-discipline, it’s a must-read for anyone seeking personal growth."
                        },
                        new Product()
                        {
                            Name = "48 Laws of Power",
                            Price = 26.00m,
                            OldPrice = null,
                            Category = "books",
                            Image = "../img/power.jpg",
                            Images = "[\"../img/power.jpg\", \"../img/power.jpg\"]",
                            Description = "The 48 Laws of Power by Robert Greene is a strategic guide to mastering power dynamics in personal and professional settings. Drawing from historical examples, it offers timeless insights for navigating complex social and business environments."
                        },
                        new Product()
                        {
                            Name = "Think and Grow Rich",
                            Price = 17.99m,
                            OldPrice = 20.99m,
                            Category = "books",
                            Image = "../img/PENGUIN.jpg",
                            Images = "[\"../img/PENGUIN.jpg\", \"../img/PENGUIN.jpg\"]",
                            Description = "Think and Grow Rich by Napoleon Hill is a classic guide to achieving wealth and success. Based on interviews with successful individuals, it outlines principles like desire, faith, and persistence to help readers unlock their potential."
                        },
                        new Product()
                        {
                            Name = "Psychology of Money",
                            Price = 19.00m,
                            OldPrice = null,
                            Category = "books",
                            Image = "../img/Psychology.jpg",
                            Images = "[\"../img/Psychology.jpg\", \"../img/Psychology.jpg\"]",
                            Description = "The Psychology of Money by Morgan Housel explores how behavior influences financial success. With engaging stories and practical insights, it teaches timeless lessons on saving, investing, and making smarter money decisions."
                        },

                        // Home & Kitchen
                        new Product()
                        {
                            Name = "Instant Pot",
                            Price = 129.99m,
                            OldPrice = 149.99m,
                            Category = "home-kitchen",
                            Image = "../img/Multi Cooker.png",
                            Images = "[\"../img/Multi Cooker1.png\", \"../img/Multi Cooker2.png\"]",
                            Description = "Instant Pot Duo 7-in-1 is a versatile multi-cooker with pressure cooking, slow cooking, steaming, and more. Features a 6-quart capacity, 14 smart programs, and stainless steel inner pot, making meal prep fast and easy for families."
                        },
                        new Product()
                        {
                            Name = "Black & Decker Mixer",
                            Price = 299.99m,
                            OldPrice = null,
                            Category = "home-kitchen",
                            Image = "../img/Black & Decker.png",
                            Images = "[\"../img/Black & Decker1.png\", \"../img/Black & Decker2.png\"]",
                            Description = "Black & Decker 5-quart Stand Mixer offers 10 speed settings and a 300-watt motor for versatile baking and cooking. Includes a mixing bowl, dough hook, and whisk attachments, ideal for home chefs creating breads, cakes, and more."
                        },
                        new Product()
                        {
                            Name = "Brita Pitcher",
                            Price = 39.99m,
                            OldPrice = 49.99m,
                            Category = "home-kitchen",
                            Image = "../img/Tank.png",
                            Images = "[\"../img/Tank1.png\", \"../img/Tank2.png\"]",
                            Description = "Brita Water Filter Pitcher provides clean, great-tasting water with advanced filtration technology. Holds 10 cups, features a filter change indicator, and reduces chlorine, lead, and other contaminants for healthier drinking water."
                        },
                        new Product()
                        {
                            Name = "Roomba Vacuum",
                            Price = 599.99m,
                            OldPrice = 699.99m,
                            Category = "home-kitchen",
                            Image = "../img/Irobot.png",
                            Images = "[\"../img/Irobot1.png\", \"../img/Irobot2.png\"]",
                            Description = "iRobot Roomba Vacuum is a smart robotic cleaner with Wi-Fi connectivity, powerful suction, and Dirt Detect technology. Compatible with Alexa and Google Assistant, it navigates floors and carpets for effortless home cleaning."
                        },
                        new Product()
                        {
                            Name = "Ninja Blender",
                            Price = 199.99m,
                            OldPrice = 229.99m,
                            Category = "home-kitchen",
                            Image = "../img/Ninja.png",
                            Images = "[\"../img/Ninja1.png\", \"../img/Ninja2.png\"]",
                            Description = "Ninja Professional Blender features a 1000-watt motor and 72-ounce pitcher for blending smoothies, soups, and frozen drinks. Includes Total Crushing blades and single-serve cups for versatile, high-performance blending."
                        },
                        new Product()
                        {
                            Name = "Philips Hue Bulbs",
                            Price = 134.99m,
                            OldPrice = null,
                            Category = "home-kitchen",
                            Image = "../img/Philips.png",
                            Images = "[\"../img/Philips1.png\", \"../img/Philips2.png\"]",
                            Description = "Philips Hue Smart Bulbs offer customizable LED lighting with 16 million colors. Control via app or voice with Alexa and Google Assistant. Energy-efficient and easy to install, they enhance ambiance in any room."
                        },

                        // Toys & Games
                        new Product()
                        {
                            Name = "LEGO Brick Box",
                            Price = 59.99m,
                            OldPrice = 79.99m,
                            Category = "toys-games",
                            Image = "../img/LEGO.png",
                            Images = "[\"../img/LEGO1.png\", \"../img/LEGO2.png\"]",
                            Description = "LEGO Classic Vibrant Creative Brick Box includes 850 pieces for endless building possibilities. With vibrant colors and versatile shapes, it sparks creativity in kids and adults, perfect for imaginative play and learning."
                        },
                        new Product()
                        {
                            Name = "UNO Flip",
                            Price = 9.99m,
                            OldPrice = null,
                            Category = "toys-games",
                            Image = "../img/Uno.png",
                            Images = "[\"../img/Uno1.png\", \"../img/Uno2.png\"]",
                            Description = "UNO Flip Card Game adds a twist to the classic game with double-sided cards and wild rules. Easy to learn and fun for all ages, it’s perfect for family game nights or parties with 2-10 players."
                        },
                        new Product()
                        {
                            Name = "Barbie Vacation House",
                            Price = 109.99m,
                            OldPrice = 129.99m,
                            Category = "toys-games",
                            Image = "../img/Barbie.png",
                            Images = "[\"../img/Barbie1.png\", \"../img/Barbie2.png\"]",
                            Description = "Barbie Modern Vacation House is a 2-story dollhouse with 6 rooms, furniture, and accessories. Encourages imaginative play with a modern design, perfect for kids aged 3+ to create fun stories with Barbie and friends."
                        },
                        new Product()
                        {
                            Name = "Nerf Blaster",
                            Price = 29.99m,
                            OldPrice = null,
                            Category = "toys-games",
                            Image = "../img/Nerf.png",
                            Images = "[\"../img/Nerf1.png\", \"../img/Nerf2.png\"]",
                            Description = "Nerf Elite 2.0 Blaster fires darts up to 90 feet with a 12-dart clip. Features customizable attachments and a tactical rail for added fun, ideal for kids and teens in action-packed outdoor games."
                        },
                        new Product()
                        {
                            Name = "Hot Wheels Track",
                            Price = 49.99m,
                            OldPrice = 59.99m,
                            Category = "toys-games",
                            Image = "../img/Hot Wheels.png",
                            Images = "[\"../img/Hot Wheels1.png\", \"../img/Hot Wheels2.png\"]",
                            Description = "Hot Wheels Track Set includes a thrilling loop and launcher for high-speed car racing. Compatible with Hot Wheels die-cast cars, it encourages creativity and competition for kids aged 4+ in solo or group play."
                        },
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
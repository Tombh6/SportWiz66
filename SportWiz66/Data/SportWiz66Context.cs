using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportWiz66.Models;

namespace SportWiz66.Data
{
    public class SportWiz66Context : DbContext
    {
        public SportWiz66Context (DbContextOptions<SportWiz66Context> options)
            : base(options)
        {
        }

        public DbSet<SportWiz66.Models.Account> Account { get; set; }

        public DbSet<SportWiz66.Models.Brand> Brand { get; set; }

        public DbSet<SportWiz66.Models.Category> Category { get; set; }

        public DbSet<SportWiz66.Models.Location> Location { get; set; }

        public DbSet<SportWiz66.Models.Order> Order { get; set; }

        public DbSet<SportWiz66.Models.OrderDetail> OrderDetail { get; set; }

        public DbSet<SportWiz66.Models.Product> Product { get; set; }

        public DbSet<SportWiz66.Models.ProductViews> ProductViews { get; set; }

        public DbSet<SportWiz66.Models.Review> Review { get; set; }

        public DbSet<SportWiz66.Models.ShoppingCart> ShoppingCart { get; set; }

        public DbSet<SportWiz66.Models.ShoppingCartItem> ShoppingCartItem { get; set; }

        public DbSet<SportWiz66.Models.About> About { get; set; }
    }
}

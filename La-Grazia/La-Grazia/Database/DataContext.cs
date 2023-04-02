using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using La_Grazia.Database.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg.Sig;
using Type = La_Grazia.Database.Models.Type;

namespace La_Grazia.Database
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Variety> Varieties { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Collaboration> Collaborations { get; set; }
        public DbSet<BasketProduct> BasketProducts { get; set; }
        public DbSet<OrderedProduct> OrderedProducts { get; set; }
        public DbSet<WishlistProduct> WishlistProducts { get; set; }
    }
}
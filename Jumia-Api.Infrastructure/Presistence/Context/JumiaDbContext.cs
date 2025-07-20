using Jumia_Api.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jumia_Api.Infrastructure.Presistence.Context
{
    public class JumiaDbContext : IdentityDbContext<AppUser>
    {
        public JumiaDbContext(DbContextOptions<JumiaDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<Admin> Admins { get; set; }
     

        public virtual DbSet<Affiliate> Affiliates { get; set; }

        public virtual DbSet<AffiliateCommission> AffiliateCommissions { get; set; }

        public virtual DbSet<AffiliateSellerRelationship> AffiliateSellerRelationships { get; set; }

        public virtual DbSet<AffiliateWithdrawal> AffiliateWithdrawals { get; set; }

        public virtual DbSet<Cart> Carts { get; set; }

        public virtual DbSet<CartItem> CartItems { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Coupon> Coupons { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }

        public virtual DbSet<HelpfulRating> HelpfulRatings { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }

        public virtual DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }

        public virtual DbSet<ProductImage> ProductImages { get; set; }



        public virtual DbSet<ProductVariant> ProductVariants { get; set; }



        public virtual DbSet<Rating> Ratings { get; set; }



        public virtual DbSet<ReviewImage> ReviewImages { get; set; }





        public virtual DbSet<Seller> Sellers { get; set; }



        public virtual DbSet<SubOrder> SubOrders { get; set; }

      

     

        public virtual DbSet<UserCoupon> UserCoupons { get; set; }

 

   

        public virtual DbSet<VariantAttribute> VariantAttributes { get; set; }

        public virtual DbSet<Wishlist> Wishlists { get; set; }

        public virtual DbSet<WishlistItem> WishlistItems { get; set; }

        //public virtual DbSet<Payment> Payments { get; set; }

    }
}

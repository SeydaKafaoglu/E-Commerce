using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Commerce
{
    public class Category
    {
        public short CategoryId { get; set; }

        [Required, Column(TypeName = "char(50)")]

        public string CategoryName { get; set; }
        

        [Required]
        public bool IsDeleted { get; set; }
    }
    public class Brand
    {
        public short BrandId { get; set; }

        [Required, Column(TypeName = "char(50)")]
        public string BrandName { get; set; }
    }
    public class City
    {
        public short CityId { get; set; }

        [Required, Column(TypeName = "char(20)")]
        public string CityName { get; set; }
    }
    public class Seller
    {
        public int SellerId { get; set; }

        [Required, Column(TypeName = "nchar(50)")]
        public string SellerName { get; set; }

        [Required]
        [Column(TypeName = "char(100)")]
        [DataType(DataType.EmailAddress)]
        public string SellerEMail { get; set; }

        [Required(ErrorMessage = "Lütfen Şifrenizi Giriniz.")]
        [Column(TypeName = "char(64)")]
        [DataType(DataType.Password)]
        public string SellerPassword { get; set; }

        [NotMapped] // veri tabanına kaydetme. iki kere kaydetmemesi için
        [Compare("SellerPassword", ErrorMessage = "şifreler uyuşmuyor")] //passwordleri karşılaştır
        [DataType(DataType.Password)] //tipi password 
        public string ConfirmPassword { get; set; }
        [Required]
        [MinLength(10), MaxLength(10)]
        //[DataType(DataType.PhoneNumber)]
        public string SellerPhone { get; set; }

        [Column(TypeName = "char(200)")]
        public string? SellerDescription { get; set; }

        [Required]
        public bool Banned { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public float? SellerRate { get; set; } //null olabilir. puan verilmemiş olabilir henüz.

        [Required]
        public short CityId { get; set; }
        public City? City { get; set; }
        public List<Product>? Products { get; set; }
    }

    public class Product
    {
        public long ProductId { get; set; }

        [Required, Column(TypeName = "char(100)")]
        public string ProductName { get; set; }

        [Required]
        public float ProductPrice { get; set; }

        [NotMapped] // veri tabanına kaydetmemesi
        [MaxLength(5)]
        public IFormFile[]? Image { get; set; }

        [Column(TypeName = "char(200)")]
        public string? ProductDescription { get; set; }

        public float? ProductRate { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        [Required]
        public short CategoryId { get; set; }

        [Required]
        public short BrandId { get; set; }

        [Required]
        public int SellerId { get; set; }

        public Category? Category { get; set; }

        public Brand? Brand { get; set; }

        public Seller? Seller { get; set; }
 
    }
    public class Customer
    {
        public long CustomerId { get; set; }

        [Required, Column(TypeName = "nchar(50)")]
        public string CustomerName { get; set; }

        [Required]
        [Column(TypeName = "nchar(50)")]
        public string CustomerSurname { get; set; }

        [Required]
        [Column(TypeName = "nchar(100)")]
        [DataType(DataType.EmailAddress)]
        public string CustomerEMail { get; set; }

        [Required]
        [MinLength(10), MaxLength(10)]
        public string CustomerPhone { get; set; }


        [Column(TypeName = "char(64)")]
        [DataType(DataType.Password)]

        public string? CustomerPassword { get; set; }

        [NotMapped] // veri tabanına kaydetme. iki kere kaydetmemesi için
        [Compare("CustomerPassword", ErrorMessage = "şifreler uyuşmuyor")] //passwordleri karşılaştır
        [DataType(DataType.Password)] //tipi password 
        public string? CustomerConfirmPassword { get; set; }

        [Required, Column(TypeName = "nchar(200)")]
        public string CustomerAddress { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
        public List<Order>? Orders { get; set; }

    }
    public class PaymentMethod
    {
        public short PaymentMethodId { get; set; }

        [Required, Column(TypeName = "nchar(30)")]
        public string PaymentMethodName { get; set; }
    }
    public class Order
    {
        public long OrderId { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        public float OrderPrice { get; set; }

        [Required]
        public bool AllDelivered { get; set; } //teslim edildi mi

        [Required]
        public bool Cancelled { get; set; } //iptal edildi mi

        [Required]
        public bool PaymentComplete { get; set; }

       

        public short? PaymentMethodId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }

        
        [Required]
        public long CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
       
        [Required]
        public bool IsCart { get; set; }    
    }
    public class ItemStatus //ürün durumu
    {
        public short ItemStatusId { get; set; }

        [Required]
        [Column(TypeName = "nchar(50)")]
        public string ItemStatusName { get; set; }

    }
    public class OrderDetailStatus
    {

        public long OrderDetailStatusId { get; set; }
        
        [Required]
        public long OrderDetailId { get; set; }
        public OrderDetail? OrderDetail { get; set; }

        [Required]
        public DateTime ChangeItemStatus { get; set; }


        [Required]
        public short ItemStatusId { get; set; }

        public ItemStatus? ItemStatus { get; set; }
    }
    public class OrderDetail //sipariş durumu
    {

        public long OrderDetailId { get; set; } 

        [Required]
        public long OrderId { get; set; }
        public Order? Order { get; set; }


        [Required]
        public long ProductId { get; set; } 
        public Product? Product { get; set; }

        
        [Required]
        public byte Count { get; set; }
        
        [Required]
        public float Price { get; set; }

        public List<OrderDetailStatus>? OrderDetailStatuses { get; set; }

    }

    //public class CreditCard
    //{
    //    [NotMapped]
    //    [Required,MinLength(13), MaxLength(19)] 
    //    public long CreditCardId { get; set; }
    //    [NotMapped]
    //    [Required]
    //    [Range(1,12)]
    //    public byte ExpiryMonth { get; set; }
    //    [Required]
    //    [NotMapped]
    //    public byte ExpiryYear { get; set; }
    //    [Required,Range(0, 999), MinLength(3)]
    //    [NotMapped]
    //    public short CVC { get; set; }
    //    [NotMapped]
    //    [Required]
    //    [MinLength(5)]
    //    public string CardHolderName{ get; set; }  // kart sahibinim adı 

    //}


}

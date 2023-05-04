using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BreweryAPI.Models;

public partial class BreweryContext : DbContext
{
    public BreweryContext()
    {
    }

    public BreweryContext(DbContextOptions<BreweryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AdminList> AdminLists { get; set; }

    public virtual DbSet<Beer> Beers { get; set; }

    public virtual DbSet<BeerCheque> BeerCheques { get; set; }

    public virtual DbSet<BeerList> BeerLists { get; set; }

    public virtual DbSet<BeerType> BeerTypes { get; set; }

    public virtual DbSet<Brewery> Breweries { get; set; }

    public virtual DbSet<BreweryBeer> BreweryBeers { get; set; }

    public virtual DbSet<BreweryIngridient> BreweryIngridients { get; set; }

    public virtual DbSet<BreweryList> BreweryLists { get; set; }

    public virtual DbSet<Cheque> Cheques { get; set; }

    public virtual DbSet<ChequeList> ChequeLists { get; set; }

    public virtual DbSet<Ingridient> Ingridients { get; set; }

    public virtual DbSet<IngridientsBeer> IngridientsBeers { get; set; }

    public virtual DbSet<IngridientsList> IngridientsLists { get; set; }

    public virtual DbSet<IngridientsType> IngridientsTypes { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SuppliersList> SuppliersLists { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserHistory> UserHistories { get; set; }

    public virtual DbSet<UserList> UserLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=FIIVA\\DA;Initial Catalog=Brewery;TrustServerCertificate=True;User ID=sa;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.IdAdmin).HasName("PK__Admin__69F567661FA9B46D");

            entity.ToTable("Admin");

            entity.HasIndex(e => e.PhoneAdmin, "UQ__Admin__4127C109B84E8178").IsUnique();

            entity.HasIndex(e => e.LoginAdmin, "UQ__Admin__B02B0AA59784DCC2").IsUnique();

            entity.Property(e => e.IdAdmin).HasColumnName("ID_Admin");
            entity.Property(e => e.BreweryId).HasColumnName("Brewery_ID");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.LoginAdmin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Login_Admin");
            entity.Property(e => e.PasswordAdmin)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Password_Admin");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("Password_Salt");
            entity.Property(e => e.PhoneAdmin)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("Phone_Admin");
        });

        modelBuilder.Entity<AdminList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Admin_List");

            entity.Property(e => e.ИнформацияОбАдмине)
                .HasMaxLength(145)
                .IsUnicode(false)
                .HasColumnName("Информация об админе");
        });

        modelBuilder.Entity<Beer>(entity =>
        {
            entity.HasKey(e => e.IdBeer).HasName("PK__Beer__F381132E4213ABED");

            entity.ToTable("Beer");

            entity.Property(e => e.IdBeer).HasColumnName("ID_Beer");
            entity.Property(e => e.BeerTypeId).HasColumnName("Beer_Type_ID");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.NameBeer)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Name_Beer");
            entity.Property(e => e.ProductionTime)
                .HasColumnType("datetime")
                .HasColumnName("Production_Time");
            entity.Property(e => e.Term).HasColumnType("datetime");
        });

        modelBuilder.Entity<BeerCheque>(entity =>
        {
            entity.HasKey(e => e.IdBeerCheque).HasName("PK__Beer_Che__D6FF04A6002EC8B4");

            entity.ToTable("Beer_Cheque");

            entity.Property(e => e.IdBeerCheque).HasColumnName("ID_Beer_Cheque");
            entity.Property(e => e.BeerId).HasColumnName("Beer_ID");
            entity.Property(e => e.ChequeId).HasColumnName("Cheque_ID");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
        });

        modelBuilder.Entity<BeerList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Beer_List");

            entity.Property(e => e.ИнформацияОПиве)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("Информация о пиве");
        });

        modelBuilder.Entity<BeerType>(entity =>
        {
            entity.HasKey(e => e.IdBeerType).HasName("PK__Beer_Typ__4661E8C002EC2CA2");

            entity.ToTable("Beer_Type");

            entity.HasIndex(e => e.NameBeerType, "UQ__Beer_Typ__DA8F0086084017B8").IsUnique();

            entity.Property(e => e.IdBeerType).HasColumnName("ID_Beer_Type");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.NameBeerType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Name_Beer_Type");
        });

        modelBuilder.Entity<Brewery>(entity =>
        {
            entity.HasKey(e => e.IdBrewery).HasName("PK__Brewery__DEA49A14EFAB23F5");

            entity.ToTable("Brewery");

            entity.Property(e => e.IdBrewery).HasColumnName("ID_Brewery");
            entity.Property(e => e.AddressBrewery)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Address_Brewery");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.NameBrewery)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Name_Brewery");
        });

        modelBuilder.Entity<BreweryBeer>(entity =>
        {
            entity.HasKey(e => e.IdBreweryBeer).HasName("PK__Brewery___189064BD85C4C13B");

            entity.ToTable("Brewery_Beer");

            entity.Property(e => e.IdBreweryBeer).HasColumnName("ID_Brewery_Beer");
            entity.Property(e => e.BeerId).HasColumnName("Beer_ID");
            entity.Property(e => e.BreweryId).HasColumnName("Brewery_ID");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
        });

        modelBuilder.Entity<BreweryIngridient>(entity =>
        {
            entity.HasKey(e => e.IdBreweryIngridients).HasName("PK__Brewery___06A5C5770B1767D6");

            entity.ToTable("Brewery_Ingridients");

            entity.Property(e => e.IdBreweryIngridients).HasColumnName("ID_Brewery_Ingridients");
            entity.Property(e => e.BreweryId).HasColumnName("Brewery_ID");
            entity.Property(e => e.IngridientId).HasColumnName("Ingridient_ID");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
        });

        modelBuilder.Entity<BreweryList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Brewery_List");

            entity.Property(e => e.ИнформацияОПивоварне)
                .HasMaxLength(269)
                .IsUnicode(false)
                .HasColumnName("Информация о пивоварне");
        });

        modelBuilder.Entity<Cheque>(entity =>
        {
            entity.HasKey(e => e.IdCheque).HasName("PK__Cheque__C247B73AF4F0DE45");

            entity.ToTable("Cheque");

            entity.Property(e => e.IdCheque).HasColumnName("ID_Cheque");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.TimeOrder)
                .HasColumnType("datetime")
                .HasColumnName("Time_Order");
            entity.Property(e => e.UserId).HasColumnName("User_ID");
        });

        modelBuilder.Entity<ChequeList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Cheque_List");

            entity.Property(e => e.ИнформацияОЧеках)
                .HasMaxLength(214)
                .IsUnicode(false)
                .HasColumnName("Информация о чеках");
        });

        modelBuilder.Entity<Ingridient>(entity =>
        {
            entity.HasKey(e => e.IdIngridient).HasName("PK__Ingridie__46C7C9B70547F9CB");

            entity.HasIndex(e => e.NameIngridient, "UQ__Ingridie__967371545FEFC168").IsUnique();

            entity.Property(e => e.IdIngridient).HasColumnName("ID_Ingridient");
            entity.Property(e => e.AdminId).HasColumnName("Admin_ID");
            entity.Property(e => e.IngridientTypeId).HasColumnName("Ingridient_Type_ID");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.NameIngridient)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Name_Ingridient");
            entity.Property(e => e.SupplierId).HasColumnName("Supplier_ID");
        });

        modelBuilder.Entity<IngridientsBeer>(entity =>
        {
            entity.HasKey(e => e.IdUsersBeer).HasName("PK__Ingridie__9CBD7A1B677A7ACC");

            entity.ToTable("Ingridients_Beer");

            entity.Property(e => e.IdUsersBeer).HasColumnName("ID_Users_Beer");
            entity.Property(e => e.BeerId).HasColumnName("Beer_ID");
            entity.Property(e => e.IngridientId).HasColumnName("Ingridient_ID");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
        });

        modelBuilder.Entity<IngridientsList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Ingridients_List");

            entity.Property(e => e.ИнформацияОбИнгридиентах)
                .HasMaxLength(373)
                .IsUnicode(false)
                .HasColumnName("Информация об ингридиентах");
        });

        modelBuilder.Entity<IngridientsType>(entity =>
        {
            entity.HasKey(e => e.IdIngridientType).HasName("PK__Ingridie__39844B618252D59A");

            entity.ToTable("Ingridients_Type");

            entity.HasIndex(e => e.NameIngridientType, "UQ__Ingridie__926F9E4ED36E2E01").IsUnique();

            entity.Property(e => e.IdIngridientType).HasColumnName("ID_Ingridient_Type");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.NameIngridientType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Name_Ingridient_Type");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.IdSubscription).HasName("PK__Subscrip__1305B005F1E47130");

            entity.ToTable("Subscription");

            entity.HasIndex(e => e.NameSubscription, "UQ__Subscrip__7A34A5345224C61B").IsUnique();

            entity.Property(e => e.IdSubscription).HasColumnName("ID_Subscription");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.NameSubscription)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Name_Subscription");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.IdSupplier).HasName("PK__Supplier__408B7094D244C9DA");

            entity.HasIndex(e => e.AddressSupplier, "UQ__Supplier__AEF95A7D71E2F7E3").IsUnique();

            entity.HasIndex(e => e.PhoneSupplier, "UQ__Supplier__C8411F06786270E5").IsUnique();

            entity.Property(e => e.IdSupplier).HasColumnName("ID_Supplier");
            entity.Property(e => e.AddressSupplier)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Address_Supplier");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.NameSupplier)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Name_Supplier");
            entity.Property(e => e.PhoneSupplier)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("Phone_Supplier");
        });

        modelBuilder.Entity<SuppliersList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("Suppliers_List");

            entity.Property(e => e.ИнформацияОПоставщиках)
                .HasMaxLength(447)
                .IsUnicode(false)
                .HasColumnName("Информация о поставщиках");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.IdToken).HasName("PK__Token__ECDC228E4F439C86");

            entity.ToTable("Token");

            entity.Property(e => e.IdToken).HasColumnName("ID_Token");
            entity.Property(e => e.TokenDateTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Token_DateTime");
            entity.Property(e => e.TokenValue)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Token_Value");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PK__User__ED4DE4426F9CCB27");

            entity.ToTable("User", tb =>
                {
                    tb.HasTrigger("User_History_Delete");
                    tb.HasTrigger("User_History_Insert");
                    tb.HasTrigger("User_History_Update");
                });

            entity.HasIndex(e => e.LoginUser, "UQ__User__5B6755AD86B0D0FF").IsUnique();

            entity.HasIndex(e => e.UserPhone, "UQ__User__AA69F367649010FF").IsUnique();

            entity.Property(e => e.IdUser).HasColumnName("ID_User");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.LoginUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Login_User");
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("Password_Salt");
            entity.Property(e => e.PasswordUser)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Password_User");
            entity.Property(e => e.SubscriptionId).HasColumnName("Subscription_ID");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(17)
                .IsUnicode(false)
                .HasColumnName("User_Phone");
        });

        modelBuilder.Entity<UserHistory>(entity =>
        {
            entity.HasKey(e => e.IdUserHistory).HasName("PK__User_His__E907BB2AEE62C82F");

            entity.ToTable("User_History");

            entity.Property(e => e.IdUserHistory).HasColumnName("ID_User_History");
            entity.Property(e => e.ChangeRecord)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Change_Record");
            entity.Property(e => e.CreateRecord)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Create_Record");
            entity.Property(e => e.IsDeleted).HasColumnName("Is_Deleted");
            entity.Property(e => e.UserHistory1)
                .IsUnicode(false)
                .HasColumnName("User_History");
            entity.Property(e => e.UserId).HasColumnName("User_ID");
        });

        modelBuilder.Entity<UserList>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("User_List");

            entity.Property(e => e.ИнформацияОПользователе)
                .HasMaxLength(207)
                .IsUnicode(false)
                .HasColumnName("Информация о пользователе");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using OmniPyme.Data;

#nullable disable

namespace OmniPyme.Web.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250408140615_AddProductTable")]
    partial class AddProductTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OmniPyme.Web.Data.Entities.Product", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.Property<string>("Description")
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.Property<double>("Price")
                    .HasColumnType("float");

                b.Property<string>("BarCode")
                    .HasMaxLength(45)
                    .HasColumnType("nvarchar(45)");

                b.Property<double>("Tax")
                    .HasColumnType("float");

                b.Property<int>("ProductCategoryId")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("ProductCategoryId");

                b.ToTable("Products");
            });

            modelBuilder.Entity("OmniPyme.Web.Data.Entities.Product", b =>
            {
                b.HasOne("OmniPyme.Web.Data.Entities.ProductCategory", "ProductCategory")
                    .WithMany("Products")
                    .HasForeignKey("ProductCategoryId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("ProductCategory");
            });

            modelBuilder.Entity("OmniPyme.Web.Data.Entities.ProductCategory", b =>
            {
                b.Navigation("Products");
            });
#pragma warning restore 612, 618
        }
    }
}

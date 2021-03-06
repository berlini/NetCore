﻿// <auto-generated />
using DoTheDishesWebservice.DataAccess.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DoTheDishesWebservice.DataAccess.Migrations
{
    [DbContext(typeof(DishesContext))]
    [Migration("20190306200631_AddRequired")]
    partial class AddRequired
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DoTheDishesWebservice.DataAccess.Models.Home", b =>
                {
                    b.Property<int>("HomeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("HomeId");

                    b.ToTable("Homes");
                });

            modelBuilder.Entity("DoTheDishesWebservice.DataAccess.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HomeId");

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<string>("Nickname")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("UserId");

                    b.HasIndex("HomeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DoTheDishesWebservice.DataAccess.Models.User", b =>
                {
                    b.HasOne("DoTheDishesWebservice.DataAccess.Models.Home", "Home")
                        .WithMany("Users")
                        .HasForeignKey("HomeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

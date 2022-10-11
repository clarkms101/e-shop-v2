﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using e_shop_api.DataBase;

namespace e_shop_api.Migrations
{
    [DbContext(typeof(EShopDbContext))]
    partial class EShopDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("e_shop_api.DataBase.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Account")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("CreatorUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("LastModifierUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Permission")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Admins");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Account = "Clark",
                            CreationTime = new DateTime(2022, 10, 11, 14, 59, 7, 267, DateTimeKind.Local).AddTicks(1687),
                            LastModificationTime = new DateTime(2022, 10, 11, 14, 59, 7, 268, DateTimeKind.Local).AddTicks(6524),
                            Password = "cc03e747a6afbbcbf8be7668acfebee5",
                            Permission = "Public"
                        });
                });

            modelBuilder.Entity("e_shop_api.DataBase.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("CreatorUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(150)");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("LastModifierUserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("e_shop_api.DataBase.Models.Coupon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("CreatorUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("DueDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("LastModifierUserId")
                        .HasColumnType("bigint");

                    b.Property<int>("Percent")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("e_shop_api.DataBase.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("varchar(250)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("CreatorUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("LastModifierUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Message")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("OrderStatus")
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime?>("PaidDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Tel")
                        .HasColumnType("varchar(25)");

                    b.Property<decimal>("TotalAmount")
                        .HasColumnType("numeric");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("SerialNumber");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("e_shop_api.DataBase.Models.OrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("CreatorUserId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("LastModifierUserId")
                        .HasColumnType("bigint");

                    b.Property<int>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int>("ProductId")
                        .HasColumnType("integer");

                    b.Property<int>("Qty")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("e_shop_api.DataBase.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Content")
                        .HasColumnType("varchar(250)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("CreatorUserId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("varchar(250)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastModificationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<long?>("LastModifierUserId")
                        .HasColumnType("bigint");

                    b.Property<int>("Num")
                        .HasColumnType("integer");

                    b.Property<decimal>("OriginPrice")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });
#pragma warning restore 612, 618
        }
    }
}

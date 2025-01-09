using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Login.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Login.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(20);

            builder.Property(x => x.FirstName)
            .HasColumnName("first_name")
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(20);

            builder.Property(x => x.LastName)
            .HasColumnName("last_name")
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(30);

            builder.Property(x => x.Password)
            .HasColumnName("password")
            .IsRequired();

            builder.Property(x => x.Role)
            .HasColumnName("role")
            .IsRequired()
            .HasColumnType("VARCHAR")
            .HasMaxLength(20);

            builder.Property(x => x.CreateDate)
            .HasColumnName("create_date")
            .IsRequired()
            .HasColumnType("DATETIME2");
        }
    }
}
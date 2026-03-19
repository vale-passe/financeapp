using API.Features.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API.Features.Users.Data;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable("Users");
        
        entity.HasKey(e => e.Id);
        
        entity.HasIndex(e => e.Email).IsUnique();
        
        entity.Property(e => e.Email).HasMaxLength(128);
        entity.Property(e => e.Firstname).HasMaxLength(64);
        entity.Property(e => e.Lastname).HasMaxLength(64);
        entity.Property(e => e.ImageAvatarUrl).HasMaxLength(256);
    }
}
using Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Dal.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.Password)
                .IsRequired();

            builder.HasOne(x => x.Role)
                .WithMany(x => x.Users)
                .IsRequired();
        }
    }
}

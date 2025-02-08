using Event.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Event.Dal.Configuration
{
    public class EventEntityConfiguration : IEntityTypeConfiguration<EventEntity>
    {
        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.Property(x => x.Location)
                .IsRequired();

            builder.Property(x => x.Category)
                .IsRequired();

            builder.HasMany(x => x.Members)
                .WithOne(x => x.EventEntity)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.MaxMember)
                .IsRequired();

            builder.Property(x => x.MaxMember)
                .IsRequired();

            builder.ToTable(x =>
                x.HasCheckConstraint(
                    "CK_EventEntity_MaxMember",
                    "MaxMember >= 0"));
        }
    }
}

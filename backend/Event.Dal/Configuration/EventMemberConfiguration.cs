using Event.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Event.Dal.Configuration
{
    public class EventMemberConfiguration : IEntityTypeConfiguration<EventMember>
    {
        public void Configure(EntityTypeBuilder<EventMember> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.EventEntity)
                .WithMany(x => x.Members);

            builder.Property(x => x.Email)
                .IsRequired();

            builder.Property(x => x.FirstName)
                .IsRequired();

            builder.Property(x => x.SecondName)
                .IsRequired();
        }
    }
}

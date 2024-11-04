using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Api.AppScanner.BarCode.Rotary.COMMON.Models;

namespace Api.AppScanner.BarCode.Rotary.EF.Mapping
{
    public class CodigoBarrasMap : IEntityTypeConfiguration<CodigoBarrasModel>
    {
        public void Configure(EntityTypeBuilder<CodigoBarrasModel> builder) 
        {
            builder.ToTable("CodigoBarras");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Tipo).IsRequired();
            builder.Property(x => x.Codigo).IsRequired();
            builder.Property(x => x.DataLeitura).HasColumnType("DateTime").IsRequired();
        }
    }
}

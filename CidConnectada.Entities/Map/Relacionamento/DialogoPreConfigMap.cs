using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Relacionamento;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Relacionamento
{
    public class DialogoPreConfigMap : EntityBaseMap<DialogoPreConfig, int>
    {
        public DialogoPreConfigMap()
        {
            ToTable("DIALOGO_PRE_CONFIG", "relac");

            Property(e => e.Key)
                .HasColumnName("ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(50);
            
            Property(e => e.TituloPadrao)
                .HasColumnName("TITULO_PADRAO")
                .HasMaxLength(50);

            Property(e => e.AssuntoDialogoEnum)
                .HasColumnName("ASSUNTO_DIALOGO_ENUM");
            
            Property(e => e.IconeNome)
                .HasColumnName("ICONE_NOME")
                .HasMaxLength(128);
            
            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID")
                .IsRequired();

            HasRequired(e => e.Secretaria).WithMany(e => e.DialogoPreConfigSet).Map(e => e.MapKey("SECRETARIA_ID"));

        }
    }
}
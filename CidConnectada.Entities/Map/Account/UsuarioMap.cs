using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Account
{
    public class UsuarioMap : EntityBaseMap<Usuario, int>
    {
        public UsuarioMap()
        {
            ToTable("USUARIO");

            Property(e => e.Key)
                .HasColumnName("ID")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(e => e.Nome)
                .HasColumnName("NOME")
                .HasMaxLength(50);

            Property(e => e.Sobrenome)
                .HasColumnName("SOBRENOME")
                .HasMaxLength(200);

            Property(e => e.NomeCompleto)
                .HasColumnName("NOME_COMPLETO")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            Property(e => e.Cpf)
                .HasColumnName("CPF")
                .HasMaxLength(11);

            Property(e => e.Rg)
                .HasColumnName("RG")
                .HasMaxLength(25);

            Property(e => e.TenantKey)
                .HasColumnName("TENANT_ID")
                .IsRequired();

            Property(e => e.OrgaoExpedidor)
                .HasColumnName("ORGAO_EXPEDIDOR")
                .HasMaxLength(25);

            Property(e => e.Status)
                .HasColumnName("STATUS_ENUM");

            Property(e => e.ConcordaTermosDeUso)
                .HasColumnName("CONCORDA_TERMOS_DE_USO");

            Property(e => e.AceitaMsgWhastApp)
                .HasColumnName("ACEITA_MSG_WA");

            Property(e => e.IndPrincipal)
                .HasColumnName("IND_PRINCIPAL");

            HasOptional(e => e.AspNetUsers).WithOptionalDependent(e => e.User).Map(e => e.MapKey("UserId"));
            HasRequired(e => e.Prefeitura).WithMany().HasForeignKey(e => e.TenantKey);
        }
    }
}
using System;
using System.ComponentModel.DataAnnotations.Schema;
using CidConnectada.Entities.Model.Account;
using Zenite.Pi.Entities.Mapping;

namespace CidConnectada.Entities.Map.Account
{
    public class DeviceMap : EntityBaseMap<Device, Guid>
    {
        public DeviceMap()
        {
            ToTable("DEVICE");

            Property(e => e.Key)
                .HasColumnName("ID")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(e => e.Name)
                .HasColumnName("NAME")
                .HasMaxLength(255);

            Property(e => e.Type)
                .HasColumnName("TYPE")
                .HasMaxLength(50);
        }
    }
}
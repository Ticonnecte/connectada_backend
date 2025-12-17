using System;
using CidConnectada.Entities.Model.Emprego;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Emprego
{
    public class CVExperienciaDto : BaseEntityModel<CVExperienciaKey>
    {
        public override CVExperienciaKey key => new CVExperienciaKey
        {
            CVId = CVId,
            ItemIndex = ItemIndex
        };
        public int CVId { get; set; }
        public byte ItemIndex { get; set; }
        public string nomeEmpresa { get; set; }
        public string funcao { get; set; }
        public DateTime periodoInicio { get; set; }
        public DateTime? periodoFinal { get; set; }

        public string atividades { get; set; }
    }
}

using CidConnectada.Entities.Model.Enums;
using CidConnectada.Entities.Model.Infos;
using System.Collections.Generic;
using Zenite.Pi.Web.Models;

public class CategoriaDto : BaseEntityModel<int>
{
    public string nome { get; set; }
    public CorEnum cor { get; set; }
    public string descricao { get; set; }
    public string iconeNome { get; set; }
    public string corNome { get; set; }

    public bool ativa {  get; set; }

}
using CidConnectada.Entities.Model.Comercios;
using CidConnectada.Webapi.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Comercios
{
    public class ProdutoDto : S3FileGenericDto
    {

        public ProdutoDto()
            : this(false)
        {
        }
        public ProdutoDto(bool isView)
        {
            _isView = isView;
        }
        public bool _isView { get; }

        [Required]
        public string nome { get; set; }
        public string descricao { get; set; }
        public string fotoCapa
        {
            get
            {
                return _isView ? _imgUrl : _base64;
            }
            set
            {
                if (_isView)
                {
                    _imgUrl = value;
                }
                else
                {
                    _base64 = value;
                }
            }
        }
        public string extensaoCapa {
            get {
                return _extensao;
            }
            set {
                _extensao = value;
            }
        }

        public decimal valor { get; set; }

        public string comercioId { get; set; }
    }
}

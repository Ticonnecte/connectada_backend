using CidConnectada.Webapi.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Comercios
{
    public class ComercioDto : S3FileGenericDto
    {
        public ComercioDto()
            : this(false)
        {
        }
        public ComercioDto(bool isView)
        {
            _isView = isView;
        }

        public bool _isView {  get; }

        [Required]
        public string nome { get; set; }
        public string descricao { get; set; }
        public string numeroWhatsApp { get; set; }
        public string numeroWhatsAppMask { get; set; }
        public byte ordemHome { get; set; }
        public string extensaoCapa
        {
            get
            {
                return _extensao;
            }
            set
            {
                _extensao = value;
            }
        }
        public string fotoCapa {
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
        public bool isActive { get; set; }
        [Required]
        public TimeSpan abreAs { get; set; }
        [Required]
        public TimeSpan fechaAs { get; set; }
        [Required]
        public long enderecoId { get; set; }
        public string enderecoCompleto { get; set; }
        public string placeId { get; set; }
        [Required]
        public int tipoComercioId { get; set; }
        public string tipoComercioNome { get; set; }

        public IList<CategoriaTipoComercioDto> categorias { get; set; }
    }
}

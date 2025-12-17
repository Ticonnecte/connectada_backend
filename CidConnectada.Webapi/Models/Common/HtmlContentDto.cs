using CidConnectada.Webapi.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenite.Pi.Web.Models;

namespace CidConnectada.Webapi.Models.Common
{
    public class HtmlContentDto : S3FileGenericDto
    {
        public HtmlContentDto()
            : this(false)
        {
        }
        public HtmlContentDto(bool isView)
        {
            _isView = isView;
        }

        protected bool _isView { get; }
        public string lead { get; set; }
        public virtual string conteudo { get; set; }
        public virtual string extensaoCapa
        {
            get {
                return _extensao;
            }

            set {
                _extensao = value;
            }
        }
        public virtual string fotoCapa
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

        public bool ativa {get; set;}
    }

}

using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Noticias
{
    public class EnvioNoticiaKey : IEntityKey
    {
        public string NoticiaId { get; set; }
        public int UsuarioId { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is EnvioNoticiaKey)
            {
                result = NoticiaId.CompareTo(((EnvioNoticiaKey)obj).NoticiaId);
                if (result == 0)
                    result = UsuarioId.CompareTo(((EnvioNoticiaKey)obj).UsuarioId);
            }
            else
            {
                throw new TypeInitializationException(obj.GetType().FullName, null);
            }

            return result;
        }

        public object[] ToArray()
        {
            return new object[2]
            {
                NoticiaId, UsuarioId
            };
        }
    }
}
using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Noticias
{
    public class NoticiaCategoriaVincKey : IEntityKey
    {
        public string NoticiaId { get; set; }
        public int CategoriaId { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is NoticiaCategoriaVincKey)
            {
                result = NoticiaId.CompareTo(((NoticiaCategoriaVincKey)obj).NoticiaId);
                if (result == 0)
                    result = CategoriaId.CompareTo(((NoticiaCategoriaVincKey)obj).CategoriaId);
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
                NoticiaId, CategoriaId
            };
        }
    }
}
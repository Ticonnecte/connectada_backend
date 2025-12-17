using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Noticias
{
    public class NoticiaLogKey : IEntityKey
    {
        public string NoticiaId { get; set; }
        public DateTime DhUpdate { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is NoticiaLogKey)
            {
                result = NoticiaId.CompareTo(((NoticiaLogKey)obj).NoticiaId);
                if (result == 0)
                    result = DhUpdate.CompareTo(((NoticiaLogKey)obj).DhUpdate);
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
                NoticiaId, DhUpdate
            };
        }
    }
}
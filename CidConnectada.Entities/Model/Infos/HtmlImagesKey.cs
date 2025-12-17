using System;
using Zenite.Pi.Entities;

namespace CidConnectada.Entities.Model.Infos
{
    public class HtmlImagesKey : IEntityKey
    {
        public int HashId { get; set; }
        public string ParentId { get; set; }

        public int CompareTo(object obj)
        {
            int result;
            if (obj is HtmlImagesKey)
            {
                result = HashId.CompareTo(((HtmlImagesKey)obj).HashId);
                if (result == 0)
                    result = ParentId.CompareTo(((HtmlImagesKey)obj).ParentId);
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
                HashId, ParentId
            };
        }
    }
}
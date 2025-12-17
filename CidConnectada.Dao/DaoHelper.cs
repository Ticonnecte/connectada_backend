using System;
using Zenite.Pi.Context;
using Zenite.Pi.IoC;

namespace CidConnectada.Dao
{
    public static class DaoHelper
    {
        private static ContextRequestMultiTenancy<int, string, int> RequestContext
        {
            get => (ContextRequestMultiTenancy<int, string, int>) ApplicationContext.Resolve<ContextRequest<int, string>>();
        }

        public static int? GetTenantId()
        {
            int? result = null;
            if (RequestContext.CacheRequest.TryGetValue("TenantId", out object tId))
            {
                Int32.TryParse(tId.ToString(), out int tenantId);
                if (RequestContext.User is null || RequestContext.TenantKey == 0)
                    result = tenantId;
            }

            return result;
        }
    }
}
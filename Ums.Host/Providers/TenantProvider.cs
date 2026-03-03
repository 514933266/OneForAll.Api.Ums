using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneForAll.Core.OAuth;

namespace Ums.Host.Providers
{
    public class TenantProvider : ITenantProvider
    {
        private IHttpContextAccessor _context;

        public TenantProvider(IHttpContextAccessor context)
        {
            _context = context;
        }

        public Guid GetTenantId()
        {
            var tenantId = _context.HttpContext?.User.Claims.FirstOrDefault(e => e.Type == UserClaimType.TenantId);
            if (tenantId != null)
            {
                return new Guid(tenantId.Value);
            }
            else
            {
                return Guid.Empty; ;
            }
        }
    }
}

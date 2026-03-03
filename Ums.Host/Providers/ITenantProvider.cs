using System;
using System.Collections.Generic;
using System.Text;

namespace Ums.Host.Providers
{
    public interface ITenantProvider
    {
        Guid GetTenantId();
    }
}

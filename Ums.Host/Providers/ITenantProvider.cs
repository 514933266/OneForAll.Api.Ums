using System;
using System.Collections.Generic;
using System.Text;

namespace Ums.Host
{
    public interface ITenantProvider
    {
        Guid GetTenantId();
    }
}

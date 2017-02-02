using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.customers.services
{
    public interface IAuthorizeService
    {
        string AuthorizeCustomer(string emailAddress, string password);

        string AuthorizeCustomer(string token);
    }
}

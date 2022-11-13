using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authorization_Authontication.Services.Interfaces
{
    public interface IMailServices
    {
        Task Sendemailasync(string toemail, string content);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oyang.Identity.IApplication.Database
{
    public interface IDatabaseAppService:IApplicationService
    {
        bool GenerateDatabase();
        void GenerateSeedData();
    }
}

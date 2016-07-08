using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SiGLDB
{
    public partial class SiGLEntities : DbContext
    {
        public SiGLEntities(string connectionstring)
            : base(connectionstring)
        {
        }
    }
}


//------------------------------------------------------------------------------

//-------1---------2---------3---------4---------5---------6---------7---------8
//       01234567890123456789012345678901234567890123456789012345678901234567890
//-------+---------+---------+---------+---------+---------+---------+---------+

// copyright:   2016 WiM - USGS

//    authors:  Jeremy K. Newson USGS Wisconsin Internet Mapping
//              
//              
//  
//   purpose:  validate user and password for required authentication. 
//
//discussion:   
//
//     

#region Comments
// 06.24.16 - TR - created
#endregion

using System;
using System.Linq;
using System.Data.Entity;

using OpenRasta.Security;
using SiGLServices.Security;
using WiM.Security;
using SiGLDB;
using SiGLServices.Utilities.ServiceAgent;


namespace SiGLServices.Security
{
    public class SiGLAuthenticationProvider : IAuthenticationProvider
    {
        public Credentials GetByUsername(string username)
        {
            using (SiGLAgent sa = new SiGLAgent())
            {
                data_manager user = sa.Select<data_manager>().Include(r => r.role).AsEnumerable().FirstOrDefault(u => string.Equals(u.username, username, StringComparison.OrdinalIgnoreCase));
                if (user == null) return (null);
                return (new WiMCredentials()
                {
                    Username = user.username,
                    salt = user.salt,
                    Password = user.password,
                    Roles = new string[] { user.role.role_name }
                });
            }//end using            
        }

        public bool ValidatePassword(Credentials credentials, string suppliedPassword)
        {
            if (credentials == null) return (false);
            WiMCredentials creds = (WiMCredentials)credentials;
            bool authenticated = Cryptography.VerifyPassword(suppliedPassword, creds.salt, creds.Password);
            return authenticated;
        }
    }//end Class SiGLBAsicAuthentication

    public enum RoleType
    {
        e_Public = 1,
        e_Manager = 2,
        e_Administrator = 3
    }
}//end namespace
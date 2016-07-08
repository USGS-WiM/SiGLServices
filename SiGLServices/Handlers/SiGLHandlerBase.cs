using System;
using System.Collections.Generic;
using System.Linq;
using WiM.Security;
using WiM.Handlers;
using OpenRasta.Security;

namespace SiGLServices.Handlers
{
    public abstract class SiGLHandlerBase : HandlerBase
    {
        #region Constants
        protected const string AdminRole = "ADMIN";
        protected const string ManagerRole = "MANAGER";
        #endregion

        #region Base Properties

        #endregion
        #region Base Routed Methods


        #endregion

        #region Base Methods

        protected EasySecureString GetSecuredPassword()
        {
            return new EasySecureString(BasicAuthorizationHeader.Parse(Context.Request.Headers["Authorization"]).Password);
        }

        public bool IsAuthorizedToEdit(string OwnerUserName)
        {
            if (string.Equals(OwnerUserName, username, StringComparison.OrdinalIgnoreCase))
                return true;
            if (IsAuthorized(AdminRole))
                return true;
            return false;
        }

        #endregion

    }
}

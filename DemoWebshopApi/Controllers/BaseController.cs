using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DemoWebshopApi.Controllers
{
    public class BaseController : ControllerBase
    {
        public string UserId
        {
            get
            {
                if (User != null) 
                {
                    var userIdNameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    if (userIdNameClaim != null)
                    {
                        return userIdNameClaim.Value;
                    }
                }

                return null;
            }
        }
    }
}

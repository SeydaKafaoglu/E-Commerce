namespace E_Commerce.Areas.Admin.Controllers
{
    public class AuthorizationClass //diğer controllerlar tarafından kullanılacağı içiçn buraya açtık
    {
        public bool IsAuthorized(string authorization, ISession session)
        {
           
           string? sessionAuthorization= session.GetString(authorization); //login olmadığında null döndürür ?
            if (sessionAuthorization=="True")
            {
                return true;
            }
            return false;

        }
    }
}

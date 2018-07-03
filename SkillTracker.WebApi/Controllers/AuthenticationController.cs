using System.Net;
using System.Net.Http;
using System.Web.Http;
using SkillTracker.Common.Exception;
using SkillTracker.Entities;
using SkillTracker.Services;
using SkillTracker.WebApi.Models;

namespace SkillTracker.WebApi.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthenticationController : BaseAPIController
    {
        private IUserService _userService;
        private ILogManager _logManager;
        public AuthenticationController(IUserService userService, ILogManager logManager)
        {
            _userService = userService;
            _logManager = logManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("getToken")]
        public HttpResponseMessage Authenticate([FromBody] LoginModel login)
        {
            var loginResponse = new UserModel { };
            LoginModel loginrequest = new LoginModel { };
            User usr = new User();

            bool isUsernamePasswordValid = false;

            if (login != null)
            {
                loginrequest.user_name = login.user_name.ToLower();
                loginrequest.password = login.password;
                usr = CheckUser(login.user_name, login.password);
                if (usr != null)
                    isUsernamePasswordValid = true;
            }

            // if credentials are valid
            if (isUsernamePasswordValid)
            {
                var token = JwtManager.GenerateToken(loginrequest.user_name);
                //return the token
                loginResponse.token = token;
                loginResponse.user_name = usr.user_name;
                loginResponse.user_id = usr.user_id;
                loginResponse.user_email = usr.user_email;
                return ToJson(loginResponse);
            }
            else
            {
                // if credentials are not valid send unauthorized status code in response
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public HttpResponseMessage Register([FromBody] RegisteModel register)
        {
            var registerResponse = new UserModel { };
            User usr = new User();
            var userExists = _userService.GetUserByUserName(register.user_name);
            if(userExists == null)
            {
                usr = Helper.CastObject<User>(register);
                _userService.CreateUser(usr);
            }
            if(usr != null)
            {
                string token = JwtManager.GenerateToken(usr.user_name);
                //return the token
                registerResponse.token = token;
                registerResponse.user_name = usr.user_name;
                registerResponse.user_id = usr.user_id;
                registerResponse.user_email = usr.user_email;
                return ToJson(registerResponse);
            }
            else
            {
                // if user not valid send Bad Request status code in response
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }
        private User CheckUser(string username, string password)
        {
            var user = _userService.GetUser(username, password);
            return user;
        }
    }
}

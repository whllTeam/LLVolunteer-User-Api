using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManager.API.Model;

namespace UserManager.API.Controllers
{
    [Route("api/UserManager")]
    [ApiController]
    public class UserManagerController : ControllerBase
    {
        private readonly UserManager<UserIdentity> _userManager;

        public UserManagerController(UserManager<UserIdentity> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> CheckHasUserName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest(false);
            }

            var user = await _userManager.FindByNameAsync(userName);
            var result = user != null;
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserVO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(false);
            }
            else
            {
                if (await _userManager.FindByNameAsync(user.UserName) != null)
                {
                    return BadRequest(false);
                }
                else
                {
                    var addUser = new UserIdentity
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                        EmailConfirmed = true,
                    };

                    var result = await _userManager.CreateAsync(addUser, user.Password);

                    return Ok(result);
                }
            }
        }
        [HttpPost("infoChange")]
        public async Task<IActionResult> ChangeUserPwd(UserChangePwd user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var currentUser = await _userManager.FindByNameAsync(user.UserName);
                if (currentUser == null)
                {
                    return BadRequest();
                }
                else
                {
                    var result = await _userManager.ChangePasswordAsync(currentUser, user.Password, user.NewPwd);
                    return Ok(result);
                }
            }
        }
    }
}
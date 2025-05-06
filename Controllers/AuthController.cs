using MangoApi.AppContext;
using MangoApi.MangoModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MangoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _dbcontext;


        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext dbcontext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbcontext = dbcontext;

        }

        [HttpGet]
        [Route("CheckRoles")]
        public IActionResult Get()
        {
            var roles = _roleManager.Roles.ToList();

            return Ok(roles);
        }


        [HttpPost]
        [Route("AddRoles")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name is required");
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                return BadRequest("Role already exists");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                return Ok("Role created successfully");
            }

            return StatusCode(500, "Internal server error");
        }


        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Retrieve the roles associated with the user
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
            {
                // Remove the roles associated with the user
                var roleResult = await _userManager.RemoveFromRolesAsync(user, roles);
                if (!roleResult.Succeeded)
                {
                    return BadRequest(roleResult.Errors);
                }
            }

            // Delete the user
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { Message = "User deleted successfully" });
            }

            // If the deletion failed, return the errors
            return BadRequest(result.Errors);
        }



        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users.ToList();

            return Ok(users);
        }


        [HttpPost]
        [Route("MapUserToRole")]
        public async Task<IActionResult> MapUsersToRole(UserModel userModel)
        {
            // Validate the input
            if (userModel.userName == null || !userModel.userName.Any())
            {
                return BadRequest("UserEmails are required");
            }

            if (string.IsNullOrWhiteSpace(userModel.UserRole))
            {
                return BadRequest("UserRole is required");
            }

            // Check if the role exists
            var roleExists = await _roleManager.RoleExistsAsync(userModel.UserRole);
            if (!roleExists)
            {
                return BadRequest("Role does not exist");
            }

            foreach (var userEmail in userModel.userName)
            {
                // Find the user by email
                var user = await _userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    return BadRequest($"User {userEmail} does not exist");
                }

                // Get current roles for the user
                var currentRoles = await _userManager.GetRolesAsync(user);

                // Check if the user already has the role
                if (currentRoles.Contains(userModel.UserRole))
                {
                    continue; // Skip if the user already has this role
                }

                // If user has other roles, remove them
                if (currentRoles.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        return BadRequest($"Failed to remove existing roles for user {userEmail}");
                    }
                }

                // Assign the user to the role
                var assignResult = await _userManager.AddToRoleAsync(user, userModel.UserRole);
                if (!assignResult.Succeeded)
                {
                    return BadRequest($"Failed to map user {userEmail} to role");
                }
            }

            return Ok("Successfully mapped users to role");
        }

        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest("user does not exist");
            }


            var roles = await _userManager.GetRolesAsync(user);

            return Ok(roles);

        }

        [HttpPost]
        [Route("RemoveUserFromRole")]
        public async Task<IActionResult> RemoveUserRole(string email, string rolename)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest("User not found ");
            }

            else
            {
                var removerole = await _userManager.RemoveFromRoleAsync(user, rolename);

                return Ok(removerole);
            }
        }


        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(UserResetPasswordint request)
        {
            // Find the user by email
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Verify the temporary password
            var passwordCheck = await _userManager.CheckPasswordAsync(user, request.TemporaryPassword);
            if (!passwordCheck)
            {
                return BadRequest(new { Message = "Temporary password is incorrect" });
            }

            // Generate the password reset token
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Reset the password
            var result = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Password reset successfully" });
            }

            // If the password reset failed, return the errors
            return BadRequest(result.Errors);
        }



        [HttpPost]
        [Route("ResetPassword1")]
        public async Task<IActionResult> ResetPasswordint(UserResetPasswordRequest request)
        {
            // Define the temporary password
            string tempPassword = "Connect@12345";

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(request.username);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            // Generate the password reset token
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Reset the password
            var result = await _userManager.ResetPasswordAsync(user, resetToken, tempPassword);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Password reset successfully" });
            }

            // If the password reset failed, return the errors
            return BadRequest(result.Errors);
        }

        [HttpGet]
        [Route("GetAllUserRoles")]
        public async Task<IActionResult> GetAllUserRoles()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.ToList();

            var userRoles = new List<UserRoleDto>();

            foreach (var user in users)
            {
                var userRolesIds = await _userManager.GetRolesAsync(user);

                if (userRolesIds.Any())
                {
                    // User has assigned roles
                    foreach (var roleName in userRolesIds)
                    {
                        var role = roles.FirstOrDefault(r => r.Name == roleName);
                        if (role != null)
                        {
                            userRoles.Add(new UserRoleDto
                            {
                                UserName = user.UserName,
                                RoleName = role.Name
                            });
                        }
                    }
                }
                else
                {
                    // User has no assigned roles
                    userRoles.Add(new UserRoleDto
                    {
                        UserName = user.UserName,
                        RoleName = "Role not assigned"
                    });
                }
            }

            return Ok(userRoles);
        }
    }
}

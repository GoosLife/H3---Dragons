using H3___Dragons.Authorization;
using H3___Dragons.DataAccess;
using H3___Dragons.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection.Metadata;

namespace H3___Dragons.Controllers
{
    [ApiController]
    [Route("")]
    public class AuthenticationController : Controller
    {
        [HttpPost]
        [Route("register")]
        public string Register(string dragonName, string password)
        {
            User user = new User(dragonName, password);

            try
            {
                MockDatabase.TryAddUser(user);

                // 201 Created - Succesfully created user
                Response.StatusCode = 201;
                return "User created successfully.";
            }
            catch (InvalidOperationException ex)
            {
                // 409 Conflict - User already exists
                Response.StatusCode = 409;
                return "A user with this username already exists. Please choose a unique username, or log in if you're already registered.";
            }
            catch { throw; }
        }

        [HttpPost]
        [Route("login")]
        public string Login(string dragonName, string password)
        {
            // Return the same response regardless of whether the user exists or not, to prevent leaking information about whether a user exists or not.
            string failedToLoginMessage = "Failed to log in.\nPlease check your username and password, and try again.";

            User user = MockDatabase.Users.Find(user => user.DragonName == dragonName);
            if (user == null)
            {
                // 401 Unauthorized - User not found. We could use 404 Not Found, but that would leak information about whether a user exists or not.
                Response.StatusCode = 401;
                return failedToLoginMessage;
            }
            // Obviously we wouldn't store passwords in plaintext in a real application, but this is just a mockup.
            else if (user.Password != password)
            {
                // 401 Unauthorized - Incorrect password
                Response.StatusCode = 401;
                return failedToLoginMessage;
            }
            else
            {
                // Generate JWT and save as cookie
                string jwt = JwtManager.GenerateJwt(user.DragonName, user.Role);
                Response.Cookies.Append("authorization", jwt);

                // 200 OK - Succesfully logged in
                Response.StatusCode = 200;
                return "Logged in successfully.";
            }
        }

        [HttpPost]
        [Route("register/musician")]
        public string RegisterMusician(string dragonName, string password)
        {
            User user = new User(dragonName, password, "musician");
            try
            {
                MockDatabase.TryAddUser(user);
                // 201 Created - Succesfully created user
                Response.StatusCode = 201;
                return "User created successfully.";
            }
            catch (InvalidOperationException ex)
            {
                // 409 Conflict - User already exists
                Response.StatusCode = 409;
                return "A user with this username already exists. Please choose a unique username, or log in if you're already registered.";
            }
            catch { throw; }
        }
    }
}

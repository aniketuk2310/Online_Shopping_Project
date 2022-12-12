using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Online_Shopping_WebAPICore.Models;
using Online_Shopping_WebAPPClient.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;

namespace Online_Shopping_WebAPPClient.Controllers
{
    public class AccountClientController : Controller
    {
        #region Login
        [HttpGet]
        public IActionResult Login(string message)
        {
            ViewBag.Message = message;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto user)
        {
            JwtSecurityTokenHandler receivedToken = new JwtSecurityTokenHandler();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync("https://localhost:44351/api/Token", content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponseToken = await response.Content.ReadAsStringAsync();

                        var cookieOptions = new CookieOptions
                        {
                            HttpOnly = true,
                            Expires = System.DateTime.UtcNow.AddHours(1),
                        };
                        Response.Cookies.Append("jwtCookie", apiResponseToken, cookieOptions);
                        return RedirectToAction("Index", "ProductClient");
                    }
                    else
                    {
                        ViewBag.Message = "Invalid credintials.";
                    }
                }
            }
            return View();
        }
        #endregion

        #region Logout
        public IActionResult Logout()
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = System.DateTime.Now,
            };
            Response.Cookies.Append("jwtCookie", string.Empty, cookieOptions);
            return RedirectToAction("Login");
        }
        #endregion

        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            registerDto.role = "customer";
            using (HttpClient httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44351/api/User", content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToAction("Login");
                    }
                }
            }
            return View();
        }
        #endregion

        #region Forgot/Reset Password
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult ResetPassword()
        {
            return View();
        }
        #endregion
    }
}

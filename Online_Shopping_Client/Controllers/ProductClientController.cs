using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Online_Shopping_WebAPICore.Models;
using Online_Shopping_WebAPPClient.Models;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Online_Shopping_WebAPPClient.Controllers
{
    public class ProductClientController : Controller
    {
        #region Index
        public async Task<IActionResult> Index()
        {
            Validate();
            List<Product> productList = new List<Product>();
            //create obj of HttpClient to initiate api call
            using (HttpClient httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44351/api/Product"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        productList = JsonConvert.DeserializeObject<List<Product>>(apiResponse)!;
                    }
                }
            }
            return View(productList);
        }
        #endregion

        #region GetProduct


        [HttpGet]
        public async Task<ActionResult<Product>> GetProduct(int? id)
        {
            Validate();
            Product product = null;

            using (HttpClient httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44351/api/Product/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(apiResponse)!;
                }
            }
            return View(product);
        }
        #endregion

        #region Add Product
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            var jwt = Request.Cookies["jwtCookie"];
            Validate();
            IEnumerable<CategoryDto> categories = new List<CategoryDto>();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                using (var response = await httpClient.GetAsync("https://localhost:44351/api/Category"))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        ViewBag.message = "Unauthorized access.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        categories = JsonConvert.DeserializeObject<List<CategoryDto>>(apiResponse)!;
                        TempData["Category"] = new SelectList(categories, "CategoryId", "CategoryName");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            Validate();
            Product receivedProduct = new Product();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44351/api/Product", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedProduct = JsonConvert.DeserializeObject<Product>(apiResponse)!;
                }
            }
            return RedirectToAction("Index");
        }
        #endregion


        #region Add To Cart
        public async Task<IActionResult> AddToCart(int id)
        {
            Validate();
            var jwt = Request.Cookies["jwtCookie"];
            int uid = Convert.ToInt32(Validate());
            var c = Request.Cookies[uid.ToString()];
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                if (Request.Cookies[uid.ToString()] == null)
                {
                    string content = id.ToString();

                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = System.DateTime.UtcNow.AddDays(1),
                    };
                    Response.Cookies.Append(uid.ToString(), content, cookieOptions);

                    TempData["alert"] = "Item Added to Cart Successfully..!!";

                }
                else if (Request.Cookies[uid.ToString()] != null)
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = System.DateTime.UtcNow.AddDays(1),
                    };
                    Response.Cookies.Append(uid.ToString(), Request.Cookies[uid.ToString()] + "|" + id.ToString(), cookieOptions);

                    TempData["alert"] = "Item Added to Cart Successfully..!!";
                }
                else
                    TempData["alert"] = "Item Cannot Add to Cart..!!";
            }
            return RedirectToAction("GetProduct", new { id });
        }
        #endregion

        #region Show Cart
        [HttpGet]
        public async Task<IActionResult> ShowCart()
        {
            Validate();
            int uid = Convert.ToInt32(Validate());
            List<Product> products = new List<Product>();
            string cartIds;
            var cart = Request.Cookies[uid.ToString()];
            if (cart != null)
            {
                cartIds = Convert.ToString(cart);
                string[] Ids = cartIds.Split('|');
                int j;
                for (int i = 0; i < Ids.Length; i++)
                {
                    j = Convert.ToInt32(Ids[i]);
                    Product product = await getCarts(j);

                    products.Add(product);
                }
            }
            return View(products);
        }
        #endregion

        #region Get Carts Method
        public async Task<Product> getCarts(int id)
        {
            var jwt = Request.Cookies["jwtCookie"];
            Validate();
            Product product = null;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                using (var response = await httpClient.GetAsync("https://localhost:44351/api/Product/" + id))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ViewBag.message = "Unauthorized access.";
                        //return RedirectToAction("Login", "AccountClient");
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(apiResponse)!;
                    //else
                    //    ViewBag.StatusCode = response.StatusCode;
                }
            }
            return product;
        }
        #endregion

        #region Validate Method
        public string Validate()
        {
            var jwt = Request.Cookies["jwtCookie"];
            string idvalue = null;
            //pass jwt token in request header
            if (jwt != null)
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(jwt);
                var namevalue = jwtSecurityToken.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;
                var role = jwtSecurityToken.Claims.First(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
                idvalue = jwtSecurityToken.Claims.First(claim => claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/serialnumber").Value;

                TempData["greet"] = namevalue;
                ViewBag.role = role;
            }

            return idvalue;
        }
        #endregion

        #region Get User Method
        public async Task<RegisterDto> getUser(int id)
        {
            RegisterDto user = null;

            using (HttpClient httpClient = new HttpClient())
            {
                //var user = await httpClient.GetAsync("https://localhost:44351/api/Token/" + id);

                using (var response = await httpClient.GetAsync("https://localhost:44351/api/User/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<RegisterDto>(apiResponse)!;
                    //else
                    //    ViewBag.StatusCode = response.StatusCode;
                }
            }
            return user;
        }
        #endregion

        #region Buy Now
        [HttpGet]
        public IActionResult BuyNow(int id)
        {
            Validate();
            HttpContext.Session.SetInt32("prodId", id);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BuyNow(OrderDto orderDto)
        {
            var jwt = Request.Cookies["jwtCookie"];
            Validate();
            var pid = HttpContext.Session.GetInt32("prodId");
            int uid = Convert.ToInt32(Validate());
            RegisterDto user = await getUser(uid);
            Product product = await getCarts(Convert.ToInt32(pid));
            user.id = uid;
            Orders orders = new Orders();
            orders.PaymentMethod = orderDto.PaymentMethod;
            orders.Product = product;
            orders.Customer = user;
            orders.OrderDate = DateTime.Now;
            orders.TotalPrice = product.UnitPrice * orderDto.Quantity;
            orders.Quantity = orderDto.Quantity;
            orders.status = OStatus.Undelivered;

            Orders orders1 = null;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                StringContent content = new StringContent(JsonConvert.SerializeObject(orders), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("https://localhost:44351/api/Order/", content))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        orders1 = JsonConvert.DeserializeObject<Orders>(apiResponse)!;
                        return RedirectToAction("Thanks");
                    }
                }
            }
            //return View();
            return RedirectToAction("Thanks");

        }
        #endregion

        #region Thanks Page
        public IActionResult Thanks()
        {
            Validate();
            return View();
        }
        #endregion

        #region Sort by Category
        [HttpGet]
        public async Task<IActionResult> SortByCategory()
        {
            Validate();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SortByCategory(int cat)
        {
            Validate();
            List<Product> productList = new List<Product>();
            //create obj of HttpClient to initiate api call
            using (HttpClient httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://localhost:44351/api/Category" + cat))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        productList = JsonConvert.DeserializeObject<List<Product>>(apiResponse)!;
                    }
                }
            }
            //return View(productList);
            return RedirectToAction("Index", productList);
        }
        #endregion
    }
}

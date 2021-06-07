using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportWiz66.Data;
using SportWiz66.Models;

namespace SportWiz66.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SportWiz66Context _context;

        public AccountsController(SportWiz66Context context)
        {
            _context = context;
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                                           Admin Mangement:
        ///                                                  
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Admin()
        {
            return View(await _context.Account.ToListAsync());
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.AccountTypes = new SelectList(Enum.GetValues(typeof(UserType)));
            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,FirstName,LastName,Email,Password,ConfirmPassword,RegistrationDate,Type")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();

                    //we may change to Role so we need to update the Claims
                    var user = _context.Account.FirstOrDefault(u => u.Id == id);
                    CookieAuthentication(user);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Admin));
            }
            return View(account);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Account.FindAsync(id);

            //delete his reviews & orders
            _context.Review.RemoveRange(_context.Review.Where(p => p.SentBy == account));
            _context.OrderDetail.RemoveRange(_context.OrderDetail.Where(p => p.AssociatedOrder.AssociatedAccount == account));
            _context.Order.RemoveRange(_context.Order.Where(p => p.AssociatedAccount == account));

            _context.Account.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Admin));
        }



        //Filter for Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Filter(string term)
        {
            List<Account> accounts;

            if (term != null)
                accounts = await _context.Account.Where(c => c.Id.ToString().Contains(term) || c.FirstName.Contains(term) ||
                                                         c.LastName.Contains(term) || c.Email.Contains(term) ||
                                                         c.Username.Contains(term)).ToListAsync();
            else
                accounts = await _context.Account.ToListAsync();


            var query = from account in accounts
                        select new
                        {
                            id = account.Id,
                            username = account.Username,
                            firstname = account.FirstName,
                            lastname = account.LastName,
                            email = account.Email,
                            type = account.Type.ToString(),
                            registrationdate = account.RegistrationDate.ToShortDateString(),
                        };

            return Json(query);
        }


        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.Id == id);
        }
        //<--------------------------------------------END: ADMIN Mangement-------------------------------------------->







        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                              Account Registration, Login, Logout, Authentication by Cookies:
        ///                                                  
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //<--------------------------------------------Account Registration-------------------------------------------->
        ///////////////////////////
        // GET: Accounts/Register
        ///////////////////////////
        public IActionResult Register()
        {
            //if already logged in
            if (User.Claims.FirstOrDefault(c => c.Type == "AccountUserName") != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        ///////////////////////////
        // POST: Accounts/Register
        ///////////////////////////
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Username,FirstName,LastName,Email,Password,ConfirmPassword")] Account account)
        {
            //Setup UserType to Customer, Registration Date.
            account.Type = UserType.Customer;
            account.RegistrationDate = DateTime.Now;


            //(Registration Field, Error Message)
            //check if Email adress already exists.
            if (_context.Account.Any(i => i.Email == account.Email))
                ModelState.AddModelError("Email", "Email address already registered to another account.");

            //check if UserName already exists.
            if (_context.Account.Any(i => i.Username == account.Username))
                ModelState.AddModelError("UserName", "That Username is taken, Try another.");


            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                CookieAuthentication(account);
                return RedirectToAction("Index", "Home");
            }
            return View(account);
        }
        //<---------------------------------------------------END------------------------------------------------------>





        //<--------------------------------------------Account Login & Cookie Auth----------------------------------------->
        ///////////////////////////
        // GET: Accounts/Login
        ///////////////////////////
        public IActionResult Login()
        {
            //if already logged in
            if (User.Claims.FirstOrDefault(c => c.Type == "AccountUserName") != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        ///////////////////////////
        // POST: Login = Check if user exsits and then create a CookieAuth D.M
        ///////////////////////////
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var account = _context.Account.FirstOrDefault(i => i.Email == email && i.Password == password);

            if (account != null)
            {
                CookieAuthentication(account);
                return RedirectToAction("Index", "Home");
            }

            //need to create error message if login failed
            return View();
        }


        /////////////////////////////////////////////////
        // Private Method: Accounts Cookie Authentication
        /////////////////////////////////////////////////
        //Session configuration[10Min] located in Startup.cs
        private async void CookieAuthentication(Account account)
        {
            //info you can use when using[Authorized], ex.[Authorize (Roles = "Admin")].
            var claims = new List<Claim>
                {
                    new Claim("Name", account.FirstName),
                    new Claim("AccountUserName", account.Username),
                    new Claim(ClaimTypes.Role, account.Type.ToString()),
                };

            //startup.cs configuration
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //options
            var authProperties = new AuthenticationProperties
            {
                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
        //<---------------------------------------------------END------------------------------------------------------>





        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        ///                                                                Customer Area:
        ///                                                  
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //Account Details for specific Id taken from the Claims 
        public async Task<IActionResult> MyAccount()
        {
            ////not logged in => so login!
            if (User.Claims.FirstOrDefault(c => c.Type == "AccountUserName") == null)
                return RedirectToAction("Login");

            var accountUserName = User.Claims.FirstOrDefault(c => c.Type == "AccountUserName").Value;
            var account = await _context.Account.FirstOrDefaultAsync(m => m.Username == accountUserName);

            return View(account);
        }

        public async Task<IActionResult> EditMyAccount()
        {
            if (User.Claims.FirstOrDefault(c => c.Type == "AccountUserName") == null) //not logged in
                return RedirectToAction("Login");

            var accountUserName = User.Claims.FirstOrDefault(c => c.Type == "AccountUserName").Value;
            var account = await _context.Account.FirstOrDefaultAsync(m => m.Username == accountUserName);

            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMyAccount(string Username, [Bind("Id,Username,FirstName,LastName,Email,Password,ConfirmPassword,CreditCardNum,RegistrationDate")] Account account)
        {
            if (User.Claims.FirstOrDefault(c => c.Type == "AccountUserName") == null) //not logged in
                return RedirectToAction("Login");

            //Details taken from Claims compared to the one with the post request to auth
            var accountUserName = User.Claims.FirstOrDefault(c => c.Type == "AccountUserName").Value;
            if (Username != accountUserName)
            {
                return RedirectToAction("Login");
            }



            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyAccount));
            }
            return RedirectToAction("MyAccount");
        }
        //<---------------------------------------------END: Customer Area--------------------------------------------->
    }




}

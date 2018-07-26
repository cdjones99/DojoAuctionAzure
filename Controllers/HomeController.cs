using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LoginReg.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LoginReg.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _context;
        public HomeController(UserContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId != null)
            {
                return RedirectToAction("dashboard");
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost]
        [Route("new")]
        public IActionResult newUser(User newUser)
        {
            if (ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                newUser.confirm_password = Hasher.HashPassword(newUser, newUser.confirm_password);
                _context.Add(newUser);
                _context.SaveChanges();
                Console.WriteLine("Form Accepted.");
                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine("Form was not Accepted.");
                return View("Index");
            }
        }
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId != null)
            {
                User currentUser = _context.user.SingleOrDefault(u => u.userid == UserId);
                List<Auction> allAuctions = _context.auction.ToList();
                List<User> allUsers = _context.user.ToList();
                ViewBag.allAuctions = allAuctions;
                ViewBag.currentUser = currentUser;
                ViewBag.allUsers = allUsers;
                return View("dashboard");
            }
            else
            {
                return View("Index");
            }
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(string Email, string Password)
        {
            Console.WriteLine("Email is: " + Email);
            Console.WriteLine("Password is: " + Password);

            User checkUser = _context.user.SingleOrDefault(user => user.Email == Email);
            if (checkUser != null && Password != null)
            {
                var Hasher = new PasswordHasher<User>();
                if (0 != Hasher.VerifyHashedPassword(checkUser, checkUser.Password, Password))
                {
                    HttpContext.Session.SetInt32("UserId", checkUser.userid);
                    return RedirectToAction("dashboard");
                }
                else
                {
                    return View("Index");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }


        [HttpGet]
        [Route("auction")]

        public IActionResult auction()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            return View("auction");
        }




        [HttpPost]
        [Route("newAuction")]
        public IActionResult newAuction(Auction newAuction)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (ModelState.IsValid)
            {
                User currentUser = _context.user.Include(user => user.Auctions).SingleOrDefault(u => u.userid == UserId);
                List<Auction> allIdeas = _context.auction.ToList();

                ViewBag.currentUser = currentUser;
                ViewBag.allIdeas = allIdeas;
                currentUser.Auctions.Add(newAuction);

                // _context.Add(newIdea);

                _context.SaveChanges();
                Console.WriteLine("Form Accepted.");
                return RedirectToAction("Dashboard");
            }
            else
            {
                Console.WriteLine("Form was not Accepted.");
                return View("auction");
            }
        }

        [HttpGet("Home/auctionDetails/{id}")]

        public IActionResult auctionDetails(int id) 
        {
        Auction auctionDet = _context.auction.Include(u => u.user).SingleOrDefault(u => u.auctionid == id);
        ViewBag.auctionDet = auctionDet;

        return View("AuctionDetails");

        }

        [HttpGet("Home/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            Auction auct = _context.auction.FirstOrDefault(a => a.auctionid == id);


            _context.Remove(auct);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}

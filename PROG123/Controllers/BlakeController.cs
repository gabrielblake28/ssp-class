using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PROG123.DAL;
using PROG123.Models;



namespace PROG123.Controllers
{
    public class BlakeController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public BlakeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn(LogInCredentialsModel logInCredentialsModel)
        {

            var person = new DALPerson(_configuration);
            var loggedInPerson = person.CheckLogInCredentials(logInCredentialsModel);

            if (loggedInPerson == null)
            {
               @ViewBag.LoginMessage = "Login Failed";
            } else
            {
                HttpContext.Session.SetString("PersonID", loggedInPerson.PersonID);
                HttpContext.Session.SetString("FName", loggedInPerson.FName);
                HttpContext.Session.SetString("LName", loggedInPerson.LName);
                Console.Write(loggedInPerson);

               @ViewBag.UserFirstName = loggedInPerson.FName;
               @ViewBag.UserLastName = loggedInPerson.LName;

            }
            

            return View("Index");
        }

        public IActionResult EnterNewProduct()
        {
            string id = HttpContext.Session.GetString("PersonID");
            if (id != null)
            {
                return View();
                
            }

            return View("Index");
        }

        public IActionResult AddProductToDB(ProductModel productModel)
        {
            //if logged in 
            // create a DALProduct object
            // call addNewProduct with the object as the param
            //save the product id that is returned
            // set the product id in the product model
            // return view with product model as the param

            string id = HttpContext.Session.GetString("PersonID");
            if (id != null)
            {
                DALProducts newProduct = new DALProducts(_configuration);
                var productId = newProduct.AddNewProduc(productModel);
                productModel.PID = productId;
                return View(productModel);
            }

            return View("Index");
        }

    }
}


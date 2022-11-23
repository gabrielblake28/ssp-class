using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PROG123.DAL;
using PROG123.Models;
using PROG123.utils;

namespace PROG123.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // this is for testing purpuse only.
            DatabaseHelper dbh = new DatabaseHelper(_configuration);
            ConnStatusModel status = dbh.GetConnectionStringAndConnectionStatus();
            ViewBag.ConnStr = status.ConnStr;
            ViewBag.DBStatus = status.DBConnectionStatus;
            ViewBag.Exception = status.Exception;
            return View();
        }

        // add your actions here 
        
        public IActionResult Page2(PersonModel personModel)
        {

            var person = new DALPerson(_configuration);
            HttpContext.Session.SetString("PersonID", person.AddPerson(personModel));
            return View(personModel);
        }

        public IActionResult EditPerson(PersonModel personModel)
        {
            return View(new DALPerson(_configuration).getPerson(HttpContext.Session.GetString("PersonID")));
        }

        public IActionResult UpdatePerson(PersonModel person)
        {
            string id = HttpContext.Session.GetString("PersonID");
            person.PersonID = id;

            DALPerson dp = new DALPerson(_configuration);

            dp.UpdatePerson(person);

            return View("page2", person);
        }

        public IActionResult DeletePerson(PersonModel person)
        {

            string id = HttpContext.Session.GetString("PersonID");
            person.PersonID = id;

            DALPerson dp = new DALPerson(_configuration);

            dp.DeletePerson(id);

            return View();
           
        }

    }
}

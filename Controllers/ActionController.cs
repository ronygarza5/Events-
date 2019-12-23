using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Exam.Controllers
{
    [Route("Action")]
    public class ActionController : Controller
    {

        private HomeContext dbContext;

        public ActionController(HomeContext context)
        {
            dbContext = context;
        }

        [HttpGet("home")]
        public IActionResult Home()
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            else
            {
                User userInDb = dbContext.Users.Include(u => u.PlannedActs).FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UserEmail"));
                ViewBag.User = userInDb;

                List<Plan> AllPlans = dbContext.Plans.Include(p => p.ShowingUp).ThenInclude(r => r.Participent).ToList();
                return View(AllPlans);
            }
        }
        [HttpGet("new")]
        public IActionResult New()
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            else
            {
                User userInDb = dbContext.Users.Include(u => u.PlannedActs).FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UserEmail"));

                ViewBag.User = userInDb;
                return View();
            }
        }
        [HttpPost("create")]
        public IActionResult Create(Plan act)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    dbContext.Plans.Add(act);
                    dbContext.SaveChanges();
                    return Redirect($"plan/{act.PlanId}");
                }
                else
                {
                    User userInDb = dbContext.Users.Include(u => u.PlannedActs).FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UserEmail"));
                    ViewBag.User = userInDb;
                    return View("New");
                }
            }
        }

        [HttpGet("plan/{planId}")]
        public IActionResult Plan(int planId)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            else
            {
                User userInDb = dbContext.Users.Include(u => u.PlannedActs).FirstOrDefault(u => u.Email == HttpContext.Session.GetString("UserEmail"));
                ViewBag.User = userInDb;

                Plan show = dbContext.Plans.Include(p => p.ShowingUp).ThenInclude(r => r.Participent).Include(p => p.Planner).FirstOrDefault(u => u.PlanId == planId);
                ViewBag.People = dbContext.Plans;
                return View(show);
            }
        }

        [HttpGet("delete/{planId}")]
        public IActionResult Delete(int planId)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            else
            {
                Plan delete = dbContext.Plans.FirstOrDefault(p => p.PlanId == planId);
                dbContext.Plans.Remove(delete);
                dbContext.SaveChanges();
                return RedirectToAction("Home");
            }
        }

        [HttpGet("rsvp/{planId}/{userId}/{status}")]
        public IActionResult Join(int planId, int userId, string status)
        {
            if (HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            else
            {
                if(status == "add")
                {
                    Rsvp response = new Rsvp();
                    response.PlanId = planId;
                    response.UserId = userId;
                    dbContext.Rsvps.Add(response);
                    dbContext.SaveChanges();
                }
                if(status == "remove")
                {
                    Rsvp remove = dbContext.Rsvps.FirstOrDefault(r => r.PlanId == planId && r.UserId == userId);
                    dbContext.Rsvps.Remove(remove);
                    dbContext.SaveChanges();
                }
                return RedirectToAction("Home");
            }
        }

    }
}


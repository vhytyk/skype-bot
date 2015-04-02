using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using SkypeBotRulesLibrary.Entities;
using SkypeBotRulesLibrary.Implementations;
using SkypeBotRulesLibrary.Interfaces;
using SkypeBotWebApi.Models;

namespace SkypeBotWebApi.Controllers
{
    public class SkypeNamesController : Controller
    {
        private ISkypeNameService _skypeNameService;

        public SkypeNamesController()
        {
            _skypeNameService = new SkypeNameService();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Skype names";

            var model = new SkypeNameInfoViewModel();
            model.SkypeNames =_skypeNameService.GetAll();

            return View(model);
        }

        public ActionResult AddNew()
        {
            return View(new SkypeNameInfo());
        }

        public ActionResult Add(SkypeNameInfo newItem)
        {
            _skypeNameService.Add(newItem);
            return RedirectToAction("Index");
        }

        public ActionResult Remove(int id)
        {
            _skypeNameService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkypeBotRulesLibrary;
using SkypeBotRulesLibrary.Entities;
using SkypeBotRulesLibrary.Implementations;
using SkypeBotWebApi.Models;

namespace SkypeBotWebApi.Controllers
{
    public class BaseController<T> : Controller where T : new()
    {
        private readonly IUniversalDal<T> _universalDal;

        public BaseController() 
        {
            _universalDal = new UniversalDal<T>();
        }

        public ActionResult Index()
        {
            ViewBag.Title = GetType().Name;
            var model = new UniversalViewModel(typeof(T));
            model.All = _universalDal.GetAll().Cast<object>().ToList();
            return View("~/Views/Universal/Index.cshtml", model);
        }

        public ActionResult AddNew()
        {
            return View("~/Views/Universal/Add.cshtml", Activator.CreateInstance<T>());
        }

        public ActionResult Add(T model)
        {
            try
            {
                _universalDal.Add(model);
            }
            catch (Exception ex)
            {
                return View("~/Views/Universal/Add.cshtml", model);
            }
            return RedirectToAction("Index");            
        }

        public ActionResult EditItem(T model)
        {
            _universalDal.Update(model);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int Id)
        {
            return View("~/Views/Universal/Edit.cshtml", _universalDal.GetById(Id));
        }

        public ActionResult Delete(int Id)
        {
            _universalDal.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
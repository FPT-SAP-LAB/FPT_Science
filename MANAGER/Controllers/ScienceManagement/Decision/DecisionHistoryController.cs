﻿using BLL.ScienceManagement.DecisionHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MANAGER.Controllers.ScienceManagement.Decision
{
    public class DecisionHistoryController : Controller
    {
        DecisionRepo dr = new DecisionRepo();
        // GET: Decision
        public ActionResult History()
        {
            List<ENTITIES.Decision> list = dr.getListDecision("");
            ViewBag.list = list;
            return View();
        }

        [HttpPost]
        public ActionResult List(string search)
        {
            if (search == null) search = "";
            try
            {
                List<ENTITIES.Decision> list = dr.getListDecision(search);
                return Json(new { list = list, mess = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { mess = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult deleteItem(int cri_id)
        {
            string mess = dr.deleteDecision(cri_id);
            return Json(new { mess = mess }, JsonRequestBehavior.AllowGet);
        }
    }
}
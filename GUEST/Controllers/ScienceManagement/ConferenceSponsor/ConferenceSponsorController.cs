﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;
using BLL.ScienceManagement.ConferenceSponsor;

namespace GUEST.Controllers
{
    public class ConferenceSponsorController : Controller
    {
        readonly ConferenceSponsorRepo repos = new ConferenceSponsorRepo();
        // GET: ConferenceSponsor
        public ActionResult Index()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
            };
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        public ActionResult Add()
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
                new PageTree("Thêm","/ConferenceSponsor/Add"),
            };
            ViewBag.countries = repos.GetAllCountries();
            ViewBag.pagesTree = pagesTree;
            return View();
        }
        public ActionResult Detail(int id)
        {
            var pagesTree = new List<PageTree>
            {
                new PageTree("Đề nghị hỗ trợ hội nghị","/ConferenceSponsor"),
                new PageTree("Chi tiết","/ConferenceSponsor/Detail"),
            };
            ViewBag.pagesTree = pagesTree;
            ViewBag.id = id;
            return View();
        }
        [ChildActionOnly]
        public ActionResult CostMenu(int id)
        {
            ViewBag.id = id;
            ViewBag.CheckboxColumn = id == 2;
            ViewBag.ReimbursementColumn = id >= 3;
            ViewBag.EditAble = id == 2;
            return PartialView();
        }
        public JsonResult GetInformationPeopleWithID(string id)
        {
            var infos = repos.GetAllProfileBy(id);
            return Json(infos, JsonRequestBehavior.AllowGet);
        }
    }
}
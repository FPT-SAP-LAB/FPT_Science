﻿using BLL.Authen;
using BLL.ScienceManagement.Citation;
using BLL.ScienceManagement.Comment;
using BLL.ScienceManagement.MasterData;
using BLL.ScienceManagement.Paper;
using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement;
using ENTITIES.CustomModels.ScienceManagement.Comment;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using GUEST.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using User.Models;

namespace User.Controllers
{
    public class CitationController : Controller
    {
        CitationRepo cr = new CitationRepo();
        MasterDataRepo md = new MasterDataRepo();
        CommentRepo crr = new CommentRepo();
        PaperRepo pr = new PaperRepo();
        // GET: Citation

        [Auther(RightID = "29")]
        public ActionResult List()
        {
            ViewBag.title = "Số trích dẫn";
            var pagesTree = new List<PageTree>
           {
               new PageTree("Số trích dẫn","/Citation/List"),
           };
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        [Auther(RightID = "29")]
        public ActionResult AddRequest()
        {
            ViewBag.title = "Đề xuất khen thưởng số trích dẫn";
            var pagesTree = new List<PageTree>
           {
               new PageTree("Đề xuất khen thưởng số trích dẫn","/Citation/AddRequest"),
           };
            ViewBag.pagesTree = pagesTree;
            return View();
        }

        [Auther(RightID = "29")]
        public ActionResult Pending()
        {
            ViewBag.title = "Số trích dẫn đang xử lý";
            var pagesTree = new List<PageTree>
           {
               new PageTree("Số trích dẫn đang xử lý","/Citation/Pending"),
           };
            ViewBag.pagesTree = pagesTree;
            LoginRepo.User u = new LoginRepo.User();
            Account acc = new Account();
            if (Session["User"] != null)
            {
                u = (LoginRepo.User)Session["User"];
                acc = u.account;
            }
            List<ListOnePerson_Citation> list = cr.GetList(acc.account_id);
            ViewBag.list = list;
            for (int i = 0; i < list.Count; i++)
            {
                list[i].note = list[i].status_id + "_" + list[i].request_id;
            }
            return View();
        }

        [Auther(RightID = "29")]
        //[HttpPost]
        public ActionResult Edit(string id)
        {
            ViewBag.title = "Chỉnh sửa số trích dẫn";
            var pagesTree = new List<PageTree>
           {
               new PageTree("Chỉnh sửa số trích dẫn","/Citation/Edit"),
           };
            ViewBag.pagesTree = pagesTree;

            AuthorInfo author = cr.getAuthor(id);
            ViewBag.author = author;

            List<ENTITIES.Citation> listCitation = cr.getCitation(id);
            ViewBag.citation = listCitation;

            List<DetailComment> listCmt = crr.GetComment(Int32.Parse(id));
            ViewBag.cmt = listCmt;

            ViewBag.request_id = id;
            RequestCitation rc = cr.getRequestCitation(id);
            ViewBag.ckEdit = rc.status_id;

            return View();
        }

        [HttpPost]
        public JsonResult addCitation(List<Citation> citation, List<AddAuthor> people)
        {
            LoginRepo.User u = new LoginRepo.User();
            Account acc = new Account();
            if (Session["User"] != null)
            {
                u = (LoginRepo.User)Session["User"];
                acc = u.account;
            }

            Author author = cr.addAuthor(people);

            cr.addCitaion(citation);

            BaseRequest b = pr.addBaseRequest(acc.account_id);
            cr.addCitationRequest(b, author);
            string mess = cr.addRequestHasCitation(citation, b);
            return Json(new { mess = mess, id = b.request_id }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult editCitation(List<Citation> citation, List<AddAuthor> people, string request_id)
        {
            //cr.addAuthor(people);
            List<Citation> oldcitation = cr.getCitation(request_id);
            Author author = cr.editAuthor(people);
            string mess = cr.editCitation(oldcitation, citation, request_id, author);
            return Json(new { mess = mess, id = request_id }, JsonRequestBehavior.AllowGet);
        }
    }
}

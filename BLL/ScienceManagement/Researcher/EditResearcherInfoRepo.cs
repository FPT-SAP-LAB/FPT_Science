﻿using ENTITIES;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.Researcher
{
    public class EditResearcherInfoRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public int EditResearcherProfile(string data)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    var editInfo = JObject.Parse(data);
                    int id = (int)editInfo["info"]["people_id"];
                    Person person = db.People.Find(id);
                    person.name = (string)editInfo["info"]["name"];
                    /////////////////////////////////////////////////
                    Profile profile = db.Profiles.Find(id);
                    int nationality = (int)editInfo["info"]["nationality"][0]["id"];
                    profile.country_id = nationality;
                    db.SaveChanges();
                    ///////////////////////////////////////////////////////
                    List<int> title_ids = new List<int>();
                    foreach (var i in editInfo["info"]["title"])
                    {
                        title_ids.Add((int)i["id"]);
                    }
                    List<Title> titles = db.Titles.Where(x => title_ids.Contains(x.title_id)).ToList<Title>();
                    profile.Titles.Clear();
                    foreach(Title t in titles)
                    {
                        profile.Titles.Add(t);
                    }
                    db.SaveChanges();
                    ///////////////////////////////////////////////////////
                    List<int> position_ids = new List<int>();
                    foreach (var i in editInfo["info"]["position"])
                    {
                        position_ids.Add((int)i["id"]);
                    }
                    List<Position> positions = db.Positions.Where(x => position_ids.Contains(x.position_id)).ToList<Position>();
                    profile.Positions.Clear();
                    foreach (Position p in positions)
                    {
                        profile.Positions.Add(p);
                    }
                    db.SaveChanges();
                    //////////////////////////////////////////////////////
                    string birthdate = (string)editInfo["info"]["dob"];
                    string phone = (string)editInfo["info"]["phone"];
                    string email = (string)editInfo["info"]["email"];
                    string website = (string)editInfo["info"]["website"];
                    string googlescholar = (string)editInfo["info"]["googlescholar"];
                    string cv = (string)editInfo["info"]["cv"];
                    profile.birth_date = DateTime.Parse(birthdate);
                    person.phone_number = phone;
                    profile.website = website;
                    profile.cv = cv;
                    person.email = email;
                    profile.google_scholar = googlescholar;
                    db.SaveChanges();
                    ///////////////////////////////////////////////////////
                    List<int> field_ids = new List<int>();
                    foreach (var i in editInfo["info"]["fields"])
                    {
                        field_ids.Add((int)i["id"]);
                    }
                    List<ResearchArea> areas = db.ResearchAreas.Where(x => field_ids.Contains(x.research_area_id)).ToList();
                    profile.ResearchAreas.Clear();
                    foreach (ResearchArea p in areas)
                    {
                        profile.ResearchAreas.Add(p);
                    }
                    db.SaveChanges();
                    trans.Commit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    trans.Rollback();
                }
            }
            return 0;
        }
    }
}
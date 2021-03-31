using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement.Researcher;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BLL.ScienceManagement.ConferenceSponsor
{
    public class ConferenceSponsorAddRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public string GetAddPageJson(int account_id, int language_id)
        {
            var Profile = (from a in db.Profiles
                           join b in db.Accounts on a.account_id equals b.account_id
                           join d in db.People on a.people_id equals d.people_id
                           where b.account_id == account_id
                           select new ProfileResearcher
                           {
                               ID = a.mssv_msnv,
                               FullName = d.name,
                               Email = b.email,
                               OfficeID = d.office_id.Value,
                               TitleID = a.title_id
                           }).FirstOrDefault();
            return JsonConvert.SerializeObject(new { Profile });
        }
        public List<Info> GetAllProfileBy(string id, int language_id)
        {
            var infos = (from a in db.Profiles
                         join b in db.People on a.people_id equals b.people_id
                         join c in db.Offices on b.office_id equals c.office_id
                         join d in db.TitleLanguages on a.title_id equals d.title_id
                         where a.mssv_msnv.Contains(id) && d.language_id == language_id
                         select new Info
                         {
                             Email = b.email,
                             MS = a.mssv_msnv,
                             Name = b.name,
                             OfficeID = c.office_id,
                             OfficeName = c.office_name,
                             TitleID = a.title_id,
                             TitleString = d.name,
                         }).Take(10).ToList();
            if (infos.Count == 0)
            {
                Info info = new Info()
                {
                    Email = "",
                    MS = id.ToUpper(),
                    Name = "",
                    OfficeID = 1,
                    OfficeName = "",
                    TitleID = 1,
                    TitleString = ""
                };
                infos = new List<Info>() { info };
            }
            return infos;
        }
        public List<Conference> GetAllConferenceBy(string name)
        {
            db.Configuration.LazyLoadingEnabled = false;
            var Conferences = db.Conferences.Where(x => x.conference_name.Contains(name)).ToList();
            return Conferences;
        }
        public string AddRequestConference(int account_id, string input, HttpPostedFileBase invite, HttpPostedFileBase paper)
        {
            List<string> FileIDs = new List<string>();
            ConferenceParticipantRepo participantRepo = new ConferenceParticipantRepo();
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    DateTime create_date = DateTime.Now;
                    Account account = db.Accounts.Find(account_id);  // Sẽ chỉnh sau khi xong tạo account
                    DataTable dt = new DataTable();
                    JObject @object = JObject.Parse(input);

                    Conference conference = @object["Conference"].ToObject<Conference>();
                    Conference temp = db.Conferences.Find(conference.conference_id);
                    int conference_id = 0;
                    string conference_name;
                    if (temp != null)
                    {
                        if (!temp.is_verified)
                        {
                            temp.website = conference.website;
                            temp.keynote_speaker = conference.keynote_speaker;
                            temp.qs_university = conference.qs_university;
                            temp.country_id = conference.country_id;
                            temp.time_start = conference.time_start;
                            temp.time_end = conference.time_end;
                            temp.formality_id = conference.formality_id;
                            temp.co_organized_unit = conference.co_organized_unit;
                        }
                        conference_id = temp.conference_id;
                        conference_name = temp.conference_name;
                    }
                    else
                    {
                        conference.is_verified = false;
                        db.Conferences.Add(conference);
                        db.SaveChanges();
                        conference_id = conference.conference_id;
                        conference_name = conference.conference_name;
                    }

                    List<HttpPostedFileBase> InputFiles = new List<HttpPostedFileBase> { paper, invite };

                    List<Google.Apis.Drive.v3.Data.File> UploadFiles = GoogleDriveService.UploadResearcherFile(InputFiles, conference_name, 1, "doanvanthang4271@gmail.com");

                    RequestConferencePolicy policy = db.RequestConferencePolicies.Where(x => x.expired_date == null).FirstOrDefault();

                    BaseRequest @base = new BaseRequest()
                    {
                        account_id = account.account_id,
                        created_date = create_date,
                        finished_date = null
                    };
                    db.BaseRequests.Add(@base);

                    File Finvite = new File()
                    {
                        link = UploadFiles[1].WebViewLink,
                        name = invite.FileName,
                        file_drive_id = UploadFiles[1].Id
                    };
                    FileIDs.Add(UploadFiles[1].Id);

                    File Fpaper = new File()
                    {
                        link = UploadFiles[0].WebViewLink,
                        name = paper.FileName,
                        file_drive_id = UploadFiles[0].Id
                    };
                    FileIDs.Add(UploadFiles[1].Id);

                    db.Files.Add(Fpaper);
                    db.Files.Add(Finvite);
                    db.SaveChanges();

                    ENTITIES.Paper Paper = new ENTITIES.Paper()
                    {
                        name = @object["paper_name"].ToString(),
                        file_id = Fpaper.file_id
                    };
                    db.Papers.Add(Paper);
                    db.SaveChanges();

                    RequestConference support = new RequestConference()
                    {
                        request_id = @base.request_id,
                        conference_id = conference_id,
                        status_id = 1,
                        policy_id = policy.policy_id,
                        editable = false,
                        reimbursement = 0,
                        attendance_start = DateTime.Parse(@object["attendance_start"].ToString()),
                        attendance_end = DateTime.Parse(@object["attendance_end"].ToString()),
                        invitation_file_id = Finvite.file_id,
                        paper_id = Paper.paper_id,
                        specialization_id = int.Parse(@object["specialization_id"].ToString())
                    };
                    db.RequestConferences.Add(support);
                    db.SaveChanges();

                    List<Cost> costs = @object["Cost"].ToObject<List<Cost>>();
                    foreach (var item in costs)
                    {
                        int total = int.Parse(dt.Compute(item.detail.Replace(",", ""), "").ToString());
                        item.editable = false;
                        item.sponsoring_organization = "FPTU";
                        item.total = total;
                        item.request_id = support.request_id;
                    }
                    db.Costs.AddRange(costs);

                    ConferenceParticipant participant = @object["ConferenceParticipant"].ToObject<ConferenceParticipant>();
                    participant.request_id = @base.request_id;
                    Person person = @object["Persons"].ToObject<Person>();
                    participantRepo.AddWithTempData(db, participant, person);

                    int? position_id = PositionRepo.GetPositionIdByAccountId(db, account_id);

                    ApprovalProcessRepo.Add(db, account_id, create_date, position_id, support.request_id, "Người đề nghị");

                    foreach (var item in policy.Criteria)
                    {
                        db.EligibilityCriterias.Add(new EligibilityCriteria
                        {
                            criteria_id = item.criteria_id,
                            is_accepted = false,
                            request_id = @base.request_id
                        });
                    }

                    db.SaveChanges();
                    trans.Commit();
                    return JsonConvert.SerializeObject(new { success = true, message = "OK", id = support.request_id });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    trans.Rollback();
                    foreach (var item in FileIDs)
                    {
                        GoogleDriveService.DeleteFile(item);
                    }
                    return JsonConvert.SerializeObject(new { success = false, message = "Có lỗi xảy ra" });
                }
            }
        }
        public class Info
        {
            public string MS { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public int OfficeID { get; set; }
            public string OfficeName { get; set; }
            public int TitleID { get; set; }
            public string TitleString { get; set; }
        }
    }
}

﻿using ENTITIES;
using ENTITIES.CustomModels.ScienceManagement.Conference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ScienceManagement.ConferenceSponsor
{
    public class ConferenceSponsorDetailRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public string GetDetailPageGuest(int request_id, int account_id, int language_id)
        {
            db.Configuration.LazyLoadingEnabled = false;
            ConferenceDetail Conference = (from r in db.BaseRequests
                                           join a in db.RequestConferences on r.request_id equals a.request_id
                                           join b in db.Conferences on a.conference_id equals b.conference_id
                                           join c in db.Countries on b.country_id equals c.country_id
                                           join d in db.Files on a.invitation_file_id equals d.file_id
                                           join e in db.Papers on a.paper_id equals e.paper_id
                                           join f in db.Files on e.file_id equals f.file_id
                                           join g in db.ConferenceStatus on a.status_id equals g.status_id
                                           join h in db.ConferenceStatusLanguages on g.status_id equals h.status_id
                                           join i in db.Formalities on b.formality_id equals i.formality_id
                                           join j in db.FormalityLanguages on i.formality_id equals j.formality_id
                                           where h.language_id == language_id && j.language_id == language_id && r.account_id == account_id && r.request_id == request_id
                                           select new ConferenceDetail
                                           {
                                               ConferenceName = b.conference_name,
                                               Website = b.website,
                                               KeynoteSpeaker = b.keynote_speaker,
                                               QsUniversity = b.qs_university,
                                               Co_organizedUnit = b.co_organized_unit,
                                               CreatedDate = r.created_date.Value,
                                               TimeEnd = b.time_end,
                                               TimeStart = b.time_start,
                                               AttendanceEnd = a.attendance_end,
                                               AttendanceStart = a.attendance_start,
                                               ConferenceID = a.conference_id,
                                               EditAble = a.editable,
                                               InvitationLink = d.link,
                                               PaperLink = f.link,
                                               PaperName = e.name,
                                               RequestID = a.request_id,
                                               CountryName = c.country_name,
                                               StatusName = h.name,
                                               FormalityName = j.name,
                                               Reimbursement = a.reimbursement
                                           }).FirstOrDefault();
            List<ConferenceParticipantExtend> Participants = (from b in db.ConferenceParticipants
                                                              join c in db.TitleLanguages on b.title_id equals c.title_id
                                                              join d in db.People on b.people_id equals d.people_id
                                                              join e in db.Offices on b.office_id equals e.office_id
                                                              where b.request_id == request_id
                                                              select new ConferenceParticipantExtend
                                                              {
                                                                  ID = b.current_mssv_msnv,
                                                                  FullName = d.name,
                                                                  OfficeName = e.office_name,
                                                                  TitleName = c.name
                                                              }).ToList();
            for (int i = 0; i < Participants.Count; i++)
            {
                Participants[i].RowNumber = 1 + i;
            }
            var Costs = db.Costs.Where(x => x.request_id == request_id).ToList();
            var ApprovalProcesses = (from a in db.ApprovalProcesses
                                     join b in db.Accounts on a.account_id equals b.account_id
                                     join c in db.PositionLanguages on a.position_id equals c.position_id
                                     where a.request_id == request_id && c.language_id == language_id
                                     select new ConferenceApprovalProcess
                                     {
                                         CreatedDate = a.created_date,
                                         PositionName = c.name,
                                         FullName = b.full_name,
                                         Comment = a.comment
                                     }).ToList();
            return JsonConvert.SerializeObject(new { Conference, Participants, Costs, ApprovalProcesses });
        }
    }
}

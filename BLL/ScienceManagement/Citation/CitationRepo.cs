﻿using BLL.ModelDAL;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.ScienceManagement;
using ENTITIES.CustomModels.ScienceManagement.Citation;
using ENTITIES.CustomModels.ScienceManagement.Paper;
using ENTITIES.CustomModels.ScienceManagement.ScientificProduct;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace BLL.ScienceManagement.Citation
{
    public class CitationRepo
    {
        readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public List<ListOnePerson_Citation> GetList(int account_id)
        {
            if (account_id <= 0 || account_id == int.MaxValue)
                return null;

            string sql = @"select STRING_AGG(c.source, ',') AS 'source',SUM(c.count) as 'count', br.created_date, rc.status_id, rc.request_id
                           from [SM_Citation].Citation c join [SM_Citation].RequestHasCitation rhc on c.citation_id = rhc.citation_id
	                            join [SM_Citation].RequestCitation rc on rhc.request_id = rc.request_id
	                            join [SM_Request].BaseRequest br on br.request_id = rc.request_id
                           where br.account_id = @id
                           group by br.created_date, rc.status_id,  rc.request_id";
            List<ListOnePerson_Citation> list = db.Database.SqlQuery<ListOnePerson_Citation>(sql, new SqlParameter("id", account_id)).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].note = list[i].status_id + "_" + list[i].request_id;
            }
            return list;
        }

        public int GetStatus(string id)
        {
            if (!int.TryParse(id, out int request_id))
                return 0;
            if (request_id <= 0 || request_id == int.MaxValue)
                return 0;
            try
            {
                RequestCitation rc = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
                return rc.status_id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        public AuthorInfo GetAuthor(string id)
        {
            if (!int.TryParse(id, out int request_id))
                return null;
            if (request_id <= 0 || request_id == int.MaxValue)
                return null;
            string sql = @"select ah.name, ah.email, o.office_abbreviation, ah.contract_id, ah.title_id, rc.total_reward, ah.bank_branch, ah.bank_number, ah.mssv_msnv, ah.tax_code, ah.identification_number, ct.name as 'contract_name', case when ah.is_reseacher is null then cast(0 as bit) else ah.is_reseacher end as 'is_reseacher', ah.identification_file_link, ah.people_id
                            from [SM_Citation].Citation c join [SM_Citation].RequestHasCitation rhc on c.citation_id = rhc.citation_id
	                            join [SM_Citation].RequestCitation rc on rhc.request_id = rc.request_id
	                            join [SM_ScientificProduct].Author ah on rc.people_id = ah.people_id
	                            join [General].Office o on o.office_id = ah.office_id
	                            join [SM_MasterData].ContractType ct on ah.contract_id = ct.contract_id
                            where rc.request_id = @id";
            AuthorInfo item = db.Database.SqlQuery<AuthorInfo>(sql, new SqlParameter("id", request_id)).FirstOrDefault();
            return item;
        }

        public List<string> GetAuthorEmail()
        {
            string sql = @"select distinct ah.email
                            from SM_Citation.RequestCitation rc join SM_ScientificProduct.Author ah on rc.people_id = ah.people_id
                            where rc.status_id in (4, 6, 7)";
            List<string> list = db.Database.SqlQuery<string>(sql).ToList();
            return list;
        }

        public RequestCitation GetRequestCitation(string id)
        {
            if (!int.TryParse(id, out int request_id) || request_id <= 0 || request_id == int.MaxValue)
                return null;
            RequestCitation rc = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
            return rc;
        }

        public string DeleteRequest(int request_id)
        {
            if (request_id <= 0 || request_id == int.MaxValue)
                return "ff";

            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    RequestCitation rp = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
                    rp.status_id = 1;
                    db.SaveChanges();
                    dbc.Commit();
                    return "ss";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return "ff";
                }
            }
        }

        public string ChangeStatus(string id)
        {
            if (!int.TryParse(id, out int request_id))
                return "ff";
            if (request_id <= 0 || request_id == int.MaxValue)
                return "ff";
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    RequestCitation rc = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
                    rc.status_id = 5;

                    Account account = rc.BaseRequest.Account;
                    NotificationRepo nr = new NotificationRepo(db);
                    int notification_id = nr.AddByAccountID(account.account_id, 4, "/Citation/Edit?id=" + request_id, false);

                    db.SaveChanges();
                    dbc.Commit();
                    return notification_id.ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return "ff";
                }
            }
        }

        public List<ENTITIES.Citation> GetCitation(string id)
        {
            string sql = @"select c.*
                           from [SM_Citation].Citation c join [SM_Citation].RequestHasCitation rhc on c.citation_id = rhc.citation_id
	                            join [SM_Citation].RequestCitation rc on rhc.request_id = rc.request_id
                           where rc.request_id = @id";
            List<ENTITIES.Citation> list = db.Database.SqlQuery<ENTITIES.Citation>(sql, new SqlParameter("id", id)).ToList();
            return list;
        }

        public Author EditAuthor(List<AddAuthor> people)
        {

            using (DbContextTransaction dbc = db.Database.BeginTransaction())
            {
                try
                {
                    AddAuthor temp = people[0];
                    Author author = db.Authors.Where(x => x.people_id == temp.people_id).FirstOrDefault();
                    author.name = temp.name;
                    author.email = temp.email;
                    if (temp.office_id == 0 || temp.office_id == null)
                    {
                        author.office_id = null;
                    }
                    else
                    {
                        author.office_id = temp.office_id;
                        author.bank_number = temp.bank_number;
                        author.bank_branch = temp.bank_branch;
                        author.tax_code = temp.tax_code;
                        author.identification_number = temp.identification_number;
                        author.mssv_msnv = temp.mssv_msnv;
                        author.is_reseacher = temp.is_reseacher;
                        author.title_id = temp.title_id;
                        author.contract_id = 1;
                        author.identification_file_link = temp.identification_file_link;
                    }
                    db.Entry(author).State = EntityState.Modified;
                    db.SaveChanges();
                    dbc.Commit();
                    return author;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return null;
                }
            }
        }

        public Author AddAuthor(List<AddAuthor> list)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    Author author = new Author();
                    foreach (var item in list)
                    {
                        author.name = item.name;
                        author.email = item.email;

                        if (item.office_id != 0)
                        {
                            author.office_id = item.office_id;
                            author.bank_number = item.bank_number;
                            author.bank_branch = item.bank_branch;
                            author.tax_code = item.tax_code;
                            author.identification_number = item.identification_number;
                            author.mssv_msnv = item.mssv_msnv;
                            author.is_reseacher = item.is_reseacher;
                            author.title_id = item.title_id;
                            author.contract_id = item.contract_id;
                            author.identification_file_link = item.identification_file_link;
                        }
                        db.Authors.Add(author);
                    }
                    db.SaveChanges();
                    dbc.Commit();
                    return author;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return null;
                }
        }

        public string AddCitationRequest(BaseRequest br, Author author)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    RequestCitation rc = new RequestCitation
                    {
                        request_id = br.request_id,
                        status_id = 3,
                        people_id = author.people_id
                        //current_mssv_msnv = author.mssv_msnv
                    };
                    db.RequestCitations.Add(rc);
                    db.SaveChanges();
                    dbc.Commit();
                    return "ss";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return "ff";
                }
        }

        public string AddCitaion(List<ENTITIES.Citation> citation)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    foreach (var item in citation)
                    {
                        db.Citations.Add(item);
                    }
                    db.SaveChanges();
                    dbc.Commit();
                    return "ss";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return "ff";
                }
        }

        public string AddRequestHasCitation(List<ENTITIES.Citation> citation, BaseRequest br)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    string sql = "";
                    List<SqlParameter> listParam = new List<SqlParameter>();
                    for (int i = 0; i < citation.Count; i++)
                    {
                        sql += "insert into [SM_Citation].RequestHasCitation values (@request, @citation" + i + ") \n";
                        SqlParameter param = new SqlParameter("@citation" + i, citation[i].citation_id);
                        listParam.Add(param);
                    }
                    SqlParameter param2 = new SqlParameter("@request", br.request_id);
                    listParam.Add(param2);
                    db.Database.ExecuteSqlCommand(sql, listParam.ToArray());
                    dbc.Commit();
                    return "ss";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return "ff";
                }
        }

        public string EditCitation(List<ENTITIES.Citation> citation, List<ENTITIES.Citation> newcitation, string request_id, Author author)
        {
            try
            {
                string sql = "";
                List<SqlParameter> listParam = new List<SqlParameter>();
                int id = int.Parse(request_id);
                BaseRequest br = db.BaseRequests.Where(x => x.request_id == id).FirstOrDefault();
                for (int i = 0; i < citation.Count; i++)
                {
                    sql += "delete from [SM_Citation].RequestHasCitation where citation_id = @citation" + i + " and request_id = @request \n";
                    SqlParameter param = new SqlParameter("@citation" + i, citation[i].citation_id);
                    listParam.Add(param);
                }
                SqlParameter param2 = new SqlParameter("@request", br.request_id);
                listParam.Add(param2);
                db.Database.ExecuteSqlCommand(sql, listParam.ToArray());

                foreach (var item in citation)
                {
                    db.Citations.Remove(db.Citations.Single(s => s.citation_id == item.citation_id));
                }
                db.SaveChanges();

                AddCitaion(newcitation);
                AddRequestHasCitation(newcitation, br);
                RequestCitation rc = db.RequestCitations.Where(x => x.request_id == br.request_id).FirstOrDefault();
                rc.people_id = author.people_id;
                rc.status_id = 3;

                db.SaveChanges();
                return "ss";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "ff";
            }
        }

        public List<PendingCitation_manager> GetListPending()
        {
            string sql = @"select acc.email, br.created_date, br.request_id, rc.status_id
                           from [SM_Citation].RequestCitation rc join [SM_Request].BaseRequest br on rc.request_id = br.request_id
	                            join [General].Account acc on br.account_id = acc.account_id
                           where rc.status_id = 3 or  rc.status_id = 5 or rc.status_id = 8";
            List<PendingCitation_manager> list = db.Database.SqlQuery<PendingCitation_manager>(sql).ToList();
            return list;
        }

        public int? GetTotalReward(string id)
        {
            int request_id = int.Parse(id);
            RequestCitation item = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
            return item.total_reward;
        }

        public string UpdateReward(string id, string total)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    int request_id = int.Parse(id);
                    string temp = total.Replace(",", "");
                    int reward = int.Parse(temp);
                    RequestCitation rc = db.RequestCitations.Where(x => x.request_id == request_id).FirstOrDefault();
                    rc.total_reward = reward;
                    rc.status_id = 4;
                    db.SaveChanges();
                    dbc.Commit();

                    return "ss";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    return "ff";
                }
        }

        public List<WaitDecisionCitation> GetListWait()
        {
            string sql = @"select po.name, o.office_abbreviation, pro.mssv_msnv, rc.total_reward, SUM(c.COUNT) as 'sum', rc.request_id
                            from [SM_Citation].RequestCitation rc join [SM_Request].BaseRequest br on rc.request_id = br.request_id
	                            join [General].Account acc on acc.account_id = br.account_id
	                            join [General].People po on acc.email = po.email
	                            join [General].Profile pro on po.people_id = pro.people_id
	                            join [General].Office o on po.office_id = o.office_id
	                            join [SM_Citation].RequestHasCitation rhc on rc.request_id = rhc.request_id
	                            join [SM_Citation].Citation c on rhc.citation_id = c.citation_id
                            where rc.status_id = 4
                            group by po.name, o.office_abbreviation, pro.mssv_msnv, rc.total_reward, rc.request_id";
            List<WaitDecisionCitation> list = db.Database.SqlQuery<WaitDecisionCitation>(sql).ToList();
            return list;
        }

        public string UploadDecision(DateTime date, int file_id, string number, string file_drive_id)
        {
            using (DbContextTransaction dbc = db.Database.BeginTransaction())
                try
                {
                    Decision decision = new Decision
                    {
                        valid_date = date,
                        file_id = file_id,
                        decision_number = number
                    };
                    db.Decisions.Add(decision);
                    db.SaveChanges();

                    List<WaitDecisionCitation> wait = GetListWait();
                    foreach (var item in wait)
                    {
                        RequestDecision request = new RequestDecision
                        {
                            request_id = item.request_id,
                            decision_id = decision.decision_id
                        };
                        db.RequestDecisions.Add(request);
                        RequestCitation rc = db.RequestCitations.Where(x => x.request_id == item.request_id).FirstOrDefault();
                        rc.status_id = 2;
                    }

                    foreach (var item in wait)
                    {
                        BaseRequest br = db.BaseRequests.Where(x => x.request_id == item.request_id).FirstOrDefault();
                        br.finished_date = DateTime.Now;
                        db.Entry(br).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                    dbc.Commit();
                    return "ss";
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    dbc.Rollback();
                    GoogleDriveService.DeleteFile(file_drive_id);
                    return "ff";
                }
        }

        public List<Citation_Appendix_1> GetListAppendix1()
        {
            string sql = @"select ah.name, ah.mssv_msnv, o.office_abbreviation, a.sum_scopus, b.sum_scholar
                            from SM_ScientificProduct.Author ah
	                            join(select ah.people_id, sum(c.count) as 'sum_scopus'
	                            from SM_Citation.Citation c join SM_Citation.RequestHasCitation rhc on c.citation_id = rhc.citation_id
		                            join SM_Citation.RequestCitation rc on rhc.request_id = rc.request_id
		                            join SM_ScientificProduct.Author ah on rc.people_id = ah.people_id
	                            where rc.status_id = 4 and c.source = 'Scopus'
	                            group by ah.people_id) as a on ah.people_id = a.people_id
	                            join(select ah.people_id, sum(c.count) as 'sum_scholar'
	                            from SM_Citation.Citation c join SM_Citation.RequestHasCitation rhc on c.citation_id = rhc.citation_id
		                            join SM_Citation.RequestCitation rc on rhc.request_id = rc.request_id
		                            join SM_ScientificProduct.Author ah on rc.people_id = ah.people_id
	                            where (rc.status_id = 4) and (c.source = 'Google Scholar' or c.source = 'Scholar')
	                            group by ah.people_id) as b on ah.people_id = b.people_id
	                            join General.Office o on ah.office_id = o.office_id
                            order by ah.name";
            List<Citation_Appendix_1> list = db.Database.SqlQuery<Citation_Appendix_1>(sql).ToList();
            return list;
        }

        public List<Citation_Appendix_2> GetListAppendix2()
        {
            string sql = @"select ah.name, ah.mssv_msnv, o.office_abbreviation, rc.total_reward
                            from SM_Citation.RequestCitation rc join SM_ScientificProduct.Author ah on rc.people_id = ah.people_id
	                            join General.Office o on o.office_id = ah.office_id
                            where rc.status_id = 4";
            List<Citation_Appendix_2> list = db.Database.SqlQuery<Citation_Appendix_2>(sql).ToList();
            return list;
        }
    }
}

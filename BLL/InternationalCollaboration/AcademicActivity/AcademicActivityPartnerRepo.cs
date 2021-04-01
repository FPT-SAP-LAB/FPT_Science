﻿using BLL.InternationalCollaboration.AcademicCollaborationRepository;
using ENTITIES;
using ENTITIES.CustomModels;
using ENTITIES.CustomModels.InternationalCollaboration.AcademicActivity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BLL.InternationalCollaboration.AcademicActivity
{
    public class AcademicActivityPartnerRepo
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public AlertModal<string> saveActivityPartner(HttpPostedFileBase evidence_file, string folder_name, SaveActivityPartner activityPartner)
        {
            using (DbContextTransaction dbContext = db.Database.BeginTransaction())
            {
                try
                {
                    if (checkDuplicatePartnerScope(activityPartner))
                    {
                        AcademicCollaborationRepo academicCollaborationRepo = new AcademicCollaborationRepo();
                        //upload file if exist
                        //upload file
                        Google.Apis.Drive.v3.Data.File f = new Google.Apis.Drive.v3.Data.File();
                        if (evidence_file != null)
                        {
                            f = academicCollaborationRepo.uploadEvidenceFile(evidence_file, "Collab partner - " + folder_name, 5, false);
                        }
                        File file = new File();
                        //save file if null, else just save activityPartner
                        if (f != null)
                        {
                            file = academicCollaborationRepo.saveFile(f, evidence_file);
                        }
                        //update to PartnerScope
                        PartnerScope partnerScope = updatePartnerScope(activityPartner.partner_id, activityPartner.scope_id, academicCollaborationRepo);
                        saveActivityPartner(file, partnerScope, activityPartner);
                        dbContext.Commit();
                        return new AlertModal<string>(null, true, "Thành công", "Thêm đối tác đồng tổ chức thành công.");
                    }
                    else
                    {
                        return new AlertModal<string>(null, false, "Lỗi", "Đối tác đã được thêm vào danh sách trước đó.");
                    }
                }
                catch (Exception e)
                {
                    dbContext.Rollback();
                    return new AlertModal<string>(null, false, "Lỗi", "Có lỗi xảy ra.");
                }
            }
        }
        public PartnerScope updatePartnerScope(int partner_id, int scope_id, AcademicCollaborationRepo academicCollaborationRepo)
        {
            PartnerScope partnerScope;
            try
            {
                partnerScope = db.PartnerScopes.Where(x => x.partner_id == partner_id && x.scope_id == scope_id).FirstOrDefault();
                if (partnerScope != null)
                {
                    academicCollaborationRepo.increaseReferenceCountOfPartnerScope(partnerScope);
                }
                else
                {
                    partnerScope = academicCollaborationRepo.savePartnerScope(partner_id, scope_id);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return partnerScope;
        }
        public void saveActivityPartner(File file, PartnerScope partnerScope, SaveActivityPartner activityPartner)
        {
            try
            {
                ActivityPartner ap = new ActivityPartner();
                ap.sponsor = activityPartner.sponsor;
                if (activityPartner.contact_point_name != null) ap.contact_point_name = activityPartner.contact_point_name;
                if (activityPartner.contact_point_email != null) ap.contact_point_email = activityPartner.contact_point_email;
                if (activityPartner.contact_point_phone != null) ap.contact_point_phone = activityPartner.contact_point_phone;
                if (activityPartner.cooperation_date_start != null) ap.cooperation_date_start = activityPartner.cooperation_date_start;
                if (activityPartner.cooperation_date_end != null) ap.cooperation_date_end = activityPartner.cooperation_date_end;
                ap.activity_id = activityPartner.activity_id;
                ap.partner_scope_id = partnerScope.partner_scope_id;
                if (file.file_id != 0) ap.file_id = file.file_id;
                db.ActivityPartners.Add(ap);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public AlertModal<ActivityPartner_Ext> getActivityPartner(int activity_partner_id)
        {
            try
            {
                var sql = @"select 
                            ap.activity_partner_id, p.partner_id , p.partner_name, 
                            ap.sponsor, 
                            cs.scope_id, cs.scope_abbreviation, cs.scope_name,
                            ap.cooperation_date_start, 
                            ap.cooperation_date_end,
                            f.file_id, f.name 'file_name',
                            ap.contact_point_name, ap.contact_point_email, ap.contact_point_phone
                            from 
                            SMIA_AcademicActivity.ActivityPartner ap 
                            left join IA_Collaboration.PartnerScope mps on ap.partner_scope_id = mps.partner_scope_id 
                            left join IA_Collaboration.[Partner] p on p.partner_id = mps.partner_id
                            left join IA_Collaboration.CollaborationScope cs on cs.scope_id = mps.scope_id
                            left join General.[File] f on f.file_id = ap.file_id
                            where ap.activity_partner_id = @activity_partner_id";
                ActivityPartner_Ext activityPartner = db.Database.SqlQuery<ActivityPartner_Ext>(sql,
                    new SqlParameter("activity_partner_id", activity_partner_id)).FirstOrDefault();
                if (activityPartner != null)
                {
                    return new AlertModal<ActivityPartner_Ext>(activityPartner, true, null, null);
                }
                else
                {
                    return new AlertModal<ActivityPartner_Ext>(null, false, "Lỗi", "Có lỗi xảy ra khi lấy thông tin đơn vị đồng tổ chức.");
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public AlertModal<string> updateActivityPartner(HttpPostedFileBase evidence_file, string folder_name, SaveActivityPartner saveActivityPartner)
        {
            using (DbContextTransaction dbContext = db.Database.BeginTransaction())
            {
                try
                {
                    if (checkDuplicatePartnerScope(saveActivityPartner))
                    {
                        //update file
                        ActivityPartner activityPartner = db.ActivityPartners.Find(saveActivityPartner.activity_partner_id);
                        AcademicCollaborationRepo academicCollaborationRepo = new AcademicCollaborationRepo();
                        Google.Apis.Drive.v3.Data.File f;
                        File old_file = db.Files.Find(activityPartner.file_id);
                        File new_file = new File();
                        if (evidence_file != null)
                        {
                            if (old_file != null)
                            {
                                //update file on Google Drive
                                f = GoogleDriveService.UpdateFile(evidence_file.FileName, evidence_file.InputStream, evidence_file.ContentType, old_file.file_drive_id);
                                new_file = academicCollaborationRepo.saveFile(f, evidence_file);
                            }
                            else
                            {
                                //upload to Goolge Drive
                                f = academicCollaborationRepo.uploadEvidenceFile(evidence_file, "Collab partner - " + folder_name, 5, false);
                                new_file = academicCollaborationRepo.saveFile(f, evidence_file);
                            }
                        }
                        else
                        {
                            if (old_file != null)
                            {
                                //delete corressponding in gg drive
                                GoogleDriveService.DeleteFile(old_file.file_drive_id);
                                //delete corressponding from File
                                new_file = removeFile(old_file);
                            }
                        }
                        //update file_id null in coress ActivityPartner
                        updateActivityPartner(activityPartner, saveActivityPartner, new_file);
                        dbContext.Commit();
                        return new AlertModal<string>(null, true, "Thành công", "Chỉnh sửa thông tin đơn vị đồng tổ chức thành công.");
                    }
                    else
                    {
                        return new AlertModal<string>(null, false, "Lỗi", "Đối tác đã được thêm vào danh sách trước đó.");
                    }
                }
                catch (Exception e)
                {
                    dbContext.Rollback();
                    throw e;
                }
            }
        }
        public File removeFile(File file)
        {
            try
            {
                db.Files.Remove(file);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return null;
        }
        public void updateActivityPartner(ActivityPartner activityPartner, SaveActivityPartner saveActivityPartner, File file)
        {
            try
            {
                AcademicCollaborationRepo academicCollaborationRepo = new AcademicCollaborationRepo();
                //update PartnerScope
                PartnerScope partnerScope = updatePartnerScope(saveActivityPartner.partner_id, saveActivityPartner.scope_id, academicCollaborationRepo);
                ActivityPartner ap = new ActivityPartner();
                ap.sponsor = saveActivityPartner.sponsor;
                if (saveActivityPartner.contact_point_name != null) ap.contact_point_name = saveActivityPartner.contact_point_name;
                if (saveActivityPartner.contact_point_email != null) ap.contact_point_email = saveActivityPartner.contact_point_email;
                if (saveActivityPartner.contact_point_phone != null) ap.contact_point_phone = saveActivityPartner.contact_point_phone;
                if (saveActivityPartner.cooperation_date_start != null) ap.cooperation_date_start = saveActivityPartner.cooperation_date_start;
                if (saveActivityPartner.cooperation_date_end != null) ap.cooperation_date_end = saveActivityPartner.cooperation_date_end;
                ap.partner_scope_id = partnerScope.partner_scope_id;
                if (file != null) ap.file_id = file.file_id;
                activityPartner.sponsor = ap.sponsor;
                activityPartner.contact_point_name = ap.contact_point_name;
                activityPartner.contact_point_email = ap.contact_point_email;
                activityPartner.contact_point_phone = ap.contact_point_phone;
                activityPartner.cooperation_date_start = ap.cooperation_date_start;
                activityPartner.cooperation_date_end = ap.cooperation_date_end;
                activityPartner.partner_scope_id = ap.partner_scope_id;
                activityPartner.file_id = ap.file_id;
                db.SaveChanges();
                //old partner_scope_id vs new partner_scope_id
                if (activityPartner.partner_scope_id != partnerScope.partner_scope_id)
                {
                    //decrease re_cou of old partner_scope
                    PartnerScope old_partner_scope = db.PartnerScopes.Find(activityPartner.partner_scope_id);
                    academicCollaborationRepo.decreaseReferenceCountOfPartnerScope(old_partner_scope);
                    if (old_partner_scope.reference_count <= 0)
                    {
                        db.PartnerScopes.Remove(old_partner_scope);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public AlertModal<string> deleteActivityPartner(int activity_partner_id)
        {
            using (DbContextTransaction dbContext = db.Database.BeginTransaction())
            {
                try
                {
                    AcademicCollaborationRepo academicCollaborationRepo = new AcademicCollaborationRepo();
                    ActivityPartner activityPartner = db.ActivityPartners.Find(activity_partner_id);
                    //delete corress file in db and gg drive
                    if (activityPartner.file_id != null)
                    {
                        File file = db.Files.Find(activityPartner.file_id);
                        //delete on drive
                        GoogleDriveService.DeleteFile(file.file_drive_id);
                        db.Files.Remove(file);
                        db.SaveChanges();
                    }
                    //decrease ref_cou
                    PartnerScope partnerScope = db.PartnerScopes.Find(activityPartner.partner_scope_id);
                    academicCollaborationRepo.decreaseReferenceCountOfPartnerScope(partnerScope);
                    //delete activi_partner
                    db.ActivityPartners.Remove(activityPartner);
                    db.SaveChanges();
                    //delete partner_scope if ref_cou < =0
                    if (partnerScope.reference_count <= 0)
                    {
                        db.PartnerScopes.Remove(partnerScope);
                        db.SaveChanges();
                    }
                    dbContext.Commit();
                    return new AlertModal<string>(null, true, "Thành công", "Xóa đối tác đồng tổ chức thành công.");
                }
                catch (Exception e)
                {
                    dbContext.Rollback();
                    throw e;
                }
            }
        }
        public bool checkDuplicatePartnerScope(SaveActivityPartner activityPartner)
        {
            try
            {
                var partnerScope = db.PartnerScopes.Where<PartnerScope>(x => x.partner_id == activityPartner.partner_id && x.scope_id == activityPartner.scope_id).FirstOrDefault();
                if (partnerScope == null)
                {
                    return true;
                }
                else
                {
                    if (activityPartner.activity_partner_id == 0)
                    {
                        ActivityPartner ap = db.ActivityPartners.Where<ActivityPartner>(x => x.partner_scope_id == partnerScope.partner_scope_id && x.activity_id == activityPartner.activity_id).FirstOrDefault();
                        if (ap == null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        ActivityPartner ap = db.ActivityPartners.Where<ActivityPartner>(x => x.activity_partner_id == activityPartner.activity_partner_id).FirstOrDefault();
                        if (ap.partner_scope_id != partnerScope.partner_scope_id)
                        {
                            ActivityPartner ap_new = db.ActivityPartners.Where<ActivityPartner>(x => x.partner_scope_id == partnerScope.partner_scope_id && x.activity_id == activityPartner.activity_id).FirstOrDefault();
                            if (ap_new == null)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}

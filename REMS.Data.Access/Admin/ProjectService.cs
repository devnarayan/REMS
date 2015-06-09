using AutoMapper;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Admin
{
    public interface IProjectService
    {
        List<ProjectModel> AllProjects();
        Project GetProject(int projectID);
        Project GetProjectByFlatID(int flatid);
        int AddProject(ProjectModel model);
        int EditProject(ProjectModel model);
        List<ProjectTypeModel> AllProjectTypes();
        List<ProjectTypeModel> GetProjectTypeList(int projectid);
        ProjectType GetProjectType(int projectTypeID);
        
        int AddProjectType(ProjectTypeModel model);
        int EditProjectType(ProjectTypeModel model);
    }
    public class ProjectService:IProjectService
    {
        public List<ProjectModel> AllProjects()
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Projects.ToList();
            Mapper.CreateMap<Project, ProjectModel>().ForMember(dest=>dest.PossessionDateSt,opts=>opts.MapFrom(src=>src.PossessionDate.Value.ToString("dd/MM/yyyy")));
            var mdl = Mapper.Map<List<Project>, List<ProjectModel>>(model);
            return mdl;
        }
        public Project GetProject(int projectID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Projects.Where(pr=>pr.ProjectID==projectID).FirstOrDefault();
            return model;
        }
        public Project GetProjectByFlatID(int flatid)
        {
            REMSDBEntities context = new REMSDBEntities();          
            IList<Project> model=  context.spGetProjectByFlatid1(flatid).ToList<Project>();
            // var model = context.Projects.Where(pr => pr.Towers.First().Floors.First().Flats.First().FlatID == flatid).FirstOrDefault();
            return model.First();
        }
        public int AddProject(ProjectModel model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    if(model.PossessionDateSt!=null)model.PossessionDate=Convert.ToDateTime(model.PossessionDateSt,FormData.IndianDateFormat());
                    Mapper.CreateMap<ProjectModel, Project>();
                    var mdl = Mapper.Map<ProjectModel, Project>(model);
                    mdl.RecordStatus = 0;
                    mdl.CrDate = DateTime.Now;
                    mdl.ReceiptNo = "0";
                    context.Projects.Add(mdl);
                    int i = context.SaveChanges();
                    return i;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }
        public int EditProject(ProjectModel model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    Mapper.CreateMap<ProjectModel, Project>();
                    var mdl = Mapper.Map<ProjectModel, Project>(model);
                    context.Projects.Add(mdl);
                    context.Entry(mdl).State = EntityState.Modified;
                    int i = context.SaveChanges();
                    return i;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }
        public List<ProjectTypeModel> AllProjectTypes(int projectID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.ProjectTypes.Where(pr=>pr.ProjectID==projectID).ToList();
            Mapper.CreateMap<ProjectType, ProjectTypeModel>();
            var mdl = Mapper.Map<List<ProjectType>, List<ProjectTypeModel>>(model);
            return mdl;
        }
        public List<ProjectTypeModel> AllProjectTypes()
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.ProjectTypes.ToList();
            Mapper.CreateMap<ProjectType, ProjectTypeModel>();
            var mdl = Mapper.Map<List<ProjectType>, List<ProjectTypeModel>>(model);
            return mdl;
        }
        public ProjectType GetProjectType(int projectTypeID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.ProjectTypes.Where(pr => pr.ProjectTypeID == projectTypeID).FirstOrDefault();
            return model;
        }
        public List<ProjectTypeModel> GetProjectTypeList(int projectID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.ProjectTypes.Where(pr => pr.ProjectID == projectID).ToList();
            Mapper.CreateMap<ProjectType, ProjectTypeModel>();
            var mdl = Mapper.Map<List<ProjectType>, List<ProjectTypeModel>>(model);
            return mdl;
        }
        public int AddProjectType(ProjectTypeModel model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    Mapper.CreateMap<ProjectTypeModel, ProjectType>();
                    var mdl = Mapper.Map<ProjectTypeModel, ProjectType>(model);
                    context.ProjectTypes.Add(mdl);
                    int i = context.SaveChanges();
                    return i;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }
        public int EditProjectType(ProjectTypeModel model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    Mapper.CreateMap<ProjectModel, Project>();
                    var mdl = Mapper.Map<ProjectTypeModel, ProjectType>(model);
                    context.ProjectTypes.Add(mdl);
                    context.Entry(mdl).State = EntityState.Modified;
                    int i = context.SaveChanges();
                    return i;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }
    }
}

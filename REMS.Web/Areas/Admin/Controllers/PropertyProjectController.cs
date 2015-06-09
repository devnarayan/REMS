using REMS.Data;
using REMS.Data.Access.Admin;
using REMS.Data.CustomModel;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.Admin.Controllers
{
    public class PropertyProjectController : Controller
    {
        ProjectService pservice = new ProjectService();

        // GET: Admin/PropertyProject
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddProject()
        {
            return View();
        }
        public ActionResult AddPropertyType()
        {
            return View();
        }


        #region AngularService
        public string GetAllProjects()
        {
            List<ProjectModel> model = pservice.AllProjects();
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetProject(int projectID)
        {
            Project model = pservice.GetProject(projectID);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string SaveProject(ProjectModel project)
        {
            project.CrBy = User.Identity.Name;
            int i = pservice.AddProject(project);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string EditProject(ProjectModel pmodel)
        {
            int i = pservice.EditProject(pmodel);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string GetAllProjectTypes()
        {
            List<ProjectTypeModel> model = pservice.AllProjectTypes();
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetProjectType(int projectID)
        {
            if (projectID == 0)
            {
                List<ProjectTypeModel> model = pservice.AllProjectTypes();
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            else
            {
                List<ProjectTypeModel> model = pservice.GetProjectTypeList(projectID);
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
        }
        public string SaveProjectType(ProjectTypeModel project)
        {
            project.CrBy = User.Identity.Name;
            project.CrDate = DateTime.Now;
            int i = pservice.AddProjectType(project);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }
        public string EditProjectType(ProjectTypeModel pmodel)
        {
            int i = pservice.EditProjectType(pmodel);
            return Newtonsoft.Json.JsonConvert.SerializeObject(i);
        }

        public string TowerView(int towerID)
        {
            TowerService tservice = new TowerService();
            List<TowerViewModel> model = tservice.TowerViewList(towerID);

            string towerhtml = "";
            foreach (var md in model)
            {
                string html = TowerHtml();
                html=  html.Replace("<% FloorNo %>", md.FloorNo.Value.ToString());
                string flats = "";
                foreach (var ft in md.FlatList)
                {
                    string fml = flathtml();
                    string st = ""; string home = "";
                    string saleStatus = "";
                    string saleStatus2 = "";
                    if (ft.Status == "Available")
                    {
                        st = "green"; home = "homeavailabe.png";
                        saleStatus = "hide";
                    }
                    else if (ft.Status == "Booked")
                    {
                        st = "blue"; home = "homebooked.png";
                        saleStatus = "";
                        saleStatus2 = "hide";
                    }
                    else //if (ft.Status == "Sale")
                    {
                        st = "red"; home = "homesale.png";
                        saleStatus = "";
                        saleStatus2 = "hide";
                    }
                    flats += fml.Replace("<% FlatNo %>", ft.FlatNo).Replace("<% FlatType %>", ft.FlatType).Replace("<% FlatSize %>", ft.FlatSize.Value.ToString() + " " + ft.FlatSizeUnit).Replace("<% FlatID %>", ft.FlatID.ToString()).Replace("<% Status %>", st).Replace("<% homepng %>", home).Replace("<% SaleStatus %>", saleStatus).Replace("<% SaleStatus2 %>", saleStatus2);
                }
                html = html.Replace("<% Flats %>", flats);
                towerhtml += html;
            }
            return towerhtml;
        }
        public string TowerHtml()
        {
            string st = @"<li>
    <div class='smart-timeline-icon'>
       <% FloorNo %><sup>th</sup>
    </div>
    <div class='smart-timeline-time'>
        <small>Floor </small>
    </div>
    <div class='smart-timeline-content'>
        <div class='row'>
           <% Flats %>
        </div>
    </div>
</li>";
            return st;
        }
        public string flathtml()
        {
            string st = @" <div class='col-sm-6 col-md-3 col-lg-3'>
                <div class='jarviswidget jarviswidget-color-<% Status %>'>
                    <header>
                        <div class=''>
                            <a href='#' class='dropdown-toggle' data-toggle='dropdown' tabindex='-1'>
                                <h2 style='display: table !important;'><img src='/Content/img/<% homepng %>' /> <% FlatNo %> - <% FlatType %></h2>
                            </a>
                            <ul class='dropdown-menu pull-right no-padding' role='menu'>
                                <li class='no-padding <% SaleStatus2 %>'><a href='/Sale/Property/NewSale/<% FlatID %>'  target='_blank'>Sale</a></li>
                                <li class='no-padding '><a href='/Admin/CreateProperty/EditFlat/<% FlatID %>' target='_blank'>Edit Flat</a></li>
                                <li class='no-padding '><a target='_blank' href='/Sale/Property/CalculatePrice/<% FlatID %>'  target='_blank'>View Details</a></li>
                                <li class='divider'></li>
                                <li class='no-padding <% SaleStatus %>'><a target='_blank' href='/Customer/Transfer/RefundProperty/<% FlatID %>'  target='_blank'>Refund Property</a></li>
                                <li class='no-padding <% SaleStatus %>'><a target='_blank' href='/Customer/ManageCustomer/EditCustomerFlatID/<% FlatID %>'  target='_blank'>Edit Customer</a></li>
                                <li class='no-padding <% SaleStatus %>'><a target='_blank' href='/Sale/Payment/Index/<% FlatID %>'  target='_blank'>Submit Payment</a></li>
                            </ul>
                        </div>
                    </header>
                    <div>
                        <div class='widget-body hidden'>
                        </div>
                    </div>
                </div>
            </div>";
            return st;
        }

        #endregion
    }
}
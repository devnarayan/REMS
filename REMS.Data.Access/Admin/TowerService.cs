using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using REMS.Data;
using System.Data.Entity;
using REMS.Data.CustomModel;

namespace REMS.Data.Access.Admin
{
    public interface ITowerService
    {
        int AddTower(Tower tower);
        int EditTower(Tower tower);
        bool IsTower(string towername);
        Tower TowerList(int towerid);
        List<Tower> TowerList(string towername);
        List<Tower> AllTower();
        List<TowerProjectModel> AllTowerProject();
        List<TowerViewModel> TowerViewList(int towerID);
    }
    public class TowerService : ITowerService
    {
        public int AddTower(Tower tower)
        {
            using (var context = new REMSDBEntities())
            {
                context.Towers.Add(tower);
                int i = context.SaveChanges();
                return i;
            }
        }
        public int EditTower(Tower tower)
        {
            using (var context = new REMSDBEntities())
            {
                context.Towers.Add(tower);
                context.Entry(tower).State = EntityState.Modified;
                int i = context.SaveChanges();
                return i;
            }
        }
        public bool IsTower(string towername)
        {
            using (var context = new REMSDBEntities())
            {
                var tow= context.Towers.Where(tw=>tw.TowerName==towername).FirstOrDefault();
                if (tow == null) return false;
                else return true;
            }
        }
         
        public Tower TowerList(int towerid)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Towers.Where(tw => tw.TowerID == towerid).FirstOrDefault();
            return model;
        }

        public List<Tower> TowerList(string towername)
        {
            List<Tower> tlist = new List<Tower>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Towers.Where(tw => tw.TowerName==towername).ToList();
            foreach (var md in model)
            {
                tlist.Add(md);
            }
            return tlist;
        }
        public List<Tower> AllTower()
        {
           // List<Tower> tlist = new List<Tower>();
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Towers.ToList();
           
            return model;
        }
        public List<TowerProjectModel> AllTowerProject()
        {
            REMSDBEntities context=new REMSDBEntities();
            List<TowerProjectModel> model = new List<TowerProjectModel>();
            var mdl = (from tw in context.Towers join pr in context.Projects on tw.ProjectID equals pr.ProjectID select new { Prj = pr, Twr = tw }).ToList();
            foreach(var md in mdl){
                model.Add(new TowerProjectModel { ProjectID = md.Prj.ProjectID, Address = md.Prj.Address, Block = md.Twr.Block, CompanyName = md.Prj.CompanyName, Location = md.Prj.Location, PName = md.Prj.PName, PossessionDateSt = md.Prj.PossessionDate.Value.ToString("dd/MM/yyyy"), ReceiptPrefix = md.Prj.ReceiptPrefix, TowerID = md.Twr.TowerID, TowerName = md.Twr.TowerName, TowerNo = md.Twr.TowerNo });
            }
            return model;
        }
        public List<TowerViewModel> TowerViewList(int towerID)
        {
            REMSDBEntities context = new REMSDBEntities();
            List<TowerViewModel> model = new List<TowerViewModel>();
            var mdl = (from tw in context.Towers join fl in context.Floors on tw.TowerID equals fl.TowerID join pr in context.Projects on tw.ProjectID equals pr.ProjectID where tw.TowerID == towerID orderby fl.FloorID descending select new { Flr = fl, Pro = pr, Twr = tw }).ToList();
            //var mdl = (from fl in context.Floors join tw in context.Towers on fl.TowerID equals tw.TowerID join pr in context.Projects on tw.ProjectID equals pr.ProjectID orderby fl.FloorID descending select new { Flr = fl, Pro = pr, Twr = tw }).ToList();
            foreach (var md in mdl)
            {
                var flat = context.Flats.Where(ft => ft.FloorID == md.Flr.FloorID).ToList();
                model.Add(new TowerViewModel { FloorID = md.Flr.FloorID, FloorNo = md.Flr.FloorNo, FloorName = md.Flr.FloorName, TowerID = md.Twr.TowerID, ProjectID = md.Pro.ProjectID, ProjectTypeID = md.Twr.ProjectTypeID, TowerName = md.Twr.TowerName, TowerNo = md.Twr.TowerNo, Block = md.Twr.Block, PName = md.Pro.PName, CompanyName = md.Pro.CompanyName, Location = md.Pro.Location, Address = md.Pro.Address, ReceiptPrefix = md.Pro.ReceiptPrefix, PossessionDateSt = md.Pro.PossessionDate.Value.ToString("dd/MM/yyyy"), FlatList = flat });
            }
            return model;
        }

    }
}

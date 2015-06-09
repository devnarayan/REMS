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
    public interface IFloorService
    {
        int AddFloor(Floor floor);
        int EditFloor(Floor floor);
        bool IsFloorNo(int? floorno, int? towerid);
        Floor FloorList(int floorid);
        List<Floor> FloorList(string floorname);
        List<Floor> AllFloor();
        List<FloorModel> AllFloor(int towerid);
        Floor GetFloorByFloorNo(int towerid, int floorid);
        List<FloorModel> GetFloorWithTowreID(int towerid);
    }
    public class FloorService : IFloorService
    {
        public int AddFloor(Floor floor)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.Floors.Add(floor);
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
        public int EditFloor(Floor floor)
        {
            using (var context = new REMSDBEntities())
            {
                context.Floors.Add(floor);
                context.Entry(floor).State = EntityState.Modified;
                int i = context.SaveChanges();
                return i;
            }
        }
        public bool IsFloorNo(int? floorno,int? towerid)
        {
            using (var context = new REMSDBEntities())
            {
                var flid= context.Floors.Where(fl => fl.FloorNo == floorno && fl.TowerID == towerid).FirstOrDefault();
                if (flid == null) return false;
                else return true;
            }
        }
        public Floor FloorList(int floorid)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Floors.Where(tw => tw.FloorID == floorid).FirstOrDefault();
            return model;
        }
        public List<Floor> FloorList(string floorname)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Floors.Where(tw => tw.FloorName == floorname).ToList();
            return model;
        }
        public List<Floor> AllFloor()
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Floors.ToList();
            return model;
        }
        public List<FloorModel> AllFloor(int towerid)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                List<FloorModel> model = new List<FloorModel>();
                var tower = context.Towers.Where(tw => tw.TowerID == towerid).FirstOrDefault();
                var floors = context.Floors.Where(fl => fl.TowerID == towerid).ToList();
                foreach (var md in floors)
                {
                    Mapper.CreateMap<Floor, FloorModel>();
                    var floor = Mapper.Map<Floor, FloorModel>(md);
                    floor.TowerNo = tower.TowerNo;
                    floor.TowerName = tower.TowerName;
                    floor.Block = tower.Block;
                    floor.NoOfFlat = md.Flats.Count();
                    model.Add(floor);
                }
                return model;
            }
        }
        public int DeleteFloor(int floorid)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {

                    Floor fl = new Floor();
                    fl.FloorID = floorid;
                    context.Floors.Add(fl);
                    context.Entry(fl).State = EntityState.Deleted;
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
        public Floor GetFloorByFloorNo(int towerid, int floorid)
        {
            REMSDBEntities context = new REMSDBEntities();
           var md= context.Floors.Where(fl => fl.TowerID == towerid && fl.FloorID == floorid).FirstOrDefault();

           return md;
        }
        public List<FloorModel> GetFloorWithTowreID(int towerid)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                List<FloorModel> model = new List<FloorModel>();
                var floors = context.Floors.Where(fl => fl.TowerID == towerid).ToList();
                foreach (var md in floors)
                {
                    Mapper.CreateMap<Floor, FloorModel>();
                    var floor = Mapper.Map<Floor, FloorModel>(md);
                    floor.TowerNo =md.Tower.TowerNo;
                    floor.TowerName =md.Tower.TowerName;
                    floor.Block =md.Tower.Block;
                    floor.NoOfFlat = md.Flats.Count();
                    model.Add(floor);
                }
                return model;
            }
        }

    }
}

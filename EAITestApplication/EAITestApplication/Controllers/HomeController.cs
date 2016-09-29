using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using EAITestApplication.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EAITestApplication.Controllers
{
    public class HomeController : Controller
    {
        //EAITestEntities1 te = new EAITestEntities1();
        //User_Building ub = new User_Building();
        public ActionResult Index()
        {

            //    List<User> users_data = te.Users.ToList();
            //    List<Building> buildings_data = te.Buildings.ToList();
            //    List<UserBuilding> userbuildings_data = te.UserBuildings.ToList();
            //    ub.users = users_data;
            //    ub.buildings = buildings_data;
            //    ub.userbuildings = userbuildings_data;

            //  //  te.Users.
            try
            {
                string connstring = ConfigurationManager.ConnectionStrings["EAITest"].ToString();
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    DataSet ds = new DataSet();
                    string Userquery = "select * from [EAITest].[dbo].Users;";
                    string BuildingsQuery = "select * from [EAITest].[dbo].Building;";
                    string usercount = @"select u.ID, COUNT(u.ID) as Buildingcount  from [EAITest].[dbo].Users u, [EAITest].[dbo].Building B, 
                                    [EAITest].[dbo].UserBuilding x where u.ID = x.UserID and x.BuildingID = B.ID group by u.ID;";
                    string Buildingcount = @"select B.ID, COUNT(B.ID) as Usercount  from [EAITest].[dbo].Users u, [EAITest].[dbo].Building B, 
                                    [EAITest].[dbo].UserBuilding x where u.ID= x.UserID and x.BuildingID=B.ID group by B.ID;";
                    string fullexecution = Userquery + BuildingsQuery + usercount + Buildingcount;
                    conn.Open();
                    DataTable userdata = new DataTable();
                    DataTable buildingsdata = new DataTable();
                    DataTable usercountdata = new DataTable();
                    DataTable Buildingcountdata = new DataTable();
                    using (SqlDataAdapter ad = new SqlDataAdapter(Userquery, conn))
                    {
                        
                       
                        ad.Fill(userdata);

                    }
                    using (SqlDataAdapter ad = new SqlDataAdapter(BuildingsQuery, conn))
                    {
                        
                        ad.Fill(buildingsdata);

                    }
                    using (SqlDataAdapter ad = new SqlDataAdapter(usercount, conn))
                    {
                        
                        ad.Fill(usercountdata);

                    }
                    using (SqlDataAdapter ad = new SqlDataAdapter(Buildingcount, conn))
                    {
                        
                        ad.Fill(Buildingcountdata);

                    }
                    userdata.Columns.Add("Buildingcount");
                    foreach (DataRow dr in userdata.Rows)
                    {
                        foreach(DataRow dv in usercountdata.Rows)
                        {
                            if(dr["ID"].ToString()==dv["ID"].ToString())
                            {
                                dr["Buildingcount"] = dv["Buildingcount"];
                            }
                        }
                    }

                    foreach (DataRow dr in userdata.Rows)
                    {
                        foreach (DataRow dv in usercountdata.Rows)
                        {
                            if (dr["ID"].ToString() == dv["ID"].ToString())
                            {
                                dr["Buildingcount"] = dv["Buildingcount"];
                            }
                        }
                    }

                }
                
                
            }
            catch(Exception ex)
            {

            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
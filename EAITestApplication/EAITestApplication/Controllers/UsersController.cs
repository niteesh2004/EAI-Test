using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EAITestApplication.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            DataTable userdata = new DataTable();
            try
            {
                string connstring = ConfigurationManager.ConnectionStrings["EAITest"].ToString();
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    DataSet ds = new DataSet();
                    string Userquery = "select * from [EAITest].[dbo].Users;";
                    
                    string usercount = @"select u.ID, COUNT(u.ID) as Buildingcount  from [EAITest].[dbo].Users u, [EAITest].[dbo].Building B, 
                                    [EAITest].[dbo].UserBuilding x where u.ID = x.UserID and x.BuildingID = B.ID group by u.ID;";
                   
                    conn.Open();
                  
                    DataTable buildingsdata = new DataTable();
                    DataTable usercountdata = new DataTable();
                    DataTable Buildingcountdata = new DataTable();
                    using (SqlDataAdapter ad = new SqlDataAdapter(Userquery, conn))
                    {


                        ad.Fill(userdata);

                    }
                   
                    using (SqlDataAdapter ad = new SqlDataAdapter(usercount, conn))
                    {

                        ad.Fill(usercountdata);

                    }
                   
                    userdata.Columns.Add("Buildingcount");
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
            catch (Exception ex)
            {

            }
            return View(userdata);
        }

       
        public ActionResult BuildingDetails(int id,string name)
        {
            ViewBag.name = name;
                    DataTable DetailData = new DataTable();
                try
                {
                    string connstring = ConfigurationManager.ConnectionStrings["EAITest"].ToString();
                    using (SqlConnection conn = new SqlConnection(connstring))
                    {
                        DataSet ds = new DataSet();
                        string DetailBuilding = @" select B.Name from[EAITest].[dbo].Users u, [EAITest].[dbo].Building B,
                                                [EAITest].[dbo].UserBuilding x where u.ID = x.UserID and x.BuildingID = B.ID and
                                                u.ID = "+id;

                                    conn.Open();          
                        using (SqlDataAdapter ad = new SqlDataAdapter(DetailBuilding, conn))
                        {
                            ad.Fill(DetailData);

                        }            
                   
                    }
                }
                catch (Exception ex)
                {

                }


            if (Request.IsAjaxRequest())
            {
                // return partial for AJAX requests
                return PartialView(DetailData);
                   
            }
            else
            {
                // return full view for regular requests
                return View(DetailData);
                    
            }

        }
    }
}
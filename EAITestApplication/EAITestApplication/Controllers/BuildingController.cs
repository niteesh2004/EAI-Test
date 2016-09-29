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
    public class BuildingController : Controller
    {
        // GET: Building
        public ActionResult Index()
        {
            string connstring = ConfigurationManager.ConnectionStrings["EAITest"].ToString();
            DataTable userdata = new DataTable();
            DataTable buildingsdata = new DataTable();
            DataTable usercountdata = new DataTable();
            DataTable Buildingcountdata = new DataTable();
            try { 
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                DataSet ds = new DataSet();
                //string Userquery = "select * from [EAITest].[dbo].Users;";
                string BuildingsQuery = "select * from [EAITest].[dbo].Building;";
                
                                    
                string Buildingcount = @"select B.ID, COUNT(B.ID) as Usercount  from [EAITest].[dbo].Users u, [EAITest].[dbo].Building B, 
                                    [EAITest].[dbo].UserBuilding x where u.ID= x.UserID and x.BuildingID=B.ID group by B.ID;";
               
                conn.Open();
              
                
                using (SqlDataAdapter ad = new SqlDataAdapter(BuildingsQuery, conn))
                {

                    ad.Fill(buildingsdata);

                }
              
                using (SqlDataAdapter ad = new SqlDataAdapter(Buildingcount, conn))
                {

                    ad.Fill(Buildingcountdata);

                }
                buildingsdata.Columns.Add("Usercount");
                foreach (DataRow dr in buildingsdata.Rows)
                {
                    foreach (DataRow dv in Buildingcountdata.Rows)
                    {
                        if (dr["ID"].ToString() == dv["ID"].ToString())
                        {
                            dr["Usercount"] = dv["Usercount"];
                        }
                    }
                }

              

            }


        }
            catch(Exception ex)
            {

            }
            return View(buildingsdata);
}
        public ActionResult UserDetail(int id,string name)
        {
            ViewBag.name = name;
            DataTable DetailData = new DataTable();
            try
            {
                string connstring = ConfigurationManager.ConnectionStrings["EAITest"].ToString();
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    DataSet ds = new DataSet();
                    string DetailBuilding = @" select u.Name from[EAITest].[dbo].Users u, [EAITest].[dbo].Building B,
                                                [EAITest].[dbo].UserBuilding x where u.ID = x.UserID and x.BuildingID = B.ID and
                                                B.ID = " + id;

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
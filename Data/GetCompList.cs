using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace GTI.Modules.ReportCenter
{
    class GetCompList
    {
        public List<CompList> getList()
        {
            string sqlConnectionString = "Data Source=" + Business.Configuration.DBServer + ";Persist Security Info=True;Password=" + "\"" + Business.Configuration.DBPassword + "\"" + ";User ID=" + Business.Configuration.DBUser + ";Initial Catalog=" + Business.Configuration.DBName + "";
            SqlConnection sc = new SqlConnection(sqlConnectionString);
                 
            List<CompList> clList = new List<CompList>();
            try
            {  

                sc.Open();
       
                using (SqlCommand command = new SqlCommand(
                    @"select c.CompID , c.CompName, ca.PlayerID
                    from Comps c
                    left join CompAward ca on ca.CompID = c.CompID", sc))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        CompList cl = new CompList();
                        cl.CompId = reader.GetInt32(0);
                        cl.CompName = reader.GetString(1);
                        
                        if (reader.IsDBNull(2))
                        {
                            cl.PlayerID = null;
                        }
                        else
                        {
                            cl.PlayerID = reader.GetInt32(2);//This can be null
                        }
                            clList.Add(cl);
                    }
                }


            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
            finally
            {
                sc.Close();
            }

            return clList;
        }

    }

    public class CompList
    {
        public int CompId; //{ get; set; }
        public string CompName; //{ get; set; }
        public int? PlayerID; //{ get; set; }

        //public CompList(int compid, string compname, int playerid)
        //{
        //    CompId = compid;
        //    CompName = compname;
        //    PlayerID = playerid;
        //}
    }



}

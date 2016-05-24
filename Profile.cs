using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.IO;
using System.Data;

namespace LinkedIn_for_sqlserver
{
    class Profile
    {
        void getindu(List<String> indu)
        {
            indu.Add("Program Development"); indu.Add("Computer Games"); indu.Add("Internet");
            indu.Add("Computer &amp; Network Security"); indu.Add("Computer Software"); indu.Add("Computer Networking");
            indu.Add("Computer Hardware"); indu.Add("Online Media"); indu.Add("Wireless");
            indu.Add("Information Services"); indu.Add("Information Technology and Services"); indu.Add("Telecommunications");
        }
        public void getdata()
        {
            List<String> indu = new List<String>();
            getindu(indu);
            SqlServerOperation SqlOperation = new SqlServerOperation();
            Output writer = new Output();
            while (true)
            {
                string query = "select top 50 * from userlink where flag = 0";
                DataTable dt = new DataTable();
                dt = SqlServerOperation.GetDataTable(query);
                int idbegin = -1,id = -1;
                DataTable dtuserlink = new DataTable();
                DataTable dtprofile = new DataTable();
                CreatTable ct = new CreatTable();
                ct.creatdt(dtuserlink, 1);
                ct.creatdt(dtprofile, 2);
                DataRow r;
                foreach (DataRow row in dt.Rows)
                {
                    if(idbegin == -1)
                    {
                        idbegin = Convert.ToInt32(row[0]);
                    }
                    string Name = null, Headline = null, Locality = null, Industry = null, Current = null, Previous = null, Education = null, Recommend = null, Connection = null, Summary = null, Experience = null, Project = null, Skill = null, Edufull = null, Link = null;
                    id = Convert.ToInt32(row[0]); string sourcelink = row[2].ToString();
                    Console.WriteLine("linkid  " + id + "   " + sourcelink);
                    try
                    {
                        var uri = new Uri(sourcelink.ToString());
                        var browser1 = new ScrapingBrowser();
                        var html1 = browser1.DownloadString(uri);
                        var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(html1);
                        var html = htmlDocument.DocumentNode;
                        System.DateTime currentTime = new System.DateTime();

                        currentTime = System.DateTime.Now;
                        Person person = new Person(html);
                        person.setindu();
                        Industry = person.getindu();
                        int flag = 0;
                        if (Industry != null)
                        {
                            for (int i = 0; i < indu.Count; i++)
                            {
                                if ((Industry.ToString()).Equals(indu[i]))
                                {
                                    flag = 1; break;
                                }
                            }
                        }
                        if (flag == 0)
                        {
                            string strsql =  "delete from userlink where id = " + id;
                            SqlServerOperation.ExecuteNonQuery(strsql);
                            continue;
                        }
                        person.setcon();
                        int tempcon = person.getcon();
                        if(tempcon < 10)
                        {
                            string strsql = "delete from userlink where id = " + id;
                            SqlServerOperation.ExecuteNonQuery(strsql);
                            continue;
                        }
                        Connection = tempcon.ToString();
                        person.setheadline();
                        person.setloc();
                        person.setname();
                        person.setothers();
                        person.setsummary();
                        person.setexp();
                        person.setproject();
                        person.setedufull();
                        person.setskill();
                        person.setlink();
                        Name = person.getname();
                        Headline = person.getheadline();
                        Locality = person.getloc();
                        Current = person.getcurrent();
                        Previous = person.getpre();
                        Education = person.getedu();
                        Recommend = person.getreco();
                        Summary = person.getsummary();
                        Experience = person.getexp();
                        Project = person.getproject();
                        Edufull = person.getedufull();
                        Skill = person.getskill();
                        Link = person.getlink();

                        var link = html.CssSelect("h4.item-title");
                        foreach (var htmlNode in link)
                        {
                            string str = htmlNode.InnerHtml.ToString();
                            if (str.IndexOf("pub-pbmap") >= 0)
                            {
                                string alsoname = htmlNode.InnerText;
                                string alsolink = str.Split('"')[1].Split('"')[0];
                                r = dtuserlink.NewRow();
                                r["username"] = alsoname;
                                r["userlink"] = alsolink;
                                r["time"] = currentTime;
                                r["flag"] = 0;
                                try
                                {
                                    dtuserlink.Rows.Add(r);
                                }
                                catch(Exception e)
                                {
                                    writer.writetofile(e);
                                }
                                
                            }  
                        }
                        r = dtprofile.NewRow();
                        r["Name"] = Name;
                        r["Headline"] = Headline;
                        r["Locality"] = Locality;
                        r["Industry"] = Industry;
                        r["Current"] = Current;
                        r["Previous"] = Previous;
                        r["Education"] = Education;
                        r["Recommend"] = Recommend;
                        r["Connection"] = Connection;
                        r["Summary"] = Summary;
                        r["Experience"] = Experience;
                        r["Project"] = Project;
                        r["Skill"] = Skill;
                        r["Edufull"] = Edufull;
                        r["Link"] = Link;
                        r["Time"] = currentTime;
                        try
                        {
                            dtprofile.Rows.Add(r);
                        }
                        catch(Exception e)
                        {
                            writer.writetofile(e);
                        }
                         
                    }
                    catch (Exception msg)
                    {
                        string strsql = "delete from userlink where id = " + id;
                        SqlServerOperation.ExecuteNonQuery(strsql);
                        Console.WriteLine(msg);
                        writer.writetofile(msg);
                        continue;
                    }
                }
                DataRowCollection rows = dtuserlink.Rows;
                rows = dtprofile.Rows;
                SqlServerOperation.ExecuteSqlBulkCopy(dtuserlink, "dbo.userlink");
                SqlServerOperation.ExecuteSqlBulkCopy(dtprofile, "dbo.userprofile");
                string temp2 = "update userlink set flag  = 1 where id <= " + id +" and id >= " + idbegin;
                SqlServerOperation.ExecuteNonQuery(temp2);
            }
        }
    }
}

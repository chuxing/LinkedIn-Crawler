using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using Newtonsoft.Json;
namespace LinkedIn_for_sqlserver
{
    public class Person
    {
        public Person() { }
        private HtmlAgilityPack.HtmlNode html { get; set; }
        public Person(HtmlAgilityPack.HtmlNode html)
        {
            this.html = html;
        }
        private string Name { get; set; }
        public void setname()
        {
            var name = html.CssSelect("#name");
            foreach (var htmlNode in name)
            {
                this.Name = htmlNode.InnerText.ToString();
                this.Name = Name.Replace('\'', '"');
            }
        }
        public string getname()
        {
            return this.Name;
        }
        private string Headline { get; set; }
        public void setheadline()
        {
            var headline = html.CssSelect(".headline");
            foreach (var htmlNode in headline)
            {
                this.Headline = htmlNode.InnerText.ToString();
                this.Headline = Headline.Replace('\'', '"');
                break;
            }
        }
        public string getheadline()
        {
            return this.Headline;
        }
        private string Locality { get; set; }
        public void setloc()
        {
            var locality = html.CssSelect(".locality");
            foreach (var htmlNode in locality)
            {
                this.Locality = htmlNode.InnerText.ToString();
                this.Locality = Locality.Replace('\'', '"');
            }
        }
        public string getloc()
        {
            return this.Locality;
        }
        private string Industry { get; set; }
        public void setindu()
        {
            var industry = html.CssSelect(".descriptor");

            foreach (var htmlNode in industry)
            {
                var tmp = htmlNode.InnerHtml.ToString();
                if (tmp.Contains("<")) continue;
                this.Industry = htmlNode.InnerText.ToString();
                this.Industry = Industry.Replace('\'', '"');
            }
        }
        public string getindu()
        {
            return this.Industry;
        }
        private string Current { get; set; }
        private string Previous { get; set; }
        private string Education { get; set; }
        private string Recommend { get; set; }
        public void setothers()
        {
            var allfour = html.CssSelect("tr");
            foreach (var htmlNode in allfour)
            {
                String fourhtml = htmlNode.InnerHtml.ToString();

                if (fourhtml.IndexOf("Current") != -1)
                {
                    var tmp = htmlNode.CssSelect("td");
                    foreach (var ht in tmp)
                    {
                        this.Current = ht.InnerText.ToString();
                        this.Current = Current.Replace('\'', '"');

                    }
                }
                else if (fourhtml.IndexOf("Previous") != -1)
                {
                    var tmp = htmlNode.CssSelect("td");
                    foreach (var ht in tmp)
                    {
                        this.Previous = ht.InnerText.ToString();
                        this.Previous = Previous.Replace('\'', '"');
                        break;
                    }
                }

                else if (fourhtml.IndexOf("Education") != -1)
                {
                    var tmp = htmlNode.CssSelect("td");
                    foreach (var ht in tmp)
                    {
                        this.Education = ht.InnerText.ToString();
                        this.Education = Education.Replace('\'', '"');
                        break;
                    }
                }

                else if (fourhtml.IndexOf("Recommendations") != -1)
                {
                    var tmp = htmlNode.CssSelect("td");
                    foreach (var ht in tmp)
                    {
                        this.Recommend = ht.InnerText.ToString();
                        break;
                    }
                }
            }
        }
        public string getcurrent()
        {
            return this.Current;
        }
        public string getpre()
        {
            return this.Previous;
        }
        public string getedu()
        {
            return this.Education;
        }
        public string getreco()
        {
            return this.Recommend;
        }

        private int Connection { get; set; }
        public void setcon()
        {
            var connections = html.CssSelect("div.member-connections");
            int tem = 0;
            this.Connection = 0;
            foreach (var htmlnode in connections)
            {
                string aa = htmlnode.InnerText.ToString();
                if (aa.IndexOf("+") < 0)
                {
                    aa = aa.Split(' ')[0];
                    tem = Convert.ToInt32(aa);
                    this.Connection = tem;
                }
                else
                {
                    this.Connection = 500;
                }
            }
        }
        public int getcon()
        {
            return this.Connection;
        }

        private string Summary { get; set; }
        public void setsummary()
        {
            var summary = html.CssSelect("#summary");
            foreach (var htmlnode in summary)
            {
                this.Summary = htmlnode.InnerText.ToString().Replace("Summary", "");
                this.Summary = Summary.Replace('\'', '"');
            }
        }
        public string getsummary()
        {
            return this.Summary;
        }
        private string Experience { get; set; }
        public void setexp()
        {
            List<Dictionary<string, string>> mylist1 = new List<Dictionary<string, string>>();
            var experience = html.CssSelect("#experience");
            mylist1.Clear();
            foreach (var htmlNode in experience)
            {
                int it = -1;
                var all = htmlNode.CssSelect(".position");
                foreach (var te in all)
                {
                    Dictionary<string, string> mydic1 = new Dictionary<string, string>();
                    var title = te.CssSelect(".item-title");
                    foreach (var te1 in title)
                        mydic1.Add("Title:", te1.InnerText);
                    var subtitle = te.CssSelect(".item-subtitle");
                    foreach (var te1 in subtitle)
                        mydic1.Add("SubTitleTitle:", te1.InnerText);
                    var date = te.CssSelect(".date-range");
                    foreach (var te1 in date)
                        mydic1.Add("Date:", te1.InnerText);
                    var location = te.CssSelect(".location");
                    foreach (var te1 in location)
                        mydic1.Add("Location:", te1.InnerText);
                    var des = te.CssSelect(".description");
                    foreach (var te1 in des)
                        mydic1.Add("Description:", te1.InnerText);
                    mylist1.Add(mydic1);
                }
                string json_data = JsonConvert.SerializeObject(mylist1);
                this.Experience = json_data.ToString();
                this.Experience = Experience.Replace('\'', '"');
            }
        }
        public string getexp()
        {
            return this.Experience;
        }
        private string Project { get; set; }
        public void setproject()
        {
            List<Dictionary<string, string>> mylist2 = new List<Dictionary<string, string>>();
            var pro = html.CssSelect("#projects");
            foreach (var htmlnode in pro)
            {
                var all = htmlnode.CssSelect(".project");
                foreach (var te in all)
                {
                    Dictionary<string, string> mydic2 = new Dictionary<string, string>();
                    var title = te.CssSelect(".item-title");
                    foreach (var te1 in title)
                        mydic2.Add("Title:", te1.InnerText);
                    var date = te.CssSelect(".date-range");
                    foreach (var te1 in date)
                        mydic2.Add("Date:", te1.InnerText);
                    var des = te.CssSelect(".contributors");
                    foreach (var te1 in des)
                        mydic2.Add("Contributors:", te1.InnerText);
                    mylist2.Add(mydic2);
                }
                string json_data = JsonConvert.SerializeObject(mylist2);
                this.Project = json_data.ToString();
                this.Project = Project.Replace('\'', '"');
            }
        }
        public string getproject()
        {
            return this.Project;
        }
        private string Edufull { get; set; }
        public void setedufull()
        {
            List<Dictionary<string, string>> mylist3 = new List<Dictionary<string, string>>();
            var education = html.CssSelect("#education");
            foreach (var htmlNode in education)
            {
                var all = htmlNode.CssSelect(".school");
                foreach (var te in all)
                {
                    Dictionary<string, string> mydic3 = new Dictionary<string, string>();
                    var title = te.CssSelect(".item-title");
                    foreach (var te1 in title)
                        mydic3.Add("Title:", te1.InnerText);
                    var subtitle = te.CssSelect(".item-subtitle");
                    foreach (var te1 in subtitle)
                        mydic3.Add("SubTitle:", te1.InnerText);
                    var date = te.CssSelect(".date-range");
                    foreach (var te1 in date)
                        mydic3.Add("Date:", te1.InnerText);
                    var location = te.CssSelect(".location");
                    foreach (var te1 in location)
                        mydic3.Add("Location:", te1.InnerText);
                    var des = te.CssSelect(".description");
                    foreach (var te1 in des)
                        mydic3.Add("Desciption:", te1.InnerText);
                    mylist3.Add(mydic3);
                }
                string json_data = JsonConvert.SerializeObject(mylist3);
                this.Edufull = json_data.ToString();
                this.Edufull = Edufull.Replace('\'', '"');
            }
        }
        public string getedufull()
        {
            return this.Edufull;
        }
        private string Skill { get; set; }
        public void setskill()
        {
            List<string> mylist4 = new List<string>();
            mylist4.Clear();
            var skills = html.CssSelect("#skills");
            foreach (var htmlNode in skills)
            {
                var skill = htmlNode.CssSelect("span");
                foreach (var te in skill)
                {
                    mylist4.Add(te.InnerText + ";");
                }
                string json_data = JsonConvert.SerializeObject(mylist4);
                this.Skill = json_data.ToString();
                this.Skill = Skill.Replace('\'', '"');
            }
        }
        public string getskill()
        {
            return this.Skill;
        }
        private string Link { get; set; }
        public void setlink()
        {
            var selflink = html.CssSelect("link");
            foreach (var htmlNode in selflink)
            {
                string te = htmlNode.OuterHtml.ToString();
                if (te.IndexOf("canonical") >= 0)
                {
                    string ll = "http:" + te.Split(':')[1].Split('"')[0];
                    this.Link = ll;
                    this.Link = Link.Replace('\'', '"');
                }
            }
        }
        public string getlink()
        {
            return this.Link;
        }
        ~Person()
        {

        }
    }
}

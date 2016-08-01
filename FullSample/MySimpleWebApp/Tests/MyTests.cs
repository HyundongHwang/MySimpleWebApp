using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySimpleWebApp.Models;
using MySimpleWebApp.MyRestProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MySimpleWebApp.Tests
{
    [TestClass]
    public class MyTests
    {
        [TestMethod]
        public async Task MoviesApi_GetListAsync()
        {
            using (var client = new MyRestProxyClient())
            {
                var resStr = await client.MoviesApi.GetListAsync();
            }
        }



        [TestMethod]
        public async Task db_clear_all()
        {
            using (var db = new mydbEntities())
            {
                var tableNameList = db.Database.SqlQuery<string>(@"
                    SELECT TABLE_NAME
                    FROM information_schema.tables
                    WHERE TABLE_SCHEMA = 'dbo'
                ").ToList();

                while (tableNameList.Any())
                {
                    for (int i = tableNameList.Count - 1; i >= 0; i--)
                    {
                        var table = tableNameList[i];
                        var sql = "";

                        if (table.StartsWith("AspNet") ||
                            table.StartsWith("__MigrationHistory"))
                        {
                            sql = $"DROP TABLE {table}";
                        }
                        else
                        {
                            sql = $"TRUNCATE TABLE {table}";
                        }

                        try
                        {
                            Trace.TraceInformation("start sql : " + sql);
                            await db.Database.ExecuteSqlCommandAsync(sql);
                            Trace.TraceInformation("end sql : " + sql);
                            tableNameList.RemoveAt(i);
                        }
                        catch (Exception ex)
                        {
                            Trace.TraceInformation("ex : " + ex.GetType().Name);
                        }
                    }
                }
            }
        }



        class Student
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        class Subject
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        class Sugang
        {
            public int id { get; set; }
            public int stuId { get; set; }
            public int subId { get; set; }
        }


        [TestMethod]
        public async Task linq_multi_join()
        {
            var stuList = new List<Student>
            {
                new Student { id = 0, name = "가", },
                new Student { id = 1, name = "나", },
                new Student { id = 2, name = "다", },
            };

            var subList = new List<Subject>
            {
                new Subject { id = 0, name = "국어", },
                new Subject { id = 1, name = "영어", },
                new Subject { id = 2, name = "수학", },
            };

            var sugList = new List<Sugang>
            {
                new Sugang { id = 0, stuId = 0, subId = 0, },
                new Sugang { id = 1, stuId = 0, subId = 1, },
                new Sugang { id = 2, stuId = 1, subId = 0, },
                new Sugang { id = 3, stuId = 1, subId = 1, },
                new Sugang { id = 4, stuId = 1, subId = 2, },
                new Sugang { id = 5, stuId = 2, subId = 2, },
            };


            {
                var list = from sug in sugList

                           join stu in stuList
                           on sug.stuId equals stu.id

                           join sub in subList
                           on sug.subId equals sub.id

                           select new
                           {
                               stu_name = stu.name,
                               sub_name = sub.name
                           };

                var json = JsonConvert.SerializeObject(list, Formatting.Indented);
            }


            {
                var list = from sub in subList

                           join sug in sugList
                           on sub.id equals sug.subId
                           into sub_sug_group

                           select new
                           {
                               sub_name = sub.name,
                               stu_list = from sug2 in sub_sug_group
                                          join stu in stuList
                                          on sug2.stuId equals stu.id
                                          select stu.name,
                           };

                var json = JsonConvert.SerializeObject(list, Formatting.Indented);
            }

            {
                var list = from sub in subList

                           join sug in sugList
                           on sub.id equals sug.subId
                           into sub_sug_group

                           select new
                           {
                               sub,
                               sub_sug_group
                           };

                var list2 = from i in list

                            from ii in i.sub_sug_group

                            join stu in stuList
                            on ii.stuId equals stu.id

                            select new
                            {
                                sub_name = i.sub.name,

                            };

                var json = JsonConvert.SerializeObject(list, Formatting.Indented);
            }




            var a = 1;
            var b = 2;
            var c = a + b;
            await Task.Delay(3000);
            Assert.IsTrue(c == 3);
        }
    }



}
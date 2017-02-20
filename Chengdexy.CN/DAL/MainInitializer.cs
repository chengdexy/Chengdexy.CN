using Chengdexy.CN.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Chengdexy.CN.DAL
{
    public class MainInitializer : DropCreateDatabaseIfModelChanges<MainContext>
    {
        protected override void Seed(MainContext context)
        {
            //
            // 初始化导航条

            var NavbarItems = new List<NavbarItem>
            {
                new NavbarItem { Text="Home", Action="Index", Route="Home" },
                new NavbarItem { Text="Programs",Action="Index",Route="Programs"},
                new NavbarItem { Text="Blogs",Action="Index",Route="Blogs"},
                new NavbarItem { Text="About",Action="Index",Route="About"}
            };
            NavbarItems.ForEach(ni => context.NavbarItems.Add(ni));
            context.SaveChanges();

            //
            // 初始化首页巨幕

            var Jumbotrons = new List<Jumbotron>
            {
                new Jumbotron
                {
                    Capital ="测试首页巨幕",
                    Describe ="描述信息描述信息描述信息描述信息描述信息描述信息描述信息描述信息描述信息描述信息",
                    DownloadButtonText="按钮文本",
                    DownloadUrl="#"
                }
            };
            Jumbotrons.ForEach(jb => context.Jumbotrons.Add(jb));
            context.SaveChanges();

            //
            // 初始化博客简述

            var BlogSketchs = new List<BlogSketch>
            {
                new BlogSketch { BlogCapital="测试博客简述1",BlogDescribe="描述信息描述信息描述信息描述信息描述信息",BlogUrl="#",ImageFileName="default.jpg"},
                new BlogSketch { BlogCapital="测试博客简述2",BlogDescribe="描述信息描述信息描述信息描述信息描述信息",BlogUrl="#",ImageFileName="default.jpg"},
                new BlogSketch { BlogCapital="测试博客简述3",BlogDescribe="描述信息描述信息描述信息描述信息描述信息",BlogUrl="#",ImageFileName="default.jpg"},
                new BlogSketch { BlogCapital="测试博客简述4",BlogDescribe="描述信息描述信息描述信息描述信息描述信息",BlogUrl="#",ImageFileName="default.jpg"}
            };
            BlogSketchs.ForEach(bs => context.BlogSketchs.Add(bs));
            context.SaveChanges();

            //
            // 初始化"关于"

            var AboutItems = new List<AboutItem>
            {
                new AboutItem("item_1","value_1"),
                new AboutItem("item_2","value_2"),
                new AboutItem("item_3","value_3"),
                new AboutItem("item_4","value_4"),
                new AboutItem("item_5","value_5"),
                new AboutItem("item_6","value_6")
            };
            AboutItems.ForEach(ai => context.AboutItems.Add(ai));
            context.SaveChanges();

            //
            // 初始化编程作品

            var Programs = new List<Program>
            {
                new Models.Program
                {
                    Ename ="TestProgram_1",
                    Cname ="测试作品1",
                    Describe ="描述信息描述信息描述信息描述信息描述信息",
                    Motive ="创作背景创作背景创作背景创作背景创作背景",
                    ProgramEditions=new List<ProgramEdition>
                    {
                        new ProgramEdition
                        {
                            PublishDate =DateTime.Now,
                            DownloadUrl="#",
                            EditionString="v1.1.1.1",
                        }
                    }
                },

                new Models.Program
                {
                    Ename ="TestProgram_2",
                    Cname ="测试作品2",
                    Describe ="描述信息描述信息描述信息描述信息描述信息",
                    Motive ="创作背景创作背景创作背景创作背景创作背景",
                    ProgramEditions=new List<ProgramEdition>
                    {
                        new ProgramEdition
                        {
                            PublishDate =DateTime.Now,
                            DownloadUrl="#",
                            EditionString="v1.2.2.2",
                        }
                    }
                }
            };
            Programs.ForEach(p => context.Programs.Add(p));
            context.SaveChanges();
        }
    }
}
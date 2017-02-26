using Chengdexy.CN.Models;
using Chengdexy.CN.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Chengdexy.CN.DAL
{
    public class MainInitializer : DropCreateDatabaseIfModelChanges<MainContext>
    {
        protected override void Seed(MainContext context)
        {
            #region 全局属性初始化
            var MainSites = new List<MainSite>
            {
                new MainSite
                {
                    TitleText ="Chengdexy is here",
                    MainCapital="Hey, It's Chengdexy",
                    SubCapital="Welcome, it's all about me."
                }
            };
            MainSites.ForEach(ms => context.MainSites.Add(ms));
            context.SaveChanges();
            #endregion
            #region 前台Models初始化
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
            // 初始化博文

            var BlogPages = new List<BlogPage>
            {
                new BlogPage
                {
                    Title ="测试博文1",
                    CreateTime =DateTime.Now,
                    Sketch="测试博文简介1",
                    Content="测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本",
                    ImagePath="img/default.jpg"
                },
                new BlogPage
                {
                    Title ="测试博文2",
                    CreateTime =DateTime.Now,
                    Sketch="测试博文简介2",
                    Content="测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本",
                    ImagePath="img/default.jpg"
                },
                new BlogPage
                {
                    Title ="测试博文3",
                    CreateTime =DateTime.Now,
                    Sketch="测试博文简介3",
                    Content="测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本",
                    ImagePath="img/default.jpg"
                },
                new BlogPage
                {
                    Title ="测试博文4",
                    CreateTime =DateTime.Now,
                    Sketch="测试博文简介4",
                    Content="测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本",
                    ImagePath="img/default.jpg"
                },
                new BlogPage
                {
                    Title ="测试博文5",
                    CreateTime =DateTime.Now,
                    Sketch="测试博文简介5",
                    Content="测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本",
                    ImagePath="img/default.jpg"
                },
                new BlogPage
                {
                    Title ="测试博文6",
                    CreateTime =DateTime.Now,
                    Sketch="测试博文简介6",
                    Content="测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本测试博文文本",
                    ImagePath="img/default.jpg"
                }
            };
            BlogPages.ForEach(bp => context.BlogPages.Add(bp));
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
            #endregion
            #region 后台Models初始化

            //
            // 初始化AdminAccount

            var adminAccounts = new List<AdminAccount>
            {
                new AdminAccount
                {
                    Account="admin",
                    Password=MD5maker.GetMd5Hash(MD5.Create(),"darkmoon1")
                }
            };
            adminAccounts.ForEach(aa => context.AdminAccounts.Add(aa));
            context.SaveChanges();

            //
            // 初始化AdminSidebarItem

            var adminSidebarItems = new List<AdminSidebarItem>
            {
                new AdminSidebarItem {Text="后台首页",Controller="Admin",Action="Index" },
                new AdminSidebarItem { Text="网站设置",Controller="Admin",Action="WebsiteSettings"},
                new AdminSidebarItem { Text="首页设置",Controller="Admin",Action="HomepageSettings"},
                new AdminSidebarItem { Text="关于页设置",Controller="Admin",Action="AboutSettings"},
                new AdminSidebarItem { Text="主导航设置",Controller="Admin",Action="NavbarSettings"},
                new AdminSidebarItem { Text="编程作品管理",Controller="Admin",Action="ProgramSettings"},
                new AdminSidebarItem { Text="博客博文管理",Controller="Admin",Action="BlogSettings"},
                new AdminSidebarItem { Text="管理员账号管理",Controller="Admin",Action="AdminSettings"},
            };
            adminSidebarItems.ForEach(asi => context.AdminSidebarItems.Add(asi));
            context.SaveChanges();

        }
    }
    #endregion
}


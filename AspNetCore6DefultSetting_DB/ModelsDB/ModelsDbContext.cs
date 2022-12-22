using System;
using AspNetCore6DefultSetting_DB.Global;
using Microsoft.EntityFrameworkCore;

namespace ModelsDB
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelsDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
#pragma warning disable CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
        public ModelsDbContext()
#pragma warning restore CS8618 // 생성자를 종료할 때 null을 허용하지 않는 필드에 null이 아닌 값을 포함해야 합니다. null 허용으로 선언해 보세요.
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            

            switch (GlobalStatic.DBType)
            {
                case "sqlite":
                    options.UseSqlite(GlobalStatic.DBString);
                    break;
                case "mysql":
                    //options.UseSqlite(GlobalStatic.DBString);
                    break;
                case "mssql":
                    //options.UseSqlServer(GlobalStatic.DBString);
                    break;

                case "inmemory":
                default:
                    //options.UseInMemoryDatabase("TestDb");
                    break;
            }
        }

        #region User
        /// <summary>
        /// 유저 사인인 정보
        /// </summary>
        public DbSet<User> User { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    idUser = 1,
                    PasswordHash = "1111",
                    SignName = "test01"
                }
                , new User
                {
                    idUser = 2,
                    PasswordHash = "1111",
                    SignName = "test02"
                });
        }
    }
}

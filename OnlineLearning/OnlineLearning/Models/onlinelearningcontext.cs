using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Onlinecourseattendance.Models
{

     public class onlinelearningcontext : DbContext
    {
        public onlinelearningcontext() : base("name=onlinelearning")
        {

            Database.SetInitializer<onlinelearningcontext>(null);
            this.Configuration.LazyLoadingEnabled = false;

            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 3600;
        }
        public DbSet<AccountManager> AccountManagerset { get; set; }
        public DbSet<Lecture> Lectureset { get; set; }
        public DbSet<Subject> Subjectset { get; set; }
        public DbSet<UserInfo> UserInfoset { get; set; }
        public DbSet<UserMaking> UserMakingset { get; set; }
        public DbSet<ForgotPassword> ForgotPasswordset { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.Ignore<ExtensionDataObject>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<AccountManager>().HasKey<int>(e => e.UserId).Ignore(e => e.EntityId);
            modelBuilder.Entity<ForgotPassword>().HasKey<int>(e => e.ID).Ignore(e => e.EntityId);

        }
    }
}
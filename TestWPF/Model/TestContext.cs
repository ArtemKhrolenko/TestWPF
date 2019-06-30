namespace TestWPF
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class TestContext : DbContext
    {

        public TestContext()
            : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            Database.SetInitializer<TestContext>(new DropCreateDatabaseIfModelChanges<TestContext>());
            base.OnModelCreating(modelBuilder);
        }

        //public DbSet<DbDigit> Digits { get; set; }
        public DbSet<SourceDigit> SourseDigits { get; set; }
        public DbSet<DestDigit> DestDigits { get; set; }


    }


}
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
        
        //Table 1
        public DbSet<SourceDigit> SourseDigits { get; set; }
        
        //Table2
        public DbSet<DestDigit> DestDigits { get; set; }


    }


}
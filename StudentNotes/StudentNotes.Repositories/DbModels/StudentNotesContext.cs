namespace StudentNotes.Repositories.DbModels
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class StudentNotesContext : DbContext
    {
        public StudentNotesContext()
            : base("name=StudentNotesContext")
        {
            var ensureCopy = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        public virtual DbSet<File> File { get; set; }
        public virtual DbSet<FileSharedGroup> FileSharedGroup { get; set; }
        public virtual DbSet<FileTagPattern> FileTagPattern { get; set; }
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupSemester> GroupSemester { get; set; }
        public virtual DbSet<GroupUser> GroupUser { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<Semester> Semester { get; set; }
        public virtual DbSet<SemesterSubject> SemesterSubject { get; set; }
        public virtual DbSet<SemesterSubjectFile> SemesterSubjectFile { get; set; }
        public virtual DbSet<SemesterUser> SemesterUser { get; set; }
        public virtual DbSet<StudySubject> StudySubject { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<UserPreferences> UserPreferences { get; set; }
        public virtual DbSet<UserSharedFile> UserSharedFile { get; set; }
        public virtual DbSet<UserVisitedSchool> UserVisitedSchool { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>()
                .HasMany(e => e.FileSharedGroup)
                .WithRequired(e => e.File)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<File>()
                .HasMany(e => e.SemesterSubjectFile)
                .WithRequired(e => e.File)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<File>()
                .HasMany(e => e.UserSharedFile)
                .WithRequired(e => e.File)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Grade>()
                .HasMany(e => e.StudySubject)
                .WithRequired(e => e.Grade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.FileSharedGroup)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.GroupSemester)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.GroupUser)
                .WithRequired(e => e.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<School>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<School>()
                .HasMany(e => e.Grade)
                .WithRequired(e => e.School)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<School>()
                .HasMany(e => e.UserVisitedSchool)
                .WithRequired(e => e.School)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Semester>()
                .HasMany(e => e.GroupSemester)
                .WithRequired(e => e.Semester)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Semester>()
                .HasMany(e => e.SemesterSubject)
                .WithRequired(e => e.Semester)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Semester>()
                .HasMany(e => e.SemesterUser)
                .WithRequired(e => e.Semester)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SemesterSubject>()
                .HasMany(e => e.FileSharedGroup)
                .WithRequired(e => e.SemesterSubject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SemesterSubject>()
                .HasMany(e => e.SemesterSubjectFile)
                .WithRequired(e => e.SemesterSubject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<StudySubject>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<StudySubject>()
                .HasMany(e => e.Semester)
                .WithRequired(e => e.StudySubject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.File)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Group)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.AdminId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.GroupUser)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SemesterUser)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.UserInfo)
                .WithRequired(e => e.User);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.UserPreferences)
                .WithRequired(e => e.User);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserSharedFile)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UserVisitedSchool)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserInfo>()
                .Property(e => e.Name)
                .IsFixedLength();
        }
    }
}

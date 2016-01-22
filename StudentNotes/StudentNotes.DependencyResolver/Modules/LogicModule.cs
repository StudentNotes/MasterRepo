using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.Services;
using StudentNotes.Repositories.Base;
using StudentNotes.Repositories.Infrastructure;
using StudentNotes.Repositories.Repositories;
using StudentNotes.Repositories.RepositoryInterfaces;

namespace StudentNotes.DependencyResolver.Modules
{
    public class LogicModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDbFactory>().To<DbFactory>().InScope(c => System.Web.HttpContext.Current);
            Bind<IUnitOfWork>().To<UnitOfWork>().InScope(c => System.Web.HttpContext.Current);

            Bind<IFileRepository>().To<FileRepository>();
            Bind<IFileSharedGroupRepository>().To<FileSharedGroupRepository>();
            Bind<IFileTagPatternRepository>().To<FileTagPatternRepository>();
            Bind<IGradeRepository>().To<GradeRepository>();
            Bind<IGroupRepository>().To<GroupRepository>();
            Bind<IGroupSemesterRepository>().To<GroupSemesterRepository>();
            Bind<IGroupUserRepository>().To<GroupUserRepository>();
            Bind<ISchoolRepository>().To<SchoolRepository>();
            Bind<ISemesterRepository>().To<SemesterRepository>();
            Bind<ISemesterSubjectFileRepository>().To<SemesterSubjectFileRepository>();
            Bind<ISemesterSubjectRepository>().To<SemesterSubjectRepository>();
            Bind<ISemesterUserRepository>().To<SemesterUserRepository>();
            Bind<IStudySubjectRepository>().To<StudySubjectRepository>();
            Bind<IUserInfoRepository>().To<UserInfoRepository>();
            Bind<IUserRepository>().To<UserRepository>();
            Bind<IUserSharedFileRepository>().To<UserSharedFileRepository>();
            Bind<IUserPreferencesRepository>().To<UserPreferencesRepository>();
            Bind<IUserVisitedSchoolRepository>().To<UserVisitedSchoolRepository>();
            Bind<ISubjectRepository>().To<SubjectRepository>();

            Bind<IFileServerService>().To<FileServerService>();
            Bind<IUserService>().To<UserService>();
            Bind<IFileService>().To<FileService>();
            Bind<ISchoolService>().To<SchoolService>();
            Bind<ISemesterSubjectService>().To<SemesterSubjectService>();
            Bind<IGroupService>().To<GroupService>();
        }
    }
}

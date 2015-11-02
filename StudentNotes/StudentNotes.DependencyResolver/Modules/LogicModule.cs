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

namespace DependencyResolver.Modules
{
    public class LogicModule : NinjectModule
    {
        public override void Load()
        {
            //throw new NotImplementedException();
            //Bind<IMiddleClass>().To<MiddleClass>();
            Bind<IDbFactory>().To<DbFactory>();
            Bind<IUnitOfWork>().To<UnitOfWork>();

            Bind<IFileRepository>().To<FileRepository>();
            Bind<IFileSharedGroupRepository>().To<FileSharedGroupRepository>();
            Bind<IFileTagPatternRepository>().To<FileTagPatternRepository>();
            Bind<IGradeRepository>().To<GradeRepository>();
            Bind<IGroupRepository>().To<GroupRepository>();
            Bind<IGroupSemesterRepositorycs>().To<GroupSemesterRepository>();
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
            Bind<IUserVisitedSchoolRepository>().To<UserVisitedSchoolRepository>();

            Bind<IUploadService>().To<UploadService>();

            //Bind<IFileSharedGroupRepository>().To<FileSharedGroupRepository>();
        }
    }
}

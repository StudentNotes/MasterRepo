using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using StudentNotes.Logic.Consts;
using StudentNotes.Logic.LogicModels;
using StudentNotes.Logic.ServiceInterfaces;
using StudentNotes.Logic.ViewModels.Home;
using StudentNotes.Logic.ViewModels.ManageGroups;
using StudentNotes.Web.Filters;
using StudentNotes.Logic.ViewModels.Common;
using StudentNotes.Logic.ViewModels.Validation;
using StudentNotes.Web.RequestViewModels.Management;

namespace StudentNotes.Web.Controllers
{
    [SessionFilter]
    public class ManagementController : Controller
    {
        private readonly ISemesterSubjectService _semesterSubjectService;
        private readonly IGroupService _groupService;
        private readonly ISchoolService _schoolService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;

        public ManagementController(ISemesterSubjectService studySubjectService, IGroupService groupService, IFileService fileService,
            IUserService userService, ISchoolService schoolService)
        {
            _semesterSubjectService = studySubjectService;
            _groupService = groupService;
            _schoolService = schoolService;
            _fileService = fileService;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult ManageUniversities()
        {
            var model = new UniversityViewModel
            {
                Universities = _schoolService.GetAllSchools().ToList()
            };

            return PartialView("~/Views/Partials/Management/ManageUniversities.cshtml", model);
        }

        [HttpGet]
        public ActionResult ConfigureUniversity(int schoolId)
        {
            var school = _schoolService.GetSchoolById(schoolId);
            var newUniversityModel = new NewUniversityViewModel()
            {
                UniversityName = school.Name,
                UniversityDescription = school.Description,
                UniversityId = schoolId,
                GradeList = _schoolService.GetAllSchoolGrades(schoolId).ToList()
            };
            return PartialView("~/Views/Partials/Management/AddGrade.cshtml", newUniversityModel);   
        }

        [HttpGet]
        public ActionResult AddNewUniversity()
        {
            var model = new NewUniversityViewModel();
            return PartialView("~/Views/Partials/Management/AddUniversity.cshtml", model);
        }

        [HttpPost]
        public ActionResult AddNewUniversity(AddNewUniversityRequest request, string ButtonType)
        {
            NewUniversityViewModel model = new NewUniversityViewModel();
            var response = request.Validate();
            if (!request.IsValid)
            {
                model.Response = response;
                return PartialView("~/Views/Partials/Management/AddUniversity.cshtml", model);
            }

            _schoolService.AddSchoolAndSave(request.UniversityName, request.UniversityDescription);

            switch (ButtonType)
            {
                case "saveUniversityAndExit":
                    return RedirectToAction("ManageUniversities");
                case "saveUniversityAndContinue":
                {
                    model.UniversityName = request.UniversityName;
                    model.UniversityDescription = request.UniversityDescription;
                    model.UniversityId = _schoolService.GetSchoolByName(request.UniversityName).SchoolId;

                    return PartialView("~/Views/Partials/Management/AddGrade.cshtml", model);   
                }
                default:
                    return PartialView("~/Views/Partials/Management/AddUniversity.cshtml", model);
            }
        }

        [HttpGet]
        public ActionResult DeleteUniversity(int id)
        {
            _schoolService.RemoveSchool(id);
            return RedirectToAction("ManageUniversities");
        }

        [HttpPost]
        public ActionResult AddGradeToSchool(ModifySchoolGradeRequest request)
        {
            var university = _schoolService.GetSchoolById(request.UniversityId);
            var model = new NewUniversityViewModel()
            {
                UniversityId = university.SchoolId,
                UniversityName = university.Name,
                UniversityDescription = university.Description
            };

            model.Response = (ResponseMessageViewModel) request.Validate();
            if (!request.IsValid)
            {
                model.UpdateGradeList();
                return PartialView("~/Views/Partials/Management/AddGrade.cshtml", model);
            }

            _schoolService.AddGradeToSchool(request.UniversityId, request.SchoolGrade);
            model.UpdateGradeList();
            return PartialView("~/Views/Partials/Management/AddGrade.cshtml", model);
        }

        [HttpPost]
        public ActionResult DeleteGradeFromSchool(ModifySchoolGradeRequest request)
        {
            _schoolService.RemoveGradeFromSchool(request.UniversityId, request.SchoolGrade);
            var school = _schoolService.GetSchoolById(request.UniversityId);

            NewUniversityViewModel model = new NewUniversityViewModel()
            {
                UniversityId = school.SchoolId,
                UniversityName = school.Name,
                UniversityDescription = school.Description,
            };

            model.UpdateGradeList();

            return PartialView("~/Views/Partials/Management/AddGrade.cshtml", model);
        }

        [HttpGet]
        public ActionResult AddStudySubjects(int schoolId)
        {
            var school = _schoolService.GetSchoolById(schoolId);

            UniversityGradeStudySubjectViewModel model = new UniversityGradeStudySubjectViewModel()
            {
                UniversityName = school.Name,
                UniversityId = school.SchoolId,
                StudyGrades = _schoolService.GetAllSchoolGrades(schoolId).ToList()
            };

            return PartialView("~/Views/Partials/Management/AddStudySubject.cshtml", model);
        }

        [HttpGet]
        public ActionResult DefineStudySubjects(int gradeId, int schoolId)
        {
            //  Get all STUDY!!! Subjects from database
            var school = _schoolService.GetSchoolById(schoolId);
            var gradeStudySubjects = _schoolService.GetAllStudySubjectsFromGrade(gradeId);

            StudySubjectViewModel model = new StudySubjectViewModel()
            {
                UniversityId = school.SchoolId,
                UniversityDescription = school.Description,
                GradeId = gradeId,
                StudySubjects = gradeStudySubjects.ToList()
            };
            
            return PartialView("~/Views/Partials/Management/StudySubjectList.cshtml", model);
        }

        [HttpGet]
        public ActionResult RemoveGradeSubject(int studySubjectId, int gradeId)
        {
            _schoolService.RemoveStudySubjectById(studySubjectId);
            var university = _schoolService.GetSchoolByGradeId(gradeId);

            return RedirectToAction("DefineStudySubjects", new {gradeId, university.SchoolId});
        }

        [HttpPost]
        public ActionResult AddGradeSubject(string studySubjectName, string studySubjectDescription, string semesterNumber, int gradeId)
        {
            //  Add semesterNumber semesters to database to gradeId position
            _schoolService.AddStudySubjectToGrade(gradeId, studySubjectName, studySubjectDescription,
                int.Parse(semesterNumber));

            var school = _schoolService.GetSchoolByGradeId(gradeId);

            return RedirectToAction("DefineStudySubjects", new {gradeId, school.SchoolId});
        }

        [HttpGet]
        public ActionResult AddSemesterSubjects(int schoolId)
        {
            //  Returns next partials (grades, studySubjects, semesters and semesterSubjects)
            SemesterSubjectViewModel model = new SemesterSubjectViewModel()
            {
                UniversityId = schoolId,
                GradeList = _schoolService.GetAllSchoolGrades(schoolId).ToList()
            };

            return PartialView("~/Views/Partials/Management/AddSemesterSubject.cshtml", model);
        }

        [HttpGet]
        public ActionResult TakeStudyGrade(int gradeId, int schoolId)
        {
            StudySubjectViewModel model = new StudySubjectViewModel()
            {
                UniversityId = schoolId,
                StudySubjects = _schoolService.GetAllStudySubjectsFromGrade(gradeId).ToList()
            };
            return PartialView("~/Views/Partials/Management/TakeStudySubject.cshtml", model);
        }

        [HttpGet]
        public ActionResult TakeStudySubject(int studySubjectId, int schoolId)
        {
            SemesterAssignSubjectViewModel model = new SemesterAssignSubjectViewModel()
            {
                UniversityId = schoolId,
                SemesterList = _schoolService.GetSemestersByStudySubjectId(studySubjectId).ToList()
            };

            return PartialView("~/Views/Partials/Management/TakeSemesterAssignSubject.cshtml", model);
        }

        [HttpGet]
        public ActionResult TakeSemester(int semesterId)
        {
            SemesterSubjects model = new SemesterSubjects()
            {
                SemesterId = semesterId,
                SemesterSubjectList = _schoolService.GetSemesterSubjectsBySemesterId(semesterId).ToList()
            };
            return PartialView("~/Views/Partials/Management/AssignSemesterSubjects.cshtml", model);
        }

        [HttpGet]
        public ActionResult RemoveSemesterSubject(int semesterSubjectId, int semesterId)
        {
            _schoolService.RemoveSemesterSubjectById(semesterSubjectId);
            return RedirectToAction("TakeSemester", new { semesterId });
        }

        [HttpPost]
        public ActionResult AddSemesterSubjectToSemester(string semesterSubjectName, int semesterId)
        {

            _schoolService.AddSemesterSubject(semesterId, semesterSubjectName);

            return RedirectToAction("TakeSemester", new {semesterId});
        }




        [HttpGet]
        public ActionResult ManageSubjectsList()
        {
            var subjectList = _semesterSubjectService.GetAllSubjects().OrderBy(s => s.Name).ToList();
            SubjectViewModel model = new SubjectViewModel(subjectList);

            return PartialView("~/Views/Partials/Management/ManageSubjects.cshtml", model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddSubject(string subjectName)
        {
            if (subjectName != "")
            {
                _semesterSubjectService.AddAndSaveSubject(subjectName);
            }

            var subjectList = _semesterSubjectService.GetAllSubjects().OrderBy(s => s.Name).ToList();
            SubjectViewModel model = new SubjectViewModel(subjectList);

            return PartialView("~/Views/Partials/Management/ManageSubjects.cshtml", model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RemoveSubjects(SubjectViewModel model)
        {
            foreach (var subject in model.Subjects)
            {
                if (subject.Value == true)
                {
                    _semesterSubjectService.DeleteSubjectAndSave(subject.Key);
                }
            }
            model = new SubjectViewModel(_semesterSubjectService.GetAllSubjects().OrderBy(s => s.Name).ToList());   
   
            return PartialView("~/Views/Partials/Management/ManageSubjects.cshtml", model);
        }

        [HttpPost]
        public ActionResult RemoveSubject(string subjectName)
        {
            if (!subjectName.IsEmpty())
            {
                _semesterSubjectService.DeleteSubjectAndSave(subjectName);
            }

            SubjectViewModel model = new SubjectViewModel(_semesterSubjectService.GetAllSubjects().OrderBy(s => s.Name).ToList());

            return PartialView("~/Views/Partials/Management/ManageSubjects.cshtml", model);
        }

        [HttpGet]
        public ActionResult ManageGroups()
        {
            List<ManageGroupViewModel> modelList = new List<ManageGroupViewModel>();
            var groups = _groupService.GetAdminGroups((int) Session["CurrentUserId"]).ToList();
            foreach (var group in groups)
            {
                var groupSemesters = _groupService.GetGroupSemesters(group.GroupId).ToList();
                string semesters = "";
                foreach (var semester in groupSemesters)
                {
                    semesters += semester.SemesterNumber + "; ";
                }

                modelList.Add(new ManageGroupViewModel()
                {
                    GroupId = group.GroupId,
                    GroupName = group.Name,
                    MemberNumber = _groupService.GetAllGroupUsers(group.GroupId).Count,
                    Semesters = semesters,
                    StudySubject = _groupService.GetStudySubjectByGroupId(group.GroupId).Name
                });
            }

            return PartialView("~/Views/Partials/ManageGroups/ManageGroupsPartial.cshtml", modelList);
        }

        [HttpGet]
        public ActionResult AddUsers(int groupId)
        {
            AddUserViewModel model = new AddUserViewModel();

            var group = _groupService.GetGroupById(groupId);
            var groupUsers = _groupService.GetAllGroupUsers(groupId).ToList();
            var semesterIds = _groupService.GetGroupSemesters(groupId).Select(s => s.SemesterId).ToList();
            List<SecureUserModel> allUsers = new List<SecureUserModel>();
            foreach (var semesterId in semesterIds)
            {
                var tmpSecureModel = _schoolService.GetUsersBySemesterId(semesterId);
                allUsers.AddRange(tmpSecureModel);
            }
            allUsers = allUsers.Distinct().ToList();

            RemoveAddedUsers(ref allUsers, ref groupUsers);

            model.GroupId = group.GroupId;
            model.GroupName = group.Name;
            model.GroupUsers = groupUsers;
            model.SemesterUsers = allUsers;


            return PartialView("~/Views/Partials/ManageGroups/AddUsersPartial.cshtml", model);
        }

        [HttpPost]
        public ActionResult RemoveUserFromGroup(int userId, int groupId)
        {
            var group = _groupService.GetGroupById(groupId);
            if (group.AdminId == userId)
            {
                HomeViewModel model = new HomeViewModel();
                model.LoginViewModel.ErrorList.Add(WebResponseCode.YouAreTheAdmin);
                return View("~/Views/LoggedIn/Index.cshtml", model);
            }

            _groupService.RemoveUserFromGroup(userId, groupId);
            _groupService.Commit();
            return RedirectToAction("AddUsers", new{groupId});
        }

        [HttpPost]
        public ActionResult AddUserToGroup(int userId, int groupId)
        {
            _groupService.AddUserToGroup(userId, groupId);
            _groupService.Commit();
            return RedirectToAction("AddUsers", new { groupId });
        }

        [HttpGet]
        public ActionResult UserDetails(int userId)
        {
            return null;
        }

        [HttpPost]
        public ActionResult DeleteGroup(int groupId)
        {
            _groupService.DeleteGroup(groupId);
            _groupService.Commit();
            return RedirectToAction("ManageGroups");
        }

        [HttpGet]
        public ActionResult GroupDetails(int groupId)
        {
            var group = _groupService.GetGroupById(groupId);
            var admin = _groupService.GetGroupAdminDetails(groupId);

            GroupDetailsViewModel model = new GroupDetailsViewModel()
            {
                GroupId = group.GroupId,
                GroupName = group.Name,
                GroupDescription = group.Description,
                CreatedOn = group.CreatedOn.ToString(),
                Admin = admin
            };
            return PartialView("~/Views/Partials/ManageGroups/GroupDetailsPartialView.cshtml", model);
        }

        [HttpGet]
        public ActionResult ChangeGroupPermissions(int userId, int groupId)
        {
            return null;
        }

        [HttpPost]
        public ActionResult UpdateGroup(int groupId, string groupName, string groupDescription)
        {
            if (groupName.IsEmpty())
            {
                HomeViewModel model = new HomeViewModel();
                model.LoginViewModel.ErrorList.Add(WebResponseCode.YouAreTheAdmin);
                return View("~/Views/LoggedIn/Index.cshtml", model);
            }
            _groupService.UpdateGroup(groupName, groupDescription, groupId);
            _groupService.Commit();

            return RedirectToAction("GroupDetails", new {groupId});
        }

        [HttpGet]
        public ActionResult UserPreferences()
        {
            var model = _fileService.GetSecureUser((int) Session["CurrentUserId"]);

            return PartialView("~/Views/Partials/Management/UserPreferencesPartial.cshtml", model);
        }

        private static void RemoveAddedUsers(ref List<SecureUserModel> semesterUsers, ref List<SecureUserModel> groupUsers)
        {
            foreach (var groupUser in groupUsers)
            {
                var userToDelete = semesterUsers.Find(u => u.UserId == groupUser.UserId);
                semesterUsers.Remove(userToDelete);
            }
        }

        [HttpPost]
        public ActionResult ChangeAvatar(HttpPostedFileBase file)
        {
            if (file.ContentLength == 0)
            {
                // obsłużyć, że user nie wybrał pliku
            }
            var fileName = $"{(int) Session["CurrentUserId"]}_avatar";
            var path = Path.Combine(Server.MapPath("~/App_Data/UserPictures"), fileName);

            file.SaveAs(path);
            _userService.AddAvatar((int)Session["CurrentUserId"], path);

            return View("~/Views/LoggedIn/Index.cshtml");
        }
    }

}
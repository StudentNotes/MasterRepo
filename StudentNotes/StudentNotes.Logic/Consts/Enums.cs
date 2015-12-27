namespace StudentNotes.Logic.Consts
{
    public enum ErrorCode
    {
        AllreadyJoinedToUniversity = 1,
        AllreadyJoinedToStudySubject,
        UniversityDoesntExist,
        StudySubjectDoesntExist,
        UniversityGradeDoesntExist,

        WrongDataPassed,

    }

    public enum NoteType
    {
        Private,
        University
    }

    public enum NoteAccess
    {
        Owner,
        Group,
        PrivateShare
    }
}



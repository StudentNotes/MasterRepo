using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotes.Logic.Consts
{
    public static class WebResponseCode
    {
        public static string JoinedSchool { get { return "Gratuluję! Dołączyłeś do wybranej uczelni/szkoły."; } }
        public static string JoinedStudySubject { get { return "Gratuluję! Zapisałeś sie na wybrany kierunek studiów."; } }

        public static string UniversityDoesntExist { get { return "Niestety taka szkoła nie istnieje w serwisie StudentNotes."; } }
        public static string StudySubjectDoesntExist { get { return "Wybrana uczelnia nie oferuje podanego kierunku kształcenia"; } }
        public static string UniversityGradeDoesntExist { get { return "Wybrana uczelnia nie przyjmowała w danym roku studentów/uczniów"; } }
        public static string WrongDataPassed { get { return "Nie wprowadziłeś wszystkich wymaganych danych. Dlatego nie mogę przeprowadzić wybranej operacji."; } }

        public static string AllreadyJoinedToStudySubject { get { return "Nie możesz sie zapisać na ten kierunek studiów, gdyż juz jesteś do niego przypisany.";} }

    }
}

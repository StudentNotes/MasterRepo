using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentNotes.Logic.Consts
{
    public static class WebResponseCode
    {
        public static string JoinedSchool { get { return "Gratuluję! Dołączyłeś do wybranej uczelni/szkoły."; } }
        public static string JoinedStudySubject { get { return "Gratuluję! Zapisałeś sie na wybrany kierunek studiów."; } }
        public static string FileUploadedSuccessfully { get { return "Pliki zostały zapisane w serwisie StudentNotes. Możesz zacząć je udostępniać."; } }
        public static string GroupAddedSuccessfully { get { return "Grupa została dodana do semestru z powodzeniem, a Ty jesteś jej administratorem!"; } }

        public static string UniversityDoesntExist { get { return "Niestety taka szkoła nie istnieje w serwisie StudentNotes."; } }
        public static string StudySubjectDoesntExist { get { return "Wybrana uczelnia nie oferuje podanego kierunku kształcenia"; } }
        public static string UniversityGradeDoesntExist { get { return "Wybrana uczelnia nie przyjmowała w danym roku studentów/uczniów"; } }
        public static string WrongDataPassed { get { return "Nie wprowadziłeś wszystkich wymaganych danych. Dlatego nie mogę przeprowadzić wybranej operacji."; } }
        public static string FileDoesntExist { get { return "Nie znalazłem takiego pliku w bazie danych."; } }
        public static string GroupNameEmpty { get { return "Nie podałeś nazwy grupy!"; } }
        public static string YouAreTheAdmin { get { return "Na osoba nie może zostać usunięta z grupy, gdyż jest jej administratorem."; } }

        public static string AllreadyJoinedToStudySubject { get { return "Nie możesz sie zapisać na ten kierunek studiów, gdyż juz jesteś do niego przypisany.";} }
        public static string SemesterAlreadyContainsGroup { get { return "Ten semestr zawiera już grupę o wskazanej nazwie. Aby zapewnić unikalność grup w obrębie semestrów podaj inną nazwę."; } }

        public static string SessionExpired { get { return "Twoja sesja wygasła. Zaloguj sie proszę ponownie."; } }
        public static string GlobalError { get { return "Akcja zakończona niepowodzeniem - błąd krytyczny!"; } }
    }
}

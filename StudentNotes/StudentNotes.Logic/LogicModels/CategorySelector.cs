using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace StudentNotes.Logic.LogicModels
{
    public class CategorySelector
    {
        private static Dictionary<string, string> categories = new Dictionary<string, string>()
        {
            {"txt", "plik tekstowy"},
            {"rtf", "plik tekstowy"},
            {"doc", "plik tekstowy"},
            {"docx", "plik tekstowy"},
            {"odt", "plik tekstowy"},
            {"pdf", "plik tekstowy"},

            {"xls", "arkusz kalkulacyjny"},
            {"xlsx", "arkusz kalkulacyjny"},
            {"ods", "arkusz kalkulacyjny"},

            {"ppt", "prezentacja multimedialna"},
            {"pptx", "prezentacja multimedialna"},
            {"odp", "prezentacja multimedialna"},

            {"jpeg", "plik graficzny"},
            {"jpg", "plik graficzny"},
            {"jps", "plik graficzny"},
            {"tiff", "plik graficzny"},
            {"png", "plik graficzny"},
            {"gif", "plik graficzny"},
            {"bmp", "plik graficzny"},
            {"xcf", "plik graficzny"},
            {"psd", "plik graficzny"},
            {"cdr", "plik graficzny"},
            {"swf", "plik graficzny"},
            {"dgn", "plik graficzny"},
            {"dwf", "plik graficzny"},
            {"dwg", "plik graficzny"},

            {"wav", "nagranie audio"},
            {"aif", "nagranie audio"},
            {"mpg", "nagranie audio"},
            {"mpeg", "nagranie audio"},
            {"mpeg2", "nagranie audio"},
            {"midi", "nagranie audio"},
            {"mp3", "nagranie audio"},
            {"wma", "nagranie audio"},

            {"3gp", "film wideo"},
            {"asf", "film wideo"},
            {"avi", "film wideo"},
            {"dv", "film wideo"},
            {"dvd", "film wideo"},
            {"flv", "film wideo"},
            {"m2ts", "film wideo"},
            {"mkv", "film wideo"},
            {"mp4", "film wideo"},
            {"smv", "film wideo"},
            {"ts", "film wideo"},
            {"wmv", "film wideo"},
            {"vcd", "film wideo"},

        }; 
        private static Dictionary<string, string>  _categoryPicturePath = new Dictionary<string, string>()
        {
            {"plik tekstowy", "/Resources/tekstFile.jpg"},
            {"arkusz kalkulacyjny", "/Resources/spreadSheet.jpg"},
            {"prezentacja multimedialna", "/Resources/presentationFile.jpg"},
            {"plik graficzny", "/Resources/graphics.jpg"},
            {"nagranie audio", "/Resources/audioFile.jpg"},
            {"film wideo", "/Resources/wideoFile.jpg"},
            {"pozostałe", "/Resources/otherFile.jpg"}
        };
        public static string GetCategory(string fileName)
        {
            var fileNameParts = fileName.Split('.');
            var fileExtenssion = fileNameParts.LastOrDefault();

            if (categories.ContainsKey(fileExtenssion))
            {
                return categories[fileExtenssion];
            }
            return "pozostałe";
        }
        public static string GetCategoryPicturePath(string categoryName)
        {
            if (categoryName.IsEmpty()) return "";
            if (_categoryPicturePath.ContainsKey(categoryName))
            {
                return _categoryPicturePath[categoryName];
            }
            return "";
        }
        
    }
}

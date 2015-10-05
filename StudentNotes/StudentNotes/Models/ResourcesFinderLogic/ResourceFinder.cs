using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web;

namespace StudentNotesWeb.Models.ResourcesFinderLogic
{
    public class ResourceFinder
    {

        //  Declaring resources manager to access the specific cultureinfo
        private static readonly ResourceManager resourceManager;

        //  Declaring culture info
        private static CultureInfo cultureInfo;

        static ResourceFinder()
        {
            resourceManager = new ResourceManager("StudentNotesWeb.Resources.Lang", typeof(StudentNotesWeb.MvcApplication).Assembly);
            cultureInfo = CultureInfo.CreateSpecificCulture("pl");
        }

        public static string GetResource(string resourceKey)
        {
            return resourceManager.GetString(resourceKey, cultureInfo);
        }

        public static void ChangeResourceLanguage(string language)
        {
            cultureInfo = CultureInfo.CreateSpecificCulture(language);
        }
    }
}
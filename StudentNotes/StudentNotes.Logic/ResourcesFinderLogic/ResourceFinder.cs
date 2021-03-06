﻿using System.Globalization;
using System.Resources;

namespace StudentNotes.Logic.ResourcesFinderLogic
{
    public class ResourceFinder
    {

        //  Declaring resources manager to access the specific cultureinfo
        private static readonly ResourceManager resourceManager;

        //  Declaring culture info
        private static CultureInfo cultureInfo;

        static ResourceFinder()
        {
            //resourceManager = new ResourceManager("StudentNotes.Web.Resources.Lang", typeof(StudentNotes.Web).Assembly);
            resourceManager = new ResourceManager("StudentNotes.Logic.Consts.Lang", typeof(StudentNotes.Logic.Consts.LogicConstants).Assembly);
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
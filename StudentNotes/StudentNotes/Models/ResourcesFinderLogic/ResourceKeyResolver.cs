using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentNotesWeb.Models.ResourcesFinderLogic
{
    public class ResourceKeyResolver
    {
        public static string HomePageNavBarAboutProject
        {
            get { return ResourceFinder.GetResource("HomePageNavBarAboutProject"); }
        }
        public static string HomePageNavBarAboutAuthor
        {
            get { return ResourceFinder.GetResource("HomePageNavBarAboutAuthor"); }
        }
        public static string HomePageNavBarSignUp
        {
            get { return ResourceFinder.GetResource("HomePageNavBarSignUp"); }
        }
        public static string HomePageNavBarFormGroupPassword
        {
            get { return ResourceFinder.GetResource("HomePageNavBarFormGroupPassword"); }
        }
        public static string HomePageNavBarFormGroupEmail
        {
            get { return ResourceFinder.GetResource("HomePageNavBarFormGroupEmail"); }
        }
        public static string HomePageNavBarFormGroupSignIn
        {
            get { return ResourceFinder.GetResource("HomePageNavBarFormGroupSignIn"); }
        }
        public static string HomePageContentAboutProject
        {
            get { return ResourceFinder.GetResource("HomePageContentAboutProject"); }
        }
    }
}
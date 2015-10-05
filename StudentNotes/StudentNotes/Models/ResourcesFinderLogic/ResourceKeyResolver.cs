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
        public static string AccordionFeaturesTitle
        {
            get { return ResourceFinder.GetResource("AccordionFeaturesTitle"); }
        }
        public static string ConstPortal
        {
            get { return ResourceFinder.GetResource("ConstPortal"); }
        }
        public static string ConstStudentNotes
        {
            get { return ResourceFinder.GetResource("ConstStudentNotes"); }
        }
        public static string ConstComfortable
        {
            get { return ResourceFinder.GetResource("ConstComfortable"); }
        }
        public static string ConstEasy
        {
            get { return ResourceFinder.GetResource("ConstEasy"); }
        }
        public static string AccordionFeaturesContentHeader
        {
            get { return ResourceFinder.GetResource("AccordionFeaturesContentHeader"); }
        }
    }
}
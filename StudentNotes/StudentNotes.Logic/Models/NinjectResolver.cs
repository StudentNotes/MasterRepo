using Ninject;

namespace StudentNotes.Logic.Models
{
    public static class NinjectResolver
    {
        private static IKernel _kernel;

        public static IKernel GetKernel()
        {
            return _kernel ?? (_kernel = new StandardKernel());
        }

        public static T GetInstance<T>()
        {
            return _kernel.Get<T>();
        }

    }
}

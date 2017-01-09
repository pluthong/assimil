
using System.Web.Mvc;
namespace Assimil.Web
{
    public class NinjectConfig
    {
        public static void RegisterNinject()
        {
            ControllerBuilder.Current.SetControllerFactory( new NinjectControllerFactory());
        }
    }
}
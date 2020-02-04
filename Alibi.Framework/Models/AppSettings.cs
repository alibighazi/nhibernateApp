namespace Alibi.Framework.Models
{
    /*
     * The app settings class contains properties defined in the appsettings.
     * json file and is used for accessing application settings via objects that
     *  are injected into classes using the ASP.NET Core built in dependency injection (DI) system.
     */

    public class AppSettings
    {
        public string Secret { get; set; }
    }
}
namespace Common
{
    public class SettingsManager : ISettingsManager
    {
        public string GetSetting(string settingName)
        {
            return System.Configuration.ConfigurationManager.AppSettings[settingName];
        }
    }
}

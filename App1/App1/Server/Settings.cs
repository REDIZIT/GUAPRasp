using Newtonsoft.Json;
using Xamarin.Forms;

namespace App1
{
    public static class Settings
    {
        public static SettingsModel Model { get; private set; }

        public static void Load()
        {
            if (Application.Current.Properties.TryGetValue("settings", out object json))
            {
                Log.ShowAlert("Get json");
                Model = JsonConvert.DeserializeObject<SettingsModel>((string)json);
            }
            else
            {
                Log.ShowAlert("Get new");
                Model = new SettingsModel();
            }
        }
        public static void Save()
        {
            if (Application.Current.Properties.ContainsKey("settings") == false)
            {
                Application.Current.Properties.Add("settings", null);
            }
            Application.Current.Properties["settings"] = JsonConvert.SerializeObject(Model);
            Application.Current.SavePropertiesAsync();
        }
    }
}

using App1.Server;
using Newtonsoft.Json;
using Xamarin.Forms;
using static Android.Graphics.ColorSpace;

namespace App1
{
    public static class Settings
    {
        public static SettingsModel Model { get; private set; }

        public static void Load()
        {
            if (Application.Current.Properties.TryGetValue("settings", out object json))
            {
                Model = JsonConvert.DeserializeObject<SettingsModel>((string)json);

                Model.overrides.Clear();
                Model.overrides.Add(new SubjectOverride()
                {
                    FromRecord = new TimeTableRecord()
                    {
                        Week = Week.Bottom,
                        Day = Day.Saturday,
                        Order = 6,
                        Subject = Model.sortedRecords[Week.Bottom][Day.Saturday][6].Subject
                    },
                    ToRecord = new TimeTableRecord()
                    {
                        Week = Week.Top,
                        Day = Day.Wednesday,
                        Order = 1,
                        Subject = Model.sortedRecords[Week.Top][Day.Wednesday][1].Subject
                    }
                });

                Model.overrides.Add(new SubjectOverride(Week.Top, Day.Wednesday, 1, Week.Top, Day.Saturday, 4));
            }
            else
            {
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

using Newtonsoft.Json;
using Xamarin.Forms;

namespace App1.Server
{
    public class Subject
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Groups { get; set; }
        public string Teachers { get; set; }

        [JsonIgnore] public Color TypeColor
        {
            get
            {
                switch (Type)
                {
                    case "Лекция": return Color.Violet;
                    case "Практическая работа": return Color.Coral;
                    case "Лабораторная работа": return Color.DodgerBlue;
                    default: return Color.White;
                }
            }
        }
    }
}

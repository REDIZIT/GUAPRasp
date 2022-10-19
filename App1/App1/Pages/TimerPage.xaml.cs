using Android.App;
using App1.Extensions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimerPage : ContentPage
    {
        public ObservableCollection<Letter> WordChars { get; private set; }
        public ObservableCollection<Letter> Chars { get; private set; }

        private int index;
        private string word;
        private AlarmRecord alarm;
        private Activity activity;

        public TimerPage(int alarm, Activity activity)
        {
            InitializeComponent();

            this.alarm = AlarmManager.Instance.TryGetTimerByID(alarm);
            this.activity = activity;

            var assembly = Assembly.GetExecutingAssembly();
            var names = assembly.GetManifestResourceNames();

            using (Stream stream = assembly.GetManifestResourceStream("App1.WordsLib.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                word = reader.ReadToEnd().Split('\n').Random().Trim();
            }

            WordChars = new();
            int i = 0;
            foreach (var item in word)
            {
                WordChars.Add(new Letter(default, 32, i));
            }


            Chars = new();
            i = 0;
            foreach (var item in word.ToCharArray().Distinct().Shuffle())
            {
                Chars.Add(new Letter(item, 42, i));
            }

            BindingContext = this;
        }

        private void CharClicked(object sender, EventArgs e)
        {
            BindableObject b = (BindableObject)sender;
            Letter c = (Letter)b.BindingContext;

            WordChars[index].Char = c.Char;
            index++;

            if (index == word.Length)
            {
                bool isValid = true;
                for (int i = 0; i < WordChars.Count; i++)
                {
                    if (WordChars[i].Char != word[i])
                    {
                        isValid = false;
                        break;
                    }
                }

                if (isValid)
                {
                    DisarmClicked(null, null);
                }
                else
                {
                    for (int i = 0; i < WordChars.Count; i++)
                    {
                        WordChars[i].Char = default;
                    }
                    index = 0;
                }
            }
        }

        private void SkipClicked(object sender, EventArgs e)
        {
            alarm.Skip();
            activity.Finish();
        }
        private void DisarmClicked(object sender, EventArgs e)
        {
            alarm.Disarm();
            activity.Finish();
        }
    }
    public class Letter : INotifyPropertyChanged
    {
        public Color BackgroundColor => Char == default ? Color.FromHex("#444") : Color.FromHex("#666");
        public char Char
        {
            get { return _char; }
            set { _char = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Char))); }
        }
        public float Scale { get; set; }
        public float FontSize { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }

        private char _char;

        public event PropertyChangedEventHandler PropertyChanged;

        public Letter(char letter, float scale, int index)
        {
            Char = letter;
            Scale = scale;
            FontSize = Scale * 0.6f;

            Column = index % 6;
            Row = index / 6;
        }
    }
}
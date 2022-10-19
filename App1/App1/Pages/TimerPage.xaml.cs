using Android.App;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private string word = "хуууй";
        private AlarmRecord alarm;
        private Activity activity;

        public TimerPage(int alarm, Activity activity)
        {
            InitializeComponent();

            this.alarm = AlarmManager.Instance.GetTimerByID(alarm);
            this.activity = activity;


            WordChars = new();
            foreach (var item in word)
            {
                WordChars.Add(new Letter(default, 32));
            }


            Chars = new();
            foreach (var item in word.ToCharArray().Distinct())
            {
                Chars.Add(new Letter(item, 42));
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

        private char _char;

        public event PropertyChangedEventHandler PropertyChanged;

        public Letter(char letter, float scale)
        {
            Char = letter;
            Scale = scale;
            FontSize = Scale * 0.6f;
        }
    }
}
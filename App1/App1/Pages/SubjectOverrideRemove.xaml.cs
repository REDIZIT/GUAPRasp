using System;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubjectOverrideRemove : ContentView
    {
        public SubjectOverride Override { get; private set; }
        public string FromAddress { get; private set; }
        public bool IsValid { get; private set; }
        public bool IsInvalid => IsValid == false;

        private Action onClicked;

        public SubjectOverrideRemove(SubjectOverride subjectOverride, Action onClicked)
        {
            Override = subjectOverride;
            this.onClicked = onClicked;

            FromAddress = GetPrettyTime(Override.FromRecord.Week, Override.FromRecord.Day, Override.FromRecord.Order);
            BindingContext = this;

            InitializeComponent();

            var occopiedRecord = TimeTable.GetRecords(Override.FromRecord.Week, Override.FromRecord.Day).FirstOrDefault(r => r.IsSameTime(Override.FromRecord));

            if (occopiedRecord != null)
            {
                toSubjectLabel.Text = occopiedRecord.Subject.Name;
            }

            IsValid = occopiedRecord == null;

            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(IsInvalid));
        }

        private string GetPrettyTime(Week week, Day day, int order)
        {
            StringBuilder b = new StringBuilder();

            b.Append(week == Week.Top ? "Верхняя неделя, " : "Нижняя неделя, ");
            b.Append(DayToString(day));
            b.Append(", ");
            b.Append(order);
            
            return b.ToString();
        }
        private string DayToString(Day day)
        {
            return day switch
            {
                Day.Monday => "понедельник",
                Day.Tuesday => "вторник",
                Day.Wednesday => "среда",
                Day.Thursday => "четверг",
                Day.Friday => "пятница",
                Day.Saturday => "суббота",
                Day.Sunday => "воскресенье",
                _ => day.ToString(),
            };
        }
        private void Button_Clicked(object sender, System.EventArgs e)
        {
            onClicked?.Invoke();
        }
    }
}
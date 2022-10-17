using App1.Server;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubjectMove : ContentView
    {
        public TimeTableRecord FromRecord { get; set; }

        public bool IsValid { get; private set; }
        public bool IsInvalid => IsValid == false;

        private TimeTable timeTable;
        private Action<SubjectOverride> onClicked;

        private Week toWeek;
        private Day toDay;
        private int toOrder;

        public SubjectMove(TimeTable timeTable, TimeTableRecord fromRecord, Action<SubjectOverride> onClicked)
        {
            FromRecord = fromRecord;
            this.timeTable = timeTable;
            this.onClicked = onClicked;

            BindingContext = this;

            InitializeComponent();
        }

        private void PickerChanged(object sender, System.EventArgs e)
        {
            toWeek = (Week)(weekPicker.SelectedIndex + 1);
            toDay = (Day)(dayPicker.SelectedIndex + 1);
            toOrder = orderPicker.SelectedIndex + 1;

            Subject toSubject = timeTable.GetRecords(toWeek, toDay).FirstOrDefault(r => r.Order == toOrder)?.Subject;

            if (toSubject != null)
            {
                toSubjectLabel.Text = toSubject.Name;
            }

            IsValid = toSubject == null;

            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(IsInvalid));
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            SubjectOverride subjectOverride = new SubjectOverride(
                FromRecord.Week, FromRecord.Day, FromRecord.Order,
                toWeek, toDay, toOrder);

            onClicked?.Invoke(subjectOverride);
        }
    }
}
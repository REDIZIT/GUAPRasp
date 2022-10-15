using App1.Server;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubjectMove : ContentView
    {
        public Subject FromSubject { get; set; }

        public bool IsValid { get; private set; }
        public bool IsInvalid => IsValid == false;

        public SubjectMove(Subject subject)
        {
            FromSubject = subject;

            BindingContext = this;

            InitializeComponent();
        }

        private void PickerChanged(object sender, System.EventArgs e)
        {
            Week week = (Week)(weekPicker.SelectedIndex + 1);
            Day day = (Day)(dayPicker.SelectedIndex + 1);
            int order = orderPicker.SelectedIndex + 1;

            Subject toSubject = TimeTable.GetRecords(week, day).FirstOrDefault(r => r.Order == order)?.Subject;

            if (toSubject != null)
            {
                toSubjectLabel.Text = toSubject.Name;
            }

            IsValid = toSubject == null;

            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(IsInvalid));
        }
    }
}
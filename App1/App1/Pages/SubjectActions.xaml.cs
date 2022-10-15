using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubjectActions : ContentView
    {
        private Action onClick;

        public SubjectActions(Action onClick)
        {
            InitializeComponent();

            this.onClick = onClick;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            onClick?.Invoke();
        }
    }
}
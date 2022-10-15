using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            InitializeComponent();
        }
        private async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                await Sheet.OpenSheet();
            }
            catch (Exception ex)
            {
                //ex.Log();
                throw;
            }
        }
    }
}
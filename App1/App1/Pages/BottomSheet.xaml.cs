using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BottomSheet : ContentView
    {
        #region Constructors & initialisation

        public BottomSheet()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            PanContainerRef.Content.TranslationY = SheetContent.Height + 60;
        }

        #endregion

        #region Properties

        public static BindableProperty SheetContentProperty = BindableProperty.Create(
            nameof(SheetContent),
            typeof(View),
            typeof(BottomSheet),
            defaultValue: default(View),
            defaultBindingMode: BindingMode.TwoWay);

        public View SheetContent
        {
            get { return (View)GetValue(SheetContentProperty); }
            set { SetValue(SheetContentProperty, value); OnPropertyChanged(); }
        }

        #endregion

        uint duration = 150;
        double openPosition = (DeviceInfo.Platform == DevicePlatform.Android) ? 20 : 60;
        double currentPosition = 0;

        public async void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Running)
            {
                currentPosition = e.TotalY;

                //PanContainerRef.Content.TranslationY = openPosition + e.TotalY;

                if (e.TotalY > 0)
                {
                    PanContainerRef.Content.TranslationY = openPosition + e.TotalY;
                }
                else
                {
                    PanContainerRef.Content.TranslationY = openPosition - Math.Sqrt(Math.Abs(e.TotalY));
                }
            }
            else if (e.StatusType == GestureStatus.Completed)
            {
                var threshold = SheetContent.Height * 0.25;

                if (currentPosition < threshold)
                {
                    await OpenSheet();
                }
                else
                {
                    await CloseSheet();
                }
            }
        }

        public async Task OpenSheet()
        {
            await Task.WhenAll
                 (
                     Backdrop.FadeTo(0.4, length: duration),
                     Sheet.TranslateTo(0, openPosition, length: duration, easing: Easing.SinIn)
                 );

            BottomSheetRef.InputTransparent = Backdrop.InputTransparent = false;
        }

        public async Task CloseSheet()
        {
            await Task.WhenAll
                (
                    Backdrop.FadeTo(0, length: duration),
                    PanContainerRef.Content.TranslateTo(x: 0, y: SheetContent.Height + 60, length: duration, easing: Easing.SinIn)
                );

            BottomSheetRef.InputTransparent = Backdrop.InputTransparent = true;
        }

        private async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            await CloseSheet();
        }
    }
}
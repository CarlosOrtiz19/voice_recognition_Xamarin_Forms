using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TP2V3
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private int click;
        public ISpeechToText speechToText;
        public string language;

        private string rememberLanguage;
        public MainPage()
        {
            InitializeComponent();
            speechToText = DependencyService.Get<ISpeechToText>();
            speechToText.OnSpeechResult += speechResultat;
            

            rememberLanguage = Preferences.Get("rememberLanguage", "FR");
            //changeLangue.BindingContext = rememberLanguage;
            changeLangue.Text = rememberLanguage;
            speechToText.SetLanguage(rememberLanguage);

        }

        async void Button_Clicked(object sender, System.EventArgs e)
        {
            if (0 == Interlocked.Exchange(ref click, 1))
            {
                await Navigation.PushModalAsync(new Resultat(), false);
                click = 0;
            }
        }

        public void speechResultat(object sender, string result)
        {
            
            resultatLabel.Text = result;
            if (result!=null)
            {
                microphoneButton.BackgroundColor = Color.White;
                Debug.WriteLine("soy el result: " + result);
            }
            
        }

        private void ImageButton_Clicked(object sender, System.EventArgs e)
        {
            speechToText.StartASR();
            microphoneButton.BackgroundColor = Color.Red;
        }

        private void Button_Clicked_1(object sender, System.EventArgs e)
        {
            laguage.IsVisible = true;
        }

        private void laguage_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                
                laguage.IsVisible = false;
                Preferences.Set(nameof(rememberLanguage), picker.Items[selectedIndex]);
                changeLangue.Text = picker.Items[selectedIndex];
                speechToText.SetLanguage(picker.Items[selectedIndex]);
                Debug.WriteLine(rememberLanguage);/// ojo
               
            }
        }
    }
}

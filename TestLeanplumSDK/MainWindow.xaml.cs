using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using TestLeanplumSDK.Leanplum;

namespace TestLeanplumSDK
{
    public partial class MainWindow : Window
    {
        Leanplum_ABTesting leanplumInstance;
        TrackData PurchaseAppleEvent, PurchaseOrangeEvent;

        public MainWindow()
        {
            InitializeComponent();

            leanplumInstance = Leanplum_ABTesting.Instance;
            leanplumInstance.SetApiVersion("1.0.6");
            leanplumInstance.SetAppIdForProductionMode(YourAppId, YourProductionId);
        }

        private void CreateTrackData() 
        {
            PurchaseAppleEvent = new TrackData("Purchase");
            PurchaseAppleEvent.SetOptionalArg("value", "30");

            PurchaseOrangeEvent = new TrackData("Purchase");
            PurchaseAppleEvent.SetOptionalArg("value", "50");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            leanplumInstance.SetUserid(userIdTextBox.Text);

            Dictionary<string, object> StartOptionalArguments = new Dictionary<string, object>();
            StartOptionalArguments.Add("systemName", "iPhone OS");
            StartOptionalArguments.Add("systemVersion", "6.0");
            StartOptionalArguments.Add("background", true);

            Dictionary<string, object> vars = leanplumInstance.Start(StartOptionalArguments);
            loginMegTextBlock.Text = (string)vars["PurchaseMessage"];

            TrackData trackEvent = new TrackData("Login");
            leanplumInstance.Track(trackEvent);
        }

        private void Purchase30_Click(object sender, RoutedEventArgs e)
        {
            leanplumInstance.Track(PurchaseAppleEvent);
        }

        private void Purchase50_Click(object sender, RoutedEventArgs e)
        {
            leanplumInstance.Track(PurchaseOrangeEvent);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            TrackData trackEvent = new TrackData("Logout");
            leanplumInstance.Track(trackEvent);
            leanplumInstance.Stop();
        }
    }
}

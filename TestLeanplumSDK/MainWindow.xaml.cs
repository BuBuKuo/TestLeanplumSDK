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
        Leanplum_ABTesting leanplum;

        public MainWindow()
        {
            InitializeComponent();
            leanplum = new Leanplum_ABTesting("1.0.6");
            leanplum.SetAppIdForProductionMode("Your App ID key", "Your Production key");
            
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            leanplum.SetUserid(userIdTextBox.Text);

            Dictionary<string, object> StartOptionalArguments = new Dictionary<string, object>();
            StartOptionalArguments.Add("systemName", "iPhone OS");
            StartOptionalArguments.Add("systemVersion", "6.0");
            StartOptionalArguments.Add("background", true);

            object vars = leanplum.Start(StartOptionalArguments);
            loginMegTextBlock.Text = GetVariantValue(vars, "PurchaseMessage");
            
            leanplum.Track("Logoin");
        }

        private void Purchase30_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> StartOptionalArguments = new Dictionary<string, object>();
            StartOptionalArguments.Add("value", "30");
            leanplum.Track("Purchase", StartOptionalArguments);
        }

        private void Purchase50_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> StartOptionalArguments = new Dictionary<string, object>();
            StartOptionalArguments.Add("value", "50");
            leanplum.Track("Purchase", StartOptionalArguments);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            leanplum.Track("Logout");
            leanplum.Stop();
        }

        private string GetVariantValue(object obj, string varsName) 
        {
            string[] varsArray = ((IEnumerable)obj).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
            string varsValue = "";
            foreach (string str in varsArray) 
            {
                int index = str.IndexOf(varsName);
                if (index > 0) 
                {
                    varsValue = str.Substring(varsName.Length +3 , str.Length - (varsName.Length+3) - 1);
                }
            }
            return varsValue;
        }
    }
}

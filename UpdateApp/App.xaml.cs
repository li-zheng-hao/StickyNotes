using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace UpdateApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected  override void OnStartup(StartupEventArgs e)
        {
            //var args= string.Join(" ",e.Args);
            //AdministratorUtil.RunAsAdmin(args);
            LangHelper.Instance.SetLang(Common.Lang.Language.Chinese);
            if (e.Args.Length != 3)
            {
                MessageBox.Show(LangHelper.Instance.Lang.StartUpArgsError);
                Environment.Exit(0);
            }
            HttpHelper.BaseUrl = UpdateApp.Properties.Resources.ServerUrl;
            int majorVersionNumber=Convert.ToInt32( e.Args[0]);
            int minorVersionNumber=Convert.ToInt32( e.Args[1]);
            int revisionNumebr=Convert.ToInt32( e.Args[2]);
            
            MainWindow window = new MainWindow();
            var vm = new MainWindowViewModel(window);
            vm.SetVersion(majorVersionNumber, minorVersionNumber, revisionNumebr);
            window.DataContext = vm;
            window.Show();
        }

    }
}

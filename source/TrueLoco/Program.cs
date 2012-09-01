using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrueLoco
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => ShowError(args.ExceptionObject as Exception);
            TaskScheduler.UnobservedTaskException += (s, e) => { e.SetObserved(); ShowError(e.Exception); };
            Application.ThreadException += (sender, args) => ShowError(args.Exception);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void ShowError(Exception exception)
        {
            MessageBox.Show(exception.Message, "True Local Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

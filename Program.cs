using System;
using System.Windows.Forms;

namespace DeforeStopDesktop
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new SplashForm());
        }
    }
}
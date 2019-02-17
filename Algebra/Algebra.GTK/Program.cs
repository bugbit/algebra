using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace Algebra.GTK
{
    class MainClass
    {
        [STAThread]
        public static void Main(string[] args)
        {
            SkiaForms.Gtk2.Init.Include();
            Gtk.Application.Init();
            Forms.Init();

            var app = new App();
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("Algebra");
            window.Show();

            Gtk.Application.Run();
        }
    }
}

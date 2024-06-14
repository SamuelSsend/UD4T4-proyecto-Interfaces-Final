using UD4T4.MVVM.Views;

namespace UD4T4
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new InicioSesionAlumno());
        }
    }
}

using Firebase.Database;
using Firebase.Database.Query;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Graphics;
using UD4T4.Models;
using UD4T4.Utilities;
using UD4T4.ViewModels;

namespace UD4T4.MVVM.Views;

/// <summary>
/// View que representa la página de inicio de sesión del usuario.
/// </summary>
public partial class InicioSesionAlumno : ContentPage
{
    UsuarioViewModel uVM = new UsuarioViewModel();
    bool modoProfesor = false;
    private string emailAdmin = "registrosfbangel@fb.es";
    private string passAdmin = "registrosfb872";
    public InicioSesionAlumno()
	{
        InitializeComponent();
        BindingContext = uVM;
        MainThread.BeginInvokeOnMainThread(new Action(async () =>
        {
            await (FirebaseConexion.obtenerTokenRegistro());
        }));
    }
    private readonly Color DarkGreenColor = Color.FromArgb("#006400");
    private readonly Color SteelBlueColor = Color.FromArgb("#4682B4");

    /// <summary>
    /// Metodo que inicia la conexion con Firebase para el login.
    /// </summary>

    //Texto que indica el modo seleccionado.
    private void OnModeButtonClicked(object sender, EventArgs e)
    {
        modoProfesor = !modoProfesor;

        if (modoProfesor)
        {
            lblModo.Text = "Sesion Profesor";
            this.BackgroundColor = DarkGreenColor; // Cambia este color al que desees
        }
        else
        {
            lblModo.Text = "Sesion Alumno";
            this.BackgroundColor = SteelBlueColor; // Cambia este color al que desees
        }
    }

    /// <summary>
    /// Evento que se ejecuta cuando se pulsa el botón de login. Se encarga de comprobar si el usuario y la contraseña son correctos y de abrir la página de menú principal.
    /// </summary>
    private async void OnLoginClicked (object sender, EventArgs e)
    {
        string hashPassword, actualUsername;
        var email = uVM.UserItem.Email;
        var password = uVM.UserItem.Password;
        bool userCorrect;

        //Si los campos de email y contraseña no están vacíos, se comprueba si el usuario y la contraseña son correctos.
        if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
        {
            hashPassword = Encriptado.GetSHA256(password);
            userCorrect = await CheckUserAndPass(email, hashPassword);

            if (userCorrect)
            {
                await FirebaseConexion.obtenerToken(email, hashPassword);

                actualUsername = await GetUsername(email);
                //Si el usuario es correcto, se abre la página de menú principal. Segun el modo seleccionado, se abre el menú principal de profesor o de alumno.
                if (!modoProfesor)
                {
                    await Navigation.PushAsync(new SesionIniciadaAlumno(actualUsername));
                } else
                {
                    await Navigation.PushAsync(new SesionIniciadaProfesor(actualUsername));
                }


            } else
            {
                await this.DisplayAlert("Error", "El usuario o la contraseña no son correctos.", "Vale");
            }

        } else
        {
            await this.DisplayAlert("Error", "Debe rellenar los campos de Usuario y Contraseña.", "Vale");
        }

    }

    /// <summary>
    /// Metodo que devuelve el nombre de usuario a partir del email.
    /// </summary>
    private async Task<string> GetUsername (string email)
    {
        var users = await FirebaseConexion.firebaseClient.Child("AlumnoUsers").OnceAsync<UsuarioDatos>();

        //Si se ha seleccionado el modo profesor, se buscan los usuarios de profesor en "ProfesorUsers" si no, se buscan en "AlumnoUsers".
        if (modoProfesor)
        {
            users = await FirebaseConexion.firebaseClient.Child("ProfesorUsers").OnceAsync<UsuarioDatos>();
        }

        var user = users.Where(u => u.Object.Email == email).FirstOrDefault();

        return user.Object.UserName;
    }

    //Si se pulsa el boton de registro, se abre la pagina de registro correspondiente a si se ha seleccionado el modo profesor o no.
    private void OnRegisterClicked(object sender, EventArgs e)
    {
        if (!modoProfesor)
        {
            //Modo alumno
            Navigation.PushAsync(new RegistroAlumno());
        } else
        {
            Navigation.PushAsync(new RegistroProfesor());
        }

    }

    /// <summary>
    /// Metodo que comprueba si el usuario y la contraseña son correctos.
    /// </summary>
    private async Task<bool> CheckUserAndPass(string email, string hashPassword)
    {
        // Realizar una consulta para verificar si el usuario ya existe

        var users = await FirebaseConexion.firebaseClient.Child("ProfesorUsers").OnceAsync<UsuarioDatos>();

        // Si no se ha seleccionado el modo profesor, se buscan los usuarios de alumnos.
        if (!modoProfesor)
        {
            users = await FirebaseConexion.firebaseClient.Child("AlumnoUsers").OnceAsync<UsuarioDatos>();
        } 

        // Devuelve si existe algun objeto
        return users.Any(u => u.Object.Email == email && u.Object.Password == hashPassword);

    }


}
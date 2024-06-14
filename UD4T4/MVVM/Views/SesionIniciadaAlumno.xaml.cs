using System.Runtime.CompilerServices;
using UD4T4.Models;
using UD4T4.Utilities;
using UD4T4.ViewModels;

namespace UD4T4.MVVM.Views;

public partial class SesionIniciadaAlumno : ContentPage
{
	private string currentUsername;
    RegistrarUsuarioViewModel RUVM = new RegistrarUsuarioViewModel();
    public SesionIniciadaAlumno(string username)
	{
		currentUsername = username;
        InitializeComponent();

        labelCabecera.Text = $"Sesion de {currentUsername}";
        InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        RUVM.Persona = await GetPersona(currentUsername);
        fotoURL.Source = RUVM.Persona.FotoUrl;
    }

    /// <summary>
    /// Se abre el gestor de dias de los alumnos para agregar actividades. (Digo agregar porque tengo el teclado en ingles y no puedo poner la tilde :( 
    /// </summary>
    private async void OnDiasClicked(object sender, EventArgs e)
	{

        await Navigation.PushAsync(new SeleccionDias(currentUsername));
    }

    /// <summary>
    /// Se abre la View para editar la información personal del alumno.
    /// </summary>
    private async void OnGestionarClicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new EditarAlumno(currentUsername));
    }

    private async Task<Persona> GetPersona(string userName)
    {
        // Realizar una consulta para verificar si el usuario ya existe
        var datosPersona = await FirebaseConexion.firebaseClient.Child("DatosPersona").OnceAsync<Persona>();

        // Devuelve si existe algun objeto
        var persona = datosPersona.FirstOrDefault(u => u.Object.UserName == userName);

        return persona.Object;

    }




}
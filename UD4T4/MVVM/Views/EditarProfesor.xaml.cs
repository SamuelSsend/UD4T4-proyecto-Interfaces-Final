using Firebase.Database;
using Firebase.Database.Query;
using UD4T4.Models;
using UD4T4.Utilities;
using UD4T4.ViewModels;

namespace UD4T4.MVVM.Views;

/// <summary>
/// View para la edición de la información de un profesor.
/// </summary>
public partial class EditarProfesor : ContentPage
{
	string currentUsername;
    RegistrarUsuarioViewModel RUVM = new RegistrarUsuarioViewModel();

    public EditarProfesor(string userName)
	{
		currentUsername = userName;
		InitializeComponent();
        InitializePageAsync();
    }

    /// Como no se puede utilizar el await en el constructor, se inicializa la página con los datos del usuario actual de manera asíncrona.
    private async void InitializePageAsync()
    {
        RUVM.Persona = await GetPersona(currentUsername);

        BindingContext = RUVM;
    }

    /// <summary>
    /// Evento que se ejecuta cuando se pulsa el botón de actualizar. Se encarga de actualizar la información del usuario en la base de datos.
    /// </summary>
    private async void OnActualizarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(RUVM.Persona.CentroEstudio))
        {
            RUVM.Persona.CentroEstudio = "No indicado";
        }
        //Si no se cumplen los campos obligatorios, se muestra un mensaje de error.

        if (!string.IsNullOrWhiteSpace(RUVM.Persona.Nombre) && !string.IsNullOrWhiteSpace(RUVM.Persona.Apellidos) && !string.IsNullOrWhiteSpace(RUVM.Persona.DNI)
             && !string.IsNullOrWhiteSpace(RUVM.Persona.CentroEstudio))
        {

            //Se actualiza la información del usuario en la base de datos.
            await FirebaseConexion.firebaseClient.Child("DatosProfesor").Child(RUVM.Persona.Key).DeleteAsync();

            await FirebaseConexion.firebaseClient.Child("DatosProfesor").Child(RUVM.Persona.Key).PutAsync(RUVM.Persona);


                await this.DisplayAlert("Confirmacion", "Informacion modificada con exito", "Okay");

                await Navigation.PopAsync();
            
        }
        else
        {
            await this.DisplayAlert("Error", "Revisa todos los campos", "Okay");
        }
    }

    /// <summary>
    /// Método que se encarga de obtener la información de un profesor mediante su nombre de usuario.
    /// </summary>
    private async Task<Persona> GetPersona(string userName)
    {
        // Realizar una consulta para verificar si el usuario ya existe
        var datosPersonas = await FirebaseConexion.firebaseClient.Child("DatosProfesor").OnceAsync<Persona>();

        // Devuelve si existe algun objeto
        var persona = datosPersonas.FirstOrDefault(u => u.Object.UserName == userName);

        return persona.Object;

    }
}
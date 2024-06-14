using Firebase.Database;
using Firebase.Database.Query;
using UD4T4.Models;
using UD4T4.ViewModels;
using UD4T4.Utilities;

namespace UD4T4.MVVM.Views;

/// <summary>
/// View que representa la edición de la información de un alumno guardada en DatosPersona.
/// </summary>
public partial class EditarAlumno : ContentPage
{

	string currentUsername;
    RegistrarUsuarioViewModel RUVM = new RegistrarUsuarioViewModel();


    public EditarAlumno(string userName)
	{
		currentUsername = userName;
		InitializeComponent();
        InitializePageAsync();
    }

    /// <summary>
    /// Se inicializa la página con los datos del usuario actual de manera asíncrona.
    /// </summary>
    private async void InitializePageAsync()
    {
        //Como no se puede utilizar el await en el constructor, se inicializa la página con los datos del usuario actual de manera asíncrona.
        RUVM.Persona = await GetPersona(currentUsername);

        BindingContext = RUVM;

        fotoURL.Source = RUVM.Persona.FotoUrl;
    }

    /// <summary>
    /// Método que se ejecuta cuando se pulsa el botón de actualizar. Se encarga de actualizar la información del usuario en la base de datos.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void OnActualizarClicked(object sender, EventArgs e)
    {
        bool profesorExist;

        //Si no se ha indicado el centro de estudios, se pone "No indicado".
        if (string.IsNullOrWhiteSpace(RUVM.Persona.CentroEstudio))
        {
            RUVM.Persona.CentroEstudio = "No indicado";
        }

        //Si no se cumplen los campos obligatorios, se muestra un mensaje de error.
        if (!string.IsNullOrWhiteSpace(RUVM.Persona.Nombre) && !string.IsNullOrWhiteSpace(RUVM.Persona.Apellidos) && !string.IsNullOrWhiteSpace(RUVM.Persona.DNI)
             && !string.IsNullOrWhiteSpace(RUVM.Persona.CentroEstudio) && !string.IsNullOrWhiteSpace(RUVM.Persona.NombreGrado)
              && !string.IsNullOrWhiteSpace(RUVM.Persona.ProfesorTutor) && !string.IsNullOrWhiteSpace(RUVM.Persona.CentroTrabajo) && !string.IsNullOrWhiteSpace(RUVM.Persona.TutorLaboral) && !string.IsNullOrWhiteSpace(RUVM.Persona.FotoUrl))
        {
            // Se comprueba si el profesor tutor indicado existe con el metodo CheckIfProfesorUserExists.
            profesorExist = await CheckIfProfesorUserExists(RUVM.Persona.ProfesorTutor);
            if (profesorExist)
            {
                // Se elimina el usuario actual de la base de datos y se añade el usuario actualizado.
                await FirebaseConexion.firebaseClient.Child("DatosPersona").Child(RUVM.Persona.Key).DeleteAsync();

                await FirebaseConexion.firebaseClient.Child("DatosPersona").Child(RUVM.Persona.Key).PutAsync(RUVM.Persona);


                await this.DisplayAlert("Confirmacion", "Informacion modificada con exito", "Okay");

                // Se vuelve a la página anterior.
                await Navigation.PopAsync();
            } else
            {
                await this.DisplayAlert("Error", "El usuario del profesor tutor indicado no existe", "Okay");
            }
            
        }
        else
        {
            await this.DisplayAlert("Error", "Revisa todos los campos", "Okay");
        }
    }

    /// <summary>
    /// Metodo que comprueba si el usuario profesor existe.
    /// </summary>
    /// <param name="userName">
    /// El nombre de usuario del profesor a comprobar.
    /// </param>
    /// <returns>Devuelve si usuario profesor existe o no.</returns>
    private async Task<bool> CheckIfProfesorUserExists(string userName)
    {
        // Realizar una consulta para verificar si el usuario ya existe
        var users = await FirebaseConexion.firebaseClient.Child("ProfesorUsers").OnceAsync<UsuarioDatos>();

        // Devuelve si existe algun objeto
        return users.Any(u => u.Object.UserName == userName);

    }

    /// <summary>
    /// Metodo que obtiene la información del usuario que se va a editar.
    /// </summary>
    /// <param name="userName">
    /// El nombre de usuario del usuario de la informacion a editar.
    /// </param>
    /// <returns></returns>
    private async Task<Persona> GetPersona(string userName)
    {
        // Realizar una consulta para verificar si el usuario ya existe
        var datosPersonas = await FirebaseConexion.firebaseClient.Child("DatosPersona").OnceAsync<Persona>();

        // Devuelve si existe algun objeto
        var persona = datosPersonas.FirstOrDefault(u => u.Object.UserName == userName);

        return persona.Object;

    }

    /// <summary>
    /// Evento que se ejecuta cuando se pulsa el botón de subir imagen. Se encarga de subir una imagen a la base de datos.
    /// </summary>
    private async void OnSubirImagenClicked(object sender, EventArgs e)
    {
        var foto = await FirebaseConexion.storageUploadPhoto();
        RUVM.Persona.FotoUrl = foto.ToString();
        fotoURL.Source = foto.ToString();
    }
}
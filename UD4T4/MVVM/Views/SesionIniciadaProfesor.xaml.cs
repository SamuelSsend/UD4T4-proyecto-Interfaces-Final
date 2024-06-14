namespace UD4T4.MVVM.Views;

/// <summary>
/// Clase que representa la vista del men� principal del profesor.
/// </summary>
public partial class SesionIniciadaProfesor : ContentPage
{
    string currentUsername;
    public SesionIniciadaProfesor(string username)
    {
        currentUsername = username;
        InitializeComponent();
        //Cabecera de la p�gina
        labelCabecera.Text = $"Sesion de {currentUsername}";

    }

    /// <summary>
    /// Al pulsar el bot�n de "Ver d�as de los alumnos", se abre la vista de selecci�n de alumnos.
    /// </summary>
    private async void OnAlumnosClicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new SeleccionAlumno(currentUsername));
    }

    /// <summary>
    /// Al pulsar el bot�n de "Gestionar informaci�n", se abre la vista de edici�n de los datos persoales del profesor.
    /// </summary>
    private async void OnGestionarClicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new EditarProfesor(currentUsername));
    }
}
namespace UD4T4.MVVM.Views;

/// <summary>
/// Clase que representa la vista del menú principal del profesor.
/// </summary>
public partial class SesionIniciadaProfesor : ContentPage
{
    string currentUsername;
    public SesionIniciadaProfesor(string username)
    {
        currentUsername = username;
        InitializeComponent();
        //Cabecera de la página
        labelCabecera.Text = $"Sesion de {currentUsername}";

    }

    /// <summary>
    /// Al pulsar el botón de "Ver días de los alumnos", se abre la vista de selección de alumnos.
    /// </summary>
    private async void OnAlumnosClicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new SeleccionAlumno(currentUsername));
    }

    /// <summary>
    /// Al pulsar el botón de "Gestionar información", se abre la vista de edición de los datos persoales del profesor.
    /// </summary>
    private async void OnGestionarClicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new EditarProfesor(currentUsername));
    }
}
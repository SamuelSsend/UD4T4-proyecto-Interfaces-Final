using Firebase.Database;
using System.Collections.ObjectModel;
using UD4T4.Models;
using UD4T4.MVVM.Models;
using UD4T4.MVVM.ViewModels;
using UD4T4.Utilities;

namespace UD4T4.MVVM.Views;

/// <summary>
/// Clase que representa la vista de la visualización de los días de un alumno.
/// </summary>
public partial class VerDiasAlumno : ContentPage
{
    VerDiasViewModel VDVM;
    public VerDiasAlumno(Persona persona)
	{
		InitializeComponent();
        VDVM = new VerDiasViewModel(persona);
		BindingContext = VDVM;
        InitializeAsync();
	}

    /// <summary>
    /// Metodo que inicializa la vista con los datos de los días del alumno seleccionado en asincrono, tambien cambia la cabecera para que muestre el nombre del alumno.
    /// </summary>
    private async void InitializeAsync()
    {
        alumnoCabecera.Text = alumnoCabecera.Text + " " + VDVM.PersonaActual.Nombre +" "+ VDVM.PersonaActual.Apellidos;
        VDVM.Dias = await GetAllDaysFromPersona();
    }


    /// <summary>
    /// Obtiene todos los días de la persona que se trabaja en el ViewModel.
    /// </summary>
    /// <returns>
    /// Una ObservableCollection de días de la persona.
    /// </returns>
    private async Task<ObservableCollection<Dia>> GetAllDaysFromPersona()
    {
        var dias = await FirebaseConexion.firebaseClient
            .Child("Days")
            .OnceAsync<Dia>();

        // Filtra los dias que contengan el usuario de la persona actual
        List<Dia> diasLista = dias
            .Where(diaSnapshot => diaSnapshot.Object.UserName == VDVM.PersonaActual.UserName)
            .Select(diaSnapshot => diaSnapshot.Object).OrderBy(dia => dia.Fecha)
            .ToList();

        // Convertir a ObservableCollection
        return new ObservableCollection<Dia>(diasLista);
    }

    /// <summary>
    /// Metodo que se ejecuta cuando se pulsa un día de la lista. Se encarga de abrir la vista de VerDiaView con el día seleccionado.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnDiaTapped(object sender, ItemTappedEventArgs e)
    {
        Dia diaSeleccionado = (Dia)e.Item;

       Navigation.PushAsync(new VerDiaProfesor(diaSeleccionado));
    }

    /// <summary>
    ///  Evento que se ejecuta cuando se pulsa el botón de generar PDF.
    /// </summary>
    private void OnGeneratePDFClicked(object sender, EventArgs e)
    {
        //Se va a la view que genera el PDF en base a los dias del alumno.
        Navigation.PushAsync(new PDF(VDVM.PersonaActual.UserName));
    }
}
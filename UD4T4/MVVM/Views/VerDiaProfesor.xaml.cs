using Firebase.Database;
using System.Collections.ObjectModel;
using UD4T4.MVVM.Models;
using UD4T4.MVVM.ViewModels;
using UD4T4.Utilities;

namespace UD4T4.MVVM.Views;

/// <summary>
/// Clase que representa la vista de la interfaz para la visualización de un día.
/// </summary>
public partial class VerDiaProfesor : ContentPage
{
	SelectorDiaViewModel SDVM = new SelectorDiaViewModel();
    public VerDiaProfesor(Dia diaElegido)
	{
		InitializeComponent();
		SDVM.Dia = diaElegido;
		BindingContext = SDVM;
        InitializationAsync();
    }

    /// <summary>
    /// Metodo que inicializa la vista con los datos del día seleccionado en asincrono.
    /// </summary>
    private async void InitializationAsync()
    {
        // Se actualiza la cabecera con la fecha del día con el formato corto del Datetime.
        cabeceraDia.Text = cabeceraDia.Text + " " + SDVM.Dia.Fecha.ToShortDateString();

        // Se obtienen las actividades del día seleccionado.
        SDVM.Actividades = await GetAllActivitiesFromDay(SDVM.Dia.Key);
    }

    /// <summary>
    /// Metodo que obtiene todas las actividades de un día.
    /// </summary>
    /// <param name="dayKey">
    ///  El identificador único del día que se va a buscar.
    /// </param>
    /// <returns></returns>
    private async Task<ObservableCollection<Trabajo>> GetAllActivitiesFromDay(string dayKey)
    {
        //Obtiene la lista de actividades de la base de datos del child "Actividades".
        var activities = await FirebaseConexion.firebaseClient
            .Child("Actividades")
            .OnceAsync<Trabajo>();

        // Filtra las actividades por DiaKey
        List<Trabajo> actividadesLista = activities
            .Where(activitySnapshot => activitySnapshot.Object.DiaKey == dayKey)
            .Select(activitySnapshot => activitySnapshot.Object)
            .ToList();

        // Convertir a ObservableCollection
        return new ObservableCollection<Trabajo>(actividadesLista);
    }


}
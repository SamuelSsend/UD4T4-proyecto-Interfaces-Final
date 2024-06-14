using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.ObjectModel;
using UD4T4.MVVM.Models;
using UD4T4.MVVM.ViewModels;
using UD4T4.Utilities;

namespace UD4T4.MVVM.Views;

/// <summary>
/// Clase parcial para la vista de selección de días.
/// </summary>
public partial class SeleccionDias : ContentPage
{
    SelectorDiaViewModel SDVM = new SelectorDiaViewModel();
    string currentUsername;
    VerDiasViewModel VDVM;

    /// <summary>
    /// Constructor de la clase SeleccionDias.
    /// </summary>
    /// <param name="userName">Nombre de usuario.</param>
    public SeleccionDias(string userName)
    {
        currentUsername = userName;
        InitializeComponent();
        BindingContext = SDVM;
        miDatePicker.Date = DateTime.Now;

        // Cargar todas las actividades al iniciar la vista
        LoadAllActivities();
    }

    /// <summary>
    /// Método para cargar todas las actividades al iniciar la vista.
    /// </summary>
    private async void LoadAllActivities()
    {
        SDVM.Actividades = await GetAllActivitiesFromUser(currentUsername);
    }

    /// <summary>
    /// Método para obtener todas las actividades de un usuario desde la base de datos.
    /// </summary>
    /// <param name="username">Nombre de usuario.</param>
    /// <returns>Colección de actividades.</returns>
    private async Task<ObservableCollection<Trabajo>> GetAllActivitiesFromUser(string username)
    {
        var activities = await FirebaseConexion.firebaseClient
            .Child("Actividades")
            .OnceAsync<Trabajo>();

        // Filtrar las actividades por el nombre de usuario
        var userActivities = activities
            .Where(a => a.Object.UserName == username)
            .Select(a => a.Object)
            .ToList();

        // Convertir la lista de actividades a ObservableCollection
        return new ObservableCollection<Trabajo>(userActivities);
    }

    /// <summary>
    /// Método invocado cuando se selecciona una fecha en el DatePicker.
    /// </summary>
    /// <param name="sender">Objeto que invoca el evento.</param>
    /// <param name="e">Argumentos del evento.</param>
    private async void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        bool existsDay = await CheckIfDayExistsOnDatabase(SDVM.Dia.Fecha, currentUsername);

        if (existsDay)
        {
            SDVM.Dia = await GetDayFromDatabase(SDVM.Dia.Fecha, currentUsername);
            AñadirEditarBotton.Text = "Añadir/Editar";
        }
        else
        {
            SDVM.Dia = new Dia
            {
                Fecha = miDatePicker.Date,
                UserName = currentUsername,
            };

            AñadirEditarBotton.Text = "Añadir/Editar";
        }

        // Se actualiza la actividad actual con el key del día seleccionado.
        SDVM.ActividadActual.DiaKey = SDVM.Dia.Key;
    }

    /// <summary>
    /// Método para verificar si un día existe en la base de datos.
    /// </summary>
    /// <param name="fecha">Fecha a verificar.</param>
    /// <param name="userName">Nombre de usuario asociado al día.</param>
    /// <returns>True si el día existe, false si no.</returns>
    private async Task<bool> CheckIfDayExistsOnDatabase(DateTime fecha, string userName)
    {
        var users = await FirebaseConexion.firebaseClient.Child("Days").OnceAsync<Dia>();
        return users.Any(u => u.Object.UserName == userName && u.Object.Fecha == fecha);
    }

    /// <summary>
    /// Método para obtener un día desde la base de datos.
    /// </summary>
    /// <param name="fecha">Fecha del día a obtener.</param>
    /// <param name="userName">Nombre de usuario asociado al día.</param>
    /// <returns>Objeto de tipo Dia.</returns>
    private async Task<Dia> GetDayFromDatabase(DateTime fecha, string userName)
    {
        var days = await FirebaseConexion.firebaseClient.Child("Days").OnceAsync<Dia>();
        var dia = days.FirstOrDefault(d => d.Object.UserName == userName && d.Object.Fecha == fecha);
        return dia.Object;
    }

    public async Task<bool> ActivityExistOnDay(string key, string dayKey)
    {
        var activities = await FirebaseConexion.firebaseClient.Child("Actividades").OnceAsync<Trabajo>();
        return activities.Any(d => d.Object.Key == key && d.Object.DiaKey == dayKey);
    }

    public async Task<Trabajo> GetActivity(string key, string dayKey)
    {
        var activities = await FirebaseConexion.firebaseClient.Child("Actividades").OnceAsync<Trabajo>();
        var activity = activities.FirstOrDefault(d => d.Object.Key == key && d.Object.DiaKey == dayKey);
        return activity.Object;
    }

    /// <summary>
    /// Método invocado cuando se hace clic en el botón de borrar.
    /// </summary>
    /// <param name="sender">Objeto que invoca el evento.</param>
    /// <param name="e">Argumentos del evento.</param>
    private async void OnBorrarClicked(object sender, EventArgs e)
    {
        if (await ActivityExistOnDay(SDVM.ActividadActual.Key, SDVM.ActividadActual.DiaKey))
        {
            await FirebaseConexion.firebaseClient
                .Child("Actividades")
                .Child(SDVM.ActividadActual.Key)
                .DeleteAsync();

            await DisplayAlert("Confirmado", "Se ha borrado la actividad", "Okay");

            SDVM.Actividades = await GetAllActivitiesFromUser(currentUsername);

            SDVM.ActividadActual = new Trabajo
            {
                DiaKey = SDVM.Dia.Key,
            };
        }
        else
        {
            await DisplayAlert("Error", "La actividad no existe o aún no se ha introducido.", "Okay");
        }
    }

// <summary>
        /// Método invocado cuando se hace clic en el botón de añadir.
        /// </summary>
        /// <param name="sender">Objeto que invoca el evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private async void OnAgregarClicked(object sender, EventArgs e)
    {
        var fecha = SDVM.Dia.Fecha;

        var actividadDesarrollada = SDVM.ActividadActual.ActividadDesarrollada;
        var tiempoEmpleado = SDVM.ActividadActual.TiempoEmpleado;
        var observaciones = SDVM.ActividadActual.Observaciones;

        bool existsDay = await CheckIfDayExistsOnDatabase(fecha, currentUsername);

        if (!string.IsNullOrWhiteSpace(actividadDesarrollada))
        {
            if (string.IsNullOrWhiteSpace(observaciones))
            {
                SDVM.ActividadActual.Observaciones = "Sin Descripcion";
            }

            // Asignar el nombre de usuario a la actividad actual
            SDVM.ActividadActual.UserName = currentUsername;

            if (existsDay)
            {
                var existingDay = await GetDayFromDatabase(fecha, currentUsername);

                if (await ActivityExistOnDay(SDVM.ActividadActual.Key, SDVM.ActividadActual.DiaKey))
                {
                    await FirebaseConexion.firebaseClient
                        .Child("Actividades")
                        .Child(SDVM.ActividadActual.Key)
                        .DeleteAsync();

                    await FirebaseConexion.firebaseClient
                        .Child("Actividades")
                        .Child(SDVM.ActividadActual.Key)
                        .PutAsync(SDVM.ActividadActual);

                    await DisplayAlert("Confirmado", "Se ha actualizado la actividad", "Okay");
                }
                else
                {
                    await FirebaseConexion.firebaseClient
                        .Child("Actividades")
                        .Child(SDVM.ActividadActual.Key)
                        .PutAsync(SDVM.ActividadActual);

                    await DisplayAlert("Confirmado", "Se ha agregado la actividad", "Okay");
                }
            }
            else
            {
                await FirebaseConexion.firebaseClient.Child("Days").Child(SDVM.Dia.Key).PutAsync(SDVM.Dia);

                await FirebaseConexion.firebaseClient
                    .Child("Actividades")
                    .Child(SDVM.ActividadActual.Key)
                    .PutAsync(SDVM.ActividadActual);

                await DisplayAlert("Confirmado", "Se ha agregado la actividad", "Okay");

                AñadirEditarBotton.Text = "Añadir";
            }

            SDVM.Actividades = await GetAllActivitiesFromUser(currentUsername);

            SDVM.ActividadActual = new Trabajo
            {
                DiaKey = SDVM.Dia.Key,
            };
        }
        else
        {
            await DisplayAlert("Error", "Los campos no pueden estar en blanco.", "Okay");
        }
    }

    /// <summary>
    /// Método invocado cuando se toca una actividad en la lista.
    /// </summary>
    /// <param name="sender">Objeto que invoca el evento.</param>
    /// <param name="e">Argumentos del evento.</param>
    private void OnActividadTapped(object sender, ItemTappedEventArgs e)
    {
        Trabajo actividadSeleccionada = (Trabajo)e.Item;
        SDVM.ActividadActual = actividadSeleccionada;
    }
}

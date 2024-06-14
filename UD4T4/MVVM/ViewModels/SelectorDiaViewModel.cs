using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UD4T4.MVVM.Models;

namespace UD4T4.MVVM.ViewModels
{
    /// <summary>
    /// Clase que representa el ViewModel para la selección de un día en la interfaz.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class SelectorDiaViewModel
    {
        /// <summary>
        /// Día seleccionado en la interfaz.
        /// </summary>
        public Dia Dia { get; set; }

        /// <summary>
        /// Actividad actual seleccionada en la interfaz.
        /// </summary>
        public Trabajo ActividadActual { get; set; }

        /// <summary>
        /// Colección de actividades que se pueden seleccionar en la interfaz.
        /// </summary>
        public ObservableCollection<Trabajo> Actividades { get; set; }

        /// <summary>
        /// Constructor de la clase SelectorDiaViewModel.
        /// </summary>
        public SelectorDiaViewModel()
        {
            //Se inicializa el día, la actividad actual y la colección de actividades a cero.
            Dia = new Dia();
            ActividadActual = new Trabajo();
            Actividades = new ObservableCollection<Trabajo>();
        }
    }
}

using Firebase.Database;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UD4T4.Models;
using UD4T4.MVVM.Models;

namespace UD4T4.MVVM.ViewModels
{
    /// <summary>
    /// Clase que representa el ViewModel para la visualización de los días de un alumno.
    /// </summary>
    [AddINotifyPropertyChangedInterface]

    public class VerDiasViewModel
    {
        /// <summary>
        /// Persona actual seleccionada en la interfaz en la View de VerDias.
        /// </summary>
        public Persona PersonaActual { get; set; }

        /// <summary>
        /// Lista de días de la persona actual.
        /// </summary>

        public ObservableCollection<Dia> Dias { get; set; }

        /// <summary>
        /// Constructor de la clase VerDiasViewModel.
        /// </summary>
        /// <param name="persona">
        /// La persona para la que se van a visualizar los días.
        /// </param>
        public VerDiasViewModel(Persona persona)
        {
            Dias = new ObservableCollection<Dia>();
            PersonaActual = persona;
        }

    }
}

using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UD4T4.Models;

namespace UD4T4.MVVM.ViewModels
{
    /// <summary>
    /// Clase que representa el ViewModel para la selección de alumnos.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class SelectorAlumnosViewModel
    {
        /// <summary>
        /// Colección de alumnos que se pueden seleccionar en la interfaz.
        /// </summary>
        public ObservableCollection<Persona> AlumnosCollection { get; set; }
        
        /// <summary>
        /// Constructor de la clase SelectorAlumnosViewModel.
        /// </summary>
        public SelectorAlumnosViewModel()
        {
            //Se inicializa la colección de alumnos.
            AlumnosCollection = new ObservableCollection<Persona>();
        }
    }
}

using PropertyChanged;
using UD4T4.Models;

namespace UD4T4.ViewModels
{
    /// <summary>
    /// ViewModel para el registro de un usuario.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class RegistrarUsuarioViewModel
    {
        /// <summary>
        /// Usuario que se va a registrar.
        /// </summary>
        public UsuarioDatos UserItem { get; set; }
        /// <summary>
        /// Datos personales del usuario que se va a registrar.
        /// </summary>
        public Persona Persona { get; set; }

        /// <summary>
        /// Constructor de la clase RegisterUserViewModel.
        /// </summary>
        public RegistrarUsuarioViewModel()
        {
            //Se generan las instancias de los objetos UserItem y Persona, que se utilizarán para el registro de un usuario y guardar sus datos personales.
            UserItem = new UsuarioDatos();
            Persona = new Persona();
        }
    }
}
using PropertyChanged;
using UD4T4.Models;

namespace UD4T4.ViewModels
{
    /// <summary>
    /// Clase que representa el ViewModel para el inicio de sesion del usuario.
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class UsuarioViewModel
    {
        /// <summary>
        /// Usuario que se va a registrar o iniciar sesion.
        /// </summary>
        public UsuarioDatos UserItem { get; set; }

        /// <summary>
        /// Constructor de la clase UserViewModel.
        /// </summary>
        public UsuarioViewModel()
        {
            UserItem = new UsuarioDatos();
        }
    }
}
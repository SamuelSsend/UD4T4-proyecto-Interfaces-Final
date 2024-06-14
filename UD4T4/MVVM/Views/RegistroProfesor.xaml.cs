using Firebase.Database;
using Firebase.Database.Query;
using System.Text.RegularExpressions;
using UD4T4.Models;
using UD4T4.Utilities;
using UD4T4.ViewModels;

namespace UD4T4.MVVM.Views;

/// <summary>
/// Clase que representa la vista de registro de profesor.
/// </summary>
public partial class RegistroProfesor : ContentPage
{
    RegistrarUsuarioViewModel RUVM = new RegistrarUsuarioViewModel();
    public RegistroProfesor()
    {
        InitializeComponent();
        BindingContext = RUVM;
        MainThread.BeginInvokeOnMainThread(new Action(async () =>
        {
            await (FirebaseConexion.obtenerTokenRegistro());
        }));
    }


    /// <summary>
    /// Evento que se ejecuta cuando se pulsa el botón de registro. Se encarga de registrar un nuevo usuario en la base de datos.
    /// </summary>

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string hashPassword;
        //Booleans de las comprobaciones
        bool userExists, emailExists, validEmail;

        if (!string.IsNullOrWhiteSpace(RUVM.UserItem.UserName) && !string.IsNullOrWhiteSpace(RUVM.UserItem.Password) && !string.IsNullOrWhiteSpace(RUVM.UserItem.Email))
        {
            if (string.IsNullOrWhiteSpace(RUVM.Persona.CentroEstudio))
            {
                RUVM.Persona.CentroEstudio = "No indicado";
            }
            //Comprobaciones de si el usuario y el correo ya existen
            validEmail = CheckIfEmailValid(RUVM.UserItem.Email);
            userExists = await CheckIfUserExists(RUVM.UserItem.UserName);
            emailExists = await CheckIfEmailExists(RUVM.UserItem.Email);


            if (!string.IsNullOrWhiteSpace(RUVM.Persona.Nombre) && !string.IsNullOrWhiteSpace(RUVM.Persona.Apellidos) && !string.IsNullOrWhiteSpace(RUVM.Persona.DNI)
                 && !string.IsNullOrWhiteSpace(RUVM.Persona.CentroEstudio))
            {
                if (validEmail)
                {
                    if (!emailExists)
                    {
                        if (!userExists)
                        {  // Agregar el nuevo usuario si no existe
                            hashPassword = Encriptado.GetSHA256(RUVM.UserItem.Password);

                            //Envio la informacion a ProfesorUsers
                            await FirebaseConexion.fbAuthClient.CreateUserWithEmailAndPasswordAsync(RUVM.UserItem.Email, hashPassword);

                            await FirebaseConexion.firebaseClient.Child("ProfesorUsers").PostAsync(new UsuarioDatos
                            {
                                UserName = RUVM.UserItem.UserName,
                                //Se encripta la contraseña
                                Password = Encriptado.GetSHA256(RUVM.UserItem.Password),
                                Email = RUVM.UserItem.Email
                            });

                            //Vinculo los datos de la persona con el usuario
                            RUVM.Persona.UserName = RUVM.UserItem.UserName;

                            //Envio la informacion a DatosProfesor
                            await FirebaseConexion.firebaseClient.Child("DatosProfesor").Child(RUVM.Persona.Key).PutAsync(RUVM.Persona);

                            await this.DisplayAlert("Confirmado", "Usuario creado correctamente", "Okay");

                            FirebaseConexion.cerrarFirebase();

                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await this.DisplayAlert("Error", "El usuario ya existe.", "Vale");
                        }
                    }
                    else
                    {
                        await this.DisplayAlert("Error", "Ya existe un usuario con ese correo electronico.", "Vale");
                    }
                } else
                {
                    await this.DisplayAlert("Error", "El correo no tiene un formato correcto.", "Vale");
                }
                
            }
            else
            {
                await this.DisplayAlert("Error", "Revisa los campos obligatorios.", "Vale");
            }


        }
        else
        {
            await this.DisplayAlert("Error", "Debe rellenar los campos de Usuario y Contraseña.", "Vale");
        }
    }

    /// <summary>
    /// Se comprueba si el correo tiene un formato valido.
    /// </summary>
    private bool CheckIfEmailValid(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        return Regex.IsMatch(email, pattern);

    }
    private async Task<bool> CheckIfUserExists(string userName)
    {
        // Realizar una consulta para verificar si el usuario ya existe
        var users = await FirebaseConexion.firebaseClient.Child("ProfesorUsers").OnceAsync<UsuarioDatos>();

        // Devuelve si existe algun objeto
        return users.Any(u => u.Object.UserName == userName);

    }

    private async Task<bool> CheckIfEmailExists(string email)
    {
        // Realizar una consulta para verificar si el usuario ya existe
        var users = await FirebaseConexion.firebaseClient.Child("ProfesorUsers").OnceAsync<UsuarioDatos>();

        // Devuelve si existe algun objeto
        return users.Any(u => u.Object.Email == email);

    }
}
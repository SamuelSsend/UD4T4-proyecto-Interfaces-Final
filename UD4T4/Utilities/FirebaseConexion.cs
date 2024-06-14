using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UD4T4.Utilities
{
    /// <summary>
    /// Clase que representa la conexión con Firebase.
    /// </summary>
    /// 
    public static class FirebaseConexion
    {
        public static FirebaseClient firebaseClient = new FirebaseClient("https://interfaces-19399-default-rtdb.europe-west1.firebasedatabase.app/");

        private static string authDomain = "interfaces-19399.firebaseapp.com";

        private static string apiKey = "AIzaSyDcVhgT4O6I9qjNIx4lG6MD941uTbWfJL4";

        private static string token = string.Empty;

        //Para la subida de archivos a storage.
        private static string rutaStorage = "interfaces-19399.appspot.com";

        private static string emailAdmin = "samuelito@gmail.com";
        private static string passAdmin = "UD41234";


        public static FirebaseAuthClient fbAuthClient;
        public static async Task obtenerTokenRegistro()
        {
            fbAuthClient = new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = apiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            });
            var credenciales = await fbAuthClient.SignInWithEmailAndPasswordAsync(emailAdmin, passAdmin);
            token = await credenciales.User.GetIdTokenAsync();
        }

        public static async Task obtenerToken(string email, string password)
        {
            fbAuthClient = new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = apiKey,
                AuthDomain = authDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            });
            var credenciales = await fbAuthClient.SignInWithEmailAndPasswordAsync(email, password);
            token = await credenciales.User.GetIdTokenAsync();
        }

        public static void cerrarFirebase()
        {
            fbAuthClient = null;
            token = string.Empty;
        }

        public static async Task<string> storageUploadPhoto()
        {

            var foto = await MediaPicker.PickPhotoAsync();
            if (foto != null)
            {
                var task = new FirebaseStorage(
                    rutaStorage,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(token),
                        ThrowOnCancel = true
                    }
                )
                .Child("Imagenes")
                .Child(foto.FileName)
                .PutAsync(await foto.OpenReadAsync());

                var urlDescarga = await task;
                return urlDescarga;

            }
            else
            {
                return "";
            }
        }

    }

 
}

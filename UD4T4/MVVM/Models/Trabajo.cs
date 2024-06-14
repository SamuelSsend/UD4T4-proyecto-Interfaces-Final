using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UD4T4.MVVM.Models
{
    /// <summary>
    /// Clase actividad que representa una actividad desarrollada por el usuario
    /// </summary>
    public class Trabajo : ISerializable
    {
        /// <summary>
        /// Clave única para la actividad.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Clave del día asociado a la actividad.
        /// </summary>
        public string DiaKey { get; set; }

        /// <summary>
        /// Descripción de la actividad desarrollada.
        /// </summary>
        public string ActividadDesarrollada { get; set; }

        /// <summary>
        /// Tiempo empleado en la actividad (en horas).
        /// </summary>
        public double TiempoEmpleado { get; set; }

        /// <summary>
        /// Observaciones adicionales sobre la actividad.
        /// </summary>
        public string Observaciones { get; set; }

        /// <summary>
        /// Nombre del usuario asociado a la actividad.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Constructor de la clase Actividad.
        /// </summary>
        public Trabajo()
        {
            //Se genera un identificador único para la actividad.
            Key = Guid.NewGuid().ToString();
            //Se inicializa el tiempo empleado a 1 hora.
            TiempoEmpleado = 1;
        }

        /// <summary>
        /// Implementación de la interfaz ISerializable para serializar los datos.
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Key", Key);
            info.AddValue("DiaKey", DiaKey);
            info.AddValue("ActividadDesarrollada", ActividadDesarrollada);
            info.AddValue("TiempoEmpleado", TiempoEmpleado);
            info.AddValue("Observaciones", Observaciones);
            info.AddValue("UserName", UserName);
        }
    }
}

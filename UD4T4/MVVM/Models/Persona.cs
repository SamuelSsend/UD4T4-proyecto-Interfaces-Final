using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UD4T4.Models
{
    /// <summary>
    /// Clase que representa una persona que puede ser un alumno o un profesor.
    /// </summary>
    public class Persona : ISerializable
    {
        /// <summary>
        /// Identificador único de la persona.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Nombre de la persona.
        /// </summary>
        public string Nombre {  get; set; }
        /// <summary>
        /// Apellidos de la persona.
        /// </summary>
        public string Apellidos { get; set; }
        /// <summary>
        /// Numero de identificación de la persona.
        /// </summary>
        public string DNI { get; set; }
        /// <summary>
        /// Centro de estudio de la persona.
        /// </summary>
        public string CentroEstudio { get; set; }
        /// <summary>
        /// Profesor tutor de la persona.
        /// </summary>
        public string ProfesorTutor { get; set; }

        /// <summary>
        /// Nombre del grado que cursa el alumno.
        /// </summary>
        public string NombreGrado { get; set; }

        /// <summary>
        /// Centro de trabajo del alumno que va a realizar las prácticas.
        /// </summary>
        public string CentroTrabajo { get; set; }

        /// <summary>
        /// Tutor laboral del alumno que va a realizar las prácticas.
        /// </summary>
        public string TutorLaboral { get; set; }

        /// <summary>
        /// Nombre de usuario de la persona asociada.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Url de la foto de la persona.
        /// </summary>
        public string FotoUrl { get; set; }

        /// <summary>
        /// Constructor de la clase Persona.
        /// </summary>

        public Persona() {
            //Se genera un identificador único para la persona.
            Key = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Implementación de la interfaz ISerializable para serializar los datos.
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Key", Key);
            info.AddValue("Nombre", Nombre);
            info.AddValue("Apellidos", Apellidos);
            info.AddValue("DNI", DNI);
            info.AddValue("CentroEstudio", CentroEstudio);
            info.AddValue("ProfesorTutor", ProfesorTutor);
            info.AddValue("NombreGrado", NombreGrado);
            info.AddValue("CentroTrabajo", CentroTrabajo);
            info.AddValue("TutorLaboral", TutorLaboral);
            info.AddValue("UserName", UserName);
        }
    }
}


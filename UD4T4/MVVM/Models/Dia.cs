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
    /// Clase que representa un día en el que se han desarrollado actividades.
    /// </summary>
    public class Dia : ISerializable
    {
        /// <summary>
        /// Identificador único del día.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Fecha del día.
        /// </summary>
        public DateTime Fecha { get; set; }
        /// <summary>
        /// Usuario identificativo del día.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Constructor de la clase Dia.
        /// </summary>
        public Dia()
        {
            //Se genera un identificador único para el día.
            Key = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Metodo para serializar los datos
        /// </summary>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Key", Key);
            info.AddValue("Fecha", Fecha);
            info.AddValue("UserName", UserName);
        }
    }
}

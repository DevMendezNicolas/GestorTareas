using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorTareas.Models
{
    public class Tarea
    {
        // Atributos estáticos para llevar un contador de IDs de tareas
        public static int ContadorId { get; private set; } = 1000;
        // Propiedades de la clase Tarea
        public int Id { get; }
        public string Descripcion { get; set; }
        public bool Completada { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Método constructor para inicializar una nueva tarea
        public Tarea(string descripcion, bool completada)
        {
            ContadorId++;
            this.Id = ContadorId;
            this.Descripcion = descripcion;
            this.Completada = completada;
            this.FechaCreacion = DateTime.Now;
        }
        // Método ToString para representar la tarea en formato legible
        public override string ToString()
        {
            return $"{(this.Completada ? "Completada -> " : "Pendiente -> ")} Id: {this.Id} Fecha: {this.FechaCreacion:d} Descripción: {this.Descripcion}";
        }

        // Método estático para crear una nueva tarea a partir de la entrada del usuario
        public static Tarea CrearTarea()
        {
            Console.WriteLine("Ingrese la descripción de la tarea:");
            string descripcion = Console.ReadLine();
            if (string.IsNullOrEmpty(descripcion))
            {
                Console.WriteLine("La descripción de la tarea no puede estar vacía.");
                return null;
            }

            int opt;
            Console.WriteLine("¿La tarea está completada? (1: Sí, 0: No)");
            if (!int.TryParse(Console.ReadLine(), out opt) || (opt != 0 && opt != 1))
            {
                Console.WriteLine("Opción inválida. Por favor, ingrese 1 para completada o 0 para no completada.");
                return null;
            }
            bool estado = (opt == 1 ? true : false);

            Tarea nuevaTarea = new Tarea(descripcion, estado);
            return nuevaTarea;


        }
    }
}

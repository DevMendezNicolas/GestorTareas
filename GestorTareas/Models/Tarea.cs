using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorTareas.Utils;

namespace GestorTareas.Models
{
    public class Tarea
    {
        // Atributos estáticos para llevar un contador de IDs de tareas
        public static int ContadorId { get; private set; } = 1000;
        // Propiedades de la clase Tarea
        public int Id { get; }
        public string Descripcion { get; private set; }
        public bool Completada { get; private set; }
        public bool Eliminada { get; private set; }
        public Prioridad EstadoPrioridad { get; private set; }
        public DateTime FechaCreacion { get; private set; }
        public DateTime FechaCompletada { get; private set; }

        // Método constructor para inicializar una nueva tarea
        public Tarea(string descripcion, Prioridad prioridad)
        {
            ContadorId++;
            this.Id = ContadorId;
            this.Descripcion = descripcion;
            this.Completada = false;
            this.EstadoPrioridad = prioridad;
            this.Eliminada = false;
            this.FechaCreacion = DateTime.Now;
        }

        // Método para actualizar la descripción de la tarea
        public void actualizarDescripcion(string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                Console.WriteLine("La descripción no puede estar vacía.");
                return;
            }
            this.Descripcion = descripcion;
        }
        // Método para actualizar si esta completada  la tarea
        public void finalizarTarea()
        {
            if (this.Completada)
            {
                Console.WriteLine($"La tarea ya está marcada como completada.");
                return;
            }
            this.Completada = true;
            this.FechaCompletada = DateTime.Now;
        }

        // Método para eliminar la tarea
        public void eliminarTarea()
        {
            if (this.Eliminada)
            {
                Console.WriteLine($"La tarea ya está eliminada.");
                return;
            }
            this.Eliminada = true;
        }
        // Método para restaurar una tarea eliminada
        public void restaurarTarea()
        {
            if (!this.Eliminada)
            {
                Console.WriteLine($"La tarea no está eliminada.");
                return;
            }
            this.Eliminada = false;
        }
        // Método para actualizar la prioridad de la tarea
        public void actualizarPrioridad(Prioridad prioridad)
        {
            if (prioridad < Prioridad.baja || prioridad > Prioridad.alta)
            {
                Console.WriteLine("Prioridad no válida. Debe ser baja, media o alta.");
                return;
            }
            this.EstadoPrioridad = prioridad;
        }
        // Método para obtener el estado de la tarea
        public string obtenerEstado()
        {
            if (this.Eliminada)
            {
                return "Eliminada";
            }
            else if (this.Completada)
            {
                return "Completada";
            }
            else
            {
                return "Pendiente";
            }
        }


        // Método ToString para representar la tarea en formato legible
        public override string ToString()
        {
            string estado = obtenerEstado();
            string infoBase = $"ID: {this.Id} | Estado: {estado} | Prioridad: {this.EstadoPrioridad} | Fecha creación: {this.FechaCreacion:dd/MM/yyyy} | Descripción: {this.Descripcion}";

            if (Completada)
            {
                infoBase += $" | Finalizada: {this.FechaCompletada:dd/MM/yyyy}";
            }

            return infoBase;
        }
        // Método para comparar dos tareas por su ID
        public override bool Equals(object obj)
        {
            if (obj is Tarea otraTarea)
            {
                return this.Id == otraTarea.Id;
            }
            return false;
        }
        // Método para obtener el hash code de la tarea
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        
    }
}

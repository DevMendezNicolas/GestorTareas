using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Schema;
using GestorTareas.Data;
using GestorTareas.Models;
using GestorTareas.Utils;

namespace GestorTareas.Services
{
    public class GestorDeTareas
    {
        // Atributos privados para almacenar las tareas
        private List<Tarea> tareas;
        // Constructor que inicializa las listas de tareas
        public GestorDeTareas()
        {
            this.tareas = ArchivoTareas.Cargar();
        }
        // Obetner todas las tareas, las eliminadas y las completadas
        public List<Tarea> ObtenerTareas()
        {
            return tareas;
        }
        // Método para agregar una nueva tarea
        public bool AgregarTarea(string descripcion, Prioridad prioridad)
        {
            if (string.IsNullOrEmpty(descripcion))
            {
                Console.WriteLine("La descripción de la tarea no puede estar vacía.");
                return false;
            }
            Tarea tarea = new Tarea(descripcion, prioridad);
            tareas.Add(tarea);
            ArchivoTareas.Guardar(tareas);
            return true;
        }
        // Métedos para listar las tareas, completadas y eliminadas
        public void ListarTareas(List<Tarea> tareas, int tipoTarea)
        {
            if (tareas == null || tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas para mostrar.");
                return;
            }
            // |0-> Pendientes |1-> Completadas |2-> Todas |3-> Eliminadas
            if (tipoTarea < 0 || tipoTarea > 3)
            {
                Console.WriteLine("Tipo de tarea no válido. Por favor, ingrese un número entre 0 y 3.");
                return;
            }
            List<Tarea> tareasFiltradas;
            string titulo;

            switch (tipoTarea)
            {
                case 0:
                    titulo = "Lista de Tareas Pendientes:";
                    tareasFiltradas = tareas.Where(t => !t.Completada).ToList();
                    break;
                case 1:
                    titulo = "Lista de Tareas Completadas:";
                    tareasFiltradas = tareas.Where(t => t.Completada).ToList();
                    break;
                case 2:
                    titulo = "Lista de completa de Tareas:";
                    tareasFiltradas = tareas.Where(t => !t.Eliminada).ToList();
                    break;
                case 3:
                    titulo = "Lista de Tareas eliminadas:";
                    tareasFiltradas = tareas.Where(t => t.Eliminada).ToList();
                    break;
                default:
                    Console.WriteLine("Tipo de tarea no válido. Por favor, ingrese un número entre 0 y 3.");
                    return;

            }
            if(tareasFiltradas == null || tareasFiltradas.Count == 0)
            { 
                Console.WriteLine("No hay tareas para mostrar.");
                return;
            }
            Console.WriteLine(titulo);
            foreach (var tarea in tareasFiltradas)
            {
                Console.WriteLine(tarea.ToString());
            }
        }
        // Método listar por orden prioridad alta a baja
        public void ListarTareasPorPrioridad()
        {
            if (tareas == null || tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas para mostrar.");
                return;
            }
            
            List<Tarea> tareasOrdenadas = tareas
                .Where(t => !t.Eliminada && !t.Completada)
                .OrderByDescending(t => t.EstadoPrioridad)
                .ThenBy(t => t.FechaCreacion)
                .ToList();

            if (tareasOrdenadas == null || tareasOrdenadas.Count == 0)
            {
                Console.WriteLine("No hay tareas pendientes para mostrar.");
                return;
            }

            Console.WriteLine("Lista de Tareas ordenadas por prioridad:");
            foreach (var tarea in tareasOrdenadas)
            {
                Console.WriteLine(tarea.ToString());
            }
        }
        // Métodos para marcar una tarea como completada o eliminarla
        public bool MarcarCompletada(int id)
        {
            if (id <= 1000)
            {
                Console.WriteLine("El ID de la tarea debe ser un número mayor a 1000.");
                return false;
            }
            Tarea tarea = tareas.FirstOrDefault(t => t.Id == id);
            if (tarea == null)
            {
                Console.WriteLine($"No se encontró la tarea con ID: {id}");
                return false;
            }
            if (tarea.Completada)
            {
                Console.WriteLine($"La tarea con ID: {id} ya está marcada como completada.");
                return false;
            }

            tarea.finalizarTarea();
            ArchivoTareas.Guardar(tareas);
            Console.WriteLine($"Tarea con ID: {id} marcada como completada.");
            return true;
        }
        public bool EliminarTarea(int id)
        {
            if (id <= 1000)
            {
                Console.WriteLine("El ID de la tarea debe ser un número mayor a 1000.");
                return false;
            }
            Tarea tarea = tareas.FirstOrDefault(t => t.Id == id);
            if (tarea == null)
            {
                Console.WriteLine($"No se encontró la tarea con ID: {id}");
                return false;
            }

            tarea.eliminarTarea();
            ArchivoTareas.Guardar(tareas);
            Console.WriteLine($"Tarea con ID: {id} eliminada.");
            return true;
        }
        // Método para buscar una tarea por ID
        public Tarea BuscarTareaPorId(int id)
        {
            if (id <= 1000)
            {
                Console.WriteLine("El ID de la tarea debe ser un número mayor a 1000.");
                return null;
            }

            if (tareas == null || tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas para buscar.");
                return null;
            }

            Tarea tarea = tareas.FirstOrDefault(t => t.Id == id);
            if (tarea == null)
            {
                Console.WriteLine($"No se encontró la tarea con ID: {id}");
                return null;
            }

            if(tarea.Eliminada)
            {
                Console.WriteLine($"La tarea con ID: {id} está eliminada.");
                return null;
            }
            Console.WriteLine($"Tarea encontrada: {tarea.ToString()}");
            return tarea;
        }
        // Método para restaurar una tarea eliminada
        public bool RestaurarTarea(int id)
        {
            if (id <= 1000)
            {
                Console.WriteLine("El ID de la tarea debe ser un número mayor a 1000.");
                return false;
            }
            //Tarea tarea = tareas.FirstOrDefault(t => t.Id == id);

            if (tareas.FirstOrDefault(t => t.Id == id) == null)
            {
                Console.WriteLine($"No se encontró la tarea eliminada con ID: {id}");
                return false;
            }
            if (!tareas.FirstOrDefault(t => t.Id == id).Eliminada)
            {
                Console.WriteLine("La tarea no está eliminada, no es necesario restaurarla.");
                return false;
            }

            tareas.FirstOrDefault(t => t.Id == id).restaurarTarea();
            ArchivoTareas.Guardar(tareas);
            Console.WriteLine($"Tarea con ID: {id} restaurada.");
            return true;
        }
        // Método para modificar una tarea existente
        public bool ModificarTarea(int id)
        {
            Tarea tarea = BuscarTareaPorId(id);
            if (tarea == null)
            {
                Console.WriteLine($"No se encontró la tarea con ID: {id}");
                return false;
            }
            if (tarea.Eliminada)
            {
                Console.WriteLine($"La tarea con ID: {id} está eliminada. No se puede modificar.");
                return false;
            }
            Console.WriteLine($"Tarea actual: {tarea.ToString()}");
            Console.Write("Ingrese la nueva descripción de la tarea: ");
            string nuevaDescripcion = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nuevaDescripcion))
            {
                Console.WriteLine("La descripción no puede estar vacía.");
                return false;
            }
            Console.Write("Ingrese la nueva prioridad de la tarea (baja, media, alta): ");
            Prioridad nuevaPrioridad;
            while (!Enum.TryParse(Console.ReadLine(), true, out nuevaPrioridad) || !Enum.IsDefined(typeof(Prioridad), nuevaPrioridad))
            {
                Console.WriteLine("Prioridad no válida. Debe ser baja, media o alta.");
                Console.Write("Ingrese la nueva prioridad de la tarea (baja, media, alta): ");
            }

            tarea.actualizarDescripcion(nuevaDescripcion);
            tarea.actualizarPrioridad(nuevaPrioridad);
            ArchivoTareas.Guardar(tareas);
            return true;
        }
    }
}

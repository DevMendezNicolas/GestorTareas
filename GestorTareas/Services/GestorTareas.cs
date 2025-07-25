using System;
using System.Collections.Generic;
using System.Linq;
using GestorTareas.Models;
using GestorTareas.Data;

namespace GestorTareas.Services
{
    public class GestorDeTareas
    {
        // Atributos privados para almacenar las tareas, eliminadas y completadas
        private List<Tarea> tareas;
        private List<Tarea> tareasEliminadas;
        private List<Tarea> tareasCompletadas;

        // Constructor que inicializa las listas de tareas, eliminadas y completadas
        public GestorDeTareas()
        {
            this.tareas = ArchivoTareas.Cargar();
            this.tareasEliminadas = ArchivoTareas.CargarEliminadas();
            this.tareasCompletadas = ArchivoTareas.CargarCompletadas();
        }
        // Obetner todas las tareas, las eliminadas y las completadas
        public List<Tarea> ObtenerTareas()
        {
            return tareas;
        }
        public List<Tarea> ObtenerTareasEliminadas()
        {
            return tareasEliminadas;
        }
        public List<Tarea> ObtenerTareasCompletadas()
        {
            return tareasCompletadas;
        }

        // Método para agregar una nueva tarea
        public bool AgregarTarea(string descripcion, bool estado)
        {
            if (string.IsNullOrEmpty(descripcion))
            {
                Console.WriteLine("La descripción de la tarea no puede estar vacía.");
                return false;
            }
            Tarea tarea = new Tarea(descripcion, estado);
            tareas.Add(tarea);
            ArchivoTareas.Guardar(tareas);
            if (estado)
            {
                tareasCompletadas.Add(tarea);
                ArchivoTareas.GuardarCompletadas(tareasCompletadas);
            }

            Console.WriteLine($"Tarea agregada con exito: {descripcion}");
            return true;
        }
        // Métedos para listar las tareas, completadas y eliminadas
        public void ListarTareas()
        {
            List<Tarea> Tareas = ObtenerTareas();
            if (Tareas.Count == 0 || Tareas == null)
            {
                Console.WriteLine("No hay tareas para mostrar.");
                return;
            }
            Console.WriteLine("Lista de Tareas:");
            foreach (var tarea in Tareas)
            {
                Console.WriteLine(tarea.ToString());
            }
            return;
        }
        public void ListarTareasCompletadas()
        {
            Console.WriteLine("Lista de Tareas Completadas:");
            List<Tarea> TareasCompletadas = ObtenerTareasCompletadas();
            if (TareasCompletadas == null || TareasCompletadas.Count == 0)
            {
                Console.WriteLine("No hay tareas completadas para mostrar.");
                return;
            }
            if (TareasCompletadas.Count == 0)
            {
                Console.WriteLine("No hay tareas completadas.");
                return ;
            }
            foreach (var tarea in TareasCompletadas)
            {
                Console.WriteLine(tarea.ToString());
            }
        }
        public void ListarTareasEliminadas()
        {
            Console.WriteLine("Lista de Tareas Eliminadas:");
            List<Tarea> TareasEliminadas = ObtenerTareasEliminadas();
            if (TareasEliminadas == null || TareasEliminadas.Count == 0)
            {
                Console.WriteLine("No hay tareas eliminadas para mostrar.");
                return ;
            }

            foreach (var tarea in TareasEliminadas)
            {
                Console.WriteLine(tarea.ToString());
            }
        }

        // metodo para listar las tareas por estado
        public void ListarTareasPorEstado(bool estado)
        {
            if (tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas para mostrar.");
                return ;
            }
            Console.WriteLine($"Lista de Tareas -> {(estado ? "Completadas" : "Pendientes")}:");
            List<Tarea> TareasFiltatradas = tareas.Where(t => t.Completada == estado).ToList();

            if (TareasFiltatradas.Count == 0)
            {
                Console.WriteLine("No hay tareas que coincidan con el estado especificado.");
                return ;
            }

            foreach (var tarea in TareasFiltatradas)
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

            tarea.Completada = true;
            tareasCompletadas.Add(tarea);
            ArchivoTareas.GuardarCompletadas(tareasCompletadas);
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

            tareas.Remove(tarea);
            tareasEliminadas.Add(tarea);
            ArchivoTareas.GuardarEliminadas(tareasEliminadas);
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
            Tarea tarea = tareasEliminadas.FirstOrDefault(t => t.Id == id);
            if (tarea == null)
            {
                Console.WriteLine($"No se encontró la tarea eliminada con ID: {id}");
                return false;
            }
            
            tareas.Add(tarea);
            tareasEliminadas.Remove(tarea);
            if(tarea.Completada)
            {
                tareasCompletadas.Add(tarea);
                ArchivoTareas.GuardarCompletadas(tareasCompletadas);
            }

            ArchivoTareas.Guardar(tareas);
            ArchivoTareas.GuardarEliminadas(tareasEliminadas);

            Console.WriteLine($"Tarea con ID: {id} restaurada.");
            return true;
        }


    }
}

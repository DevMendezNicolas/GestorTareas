using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using GestorTareas.Models;
using System.Data;
using System.IO;

namespace GestorTareas.Data
{
    public static class ArchivoTareas
    {
        // Rutas de los archivos JSON donde se guardarán las tareas, eliminadas y completadas
        private static string rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "tareas.json");
        private static string rutaArchivoEliminadas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "tareasEliminadas.json");
        private static string rutaArchivoCompletadas = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "tareasCompletadas.json");

        // Métodos para guardar las tareas, eliminadas y completadas en archivos JSON
        public static void Guardar(List<Tarea> tareas)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(tareas, opciones);
            Directory.CreateDirectory(Path.GetDirectoryName(rutaArchivo));
            File.WriteAllText(rutaArchivo, json);
        }
        public static void GuardarEliminadas(List<Tarea> tareas)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(tareas, opciones);
            Directory.CreateDirectory(Path.GetDirectoryName(rutaArchivoEliminadas));
            File.WriteAllText(rutaArchivoEliminadas, json);
        }
        public static void GuardarCompletadas(List<Tarea> tareas)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(tareas, opciones);
            Directory.CreateDirectory(Path.GetDirectoryName(rutaArchivoCompletadas));
            File.WriteAllText(rutaArchivoCompletadas, json);
        }

        // Métodos para cargar las tareas, eliminadas y completadas desde archivos JSON
        public static List<Tarea> Cargar()
        {
            if (!File.Exists(rutaArchivo))
            {
                return new List<Tarea>();
            }
                
            string json = File.ReadAllText(rutaArchivo);
            return JsonSerializer.Deserialize<List<Tarea>>(json);
        }
        public static List<Tarea> CargarEliminadas()
        {
            if (!File.Exists(rutaArchivoEliminadas))
            {
                return new List<Tarea>();
            }

            string json = File.ReadAllText(rutaArchivoEliminadas);
            return JsonSerializer.Deserialize<List<Tarea>>(json);
        }

        public static List<Tarea> CargarCompletadas()
        {
            if (!File.Exists(rutaArchivoCompletadas))
            {
                return new List<Tarea>();
            }

            string json = File.ReadAllText(rutaArchivoCompletadas);
            return JsonSerializer.Deserialize<List<Tarea>>(json);
        }
    }
}

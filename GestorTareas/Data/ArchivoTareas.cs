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
        // Rutas de los archivos JSON donde se guardarán las tareas
        private static string rutaArchivo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "tareas.json");
        // Métodos para guardar las tareas en archivos JSON
        public static void Guardar(List<Tarea> tareas)
        {
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(tareas, opciones);
            Directory.CreateDirectory(Path.GetDirectoryName(rutaArchivo));
            File.WriteAllText(rutaArchivo, json);
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
    }
}

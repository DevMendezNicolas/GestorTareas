using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorTareas.Models;
using GestorTareas.Data;
using GestorTareas.Services;
using System.Text.Json;
using System.IO;
using System.Threading;
using GestorTareas.Utils;


namespace GestorTareas
{
    public class Program
    {
        // Método para mostrar el menú principal
        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("*===========================================*");
            Console.WriteLine("*           -> Gestor de Tareas <-          *");
            Console.WriteLine("*  1. Agregar Tarea                         *");
            Console.WriteLine("*  2. Listar Todas las Tareas               *");
            Console.WriteLine("*  3. Listar Tareas Completadas             *");
            Console.WriteLine("*  4. Listar Tareas Pendientes              *");
            Console.WriteLine("*  5. Listar Tareas Eliminadas              *");
            Console.WriteLine("*  6. Listar Tareas por Prioridad           *");
            Console.WriteLine("*  7. Marcar Tarea como Completada          *");
            Console.WriteLine("*  8. Buscar Tarea por Id                   *");
            Console.WriteLine("*  9. Modificar Tarea                       *");
            Console.WriteLine("* 10. Eliminar Tarea                        *");
            Console.WriteLine("* 11. Restaurar Tarea                       *");
            Console.WriteLine("* 12. Salir                                 *");
            Console.WriteLine("*===========================================*");
        }

        // Método para seleccionar una opción del menú
        static int seleccionarOpcion()
        {
            Console.Write("Seleccione una opción: -> ");
            int opcion;
            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("Por favor, ingrese un número válido.");
                return -1; // Opción inválida
            }
            return opcion;
        }
        // Método principal que ejecuta el gestor de tareas
        static void Main(string[] args)
        {
            GestorDeTareas gestor = new GestorDeTareas();
            bool salir = false;

            while (!salir)
            {
                Program.MostrarMenu();
                int opcion = Program.seleccionarOpcion();
                switch (opcion)
                {
                    case 1:
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Agregando una nueva tarea...            *");
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*===========================================*");
                        Console.Write(" --> Ingrese la descripción de la tarea: ");

                        string descripcion = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(descripcion))
                        {
                            Console.WriteLine("La descripción no puede estar vacía.");
                            continue;
                        }

                        Console.WriteLine(" -> Seleccione la prioridad de la tarea:");
                        Console.WriteLine("     1. Baja - 2. Media - 3. Alta");
                        int prioridad;
                        if (!int.TryParse(Console.ReadLine(), out prioridad) || prioridad < 1 || prioridad > 3)
                        {
                            Console.WriteLine("Prioridad inválida. Por favor, ingrese un número entre 1 y 3.");
                            continue;
                        }

                        if (gestor.AgregarTarea(descripcion, (Prioridad)prioridad))
                        {
                            Console.WriteLine("Tarea agregada exitosamente.");
                        }
                        else
                        {
                            Console.WriteLine("Error al agregar la tarea. Por favor, intente nuevamente.");
                        }
                            break;
                    case 2:
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Listando todas las tareas...            *");
                        Console.WriteLine("*===========================================*");
                        gestor.ListarTareas(gestor.ObtenerTareas(), 2);
                        break;
                    case 3:
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Listando tareas completadas...          *");
                        Console.WriteLine("*===========================================*");
                        gestor.ListarTareas(gestor.ObtenerTareas(), 1);
                        break;
                    case 4:
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Listando tareas pendientes...           *");
                        Console.WriteLine("*===========================================*");
                        gestor.ListarTareas(gestor.ObtenerTareas(), 0);
                        break;
                    case 5:
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Listando tareas eliminadas...           *");
                        Console.WriteLine("*===========================================*");
                        gestor.ListarTareas(gestor.ObtenerTareas(), 3);
                        break;
                    case 6:
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Listando tareas por Prioridad...        *");
                        Console.WriteLine("*===========================================*");
                        gestor.ListarTareasPorPrioridad();
                        break;
                    case 7:
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Marcar tarea como completada...         *");
                        Console.WriteLine("*===========================================*");
                        Console.Write("Ingrese el ID de la tarea a completar: ");
                        int idCompletar;
                        if (!int.TryParse(Console.ReadLine(), out idCompletar) || idCompletar <= 1000)
                        {
                            Console.WriteLine("El ID debe ser un número válido y mayor a 1000.");
                            continue;
                        }

                        if (gestor.BuscarTareaPorId(idCompletar) == null)
                        {
                            Console.WriteLine($"No se encontró una tarea con ID: {idCompletar}");
                            continue;
                        }

                        gestor.BuscarTareaPorId(idCompletar).finalizarTarea();
                        break;
                    case 8:
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Buscar tarea por ID...                  *");
                        Console.WriteLine("*===========================================*");
                        Console.Write("Ingrese el ID de la tarea a buscar: ");
                        int idBuscar;
                        if (!int.TryParse(Console.ReadLine(), out idBuscar) || idBuscar <= 1000)
                        {
                            Console.WriteLine("El ID debe ser un número válido y mayor a 1000.");
                            continue;
                        }

                        Tarea tareaEncontrada = gestor.BuscarTareaPorId(idBuscar);

                        if (tareaEncontrada == null)
                        {
                            Console.WriteLine($"No se encontró una tarea con ID: {idBuscar}");
                            continue;
                        }

                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Tarea encontrada:                       *");
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine(tareaEncontrada.ToString());


                        break;
                    case 9:
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Modificar tarea...                      *");
                        Console.WriteLine("*===========================================*");
                        Console.Write("Ingrese el ID de la tarea a modificar: ");
                        int idModificar;
                        if (!int.TryParse(Console.ReadLine(), out idModificar) || idModificar <= 1000)
                        {
                            Console.WriteLine("El ID debe ser un número válido y mayor a 1000.");
                            continue;
                        }

                        if (gestor.BuscarTareaPorId(idModificar) == null)
                        {         
                            Console.WriteLine($"No se encontró una tarea con ID: {idModificar}");
                            continue;
                        }
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Modificando tarea...                    *");
                        Console.WriteLine("*===========================================*");
                        if (gestor.ModificarTarea(idModificar))
                        {
                            Console.WriteLine("Tarea modificada exitosamente.");
                        }
                        else
                        {
                            Console.WriteLine("Error al modificar la tarea. Por favor, intente nuevamente.");
                        }
                        break;
                    case 10:
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Eliminar tarea...                       *");
                        Console.WriteLine("*===========================================*");
                        Console.Write("Ingrese el ID de la tarea a eliminar: ");
                        int idEliminar;
                        if (!int.TryParse(Console.ReadLine(), out idEliminar) || idEliminar <= 1000)
                        {
                            Console.WriteLine("El ID debe ser un número válido y mayor a 1000.");
                            continue;
                        }
                        if (gestor.BuscarTareaPorId(idEliminar) == null)
                        {
                            Console.WriteLine($"No se encontró una tarea con ID: {idEliminar}");
                            continue;
                        }
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Eliminando tarea...                     *");
                        Console.WriteLine("*===========================================*");
                        if (gestor.BuscarTareaPorId(idEliminar).Eliminada)
                        {
                            Console.WriteLine("La tarea ya está eliminada.");
                            continue;
                        }

                        gestor.BuscarTareaPorId(idEliminar).eliminarTarea();
                        Console.WriteLine("Tarea eliminada exitosamente.");
                        break;
                    case 11:
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Restaurar tarea eliminada...            *");
                        Console.WriteLine("*===========================================*");
                        Console.Write("Ingrese el ID de la tarea a restaurar: ");
                        int idRestaurar;
                        if (!int.TryParse(Console.ReadLine(), out idRestaurar) || idRestaurar <= 1000)
                        {
                            Console.WriteLine("El ID debe ser un número válido y mayor a 1000.");
                            continue;
                        }
                        if (gestor.BuscarTareaPorId(idRestaurar) == null)
                        {
                            Console.WriteLine($"No se encontró una tarea con ID: {idRestaurar}");
                            continue;
                        }
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Restaurando tarea...                    *");
                        Console.WriteLine("*===========================================*");
                        if (gestor.BuscarTareaPorId(idRestaurar).Eliminada)
                        {
                            gestor.RestaurarTarea(idRestaurar);
                            Console.WriteLine("Tarea restaurada exitosamente.");
                        }
                        else
                        {
                            Console.WriteLine("La tarea no está eliminada, no es necesario restaurarla.");
                        }
                        break;
                    case 12:
                        Console.WriteLine("*===========================================*");
                        Console.WriteLine("*   Saliendo del gestor de tareas...        *");
                        Console.WriteLine("*===========================================*");
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida, intente nuevamente.");
                        Thread.Sleep(1500);
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
            ArchivoTareas.Guardar(gestor.ObtenerTareas());
        }
    }
}

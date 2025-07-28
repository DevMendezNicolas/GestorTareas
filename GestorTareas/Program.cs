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
            Console.WriteLine("*===================================*");
            Console.WriteLine("*      -> Gestor de Tareas <-       *");
            Console.WriteLine("*  1. Agregar Tarea                 *");
            Console.WriteLine("*  2. Listar Tareas                 *");
            Console.WriteLine("*  3. Listar Tareas Completadas     *");
            Console.WriteLine("*  4. Listar Tareas Eliminadas      *");
            Console.WriteLine("*  5. Marcar Tarea como Completada  *");
            Console.WriteLine("*  6. Eliminar Tarea                *");
            Console.WriteLine("*  7. Restaurar Tarea eliminada     *");
            Console.WriteLine("*  8. Buscar tarea por Id           *");
            Console.WriteLine("*  9. Salir                         *");
            Console.WriteLine("*===================================*");
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
                        Console.WriteLine("*===================================*");
                        Console.WriteLine("*    Agregando una nueva tarea...   *");
                        Console.WriteLine("*===================================*");
                        Console.Write("Ingrese la descripción de la tarea: ");
                        string descripcion = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(descripcion))
                        {
                            Console.WriteLine("La descripción no puede estar vacía.");
                            continue;
                        }

                        int opt;
                        Console.WriteLine("¿La tarea está completada? (1: Sí, 0: No)");
                        if (!int.TryParse(Console.ReadLine(), out opt) || (opt != 0 && opt != 1))
                        {
                            Console.WriteLine("Opción inválida. Por favor, ingrese 1 para completada o 0 para no completada.");
                            continue;
                        }
                        bool estado = (opt == 1 ? true : false);

                        Console.WriteLine("Seleccione la prioridad de la tarea:");
                        Console.WriteLine("1. Baja - 2. Media - 3. Alta");
                        int prioridad;
                        if (!int.TryParse(Console.ReadLine(), out prioridad) || prioridad < 1 || prioridad > 3)
                        {
                            Console.WriteLine("Prioridad inválida. Por favor, ingrese un número entre 1 y 3.");
                            continue;
                        }
                        
                        gestor.AgregarTarea(descripcion, estado, (Prioridad)prioridad);
                        break;
                    case 2:
                        Console.WriteLine("*===================================*");
                        Console.WriteLine("*    Listando todas las tareas...   *");
                        Console.WriteLine("*===================================*");
                        gestor.ListarTareas();
                        break;
                    case 3:
                        Console.WriteLine("*===================================*");
                        Console.WriteLine("*   Listando tareas completadas...  *");
                        Console.WriteLine("*===================================*");
                        gestor.ListarTareasCompletadas();
                        break;
                    case 4:
                        Console.WriteLine("*===================================*");
                        Console.WriteLine("*   Listando tareas eliminadas...   *");
                        Console.WriteLine("*===================================*");
                        gestor.ListarTareasEliminadas();
                        break;
                    case 5:
                        Console.WriteLine("*===================================*");
                        Console.WriteLine("*   Marcar tarea como completada... *");
                        Console.WriteLine("*===================================*");
                        // verificar si realmente se desea marcar una tarea como completada
                        Console.WriteLine("¡Atención! Esta acción marcará una tarea como completada.");
                        Console.WriteLine("¿Está seguro de que desea continuar? (S/N)");
                        string inputCompletada = Console.ReadLine();
                        string completada = string.IsNullOrWhiteSpace(inputCompletada) ? "N" : inputCompletada.ToUpper();
                        if (completada != "S")
                        {
                            Console.WriteLine("Operación cancelada. No se marcará ninguna tarea como completada.");
                            Console.WriteLine("Presione cualquier tecla para continuar...");
                            Console.ReadKey();
                            continue;
                        }

                        Console.Write("Ingrese el ID de la tarea a marcar como completada: ");
                        int id;
                        if (!int.TryParse(Console.ReadLine(), out id) || id <= 1000)
                        {
                            Console.WriteLine("El ID de la tarea debe ser un número mayor a 1000.");
                            continue;
                        }
                        gestor.MarcarCompletada(id);
                        break;
                    case 6:
                        Console.WriteLine("*===================================*");
                        Console.WriteLine("*        Eliminar una tarea...      *");
                        Console.WriteLine("*===================================*");
                        Console.WriteLine("¡Atención! Esta acción eliminará la tarea de forma permanente.");
                        Console.WriteLine("¿Está seguro de que desea continuar? (S/N)");
                        string inputEliminar = Console.ReadLine();
                        string eliminacion = string.IsNullOrWhiteSpace(inputEliminar) ? "N" : inputEliminar.ToUpper();
                        if (eliminacion != "S")
                        {
                            Console.WriteLine("Operación cancelada. No se eliminará ninguna tarea.");
                            Console.WriteLine("Presione cualquier tecla para continuar...");
                            Console.ReadKey();
                            continue;
                        }

                        Console.WriteLine("Ingrese el ID de la tarea a eliminar (debe ser mayor a 1000):");

                        int idEliminar;
                        if (!int.TryParse(Console.ReadLine(), out idEliminar) || idEliminar <= 1000)
                        {
                            Console.WriteLine("El ID de la tarea debe ser un número mayor a 1000.");
                            continue;
                        }
                        gestor.EliminarTarea(idEliminar);
                        break;
                    case 7:
                        Console.WriteLine("*===================================*");
                        Console.WriteLine("*     Restaurar tarea eliminada     *");
                        Console.WriteLine("*===================================*");
                        Console.WriteLine("¡Atención! Esta acción restaurará una tarea eliminada.");
                        Console.WriteLine("¿Está seguro de que desea continuar? (S/N)");
                        string inputRestaurar = Console.ReadLine();
                        string restauracion = string.IsNullOrWhiteSpace(inputRestaurar) ? "N" : inputRestaurar.ToUpper();
                        if (restauracion != "S")
                        {
                            Console.WriteLine("Operación cancelada. No se restaurará ninguna tarea.");
                            Console.WriteLine("Presione cualquier tecla para continuar...");
                            Console.ReadKey();
                            continue;
                        }
                        Console.Write("Ingrese el ID de la tarea a restaurar: ");
                        int idRestaurar;
                        if (!int.TryParse(Console.ReadLine(), out idRestaurar) || idRestaurar <= 1000)
                        {
                            Console.WriteLine("El ID debe ser un número válido y mayor a 1000.");
                            continue;
                        }
                        gestor.RestaurarTarea(idRestaurar);
                        break;
                    case 8:
                        Console.WriteLine("*===================================*");
                        Console.WriteLine("*     Buscar tarea por ID           *");
                        Console.WriteLine("*===================================*");
                        Console.Write("Ingrese el ID de la tarea a buscar: ");
                        int idBuscar;
                        if (!int.TryParse(Console.ReadLine(), out idBuscar) || idBuscar <= 1000)
                        {
                            Console.WriteLine("El ID debe ser un número válido y mayor a 1000.");
                            continue;
                        }
                        Tarea tareaEncontrada = gestor.BuscarTareaPorId(idBuscar);
                        if (tareaEncontrada != null)
                        {
                            Console.WriteLine("Tarea encontrada:");
                            Console.WriteLine(tareaEncontrada.ToString());
                        }
                        else
                        {
                            Console.WriteLine($"No se encontró una tarea con ID: {idBuscar}");
                        }
                        break;
                    case 9:
                        Console.WriteLine("*===================================*");
                        Console.WriteLine("*  Saliendo del gestor de tareas... *");
                        Console.WriteLine("*===================================*");
                        salir = true;
                        Thread.Sleep(2500); 
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

        }
    }
}

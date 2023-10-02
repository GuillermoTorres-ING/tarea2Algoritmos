using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        int tam = 10000;
        int[] arr = Enumerable.Range(1, tam).ToArray();
        int target = new Random().Next(1, tam);

        // Estima el uso de memoria antes de crear el arreglo.
        long memoriaAntes = GC.GetTotalMemory(true);

        mezcla(arr); //Se desorganiza el algoritmo a calcular
        int[] arrCopy = (int[])arr.Clone();

        // Se hacce una calculo del uso de memoria después de crear el algoritmo
        long memoriaDespues = GC.GetTotalMemory(true);

        //Formulita para obtener una estimación del uso de memoria xddd
        long estimacionUsoMemoria = memoriaDespues - memoriaAntes;

        string masRapido = "Ninguno";

        var tasks = new Task[7]; // Añadimos espacio para el método de inserción.

        tasks[0] = Task.Run(() => MedirTiempo(() => busquedaSec(arr, target, ref masRapido), "Busqueda Secuencial"));
        tasks[1] = Task.Run(() => MedirTiempo(() => Burbuja(arrCopy, ref masRapido), "Bubble Sort"));
        tasks[2] = Task.Run(() => MedirTiempo(() => QuickSort(arr, 0, arr.Length - 1, ref masRapido), "Quick Sort"));
        tasks[3] = Task.Run(() => MedirTiempo(() => BusquedaBinaria(arrCopy, target, ref masRapido), "Búsqueda Binaria"));
        tasks[4] = Task.Run(() => MedirTiempo(() => Insercion(arrCopy, ref masRapido), "Inserción"));
        tasks[5] = Task.Run(() => { }); // Esto es para asegurar que se muestre el Bubble Sort.
        tasks[6] = Task.Run(() => { }); // Esto es para asegurar que se muestre el Inserción.

        Task.WhenAll(tasks).Wait();

        Console.WriteLine("\nLos tiempos son mostrados en Orden Ascendente... ");
        Console.WriteLine($"Estimación del uso de memoria: {estimacionUsoMemoria:F2} KB");
    }

    static void MedirTiempo(Action action, string nombre) // ejecuta la acción mientras mide el tiempo
    {
        var reloj = new Stopwatch();
        reloj.Start();
        action();
        reloj.Stop();
        Console.WriteLine($"Tiempo ({nombre}): {reloj.Elapsed.TotalMilliseconds} Milisegundos");
    }

    static void busquedaSec(int[] arr, int target, ref string resultado)
    {
        for (int i = 0; i < arr.Length; ++i)
        {
            if (arr[i] == target)
            {
                resultado = "Busqueda Secuencial";
                return;
            }
        }
    }

    static void Burbuja(int[] arr, ref string resultado)
    {
        int n = arr.Length;
        for (int i = 0; i < n - 1; ++i)
        {
            for (int j = 0; j < n - i - 1; ++j)
            {
                if (arr[j] > arr[j + 1])
                {
                    int temp = arr[j];
                    arr[j] = arr[j + 1];
                    arr[j + 1] = temp;
                }
            }
        }
        resultado = "Bubble Sort";
    }

    static void QuickSort(int[] arr, int low, int high, ref string resultado)
    {
        if (low < high)
        {
            int pivot = Partition(arr, low, high);
            QuickSort(arr, low, pivot - 1, ref resultado);
            QuickSort(arr, pivot + 1, high, ref resultado);
        }
        resultado = "Quick Sort";
    }

    static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];//Coge el elemento más a la derecha como pivote.
        int i = (low - 1);// Inicializa un índice que rastrea la posición del elemento más pequeño.
        for (int j = low; j <= high - 1; j++)// Recorre el arreglo desde el índice 'low' hasta 'high - 1'.
        {
            if (arr[j] < pivot)
            {
                i++;
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
        int temp1 = arr[i + 1];
        arr[i + 1] = arr[high];
        arr[high] = temp1;
        return (i + 1);
    }

    static void BusquedaBinaria(int[] arr, int target, ref string resultado)
    {
        int left = 0;
        int right = arr.Length - 1;
        while (left <= right)
        {
            int middle = left + (right - left) / 2;
            if (arr[middle] == target)
            {
                resultado = "Búsqueda Binaria";
                return;
            }
            if (arr[middle] < target)
                left = middle + 1;
            else
                right = middle - 1;
        }
    }

    static void Insercion(int[] arr, ref string resultado)
    {
        int n = arr.Length;
        for (int i = 1; i < n; ++i)
        {
            int key = arr[i];
            int j = i - 1;
            while (j >= 0 && arr[j] > key)
            {
                arr[j + 1] = arr[j];
                j = j - 1;
            }
            arr[j + 1] = key;
        }
        resultado = "Inserción";
    }

    static void mezcla<T>(T[] arr) //Mezcla aleatoriamente los elementos de un arreglo 
                                   // Fisher-Yates shuffle es un algoritmo para generar permutaciones aleatorias. Toma un tiempo proporcional al número total de elementos que se barajan y los baraja en su lugar.
                                   // El algoritmo intercambia el elemento en cada iteración al azar entre todos los índices no visitados restantes, incluido el propio elemento.
    {
        Random rand = new Random();
        int n = arr.Length;
        for (int i = 0; i < n; i++)
        {
            int j = i + rand.Next(n - i);
            T temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }
    }

        // lee la array desde el índice más alto hasta el más bajo
        // generar un número aleatorio `j` tal que `0 <= j <= i`
    // intercambiar el elemento actual con el índice generado aleatoriamente
}

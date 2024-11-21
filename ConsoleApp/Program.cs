// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;

class Program
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    const byte VK_SHIFT = 0x10; // Código para tecla Shift
    const uint KEYEVENTF_KEYUP = 0x0002;

    static void SimulateKeyPress()
    {
        while (true)
        {
            keybd_event(VK_SHIFT, 0, 0, UIntPtr.Zero); // Pressiona Shift
            keybd_event(VK_SHIFT, 0, KEYEVENTF_KEYUP, UIntPtr.Zero); // Solta Shift
            Thread.Sleep(30000); // Repetir a cada 30 segundos
        }
    }

    [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
    public static extern bool SetCursorPos(int X, int Y);

    static void SimulateMouseMovement()
    {
        int x = 0, y = 0;

        while (true)
        {
            x = x == 0 ? 1 : 0; // Alterna a posição
            y = y == 0 ? 1 : 0;
            SetCursorPos(x, y);
            Thread.Sleep(30000); // Move o mouse a cada 30 segundos
        }
    }

    // Importando as funções necessárias da API do Windows
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

    [FlagsAttribute]
    public enum EXECUTION_STATE : uint
    {
        ES_SYSTEM_REQUIRED = 0x00000001,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_CONTINUOUS = 0x80000000,
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Bem-vindo - Não deixarei seu computador descansar :P");

        bool continuar = true;
        do
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("---------------------\n");
            Console.WriteLine("1. Iniciar programa");
            Console.WriteLine("2. Sair do programa");
            Console.Write("\nEscolha uma opção: ");

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            string escolha = Console.ReadLine();
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            switch (escolha)
            {
                case "1":
                    IniciarPrograma();
                    break;
                case "2":
                    continuar = false;
                    break;
                default:
                    Console.WriteLine("\nOpção inválida. Por favor, escolha novamente.");
                    break;
            }

            // Verifica se a tecla "E" foi pressionada para encerrar o programa
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.E)
                {
                    continuar = false;
                }
            }

        } while (continuar);
    }

    static void IniciarPrograma()
    {
        // Impede que o sistema entre em modo de repouso ou desligue a tela
        SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED);

        //Simula movimentação do mouse
        //SimulateMouseMovement();

        //Simula o precionamento de teclas
        SimulateKeyPress();

        Console.WriteLine("\nMonitorando...\n");

        // Array com os caracteres do loading
        char[] loadingChars = { '|', '/', '-', '\\', '|' };

        // Tempo total para simular o loading
        int intervalo = 100; // 100 milissegundos

        // Loop para simular o loading
        while (!Console.KeyAvailable)
        {
            foreach (char c in loadingChars)
            {
                Console.Write("\r{0}", c);
                Thread.Sleep(intervalo);
            }
        }

        // Lê e descarta a tecla pressionada para limpar o buffer de entrada
        Console.ReadKey(true);

        Console.WriteLine("\n\n\nPrograma encerrado.!");

        // Restaura o comportamento padrão do sistema
        SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
    }
}

using System.Runtime.InteropServices;

class Program
{
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

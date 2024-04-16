using System.Runtime.InteropServices;

public class Program
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
        // Impede que o sistema entre em modo de repouso ou desligue a tela
        SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_SYSTEM_REQUIRED);

        // Mantém o aplicativo em execução para manter o sistema ativo
        Console.WriteLine("Pressione qualquer tecla para sair.");
        Console.ReadKey();

        // Restaura o comportamento padrão do sistema
        SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
    }
}


using System;
using System.IO;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using System.Windows.Forms;
using Log = Serilog.Log;
using Serilog.Sinks;

public class JsonFileSenderService
{
    private readonly string _endpoint;
    private readonly int _maxRetries;
    private readonly TimeSpan _delayBetweenRetries;

    public JsonFileSenderService(string endpoint, int maxRetries = 3, int delayBetweenRetriesInSeconds = 2)
    {
        _endpoint = endpoint;
        _maxRetries = maxRetries;
        _delayBetweenRetries = TimeSpan.FromSeconds(delayBetweenRetriesInSeconds);

        // Defina o caminho do log
        string logDirectory = @"C:\Logs\acg audit files\Logs"; // Caminho absoluto para os logs

        // Criar a pasta de logs se não existir
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        // Configurar o Serilog para usar o caminho absoluto
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(logDirectory, "log-.txt"), rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }

    public async Task SendJsonFileAsync(string filePath)
    {
        if (!IsInternetAvailable())
        {
            Log.Error("Erro: Sem conexão com a Internet.");
            throw new InvalidOperationException("Erro: Sem conexão com a Internet.");
        }

        if (!await IsEndpointAccessibleAsync())
        {
            Log.Error("Erro: Endpoint não acessível.");
            throw new InvalidOperationException("Erro: Endpoint não acessível.");
        }

        using (HttpClient client = new HttpClient())
        {
            string jsonContent = await File.ReadAllTextAsync(filePath);
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                Log.Error("Erro: O JSON gerado está vazio ou nulo.");
                throw new InvalidOperationException("Erro: O JSON gerado está vazio ou nulo.");
            }

            //Log.Information($"JSON a ser enviado: {jsonContent}");
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            for (int i = 0; i < _maxRetries; i++)
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync(_endpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Log.Information("Arquivo JSON enviado com sucesso!");
                        return; // Saia do método se o envio for bem-sucedido
                    }
                    else
                    {
                        Log.Warning($"Falha ao enviar o arquivo. Código de status: {response.StatusCode}");
                        throw new InvalidOperationException($"Falha ao enviar o arquivo. Código de status: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Log.Error(ex, "Erro ao enviar o arquivo JSON. Verifique se o firewall ou antivírus está bloqueando a requisição.");
                    throw new InvalidOperationException("Erro ao enviar o arquivo JSON. Verifique se o firewall ou antivírus está bloqueando a requisição.");
                }
                catch (DllNotFoundException ex)
                {
                    Log.Error(ex, "Uma DLL necessária não foi encontrada. Verifique se todas as dependências estão instaladas.");
                    throw new InvalidOperationException("Uma DLL necessária não foi encontrada. Verifique se todas as dependências estão instaladas.");
                }
                catch (SystemException ex)
                {
                    Log.Error(ex, "Erro no sistema operacional. Por favor, verifique o estado do sistema: {MensagemErro}", ex.Message);
                    throw new InvalidOperationException("Erro no sistema operacional. Por favor, verifique o estado do sistema.");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Erro inesperado ao enviar o arquivo JSON.");
                    throw new InvalidOperationException($"Erro inesperado: {ex.Message}");
                }

                // Espera antes de tentar novamente
                Log.Information($"Tentando novamente em {_delayBetweenRetries.TotalSeconds} segundos...");
                await Task.Delay(_delayBetweenRetries);
            }

            Log.Error("Erro: O envio do arquivo JSON falhou após várias tentativas.");
            throw new InvalidOperationException("Erro: O envio do arquivo JSON falhou após várias tentativas.");
        }
    }

    public async Task SendDeviceInfoAsync(string jsonFilePath)
    {
        if (!IsInternetAvailable())
        {
            Log.Error("Erro: Sem conexão com a Internet.");
            throw new InvalidOperationException("Erro: Sem conexão com a Internet.");
        }

        if (!await IsEndpointAccessibleAsync())
        {
            Log.Error("Erro: Endpoint não acessível.");
            throw new InvalidOperationException("Erro: Endpoint não acessível.");
        }

        using (HttpClient client = new HttpClient())
        {
            // Lê o conteúdo do arquivo JSON
            string jsonContent = await File
            .ReadAllTextAsync(jsonFilePath);

            // Cria o conteúdo da requisição
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Define o endpoint para onde o JSON será enviado
            string endpoint = "http://localhost:18194/api/v1.0/pub/inventario/device-info"; // Altere para o seu endpoint real

            for (int i = 0; i < _maxRetries; i++)
            {
                try
                {
                    // Envia a requisição POST
                    HttpResponseMessage response = await client.PostAsync(endpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Log.Information("JSON enviado com sucesso!");
                        return; // Sai do método se o envio for bem-sucedido
                    }
                    else
                    {
                        Log.Warning($"Falha ao enviar o JSON. Código de status: {response.StatusCode}");
                        throw new InvalidOperationException($"Falha ao enviar o JSON. Código de status: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Log.Error(ex, "Erro ao enviar o JSON. Verifique se o firewall ou antivírus está bloqueando a requisição.");
                    throw new InvalidOperationException("Erro ao enviar o JSON. Verifique se o firewall ou antivírus está bloqueando a requisição.");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Erro inesperado ao enviar o JSON.");
                    throw new InvalidOperationException($"Erro inesperado: {ex.Message}");
                }

                // Espera antes de tentar novamente
                Log.Information($"Tentando novamente em {_delayBetweenRetries.TotalSeconds} segundos...");
                await Task.Delay(_delayBetweenRetries);
            }

            Log.Error("Erro: O envio do JSON falhou após várias tentativas.");
            throw new InvalidOperationException("Erro: O envio do JSON falhou após várias tentativas.");
        }
    }

    private bool IsInternetAvailable()
    {
        try
        {
            using (var ping = new Ping())
            {
                var reply = ping.Send("8.8.8.8", 1000); // Google DNS
                return reply.Status == IPStatus.Success;
            }
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> IsEndpointAccessibleAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                var response = await client.GetAsync(_endpoint);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }

    private void ShowErrorMessage(string message)
    {
        MessageBox.Show(message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
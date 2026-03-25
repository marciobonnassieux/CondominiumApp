namespace Edifacil.Mobile.Services;

public static class ApiService
{
    public static HttpClient GetClient()
    {
        // Usando o IP fÃ­sico da sua mÃ¡quina (Wi-Fi) no lugar do 10.0.2.2 (que funciona APENAS em emuladores).
        // Isso permite que o seu celular real consiga traÃ§ar a rota atÃ© o computador pela rede.
        var baseAddress = "http://192.168.1.14:5114"; 

        var client = new HttpClient();
        client.BaseAddress = new Uri(baseAddress);
        
        // Define um limite de tempo curto (7 segundos) para barrar loops de loading eternos caso a rede caia.
        client.Timeout = TimeSpan.FromSeconds(7);
        
        return client;
    }
}


# ConfiguraÃ§Ã£o do Ambiente e InstalaÃ§Ã£o

O presente documento detalha os requisitos estruturais e o arsenal de comandos consolidados para preparar e executar o ecossistema pleno do **Edifacil Management System**.

## 1. Ferramental Requerido
O projeto foi forjado utilizando *Toolings* em versÃ£o Preview para obter mÃ¡ximo desempenho:
- **.NET 10 SDK** (ou superior).
- **SQL Server LocalDB** (Motor autÃ´nomo para Database sem necessidade de contÃªineres).
- **Android SDK** (API NÃ­vel 36) alvejando a compilaÃ§Ã£o do lado MÃ³vel (.NET MAUI).

## 2. IniciaÃ§Ã£o Silenciosa
Estes comandos resolvem as dependÃªncias sem intervenÃ§Ã£o humana:
```powershell
# Sincroniza e baixa todos os pacotes Nuget
dotnet restore EdifacilManagement.slnx

# InstalaÃ§Ã£o automÃ¡tica dos componentes Google Android vitais:
dotnet build -t:InstallAndroidDependencies -f net10.0-android "-p:AndroidSdkDirectory=$env:LOCALAPPDATA\Android\Sdk" "-p:AcceptAndroidSDKLicenses=True" src\Edifacil.Mobile\Edifacil.Mobile.csproj
```

## 3. Disparo SimultÃ¢neo (Runtime)
O ambiente possui Tasks nativas do VS Code, mas as linhas absolutas de terminal sÃ£o:

### A) API Backend e Database (Servidor de Retaguarda)
Inicia o `EdifacilDbContext`, orquestra a Seed inicial e sobe na porta universal via Kestrel:
```powershell
dotnet run --project src\Edifacil.Api\Edifacil.Api.csproj
```
*- Por padrÃ£o, os endpoints interativos podem ser invocados e analisados em `http://localhost:5114` pela interface do [Scalar].*

### B) CompilaÃ§Ã£o ao Vivo no Emulador Android (MAUI)
Engatilha a vigÃ­lia estrita dos arquivos XAML e envia atualizaÃ§Ãµes quentes de layout:
```powershell
dotnet watch --project src\Edifacil.Mobile/Edifacil.Mobile.csproj run -f net10.0-android
```

---

## 4. Testes Remotos em Aparelho FÃ­sico (Ex: Celulares Via Wi-Fi)
Caso queira compilar o `App` no seu celular nativo via **Android Debug Bridge (ADB)** utilizando a mesma rede Wifi, a arquitetura possui bypass explÃ­cito:
1. **Host**: Garantido que o `launchSettings.json` da API repousou em `http://0.0.0.0:5114`.
2. **Firewall**: As Inbound Rules locais do Windows para a porta 5114 estÃ£o desbloqueadas.
3. **App IP**: Dentro do arquivo raiz do App (`Edifacil.Mobile\Services\ApiService.cs`), modifique a URI referencial fixada de `10.0.2.2` para o **IPv4 principal** do seu roteador (ex: `192.168.1.14`) e execute o *run* conectando o aparelho no computador.


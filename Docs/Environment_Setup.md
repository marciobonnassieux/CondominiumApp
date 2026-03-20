# Configuração do Ambiente e Instalação

O presente documento detalha os requisitos estruturais e o arsenal de comandos consolidados para preparar e executar o ecossistema pleno do **Condominium Management System**.

## 1. Ferramental Requerido
O projeto foi forjado utilizando *Toolings* em versão Preview para obter máximo desempenho:
- **.NET 10 SDK** (ou superior).
- **SQL Server LocalDB** (Motor autônomo para Database sem necessidade de contêineres).
- **Android SDK** (API Nível 36) alvejando a compilação do lado Móvel (.NET MAUI).

## 2. Iniciação Silenciosa
Estes comandos resolvem as dependências sem intervenção humana:
```powershell
# Sincroniza e baixa todos os pacotes Nuget
dotnet restore CondominiumManagement.slnx

# Instalação automática dos componentes Google Android vitais:
dotnet build -t:InstallAndroidDependencies -f net10.0-android "-p:AndroidSdkDirectory=$env:LOCALAPPDATA\Android\Sdk" "-p:AcceptAndroidSDKLicenses=True" src\Condominium.Mobile\Condominium.Mobile.csproj
```

## 3. Disparo Simultâneo (Runtime)
O ambiente possui Tasks nativas do VS Code, mas as linhas absolutas de terminal são:

### A) API Backend e Database (Servidor de Retaguarda)
Inicia o `CondominiumDbContext`, orquestra a Seed inicial e sobe na porta universal via Kestrel:
```powershell
dotnet run --project src\Condominium.Api\Condominium.Api.csproj
```
*- Por padrão, os endpoints interativos podem ser invocados e analisados em `http://localhost:5114` pela interface do [Scalar].*

### B) Compilação ao Vivo no Emulador Android (MAUI)
Engatilha a vigília estrita dos arquivos XAML e envia atualizações quentes de layout:
```powershell
dotnet watch --project src\Condominium.Mobile/Condominium.Mobile.csproj run -f net10.0-android
```

---

## 4. Testes Remotos em Aparelho Físico (Ex: Celulares Via Wi-Fi)
Caso queira compilar o `App` no seu celular nativo via **Android Debug Bridge (ADB)** utilizando a mesma rede Wifi, a arquitetura possui bypass explícito:
1. **Host**: Garantido que o `launchSettings.json` da API repousou em `http://0.0.0.0:5114`.
2. **Firewall**: As Inbound Rules locais do Windows para a porta 5114 estão desbloqueadas.
3. **App IP**: Dentro do arquivo raiz do App (`Condominium.Mobile\Services\ApiService.cs`), modifique a URI referencial fixada de `10.0.2.2` para o **IPv4 principal** do seu roteador (ex: `192.168.1.14`) e execute o *run* conectando o aparelho no computador.

# Configuração do Ambiente e Instalação

Este documento detalha os requisitos e a lista de comandos consolidados para preparar, compilar e executar o ecossistema do **Condominium Management System**.

## 1. Preparação do Ambiente
O projeto exige as seguintes ferramentas instaladas:
- **.NET 10 SDK** (ou superior).
- Instância do **SQL Server LocalDB** (geralmente embarcado à instalação do Visual Studio).
- **Android SDK** (API Nível 36) para compilar o projeto móvel (.NET MAUI).
- **Emulador Android** (Ex: Pixel 9) devidamente configurado e acelerado.

## 2. Instalação e Restauração Inicial
Passos de instalação do Android SDK (se recém formatado), compilação base e extração das dependências para rodar com o emulador:

```powershell
# Extrair todas as referências do repositório inteiro (.slnx)
dotnet restore CondominiumManagement.slnx

# Instalar os requisitos de Android da API 36 de forma não interacional:
dotnet build -t:InstallAndroidDependencies -f net10.0-android "-p:AndroidSdkDirectory=$env:LOCALAPPDATA\Android\Sdk" "-p:AcceptAndroidSDKLicenses=True" src\Condominium.Mobile\Condominium.Mobile.csproj
```

## 3. Comandos de Execução
Todos estes comandos já estão orquestrados no mapa de Tasks do VS Code (`.vscode/tasks.json`). Se for usar via powershell puro:

### A) Iniciar Sistema Operacional Virtual (Android)
Este comando engatilha a abertura em background do seu emulador de preferência:
```powershell
& "$env:LOCALAPPDATA\Android\Sdk\emulator\emulator.exe" -avd Pixel_9
```

### B) Ligar a API Backend RESTful
Rodando de dentro de `Condominium/src/Condominium.Api`. A API executa a auto-população de sua estrutura relacional (Banco de Dados) antes de subir na porta `5114`:
```powershell
dotnet run
```
A sua interface interativa Swagger moderna habita na rota paralela à execução, `/scalar/v1`.

### C) Compilar o Aplicativo Nativo (Deploy Estático)
Comando que manda o projeto Mobile nativo direto para a tela do Android rodando via Debug:
```powershell
dotnet build -t:Run -f net10.0-android src/Condominium.Mobile/Condominium.Mobile.csproj
```

### D) Ativar Assistente de Edição Rápida (Hot Reload 🔥)
Recupera qualquer alteração de Layout do APP instantaneamente sob a interface (apenas suba a API com antecedência antes disso):
```powershell
dotnet watch --project src/Condominium.Mobile/Condominium.Mobile.csproj run -f net10.0-android
```

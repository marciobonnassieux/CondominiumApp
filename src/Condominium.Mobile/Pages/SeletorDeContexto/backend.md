⚙️ **Especificação Técnica: Backend & Lógica**
Caminho sugerido: `Pages/SeletorDeContexto/backend.md`

🧩 **Arquitetura da ViewModel (SeletorDeContextoViewModel)**
- **Propriedades Observáveis (`[ObservableProperty]`)**:
  - `ObservableCollection<ContextoUsuarioModel> ContextosDisponiveis`: Lista de condomínios e papéis (roles) aos quais o usuário tem acesso.
  - `bool IsBusy`: Indica que o login no contexto selecionado está em andamento.
- **Comandos (`[RelayCommand]`)**:
  - `CarregarContextosCommand()`: Idealmente os contextos já estarão injetados após o login (via token JWT ou response do login), mas caso precise fazer um fetch adicional `/api/usuario/contextos`, faremos aqui de forma leve.
  - `SelecionarContextoAsync(ContextoUsuarioModel contexto)`: Define o contexto global na aplicação e navega para a rota principal do App.
  - `SairDaContaAsync()`: Invalida os tokens, limpa dados de navegação/Preferences e redireciona para a `LoginPage`.
- **Navegação**:
  - `INavigationService` para root page swap. Trocar `App.MainPage` do Shell atual dependendo da role: Se for Síndico/Porteiro vai para `AppShellPortaria`, se Morador para `AppShellMorador`.
- **Lógica de Estado Global**:
  - Ao executar `SelecionarContextoAsync`, a ViewModel deve popular um Singleton (ex: `IAppEnvironment` ou `AppContextService`) com o `CondominioId` ativo e a `UserRole` ativa para que todos os Requests HTTP passem a usá-los, seja via Header customizado (`x-condominio-id`) ou URL params.

🌐 **Integração de Dados**
- **Services Necessários**:
  - `ISecureStorageService`:
    - `RemoveAll()` limpar todos os JWT armazenados na sessão ao clicar Sair.
  - `ISessionManager`:
    - Configurar a role corrente para injetar nos interceptadores de requisição HttpClient.
    - Se a API exigir, trocar um "Token de Autenticação Geral" por um "Token de Contexto Específico" invocando um Endpoint `/api/auth/assumir-contexto/{condominioId}`.

🔒 **Regras de Negócio e Segurança**
- **Autologin para Contexto Único**: Importante regra UX: Se após fazer o Login a API retornar apenas 1 único contexto (ex: o cara acabou de ser criado e só tem 1 apê e 1 condomínio), a ViewModel de Login NÃO DEVE passar por aqui. Ela deve auto-selecionar o contexto e pular (bypass) essa tela direto pro dashboard.
- **Logs de Auditoria**: Se aplicável, ao assumir um cargo de síndico/administrador via aplicativo, reportar para a API de logs.

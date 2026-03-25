⚙️ **Especificação Técnica: Backend & Lógica**
Caminho sugerido: `Pages/Login/backend.md`

🧩 **Arquitetura da ViewModel (LoginViewModel)**
- **Propriedades Observáveis (`[ObservableProperty]`)**:
  - `string Identificador`: Recebe o E-mail, User ID ou CPF do usuário.
  - `string Senha`: Recebe a senha digitada.
  - `bool IsPasswordHidden`: Controla a visibilidade do campo de senha (inicializado como `true`).
  - `bool IsBusy`: Indica que o processo de autenticação está ocorringo (desabilita botões e mostra loader).
- **Comandos (`[RelayCommand]`)**:
  - `LoginAsync()`: Executa validações locais e chama o serviço de autenticação.
  - `TogglePasswordVisibilityCommand()`: Alterna o valor de `IsPasswordHidden`.
  - `EsqueceuSenhaCommand()`: Navega para o fluxo de recuperação de senha.
  - `CriarContaCommand()`: Navega para a tela `CriacaoDeContaPage`.
- **Navegação**:
  - Requer `INavigationService` para redirecionamento pós-login (DashboardPortaria, DashboardMorador, ou SeletorDeContexto) e para os fluxos de recuperação/criação.
- **Lógica de Validação e Formatação**:
  - Antes de enviar à API, sanitizar o campo `Identificador` (remover espaços em branco, formatar CPF se for numérico de 11 dígitos, ou manter raw se for e-mail).
  - Validar se `Identificador` e `Senha` não estão vazios.

🌐 **Integração de Dados**
- **Services Necessários**:
  - `IAutenticacaoService`:
    - `Task<LoginResponseDto> AutenticarAsync(LoginRequestDto request)`: POST `/api/auth/login`. O DTO deve suportar o campo identificador genérico (Login) e Password.
  - `IContextoUsuarioService` ou `ISecureStorageService`:
    - Salvar o JWT (Token de Acesso) e Refresh Token (se houver) no cofre nativo (`SecureStorage.SetAsync("auth_token", jwt)`).
    - Salvar os dados básicos de perfil/claims extraídos do token.
  - `IDialogService`:
    - Para apresentar alertas como "Credenciais inválidas" ou "Erro de conexão".

🔒 **Regras de Negócio e Segurança**
- **Sessão e JWT**: Ao receber sucesso da API, salvar o header `Authorization` globalmente no `HttpClient` principal (`HttpHandler` / DelegatingHandler).
- **Tratamento de Exceções**: 
  - Erros 401 (Unauthorized): Exibir mensagem genérica ("Usuário ou senha inválidos") para evitar enumeration attacks.
  - Erros 403 (Forbidden): Caso o usuário esteja bloqueado pela administração.
  - Timeout: Informar para verificar a conexão de internet.
- **Roteamento Inteligente**: O backend (ou ViewModel, analisando os claims do JWT) deve direcionar o usuário para a StartPage correta. Se o JWT possuir múltiplos papéis (ex: Morador + Porteiro), rotear para a tela `SeletorDeContextoPage`.

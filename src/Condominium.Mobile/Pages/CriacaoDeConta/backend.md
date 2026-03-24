⚙️ **Especificação Técnica: Backend & Lógica**
Caminho sugerido: `Pages/CriacaoDeConta/backend.md`

🧩 **Arquitetura da ViewModel (CriacaoDeContaViewModel)**
- **Propriedades Observáveis (`[ObservableProperty]`)**:
  - `string Email`: E-mail inserido pelo usuário.
  - `string ConfirmarEmail`: E-mail de confirmação.
  - `string Senha`: Senha criada pelo usuário.
  - `string ConfirmarSenha`: Confirmação da senha.
  - `bool IsPasswordHidden`: Controla a visibilidade do texto no campo de senha (padrão: true).
  - `bool IsConfirmPasswordHidden`: Controla a visibilidade do texto no campo de confirmar senha (padrão: true).
  - `bool IsBusy`: Indica operação em andamento durante a requisição de salvamento.
- **Comandos (`[RelayCommand]`)**:
  - `TogglePasswordVisibilityCommand()`: Alterna o valor de `IsPasswordHidden`.
  - `ToggleConfirmPasswordVisibilityCommand()`: Alterna o valor de `IsConfirmPasswordHidden`.
  - `SalvarCommand()`: Executa as validações, exibe loading e chama o serviço de registro.
  - `VoltarCommand()`: Retorna para a tela anterior via `NavigationService`.
- **Navegação**:
  - Dependência de `INavigationService` para executar `GoBackAsync()` ou variação (fechar modal).
- **Lógica de Validação (Regras de Negócio Críticas)**:
  - O método `SalvarCommand()` deve executar as seguintes validações síncronas antes da chamada de rede:
    1. **E-mail Inválido**: Verificar formatação Regex de `Email` e `ConfirmarEmail`.
    2. **E-mails Incompatíveis**: `Email` deve ser igual a `ConfirmarEmail`.
    3. **Senhas Incompatíveis**: `Senha` deve ser igual a `ConfirmarSenha`.
    4. **Força da Senha**: Mínimo de 8 caracteres, contendo pelo menos 1 letra e 1 número (Usar expressão regular ou validação customizada).
  - Em caso de falha de validação, informar ao usuário via `DialogService` com mensagem clara e abortar operação.

🌐 **Integração de Dados**
- **Services Necessários**:
  - `IAutenticacaoService` (ou `IUsuarioService`):
    - `Task RegistrarContaAsync(RegistrarContaRequestDto request)`: Faz POST na rota apropriada de sign-up (ex: `/api/auth/register`).
  - `IDialogService`: Para exibir alertas de erro (ex: falhas de validação ou de rede).

🔒 **Regras de Negócio e Segurança**
- **Sensibilidade de Dados**: As senhas nunca devem ser logadas ou enviadas em plain-text localmente, a persistência deve acontecer estritamente usando HTTPS.
- **Tratamento de Exceções**: Se a API retornar erro HTTP 400 (ex: "Usuário já existe"), repassar a mensagem de erro específica ao usuário via `DialogService`. Se retornar 500, exibir erro genérico de forma amigável ("Tente novamente").

⚙️ **Especificação Técnica: Backend & Lógica**
Caminho sugerido: `Pages/VerificacaoOTP/backend.md`

🧩 **Arquitetura da ViewModel (VerificacaoOTPViewModel)**
- **Propriedades Observáveis (`[ObservableProperty]`)**:
  - `string CodigoDigitado`: Property agregada ou string de 6 caracteres representando o TOTP/OTP recebido. Pode ser segmentada em `Digito1`, `Digito2`...`Digito6` se o controle de UI assim exigir para facilitar os bindings via BindingContext independente.
  - `string EmailDestino`: Endereço de e-mail ao qual o código foi enviado para exibir na instrução.
  - `int SegundosRestantes`: Contador regressivo numérico.
  - `string TempoFormatado`: Derivada de SegundosRestantes (ex: "04:59").
  - `bool IsResendEnabled`: Habilitado quando a contagem zerar.
  - `bool IsVerificarEnabled`: Habilita apenas quando a prop. de código tiver Length == 6.
  - `bool IsBusy`: Indica processo de verificação online.
  - `bool HasError`: Flag que aciona feedback visual de erro na UI (border vermelha).
- **Comandos (`[RelayCommand]`)**:
  - `IniciarTimerCommand()`: Disparado no OnAppearing, seta a contagem (ex: 300) e loop de decremento por segundo.
  - `VerificarCodigoAsync()`: Envia o OTP à API para validação do Token de Acesso / Reset.
  - `ReenviarCodigoAsync()`: Chama API pra despachar um novo e-mail e restarta o Timer.
  - `VoltarCommand()`: Cancela operação e retorna à view anterior.
- **Navegação**:
  - Requer injeção de `INavigationService` tanto para cancelamento (`GoBack`) quanto para progresso (dependendo do fluxo originador - se for Setup irá pra tela de Senha Nova, se for Login vai pro Dashboard).

🌐 **Integração de Dados**
- **Services Necessários**:
  - `IAutenticacaoService` ou `IUsuarioMfaService`:
    - `Task<bool> ValidarOTPAsync(string codigo)`: Efetua requisição POST (ex: `/api/auth/validate-otp`).
    - `Task ReenviarOTPAsync()`: Requisição POST para `/api/auth/resend-otp` ou reenvio de confirmação de e-mail.

🔒 **Regras de Negócio e Segurança**
- **Timer Autônomo**: Deve usar `IDispatcherTimer` para gerenciar a interface no MainThread. 
- **Tratamento de Erros Client-side**: 
  - Limpar os dígitos (Varrer string code) e definir `HasError = true` mediante API Response = 400/Unauthorized (Código Incorreto).
  - Usar `IHapticFeedback` para vibrar o telefone caso o código esteja errado.
- **Bloqueio contra Brute-Force**: O endpoint deve possuir Rate Limit; a UI reflete isso mantendo o botão "Reenviar" desativado por minutos impostos.

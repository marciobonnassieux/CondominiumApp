🎨 **Especificação Técnica: Frontend & UI**
Caminho sugerido: `Pages/VerificacaoOTP/frontend.md`

🖼️ **Estrutura de Layout (VerificacaoOTPPage.xaml)**
- **Page Root**: Empregar um `ContentPage` com uma cor de Fundo pastel (System Light Blue/Gray tint).
- **Caixa de Diálogo Modal (Inner Card)**:
  - Usar um `Border` ou `Frame` branco (`#FFFFFF`) preenchendo 90% da largura, alinhado verticalmente ao centro para parecer um dialog, com `CornerRadius="16"`.
  - Margin superior para o Botão "Voltar" (Seta Esq) que vive fora ou no edge do Card.
- **Ícone e Instruções**:
  - Container circular de badge (Ex: Frame com `HeightRequest="60" WidthRequest="60" CornerRadius="30"`) contendo a imagem SVG Cadeado Destrancado.
  - Título principal (`TitleLabelStyle`, negrito, dark blue).
  - Label explicativo (Hint/Subtitle Style) contendo span para dar destaque (Bold) ao `EmailDestino`.
- **OTP Input (Bloco Crítico)**:
  - O visual esperado é 6 quadrados ("digit boxes").
  - Tecnologias aplicáveis: 
    - a) Componente de Terceiros (ex: `UraniumUI` OTPInput, ou plugin específico).
    - b) Hand-coded: 6 caixas `Border` dispostas em `HorizontalStackLayout`. Cada uma com um Entry (`MaxLength="1"`, `Keyboard="Numeric"`, fundo Transparente e text centrado). E programar `TextChanged` handler no code-behind (ou Behaviors) pro foco avançar em cadeia.
  - State Manager: Caixa atual focada ganha contorno azul primário. Se a property ViewModel `HasError` = true, as borders ganham cor vermelhas num tom de alerta.
- **Temporizador e Reenvio**:
  - `HorizontalStackLayout` centralizado: 
    - Badge Pill (Arredondado longo) com Fundo bege, Ícone Relógio escuro e o Label ligando ao `TempoFormatado` ("04:59").
  - Abaixo, Label "Reenviar código" (`TextDecorations="Underline"` e `TextColor="Primary"` se `IsResendEnabled`, caso contrário, cinza e texto simples). Configurar TapGesture.
- **Rodapé do Card**:
  - Button "VERIFICAR" Ocupando toda largura da base do card, Ícone Arrow inserido à direita (`ImageSource`, ContentLayout="Right"). `IsEnabled="{Binding IsVerificarEnabled}"`.

💅 **Estilização e UX**
- **Design System (`App.xaml`)**:
  - Cor do Relógio deve ser atenuada (Warm/Bege) para alertar limite de tempo sem causar pânico agressivo; se chegar aos 10 segundos derradeiros, pode mudar pra vermelho usando um DataTrigger de converter em ViewModel (ex: `IsFinishingTimer`).
- **Animações (Microinterações)**:
  - Animação "Shake" (Tremor esquerda-direita rapido) acionado no frame principal quando ocorrer o erro de validação.

♿ **Acessibilidade**
- Focus Acessibility: O App deve instruir "Digite o código 1 de 6", etc.
- Para simplificar e evitar preenchimentos confusos usando VoiceOver, os OTP Inputs customizados costumam inserir o valor "1 de 6 preenchido" como property custom de acessibilidade.

🎨 **Especificação Técnica: Frontend & UI**
Caminho sugerido: `Pages/Login/frontend.md`

🖼️ **Estrutura de Layout (LoginPage.xaml)**
- **Page Root**: `ContentPage` sem NavBar de Navegação padrão (`NavigationPage.HasNavigationBar="False"`).
- **Background**: Fundo off-white texturizado ou preenchimento sólido cinza super claro.
- **Container Principal**: `ScrollView` preenchendo o espaço (`FillAndExpand`), com `VerticalStackLayout` ou `Grid` centralizando o conteúdo verticalmente (`CenterAndExpand`).
- **Logo e Header (Identidade Visual)**:
  - Container vertical centralizado acima do formulário.
  - `HorizontalStackLayout` ou ícone vetorial (`Path` / `FontImageSource`) para o ícone do prédio azul e dourado.
  - Título principal `Label` com a fonte grossa (ex: Montserrat/Inter Bold), texto "EDIFÁCIL", cor `#001F3F`, com `CharacterSpacing="2"`.
  - Subtítulo `Label`.
- **Card do Formulário**:
  - `Frame` ou `Border` com `CornerRadius="24"`, fundo `#FFFFFF` (Branco), e `Shadow` difuso para destacar do fundo. `Margin="20"`, `Padding="30"`.
  - **Inputs Customizados (`Entry` + Ícones)**:
    - Construir visual complexo combinando `Grid` ou container `Border` de fundo azul claro (`#E8F1FF`), com `CornerRadius="12"`.
    - **Identificador**: Ícone `Image` (usuário) na esquerda, `Entry` translúcido preenchendo o restante (`Keyboard="Default"` para aceitar texto e dígitos). Placeholder "seuemail@condominio.co" cinza. Opcional `Label` descritivo superior flutuante ou externo: "Email, User ID or CPF".
    - **Senha**: Mesma estrutura baseada no Identificador, porém ícone da esquerda de cadeado, fechando à direita com um `ImageButton` de "Olho", atrelado ao `TogglePasswordVisibilityCommand`. O Entry principal deve usar `IsPassword="{Binding IsPasswordHidden}"`.
- **Botão Principal**:
  - `Button` "LOGIN" com cantos arredondados, preenchimento total horizontal, fundo azul escuro, atrelado ao `LoginCommand`.
- **Footer do Card (Links)**:
  - Separados por margens, usar links com TapGestureRecognizer em textos da cor cinza chumbo (`#555`).
  - Textos sublinhados nativamente (`TextDecorations="Underline"`).
- **Feedback Visual (Overlay)**:
  - Um painel transparente ou escurecido cobrindo o card, contendo um `ActivityIndicator` animado para bloquer interações quando `IsBusy` for `true`.

💅 **Estilização e UX**
- **Design System (`App.xaml`)**:
  - Reaproveitar `PrimaryColor` (Azul escuro) e `SecondaryColor` / Highlight (Amarelo ouro `#F1C40F`) nos componentes SVG.
  - Implementar Focus Visuals: Ao dar tap/foco nos Inputs, a borda do field deve mudar levemente de cor (ex: Azul vibrante), evidenciando o campo ativo.
- **Microinterações**:
  - A tecla Enter (Return) do campo de identificador deve automaticamente dar Focus() no campo da Senha. A tecla de retorno da Senha deve submeter o form (`ReturnCommand="{Binding LoginCommand}"`).

♿ **Acessibilidade**
- Aplicar corretamente `SemanticProperties.Hint="Digite seu e-mail, id único ou cpf"` no campo de usuário.
- Tratar o campo de senha com `SemanticProperties.Hint="Digite sua senha"`, evitando leitura literal.
- Indicar visualmente e para os leitores de tela os estados de validação de erro nos inputs.

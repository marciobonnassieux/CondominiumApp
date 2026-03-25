🎨 **Especificação Técnica: Frontend & UI**
Caminho sugerido: `Pages/CriacaoDeConta/frontend.md`

🖼️ **Estrutura de Layout (CriacaoDeContaPage.xaml)**
- **Page Root**: Utilizar `ContentPage`. Aplicar uma imagem de fundo (background image/color) de acordo com o design (se aplicável, normalmente um fundo sóbrio ou gradiente).
- **Container Principal**: Um `ScrollView` envolvendo o conteúdo principal, útil para evitar sobreposição do teclado virtual nos campos do formulário.
- **Header**:
  - Uma `Grid` horizontal no topo: Botão `ImageButton` (seta para esquerda) com binding para `VoltarCommand`. Logo no centro (ou abaixo da seta dependendo do mockup). Label "EDIFÁCIL - O APP DO CONDOMÍNIO".
- **Corpo Principal (Card)**:
  - Usar um `Frame` ou `Border` estilizado com sombra leve (Shadow) centralizado verticalmente.
  - Títulos descritivos (`Label`): "Criar Nova Conta" (HeaderLabelStyle) e "Preencha os dados abaixo..." (SubtitleLabelStyle).
  - **Campos de Entrada (`Grid` / `StackLayout`)**:
    - Usar containers customizados para os `Entry` que contenham label flutuante ou placeholders.
    - E-mail e Confirmação de E-mail: `Keyboard="Email"`.
    - Senha e Confirmação de Senha: 
      - Usar um `Grid` ou `HorizontalStackLayout` contendo o `Entry` (`IsPassword="{Binding IsPasswordHidden}"`) e um `ImageButton` com ícone de "olho", atrelado ao `TogglePasswordVisibilityCommand`.
  - **Tooltips/Informes**:
    - `Label` pequena com texto de aviso sobre critérios de senha (cor neutra ou ícone de aviso).
  - **Área de Ações**:
    - `Button` "SALVAR" (PrimaryButton Style).
    - `Button` "VOLTAR" atrelado ao `VoltarCommand` (SecondaryButton Style ou FlatButton com BackgroundColor="Transparent"). O fundo branco/transparente.
- **Footer**:
  - Um `Label` fixo na base inferior (AbsoluteLayout ou Grid Row="Bottom") com texto pequeno: "CONCIERGE DIGITAL EXPERIENCE • 2024".

💅 **Estilização e UX**
- **Design System (`App.xaml`)**:
  - Usar cores como `PrimaryColor` para o botão Salvar, tipografia de inputs, bordas e FocusColors consistentes.
- **Estados Visuais (`VisualStateManager`)**:
  - Feedback tátil em botões.
  - O ícone de do "olho" na senha deve trocar sua imagem (`eye.png` ou `eye-off.png`) baseando-se no estado `IsPasswordHidden` e `IsConfirmPasswordHidden` via `DataTrigger`.
- **Feedback de Carregamento**:
  - Quando "SALVAR" for acionado e `IsBusy` for `true`, o botão "SALVAR" deve ser desabilitado, e um `ActivityIndicator` (Overlay global ou botão animado) deve ser exibido na tela (`IsVisible="{Binding IsBusy}"`).
- **Comportamento do Teclado**:
  - Configurar `ReturnCommand` nos Entries para pular automaticamente de um campo para o outro, otimizando o preenchimento pelo usuário em mobile. O campo de Senha final deve focar o Enter em validar a tela.

♿ **Acessibilidade**
- Aplicar `SemanticProperties.Description` no botão do "olho" de senha para auxiliar leitores de tela indicando "Mostrar senha" ou "Esconder senha".
- Configurar `SemanticProperties.HeadingLevel="Level1"` no título "Criar Nova Conta".

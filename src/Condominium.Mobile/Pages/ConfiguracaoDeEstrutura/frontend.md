🎨 **Especificação Técnica: Frontend & UI**
Caminho sugerido: `Pages/ConfiguracaoDeEstrutura/frontend.md`

🖼️ **Estrutura de Layout (ConfiguracaoDeEstruturaPage.xaml)**
- **Page Root**: Utilizar `ContentPage` com uma `Grid` principal dividida em duas linhas principais: conteúdo (estrelinha `*`) e Bottom Tab Bar (`Auto`).
- **Container Principal**: Um `ScrollView` englobando todo o conteúdo rolavel para suporte em telas menores. Dentro dele, um `StackLayout` ou `VerticalStackLayout` espaçado.
- **Header**:
  - `Grid` horizontal com 3 colunas (Ícone Menu, Logo, Ícone Notificações).
- **Seção "Blocos Existentes"**:
  - Label de título: "GESTÃO ADMINISTRATIVA" (estilo subtítulo) e "Configurar Edifício" (estilo título).
  - Sub-header com contador: "4 BLOCOS".
  - **Lista de Blocos**: Utilizar um `CollectionView` atrelado ao `Blocos` da ViewModel. Definir o `ItemTemplate` contendo um `Frame` ou `Border` (Card) com um `Grid` interno para posicionar os textos (Nome do Bloco, Unidades) à esquerda e os ícones de ação (Edição/Lixeira) à direita.
- **Seção "Adicionar Bloco"**:
  - Container em `Border` / `Frame` estilizado como card (com sombra ou borda sutil).
  - Títulos descritivos ("ADICIONAR BLOCO") e Textos de Ajuda formatados (`Label` com texto menor e cor mais clara).
  - **Inputs (`Entry`)**: 
    - `Entry` para "NOME DO BLOCO" (teclado `Text`).
    - Uma `Grid` com 2 colunas para colocar lado a lado: `Entry` "DE (UNIDADE)" e `Entry` "ATÉ (UNIDADE)", ambos com teclado `Numeric`.
  - **Alerta Informativo**: Usar um `Border` com cantos arredondados, fundo cinza claro/amarelado e um ícone de "info", contendo a explicação da geração automática.
  - **Botão Primário**: `Button` com texto "CONFIRMAR BLOCO" e um ícone (+), atrelado ao `ConfirmarBlocoCommand`.
- **Bottom Tab Bar**:
  - Se não for controlado globalmente via `AppShell`, criar um componente/Grid fixado na última linha da tela contendo ícones para as seções (Início, Volumes, Pessoas, Segurança).

💅 **Estilização e UX**
- **Design System (`App.xaml`)**:
  - Utilizar cores temáticas (ex: `PrimaryColor` para o card do botão primário, `DangerColor` para o botão de exclusão).
  - Usar os estilos tipográficos nomeados (`HeaderLabelStyle`, `SubtitleLabelStyle`, `InputLabelStyle`).
- **Estados Visuais (`VisualStateManager`)**:
  - Definir VSM para os botões e entries em estados `Normal`, `Focused`, e `Disabled` (especialmente se o botão primário for desabilitado enquanto a ViewModel `IsBusy`).
- **Feedback de Carregamento**:
  - Adicionar um `ActivityIndicator` centralizado (Overlay) com `IsVisible="{Binding IsBusy}"` e `IsRunning="{Binding IsBusy}"` para bloquear a tela durante operações de rede.
- **Responsividade e Performance**:
  - Ajustar o espaçamento (`Spacing`) e margens no `VerticalStackLayout` para manter respiro na tela (`Padding="20"`).
  - `CollectionView` deverá ser testada para reciclagem suave dos itens caso haja muitos blocos.

♿ **Acessibilidade**
- Aplicar `SemanticProperties.Hint` nos botões de ícone (Menu, Lixeira, Edição) indicando a ação (ex: "Excluir bloco respectivo").
- Aplicar `SemanticProperties.HeadingLevel` nos Títulos principais.

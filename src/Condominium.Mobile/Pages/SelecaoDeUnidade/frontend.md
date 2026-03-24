🎨 **Especificação Técnica: Frontend & UI**
Caminho sugerido: `Pages/SelecaoDeUnidade/frontend.md`

🖼️ **Estrutura de Layout (SelecaoDeUnidadePage.xaml)**
- **Page Root**: Empregar um `ContentPage` gerenciado pelo AppShell (Bottom Tab Bar aparente).
- **Header e Área de Filtros**:
  - `Grid` superior com botão Voltar, "Selecionar Destino", e Logo.
  - Espaço de Search: `SearchBar` atrelado ao `TermoDaBusca` (TextChanged behavior => AplicarFiltrosCommand).
  - Picker de Bloco: Customizado para parecer um input moderno redondo.
  - Chips Horizontais (Andares): `CollectionView` com orientação Horizontal. Cada aba de andar pode ser um `Border` com canto arredondado que troca de background (Preto para selecionado, Branco/Cinza para não selecionado) baseando-se no `AndarSelecionado`.
- **Grid de Unidades (`CollectionView`)**:
  - Layout: `GridItemsLayout` com `Span="3"` e `Span="4"` dependendo de `DeviceDisplay.MainDisplayInfo.Width` para responsividade (ou fixo em 3/4 colunas).
  - Binding à `UnidadesFiltradas`.
  - **ItemTemplate (Cada Apartamento)**:
    - O container (`Frame` / `Border`) reage à seleção. 
    - Usar VSM (VisualStateManager) ou Triggers: Se `BindingContext == UnidadeSelecionada`, aplicar cor de destaque (Background Primário/Dourado, Texto Branco).
    - Status ("DISPONÍVEL" ou "OCUPADO"): Labels minúsculos embaixo do número. Ocupados devem ter opacidade reduzida (`Opacity="0.5"`) ou cor cinza de fundo indicando estado inativo com restrição de interação se a regra de negócio ditar isso.
- **Rodapé Fixo (Sticky Bar)**:
  - `Grid` com duas colunas fixada no final (`Grid.Row="Bottom"`).
  - Esquerda: Ícone + Label indicando "UNIDADE SELECIONADA" e abaixo mostrando o número (Ex: `{Binding UnidadeSelecionada.Numero, TargetNullValue='Nenhuma'}`).
  - Direita: Botão "CONFIRMAR". Propriedade `IsEnabled="{Binding TemUnidadeSelecionada}"`.
- **Bottom Tab Bar**:
  - Guiado pelo AppShell nativo ou componente customizado.

💅 **Estilização e UX**
- **Design System (`App.xaml`)**:
  - O visual do Grid de apartamentos precisa seguir o conceito de "Chips" selecionáveis, similar a seleção de assentos de avião/cinema (feedback tátil e estado claro de seleção).
- **Interação de Filtros**:
  - Aplicar animação sutil nos números que somem/aparecem ao filtrar a barra de pesquisa em tempo real.

♿ **Acessibilidade**
- Avisar o leitor de telas via Announce quando uma unidade for tocada: "Unidade 101 selecionada".
- Os botões (cards) do Grid devem ter ContentDescriptions explícitas (Ex: `SemanticProperties.Hint="Apartamento 101, disponível"`).

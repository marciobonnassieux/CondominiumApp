🎨 **Especificação Técnica: Frontend & UI**
Caminho sugerido: `Pages/DashboardMobilePortaria/frontend.md`

🖼️ **Estrutura de Layout (DashboardMobilePortariaPage.xaml)**
- **Page Root**: Utilizar `ContentPage` com uma cor de fundo do sistema (cinza bastante claro ou padronizado, ex: `#F6F7F9`) focado em legibilidade de alto contraste.
- **Pull To Refresh**: Envolver o corpo da página em um `RefreshView` linkado ao `IsRefreshing` e `RefreshCommand`.
- **Top Header**:
  - Construir um cabeçalho customizado (não a TitleBar nativa), contendo:
    - Opcional `Frame/Border` circular para o Avatar do porteiro.
    - `Label` com boas-vindas: "Edifácil", "Dashboard Portaria" (TitleLabelStyle) e "Porteiro Carlos - 15 Out" (Subtitle/MetaLabelStyle).
    - Ícone de notificação na extrema direita (Sino) atrelado a um comando ou flyout.
- **Área de Ações (Acesso Rápido)**:
  - Container "Largo": Botão ou Border expansivo ("RECEBER ENCOMENDAS") usando fundo escuro (`PrimaryDarkColor`), preenchimento horizontal com ícone.
  - `Grid` com colunamento de "50*" para os próximos botões (lado a lado):
    - Border "NOTIFICAR MORADORES" (Corrente/Fundo principal) contendo um StackLayout vertical (Ícone grande + texto em wrap).
    - Border "REGISTRAR VISITANTE".
- **Resumo do Turno**:
  - Texto de cabeçalho "Resumo do Turno" à esquerda e label minúscula "TEMPO REAL" à direita.
  - `Grid` ou `HorizontalStackLayout` com os pequenos Cards contendo números extravagantes (`Label.FontSize="48"`) para as estatísticas.
- **Atividade Recente (Streaming List)**:
  - Cabeçalho "Atividade Recente" com label de atalho "Ver tudo" (cor azul/primária, sublinhado).
  - Usar `BindableLayout.ItemsSource` ou `CollectionView` (se a lista não for de altura intrínseca, usar recapeamento suave) para exibir a `AtividadesRecentes`.
  - O ItemTemplate deve exibir o tipo da atividade trocando a cor do ícone de acordo, título em negrito, e detalhe (hora) como texto de metadados.

💅 **Estilização e UX**
- **Design System (`App.xaml`)**:
  - Utilização abundante de sombras (`Shadow`) projetadas atrás dos cards, garantindo separação e indicação de elementos clicáveis/táteis.
  - As bordas arredondadas (Corners) devem seguir os tokens definidos (ex: `CornerRadius="12"`).
- **Tratamento "Empty State"**:
  - Se `AtividadesRecentes` estiver vazio, mostrar um gráfico ou ilustração vazada de "Sem atividade no momento" num Frame de borda tracejada, preenchendo o espaço.
- **Atualização Suave**:
  - Evitar piscar os contadores na tela ao serem notificados pelo WebSocket/SignalR; usar valores que substituem in place via INotifyPropertyChanged de forma discreta.

♿ **Acessibilidade**
- Em listas com scroll, habilitar agrupamento e ordenação via `SemanticProperties.Hint` no ItemTemplate pra contextualizar melhor com Voice Over.
- Números estatísticos enormes (`45`, `12`) devem ser contornados pelo texto semântico, e.g. "Existem 45 encomendas pendentes".

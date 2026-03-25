🎨 **Especificação Técnica: Frontend & UI**
Caminho sugerido: `Pages/GestaoDeVinculosECargos/frontend.md`

🖼️ **Estrutura de Layout (GestaoDeVinculosPage.xaml)**
- **Page Root**: Utilizar `ContentPage` gerenciado pelo AppShell ou com BottomNavBar customizada.
- **Container Principal**: `Grid` com linhas parHeader (`Auto`), Barra de Busca (`Auto`), Botão de VincularNovo (`Auto`), Lista de Equipe (`*`) e Botão Flutuante de Salvar (`Auto` ancorado em Bottom).
- **Header**:
  - `Grid` horizontal: Botão de voltar (esquerda), "Gestão de Vínculos" (centro) e botão de lupa/buscar (direita). Ao clicar na lupa, exibir o `SearchBar` atrelado a `TermoBusca`.
- **Ação de Vinculação**:
  - `Frame` ou `Border` horizontal atuando como botão (TapGestureRecognizer atrelado ao `VincularNovoCpfCommand`), com ícone de Adicionar Usuário e Label "Vincular novo usuário por CPF" encostado com cor de destaque secundária.
- **Listagem de Equipe (`CollectionView`)**:
  - `Label` de cabeçalho "EQUIPE VINCULADA" e label do contador ("{Binding TotalUsuarios} USUÁRIOS") estilizados com fontes menores/uppercase.
  - **ItemTemplate (`VinculoUsuarioModel`)**:
    - `Grid` ou `Border` como Container do item.
    - O conteúdo do template inclui:
      - Avatar Circular (`Frame` + `Image` ou Placeholder com iniciais). Indicador de status online (`Ellipse` verde sobreposto no canto).
      - Coluna de Texto: Nome em negrito (`Label` Title) e CPF mascarado embaixo (`Label` Subtitle).
      - Dropdown (`Picker`): Posicionado à direita, atrelado a propriedade `CargoIdSelecionado` (ou objecto `Cargo`) biderecional. ItemSource apontando pro x:Reference da página ou ViewModel (`BindingContext.CargosDisponiveis`). Implementar `SelectedIndexChanged` mapeando o `CargoAlteradoCommand`.
- **Botões Flutuantes/Ações de Massa**:
  - `Button` "SALVAR ALTERAÇÕES" com ícone "✔️" afixado na parte inferior da tela (acima da Nav Bar interior). Visibilidade (`IsVisible`) controlada pela booleana `TemAlteracoesNaoSalvas`.
- **Bottom Tab Bar**:
  - O estado atual da Tab deve destacar o ícone de GESTÃO.

💅 **Estilização e UX**
- **Design System (`App.xaml`)**:
  - Padrão visual elegante para o Avatar usando conversores para criar Iniciais se não houver foto enviada pelo usuário.
  - O estilo do `<Picker>` padrão MAUI pode ser rudimentar; é recomendável aplicar um Style customizado para remover bordas indesejadas e alinhar corretamente o texto à direita ou na segunda linha.
- **Interatividade**:
  - Mostrar `ActivityIndicator` central (`IsVisible={Binding IsBusy}`) bloqueando cliques simultâneos enquanto "Salvar Alterações" ocorre.
  - Ao salvar com sucesso, usar Toast ou Snackbar do CommunityToolkit (ex: `await Toast.Make("Cargos atualizados").Show()`) e esconder o botão de salvar.

♿ **Acessibilidade**
- Informar contexto no Picker do Cargo, ex: `SemanticProperties.Hint="Alterar cargo de Ricardo Silveira"`.
- O ItemTemplate deve ter uma leitura concisa para VoiceOver.

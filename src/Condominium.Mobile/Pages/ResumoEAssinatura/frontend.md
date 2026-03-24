🎨 **Especificação Técnica: Frontend & UI**
Caminho sugerido: `Pages/ResumoEAssinatura/frontend.md`

🖼️ **Estrutura de Layout (ResumoEAssinaturaPage.xaml)**
- **Page Root**: Empregar um `ContentPage` gerenciado pelo AppShell para ter BottomNavBar, e um `ScrollView` abarcando tudo para evitar bloqueio dependendo do aspect ratio.
- **Header**:
  - Layout horizontal nativo do shell (NavigationPage.TitleView) ou Grid customizado: Voltar, "Finalizar Recebimento", Ícone Edifácil.
- **Listagem Resumo (Cards Consolidadores)**:
  - Cabeçalho: "RESUMO DO LOTE" Label + Badge/Frame pequeno "{Binding TotalEntregas} ENTREGAS".
  - `BindableLayout` ou `CollectionView` (com `ItemsSource="{Binding EntregasAgrupadas}"`):
    - **Card de cada Destino**: `Frame` ou `Border`. Interior: 
      - Ícone de Apartamento/Prédio de um lado.
      - Textos: Título ("Bloco A - Apto 101"), Subtítulo em fonte menor cinza ("2 volumes registrados").
      - Lado Direito: Caixa (box icon) em verde com ícone de check.
- **Painel de Assinatura (Controle Customizado)**:
  - Label "ASSINATURA DO ENTREGADOR".
  - **Signature Pad**: Utilizar um controle de terceiros como `CommunityToolkit.Maui.Views.DrawingView` ou nativo mapeado pra captura de traços.
    - O fundo (`Background`) do DrawingView: Aplicar padrão de dots ou cor cinza claro pastel (`#FFF5F5F5`).
    - Propriedade de linha do desenho: Espessura (`LineWidth="3"`) cor da tinta primária escura.
  - Sobrescrito/Absoluto: Um `Label` com opacidade no centro (ex: "Assine neste campo") que se esconde quando as linhas (`Lines.Count > 0`) forem desenhadas.
  - Botão "LIMPAR" posicionado no rodapé ou no topo do card do Signature, ligado ao `LimparAssinaturaCommand`.
- **Card Alerta de Segurança**:
  - Um `Border` de cantos arredondados e cor amarelo limão suave ou laranja (Atenção).
  - Ícone de escudo/cadeado. Texto "PROTOCOLO DE SEGURANÇA" (Bold).
  - Descrição: "... os moradores vinculados receberão notificações push instantaneamente."
- **Botão Final e Nav Bar**:
  - Um Primary Button verde ou azul escuro, ocupando largura total, com ícone embutido. Texto atrelado ("FINALIZAR LOTE...").

💅 **Estilização e UX**
- **Design System (`App.xaml`)**:
  - Garantir consistência nas bordas dos cards, mantendo uma hierarquia visual onde a área de assinatura salte aos olhos (borda sólida ou frame mais largo).
- **Interação SignaturePad**:
  - Deve capturar a ação final e converter para `Stream`/Imagem apenas ao clicar no botão FINALIZAR, não salvando no dispositivo. Pode exigir travamento do Scroll nativo vertical *só* dentro da área do signature para que o dedo não faça scroll na página inteira acidentalmente.

♿ **Acessibilidade**
- Avisar audivelmente ao focar no controle de assinatura que a área demanda toque capacitivo de escrita (para VoiceOver tratar como imagem).
- Associar o Card de Alerta usando Automation Properties com Announce para ressoar "Atenção".

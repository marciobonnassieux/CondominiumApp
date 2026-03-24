🎨 **Especificação Técnica: Frontend & UI**
Caminho sugerido: `Pages/RegistroDeVolume/frontend.md`

🖼️ **Estrutura de Layout (RegistroDeVolumePage.xaml)**
- **Page Root**: Empregar um `ContentPage`. O fundo não precisa ter cor sólida atrás, pois a view da câmera preencherá a tela inteira. A Navigation Bar nativa deve ser ocultada para maximizar a área de vídeo.
- **Layout de Sobreposição (`Grid` Absoluta ou z-index)**:
  - Definir uma `Grid` principal.
  - A camada base (`Grid.RowSpan="3"`) contém a visualização ao vivo: `<CameraBarcodeReaderView>` (se usar ZXing.Net.Maui) ou controle de câmera ML Kit.
  - As camadas subsequentes desenham as caixas translúcidas em torno do conteúdo.
- **Top Header Transparente**:
  - Um `Grid` no topo com leve gradiente escurecido (`LinearGradientBrush` de transparente pra preto 50%) garantindo que os ícones brancos de Retornar e "PRÓXIMA UNIDADE" fiquem legíveis mesmo se a câmera apontar para luz forte.
- **Sinalizador / Overlays de Viewfinder**:
  - Criar uma máscara semitransparente usando múltiplos `BoxView` preto translúcidos, deixando uma janela em destaque (Frame vazio com bordas laranjas brilhantes).
  - Um texto de ajuda centralizado embaixo/acima do quadro: "ALINHE A ETIQUETA".
- **Bottom Sheet ("Pacotes Registrados")**:
  - Uma área de Drawer presa na base usando um componente customizado ou biblioteca (ex: UraniumUI BottomSheet / Plugin).
  - Parte superior exibindo o Handle (um tracinho cinza `BoxView` com contorno arredondado para sugerir swipe up/down) e título.
  - Essa área congrega a `CollectionView` linkada a `PacotesRegistrados`. Com tamanho fixo e barra de scroll.
  - **ItemTemplate do Pacote**:
    - `Grid` com colunas: Ícone (Caixa preta ou cinza), Texto Principal (Código Lido ex: "Volume 123456" em Bold), Subtexto ("Lido com sucesso" em verde), e Indicador visual na ponta direita (Checkmark Dourado).

💅 **Estilização e UX**
- **Design System (`App.xaml`)**:
  - Botão de "PRÓXIMA UNIDADE" estilizado como um Flat Button, com letras maiúsculas da Highlight Color (`SecondaryColor`/Dourado).
- **UX de Escaneamento (Continuous Flow)**:
  - Não exibir Dialog/Alerta cada vez que um pacote for lido. Exibir um pequeno Toast translúcido que some e acionar vibração simultânea + bip curto sonoro (usando plugin de multimídia). "Piscada" instantânea do Viewfinder para cor verde em caso de sucesso melhora muito o feedback real do porteiro.

♿ **Acessibilidade**
- Aplicar Automation Properties declarando no header os botões (`SemanticProperties.Description`) para navegação guiada por voz.
- Como o fluxo é predominantemente visual, talvez implementar um botão manual alternativo (digitar código) seja uma boa prática para casos em que a câmera falhe ou etiquetas rasgadas não consigam ser lidas por OCR/Visão.

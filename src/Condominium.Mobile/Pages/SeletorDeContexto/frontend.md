🎨 **Especificação Técnica: Frontend & UI**
Caminho sugerido: `Pages/SeletorDeContexto/frontend.md`

🖼️ **Estrutura de Layout (SeletorDeContextoPage.xaml)**
- **Page Root**: Empregar um `ContentPage` sem `<Shell.NavBarIsVisible>`, mantendo a tela vazia como App Root. Fundo branco limpo.
- **Container Centralizado**: O `ScrollView` tem uma formatação base de coluna, centralizando os elementos verticalmente, útil para quando só houver 2 cards.
- **Topografia**:
  - Títulos descritivos (`Label`): "O APP DO CONDOMÍNIO" (Subtitulo Dourado Pequeno), "Onde você deseja atuar hoje?" (Título DarkBlue Bold `HeaderLabelStyle`).
- **Cards de Acesso (`CollectionView`)**:
  - Lista simples (`VerticalStackLayout` com Binding no `.ItemsSource` se for CollectionView).
  - **ItemTemplate (ContextoUsuarioModel)**:
    - Container principal é um `Frame` ou `Border` com cantos suaves. `TapGestureRecognizer` ativado para o comando `SelecionarContextoAsync`.
    - Uma cor vibrante de GradientBackground (ou sólido com gráfico vetor em opacidade 10%) pode ser aplicada para dar tom Premium. Alternativamente, usar fundo branco básico `CardStyle`.
    - **Grade Interna (`Grid`)**:
      - Coluna Direita Extrema: `Path` SVG ou ícone em background translúcido "cortado" dentro do cantinho do frame, e uma setinha dourada para a direita.
      - Coluna Esquerda: Ícone representativo (`Image` / `FontImageSource`) - Portaria, Prédio Comercial, Casa.
      - Centro: Stack com Textos - Nome do Condomínio ("Edifício Solar") e Role Subtítulo ("SÍNDICO").
- **Footer Fixo**:
  - Na parte extrema inferior, um simples `HorizontalStackLayout` contendo o ícone de logout e o Label "Sair da conta", ambos com `TextDecorations="Underline"` ligados ao `SairDaContaCommand`.

💅 **Estilização e UX**
- **Design System (`App.xaml`)**:
  - Essa tela lida com marca visual de transição, portanto a estética "Premium" é primordial. A combinação do dark blue e golden yellow e fontes amplas é exigida.
  - Implementar Feedback de Clique nos botões (Fade To/Scale To) usando as animações contidas no .NET MAUI Community Toolkit.
- **Estado Visual Vazio**:
  - Se a lista for vazia (erro raro), exibir "Nenhum vínculo encontrado. Procure o seu síndico.".

♿ **Acessibilidade**
- Tratar os frames como botões, declarando o AutomationId adequado para permitir tap fácil para leitores ("Entrar no condomínio X como Síndico").
- Aumentar área de toque (hit area) do botão Sair da conta (Padding > 15).

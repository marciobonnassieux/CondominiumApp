# Análise da Tela: Qual a unidade? (Selecionar Destino)

## Objetivo da Tela
Proporcionar a seleção pontual de um bloco e um apartamento para o qual uma determinada notificação, entrega ou vistoria, etc. será atribuída. Possui ferramentas de filtro para achar a unidade facilmente.

## Componentes Visuais e Fluxos Identificados
1. **Header Principal**:
   - Ícone Back, Texto "Selecionar Destino", Logo "EDIFÁCIL".

2. **Área de Pesquisa e Filtros**:
   - Textbox superior: "LOCALIZAÇÃO", "Qual a unidade?".
   - Input de Pesquisa ("Pesquisar Unidade") com ícone de Lupa.
   - Picker/Dropdown de Bloco: Ex "Bloco A". Permite alternar o Grid abaixo.
   - Scroll Horizontal de Filtros Extras: "Todos", "1º Andar", "2º Andar", servindo para segmentar visualmente os andares mostrados daquele edifício e evitar lista extensa.

3. **Grid de Apartamentos Dispiníveis**:
   - Label com badge: "APARTAMENTOS DISPONÍVEIS - BLOCO A".
   - Um layout de grid (3 colunas, aparentemente CollectionView ou GridBox).
   - Cada célula representa uma Unidade:
     - Título Grande: Número (Ex: "101", "102").
     - Subtexto: Status ou estado (Ex: "DISPONÍVEL" ou "OCUPADO").
     - Estilo Diferenciado: Unidades ocupadas aparecem mais acinzentadas, sem sombra forte, demonstrando inatividade (se isso for uma tela para locação/mudança) OU significando apenas que já existem encomendas ali. O termo exato na imagem aponta "Disponível" para as selecionáveis.

4. **Rodapé de Seleção (Sticky Bar)**:
   - Resumo da informação pendente de avanço. 
   - Ícone de chave, label "UNIDADE SELECIONADA". Valor atual: "Nenhuma".
   - Botão de Avanço "CONFIRMAR". Fica com visual desabilitado caso não exista nenhuma seleção ativa no grid.
   - Bottom Tab Bar (Aba de "VOLUMES" ativa, indicando ser do fluxo de registro de pacotes).

## Regras de Negócio e Lógicas Necessárias
- A propriedade `ItemsSource` do Grid de botões numéricos será atualizada pelas seleções (em cascata) do Bloco Picker e do Scroll Horizontal (Andar).
- A busca em "Pesquisar Unidade" deve filtrar localmente o ObservableCollection que preenche a CollectionView do Grid, por número correspondente.
- Lógica Single Selection no Grid: Clicar sobre "104" destaca este quadrado, altera a SelectedItem da page/viewmodel, e atualiza o Rodapé de Seleção para "104", habilitando dinâmicamente o Botão "CONFIRMAR".

⚙️ **Especificação Técnica: Backend & Lógica**
Caminho sugerido: `Pages/SelecaoDeUnidade/backend.md`

🧩 **Arquitetura da ViewModel (SelecaoDeUnidadeViewModel)**
- **Propriedades Observáveis (`[ObservableProperty]`)**:
  - `ObservableCollection<BlocoModel> Blocos`: Lista de blocos do condomínio.
  - `BlocoModel BlocoSelecionado`: Filtro atual de bloco.
  - `ObservableCollection<string> Andares`: Filtro horizontal de andares (ex: "Todos", "1º Andar").
  - `string AndarSelecionado`: Andar filtrado atual.
  - `string TermoDaBusca`: Texto da searchbar.
  - `ObservableCollection<UnidadeModel> UnidadesDisponiveis`: Lista mestra de unidades do bloco selecionado.
  - `ObservableCollection<UnidadeModel> UnidadesFiltradas`: Lista exibida no Grid, resultado da aplicação dos três filtros (Bloco, Andar, Busca).
  - `UnidadeModel UnidadeSelecionada`: A unidade clicada pelo usuário.
  - `bool IsBusy`: Indica carregamento de dados.
- **Propriedades Calculadas**:
  - `bool TemUnidadeSelecionada => UnidadeSelecionada != null;` (Usada para habilitar o botão "CONFIRMAR").
- **Comandos (`[RelayCommand]`)**:
  - `CarregarDadosAsync()`: Busca a estrutura do condomínio.
  - `AplicarFiltrosCommand()`: Atualiza `UnidadesFiltradas` baseado no Bloco, Andar e Termo de Busca atuais.
  - `SelecionarUnidadeCommand(UnidadeModel unidade)`: Atribui a unidade à propriedade selecionada e notifica mudança para reavaliar `TemUnidadeSelecionada`.
  - `ConfirmarSelecaoCommand()`: Retorna a unidade selecionada para o fluxo de origem ou avança para a próxima etapa passando a unidade e os pacotes (se for fluxo de entrega).
- **Navegação**:
  - Uso de passagem de parâmetros (ex: evento ou `GoBackAsync` passando retorno) dependendo da origem.

🌐 **Integração de Dados**
- **Services Necessários**:
  - `IEstruturaCondominioService`:
    - `Task<EstruturaCompletaDto> ObterEstruturaAsync()`: Traz blocos e suas unidades em cache para filtragem rápida local.
- Se o caso de uso exigir validação online (ex: "Posso entregar neste apto agora?"), adicionar chamada de serviço; caso contrário, a lógica é puramente de filtragem in-memory após o fetch de estrutura.

🔒 **Regras de Negócio e Segurança**
- **Filtro em Cascata**: Sempre que `BlocoSelecionado` mudar:
  1. O Picker/Scroll de Andares deve ser recriado com base nos andares *daquele* bloco específico.
  2. `AndarSelecionado` deve voltar para "Todos".
  3. `UnidadeSelecionada` deve ser limpa (`null`).
  4. Executar `AplicarFiltrosCommand()`.

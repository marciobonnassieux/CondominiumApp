⚙️ **Especificação Técnica: Backend & Lógica**
Caminho sugerido: `Pages/ConfiguracaoDeEstrutura/backend.md`

🧩 **Arquitetura da ViewModel (ConfiguracaoDeEstruturaViewModel)**
- **Propriedades Observáveis (`[ObservableProperty]`)**:
  - `ObservableCollection<BlocoModel> Blocos`: Lista dos blocos existentes no condomínio.
  - `string NovoBlocoNome`: Nome do novo bloco a ser adicionado.
  - `int? NovaUnidadeDe`: Intervalo inicial de unidades.
  - `int? NovaUnidadeAte`: Intervalo final de unidades.
  - `bool IsBusy`: Indica operação em andamento (carregamento/salvamento).
  - `int TotalBlocos`: Propriedade calculada baseada na contagem da lista `Blocos`.
- **Comandos (`[RelayCommand]`)**:
  - `CarregarBlocosAsync()`: Busca a lista de blocos na API ao inicializar a tela.
  - `ConfirmarBlocoAsync()`: Valida os dados inseridos e chama o serviço para criar um novo bloco.
  - `EditarBlocoCommand(BlocoModel bloco)`: Prepara o estado para edição ou navega para tela de edição (se aplicável).
  - `ExcluirBlocoAsync(BlocoModel bloco)`: Exibe diálogo de confirmação e chama o serviço de exclusão.
- **Navegação**:
  - Injeção de dependência de `INavigationService` caso haja navegação atrelada aos botões de edição ou bottom tab bar.
- **Lógica de Validação**:
  - O método `ConfirmarBlocoAsync()` deve verificar se `NovoBlocoNome` não está vazio.
  - Validar se `NovaUnidadeDe` e `NovaUnidadeAte` têm valores e se `NovaUnidadeAte` >= `NovaUnidadeDe`.
  - Exibir alerta (usando um serviço de diálogo) se a validação falhar.

🌐 **Integração de Dados**
- **Services Necessários**:
  - `IEstruturaCondominioService`:
    - `Task<IEnumerable<BlocoModel>> ObterBlocosAsync()`: GET na rota `/api/condominio/blocos`.
    - `Task CriarBlocoAsync(CriarBlocoRequestDto request)`: POST na rota `/api/condominio/blocos` contendo os limites e o nome.
    - `Task ExcluirBlocoAsync(Guid blocoId)`: DELETE na rota `/api/condominio/blocos/{id}`. Deve tratar retorno de erro caso existam entidades dependentes (HTTP 409 Conflict ou similar).
  - `IDialogService`: Para exibir alertas de confirmação de exclusão e mensagens informativas ("As unidades serão geradas...").

🔒 **Regras de Negócio e Segurança**
- **Confirmação de Deleção**: `ExcluirBlocoAsync` deve invocar `await DialogService.DisplayAlertAsync("Confirmação", "Deseja realmente excluir este bloco?", "Sim", "Cancelar")` antes de prosseguir.
- **Tratamento de Exceções**: Se a criação em lote falhar por timeout ou erro, capturar a exceção e avisar o usuário, garantindo resiliência (try/catch blocos envolvendo as chamadas da API).

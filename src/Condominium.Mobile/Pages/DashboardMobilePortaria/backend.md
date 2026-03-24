⚙️ **Especificação Técnica: Backend & Lógica**
Caminho sugerido: `Pages/DashboardMobilePortaria/backend.md`

🧩 **Arquitetura da ViewModel (DashboardMobilePortariaViewModel)**
- **Propriedades Observáveis (`[ObservableProperty]`)**:
  - `string NomeUsuario`: Nome do porteiro logado (ex: "Porteiro Carlos").
  - `string DataTurno`: Data atual do turno (ex: "15 Out 2024").
  - `int EncomendasPendentes`: Contador de encomendas pendentes de retirada.
  - `int NovosVisitantes`: Contador de visitantes do turno atual.
  - `ObservableCollection<AtividadeRecenteModel> AtividadesRecentes`: Lista contendo o log das últimas atividades (entrega, entrada autorizada, etc).
  - `bool IsBusy`: Indica operação de carregamento dos dados da dashboard.
  - `bool IsRefreshing`: Utilizado pelo controle de `RefreshView` para pull-to-refresh.
- **Comandos (`[RelayCommand]`)**:
  - `CarregarDashboardAsync()`: Busca os contadores e a lista truncada de atividades recentes. Acionado no `OnAppearing`.
  - `RefreshCommand()`: Limpa ou recarrega a página ao atualizar manualmente.
  - `NavegarReceberEncomendasCommand()`: Redireciona para o fluxo de recebimento.
  - `NavegarNotificarMoradoresCommand()`: Redireciona para listagem/notificação.
  - `NavegarRegistrarVisitanteCommand()`: Redireciona para cadastro manual ou leitura de QR.
  - `VerTodasAtividadesCommand()`: Redireciona para uma tela de listagem completa de log.
- **Navegação**:
  - `INavigationService` implementando as rotas baseadas nos comandos acima. O uso de QueryParameters pode ser dispensável nessa view primária.
- **Lógica de Tempo Real (Real-time)**:
  - Inicialização de uma conexão WebSocket ou SignalR para escutar novos eventos (ex: "NovaEncomendaRecebida", "NovoVisitanteRegistrado") e atualizar as propriedades `EncomendasPendentes`, `NovosVisitantes` e adicionar itens no topo da `AtividadesRecentes` em vez de fazer pull-polling.

🌐 **Integração de Dados**
- **Services Necessários**:
  - `IDashboardPortariaService`:
    - `Task<ResumoTurnoDto> ObterResumoAsync()`: GET `/api/portaria/dashboard/resumo`. Retorna contadores e estatísticas.
    - `Task<IEnumerable<AtividadeRecenteModel>> ObterAtividadesRecentesAsync(int limite = 5)`: GET `/api/portaria/dashboard/atividades`. Retorna apenas os mais novos.
  - `ISignalRClient` ou `IRealtimeNotificationService`:
    - Abstração para iniciar a conexão com o hub da portaria e expor eventos `EventHandler` ou `IObservable` capturados pela ViewModel.

🔒 **Regras de Negócio e Segurança**
- **Token Claims**: O nome do usuário pode ser derivado diretamente dos claims do token JWT salvo no SecureStorage.
- **Recursos Desconectados**: Em caso de perda da conexão TCP do WebSocket, a tela deve tentar se reconectar ou exibir um ícone de status "Offline" na barra de avisos, garantindo transparência ao porteiro.

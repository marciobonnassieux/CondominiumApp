⚙️ **Especificação Técnica: Backend & Lógica**
Caminho sugerido: `Pages/RegistroDeVolume/backend.md`

🧩 **Arquitetura da ViewModel (RegistroDeVolumeViewModel)**
- **Propriedades Observáveis (`[ObservableProperty]`)**:
  - `ObservableCollection<PacoteLidoModel> PacotesRegistrados`: Lista contendo os códigos/informações lidas na sessão atual.
  - `bool IsCâmeraAtiva`: Controla se a visualização da câmera está rodando.
  - `bool IsProcessandoLeitura`: Flag para evitar múltiplas leituras simultâneas do mesmo código de barras enquanto processa a primeira via API.
- **Comandos (`[RelayCommand]`)**:
  - `LigarCameraCommand()` / `DesligarCameraCommand()`: Controla o ciclo de vida do scanner vinculado à navegação da página (OnAppearing/OnDisappearing).
  - `CodigoLidoCommand(string codigoBarras)`: Invocado pelo handler da câmera quando há sucesso no reconhecimento. Deve invocar serviço de validação, adicionar à `PacotesRegistrados` e acionar vibração haptics.
  - `RemoverPacoteCommand(PacoteLidoModel pacote)`: (Opcional) Permite ao usuário remover um pacote lido incorretamente da lista.
  - `ProximaUnidadeCommand()`: Avança para a próxima tela do fluxo passando a lista `PacotesRegistrados` como parâmetro de navegação, onde o usuário fará o de-para vinculando à unidade/morador.
- **Navegação**:
  - `INavigationService` implementando o avanço do fluxo passando dados e rota de cancelamento/voltar.
- **Lógica Específica do Scanner**:
  - Implementar debouncing no `CodigoLidoCommand`. Um volume não pode ser adicionado repetidamente se a câmera disparar 10 eventos no mesmo segundo. Adicionar o código processado numa lista temporária `_codigosLidosNaSessao` para ignorá-lo pelos próximos 5 segundos.

🌐 **Integração de Dados**
- **Services Necessários**:
  - `IVolumeService`:
    - *(Opcional)* `Task<InfoPacoteDto> ValidarCodigoSroAsync(string codigo)`: Bate no backend ou via Regex para validar se o código lido segue um formato padrão de transportadora antes de jogar na tela como sucesso.
  - `IHapticFeedback`: Interface nativa do `Microsoft.Maui.Devices.HapticFeedback` para acionar a vibração assim que o ML/Zxing identificar um código.
  - `IMediaService` / `IPermissionsService`: Para solicitar a permissão de uso da câmera antes de abrir a tela.

🔒 **Regras de Negócio e Segurança**
- **Permissões Nativas**: Tratar a recusa da permissão de "Câmera" alertando que a funcionalidade necessita do acesso, com botão nas configs do app.
- **Persistência Temporária**: Uma queda do app não deveria perder o lote. Se houver muitos pacotes, pode valer a pena estocar os identificadores lidos no cache local até que se clique em "Próxima unidade".

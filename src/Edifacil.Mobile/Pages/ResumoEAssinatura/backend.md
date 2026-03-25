⚙️ **Especificação Técnica: Backend & Lógica**
Caminho sugerido: `Pages/ResumoEAssinatura/backend.md`

🧩 **Arquitetura da ViewModel (ResumoEAssinaturaViewModel)**
- **Propriedades Observáveis (`[ObservableProperty]`)**:
  - `ObservableCollection<ResumoEntregaModel> EntregasAgrupadas`: Lista de entregas (pacotes lidos) agrupadas por unidade (Apto/Bloco).
  - `int TotalEntregas`: Total de pacotes de todas as unidades consolidadas.
  - `byte[] AssinaturaBytes`: Imagem da assinatura capturada pelo pad.
  - `bool IsBusy`: Indica que o fechamento do lote está sendo processado na API.
  - `bool PodeFinalizar`: Habilita o botão final. Uma regra seria exigir `EntregasAgrupadas.Any()` e (opcionalmente) `AssinaturaBytes != null`.
- **Comandos (`[RelayCommand]`)**:
  - `LimparAssinaturaCommand()`: Invocado pelo botão limpar. Limpa os bytes locais e aciona evento pro frontend resetar o UI Canvas.
  - `CapturarAssinaturaCommand(byte[] bytes)`: Converte/armazena o desenho feito no SignaturePad para Base64 ou Byte Array em memória.
  - `FinalizarLoteCommand()`: Confirma o envio, aciona bloqueio `IsBusy`, anexa o arquivo de assinatura e envia pra API de recebimento.
  - `VoltarCommand()`: Retorna ao Seletor de Unidades ou Leitor da Câmera.
- **Navegação**:
  - `INavigatedAware` (ou similar) para receber pelo roteamento (`IDictionary<string, object> parameters`) os pacotes escaneados no fluxo anterior.
  - Ao finalizar com sucesso, rotear o app de volta para a tela inicial ou Dashboard.

🌐 **Integração de Dados**
- **Services Necessários**:
  - `ILoteRecebimentoService`:
    - `Task<FinalizacaoLoteResponseDto> EnviarLoteRecebimentoAsync(EnvioLoteDto request)`: Submete o POST. O modelo DTO deve conter as entidades vinculadas (`PacoteId`, `UnidadeId`) e a assinatura digital em `MultipartFormData` (ou string em Base64).
  - `IDialogService`: Exibir erro se o pacote já foi recebido por outro aparelho simultaneamente, ou Toast de Sucesso "Lote finalizado com sucesso. Moradores notificados".
  - A lógica de disparo de notificação Push acontece estritamente no backend (API).

🔒 **Regras de Negócio e Segurança**
- **Validação Offline**: Não permitir a finalização se o Pad de Assinatura estiver completamente em branco (verificando os strokes do controle ou se os pixels formam padrão nulo).
- **Prova Criptográfica**: Dependendo do nível de auditoria, o app front-end pode embutir uma meta-tag/watermark EXIF com as coordenadas GPS do momento da assinatura, embora isto adicione complexidade.

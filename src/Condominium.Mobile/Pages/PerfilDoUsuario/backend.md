⚙️ **Especificação Técnica: Backend & Lógica**
Caminho sugerido: `Pages/PerfilDoUsuario/backend.md`

🧩 **Arquitetura da ViewModel (PerfilUsuarioViewModel)**
- **Propriedades Observáveis (`[ObservableProperty]`)**:
  - `string NomeCompleto`: Nome do usuário.
  - `string Cpf`: CPF do usuário (com formatação visual aplicada pelo frontend).
  - `string Telefone`: Número de telefone de contato (com máscara).
  - `string Email`: E-mail de cadastro (Read-only na UI).
  - `ImageSource FotoPerfil`: Fonte da imagem atual (URL da web, arquivo local de cache ou null).
  - `FileResult NovaFoto`: Armazena o arquivo temporário caso o usuário escolha uma nova imagem da câmera/galeria, antes de fazer upload.
  - `bool IsBusy`: Indica operação em andamento (salvar dados ou fazer upload da foto).
  - `bool TemAlteracoesNaoSalvas`: Flag para habilitar o botão de Salvar. (Também conhecida como `IsDirty`).
- **Comandos (`[RelayCommand]`)**:
  - `CarregarPerfilAsync()`: Realiza a carga inicial dos dados do usuário logado.
  - `SelecionarFotoCommand()`: Abre ActionSheet perguntando entre "Tirar Foto" ou "Escolher da Galeria". Usa `MediaPicker`.
  - `SalvarAlteracoesAsync()`: Faz upload da `NovaFoto` (se existir) e envia as alterações de texto via API se `TemAlteracoesNaoSalvas` for true.
- **Navegação**:
  - `INavigationService` implementando a volta pra tela anterior (`GoBackAsync`).
- **Lógica de "IsDirty"**:
  - Ao carregar os dados no início, armazenar os valores originais em variáveis privadas. No método `.OnPropertyChanged()` das propriedades correspondentes, comparar com os originais para setar `TemAlteracoesNaoSalvas = true`.

🌐 **Integração de Dados**
- **Services Necessários**:
  - `IUsuarioPerfilService`:
    - `Task<PerfilDto> ObterMeuPerfilAsync()`: Requisição GET em `/api/usuario/perfil` (usando o JWT para identificar o user no server).
    - `Task AtualizarPerfilAsync(AtualizarPerfilRequestDto request)`: Requisição PUT em `/api/usuario/perfil`.
  - `IMidiaUploadService`:
    - `Task<string> UploadImagemPerfilAsync(FileResult arquivo)`: Envia o binário para um storage (ex: Azure Blob ou AWS S3) e retorna a nova URL pública. Integrado dentro de `SalvarAlteracoesAsync`.
  - `IMediaPicker`: Interface nativa do MAUI para lidar com galeria/câmera.

🔒 **Regras de Negócio e Segurança**
- **Campo de E-mail Bloqueado**: Como o e-mail geralmente atua como ID forte do Identity, a troca de e-mail deve ser proibida aqui. É um campo de visualização apenas (identidade).
- **Validações Client-side**: 
  - Validar CPF se foi formatado/digitado corretamente (se o condomínio exigir consistência de documento).
  - Upload de imagens deve prever limites de tamanho, comprimindo o `FileResult` se for maior que ex: 2MB usando uma biblioteca de manipulação gráfica ou API antes do post.
- **Armazenamento Seguro Local**: Se algum dado sensível (como telefone ou nome em cache) ficar armazenado offline, usar `Preferences` ou banco de dados seguro caso necessário, embora a regra seja buscar da web a cada abertura do perfil.

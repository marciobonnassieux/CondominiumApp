🎨 **Especificação Técnica: Frontend & UI**
Caminho sugerido: `Pages/PerfilDoUsuario/frontend.md`

🖼️ **Estrutura de Layout (PerfilUsuarioPage.xaml)**
- **Page Root**: Empregar um `ContentPage` circundando seu interior em um `ScrollView` que expande verticalmente. Cor de fundo padrão cinza claro para destacar os cards internos brancos.
- **Header**:
  - `Grid` superior com botão de voltar na coluna esquerda (`ImageButton`), logo/título no centro ("Perfil" com TitleLabelStyle) e opcional logo Edifácil na extrema direita.
- **Avatar Interativo (Foto do Perfil)**:
  - Container circular (ex: `Frame` com tamanho igual de altura/largura e CornerRadius igual à metade).
  - Usar uma imagem mapeada com propriedade `Source="{Binding FotoPerfil}"`. 
  - Usar fallback ou placeholder de "silhueta" caso `FotoPerfil` seja null.
  - Sobrescrito num `AbsoluteLayout` ou `Grid`, um pequeno botão circular no canto inferior direito contendo o ícone da câmera associado ao `SelecionarFotoCommand`.
- **Formulário de Dados Pessoais (Cards Estilizados)**:
  - Criar um `VerticalStackLayout` central, agrupando blocos de campos de entrada.
  - Os blocos `Entry` devem estar contidos em `Border` / `Frame` que atua como barra individual:
    - **NOME COMPLETO**: Prefix Icon: Usuário de terno. Entry binded to `NomeCompleto`.
    - **CPF**: Prefix Icon: ID Text ou Card. Adicionar um CommunityToolkit `MaskedBehavior` (ex: Mask="XXX.XXX.XXX-XX") ligando a `Cpf`. Keyboard="Numeric".
    - **TELEFONE**: Prefix Icon: Fone/Phone. Aplicar behavior de máscara para formato celular "(XX) XXXXX-XXXX". Keyboard="Telephone".
    - **E-MAIL CADASTRADO**: Prefix Icon: Envelope. Definir `IsReadOnly="True"` e visualmente o fundo cinza claro para denotar falta de edição. Sufixo direito: Ícone de Cadeado escuro para dar a sugestão exata de que a alteração não é permitida por razões de segurança.
- **Card: Aviso de Privacidade**:
  - Posicionado abaixo do form. Um `Border` de cantos curvos de cor sutil (verde pastel ou azul clarinho) contendo um Shield Icon.
  - Textos de proteção da LGPD: "Segurança de Dados", "Seus dados estão protegidos por criptografia de ponta a ponta e armazenados seguindo normas rigorosas de leis e privacidade".
- **Botão Principal (Salvar)**:
  - Botão Primary ("SALVAR ALTERAÇÕES" com ícone de check) fixado na parte inferior.
  - O valor `IsEnabled` atrelado a `{Binding TemAlteracoesNaoSalvas}`, ficando translúcido até que a View Model detecte mudanças.

💅 **Estilização e UX**
- **Design System (`App.xaml`)**:
  - Aplicar as cores centrais e as definições tipográficas padronizadas do sistema de UI Edifácil. Borda `PrimaryDarkColor` em campos em estado de focus.
- **Upload Progressivo UX**:
  - Ao invocar o picker de imagens (Galeria/Câmera), se a foto for escolhida ela precisa ser mostrada imediatamente (`NovaFoto`) mesmo antes do `SalvarAlteracoesAsync` finalizar o envio no backend, melhorando a percepção visual do app.

♿ **Acessibilidade**
- Aplicar `SemanticProperties.Description="Alterar foto do perfil"` no ícone da câmera.
- Campos com Behavior de máscara podem não ser lidos com a máscara corretamente por leitores de tela dependendo da config do SO. Orientar no `Hint` o formato esperado se necessário.

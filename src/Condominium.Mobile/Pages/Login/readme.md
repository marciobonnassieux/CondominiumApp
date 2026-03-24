# Análise da Tela: Login (Edifácil)

## Objetivo da Tela
A tela de Login é a porta de entrada principal do aplicativo "Edifácil". Seu objetivo é autenticar moradores, funcionários e síndicos, permitindo o acesso às funcionalidades específicas de cada perfil no condomínio.

## Componentes Visuais e Fluxos Identificados
1. **Background e Layout Base**:
   - Fundo com padrão de micro-pontos (dot pattern) em off-white.
   - Card central (Border) branco com cantos arredondados (Radius 24) e sombra difusa para destaque.
   - ScrollView para garantir acessibilidade em telas menores ou com teclado aberto.

2. **Identidade Visual (Header)**:
   - Ícone de prédio estilizado: Composto por um fundo azul escuro (`#001F3F`) e janelas/blocos em amarelo ouro (`#F1C40F`).
   - Logotipo textual: "EDIFÁCIL" em negrito, cor azul escura, com espaçamento entre caracteres.
   - Slogan: "O APP DO CONDOMÍNIO" em cinza médio.

3. **Formulário de Acesso**:
   - **Campo Identificador**: 
     - Rótulo superior: "Email, User ID or CPF".
     - Campo com fundo azul claro suave (`#E8F1FF`).
     - Ícone de usuário (👤) como prefixo visual.
     - Placeholder indicativo: "seuemail@condominio.co".
   - **Campo de Senha**:
     - Rótulo superior: "Password".
     - Campo com fundo azul claro suave.
     - Ícone de cadeado (🔒) como prefixo visual.
     - Botão de alternância de visibilidade (👁) à direita para exibir/ocultar a senha.

4. **Ações e Navegação**:
   - **Botão Principal (LOGIN)**: Botão de largura total em azul escuro com texto em branco e negrito.
   - **Links de Suporte**:
     - "Esqueceu a senha?": Texto sublinhado em cinza para fluxo de recuperação.
     - "Novo por aqui? Criar conta": Chamada para o fluxo de cadastro de novos usuários.

5. **Feedback de Estado**:
   - `ActivityIndicator` (Loading) posicionado abaixo do botão de login para indicar processamento de autenticação.

## Regras de Negócio e Lógicas Necessárias
- **Validação de Entrada**: O campo identificador deve aceitar diferentes formatos (E-mail, CPF ou ID interno). A lógica de backend deve ser capaz de distinguir o tipo de credencial.
- **Segurança**:
  - A senha deve ser ocultada por padrão (`IsPassword="True"`).
  - O botão de "olho" deve alternar dinamicamente o estado de visibilidade da senha.
- **Persistência de Sessão**: Após o login bem-sucedido, o token de autenticação (JWT) deve ser armazenado com segurança (ex: `Preferences` ou `SecureStorage`).
- **Navegação Condicional**: Dependendo do perfil retornado pela API (Síndico, Morador, Porto), o usuário deve ser direcionado para o Dashboard correspondente ou para o Seletor de Contexto caso possua múltiplos vínculos.
- **Tratamento de Erros**: Exibir alertas claros para "Usuário não encontrado", "Senha incorreta" ou "Problemas de conexão com o servidor".

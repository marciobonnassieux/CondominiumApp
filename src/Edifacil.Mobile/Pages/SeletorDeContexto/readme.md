# AnÃ¡lise da Tela: Onde vocÃª deseja atuar hoje? (Seletor de Contexto)

## Objetivo da Tela
Para usuÃ¡rios que possuem mÃºltiplos registros em edifÃ­cios na plataforma, atuar como uma tela de interceptaÃ§Ã£o logo apÃ³s o login. Permite escolher em qual CondomÃ­nio e/ou qual Cargo/Perfil (Morador, SÃ­ndico, Zelador) ele quer entrar na sessÃ£o atual do app.

## Componentes Visuais e Fluxos Identificados
1. **Header Emptystate**:
   - Apenas logo "EDIFÃCIL" Ã  esquerda e sino de notificaÃ§Ã£o Ã  direita. NÃ£o hÃ¡ botÃ£o de "voltar", implicando que seja uma base hierÃ¡rquica.

2. **Topografia Inicial**:
   - Destaque "O APP DO CONDOMÃNIO".
   - TÃ­tulo Grande: "Onde vocÃª deseja atuar hoje?".
   - Subtexto explicativo "Selecione o perfil de acesso para continuar sua jornada.".

3. **Cards de Acesso**:
   - Uma listagem de `Contextos/Tenant` associados a este usuÃ¡rio:
   - **Card 1**: "EdifÃ­cio Solar" subtitulado como "SÃNDICO". Exibe Ã­cone corporativo Ã  esquerda, e um grafismo de edifÃ­cio mesclado suavemente com a seta dourada Ã  direita.
   - **Card 2**: "Residencial Park" subtitulado como "MORADOR". Exibe Ã­cone de apartamento, com marca d'Ã¡gua de casa e seta dourada Ã  direita.
   - Efeito visual importante: Os Ã­cones da direita de cada card possuem grÃ¡ficos (vetores .svg preferencialmente) compondo o contorno de background.

4. **Footer Link**:
   - Label ou botÃ£o Flat na parte inferior da tela "SAIR DA CONTA" junto de um Ã­cone que demonstra egress.

## Regras de NegÃ³cio e LÃ³gicas NecessÃ¡rias
- A lista de Cards deve ser renderizada baseada na resposta de AutenticaÃ§Ã£o/Login (que devolve um Array de "PermissÃµes/Tenants").
- Cada item deve definir no Command duas variÃ¡veis para o framework: Qual o `EdifacilId` selecionado e qual a `Role/Context` escolhida (se a navegaÃ§Ã£o difere muito de Morador vs. SÃ­ndico).
- Esta tela serve como App.MainPage na sequÃªncia do Identity. Ao clicar em um card, a Application Store atualizarÃ¡ o estado global com este "CondomÃ­nio Corrente" e navegarÃ¡ pro TabBar adequado (AppShell navigation base).
- O botÃ£o "Sair da Conta" realiza o Clear local dos Tokens de auth, Preferences e forÃ§a o reset do Root de volta pra "TelaDeLogin".


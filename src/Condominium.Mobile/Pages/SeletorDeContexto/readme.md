# Análise da Tela: Onde você deseja atuar hoje? (Seletor de Contexto)

## Objetivo da Tela
Para usuários que possuem múltiplos registros em edifícios na plataforma, atuar como uma tela de interceptação logo após o login. Permite escolher em qual Condomínio e/ou qual Cargo/Perfil (Morador, Síndico, Zelador) ele quer entrar na sessão atual do app.

## Componentes Visuais e Fluxos Identificados
1. **Header Emptystate**:
   - Apenas logo "EDIFÁCIL" à esquerda e sino de notificação à direita. Não há botão de "voltar", implicando que seja uma base hierárquica.

2. **Topografia Inicial**:
   - Destaque "O APP DO CONDOMÍNIO".
   - Título Grande: "Onde você deseja atuar hoje?".
   - Subtexto explicativo "Selecione o perfil de acesso para continuar sua jornada.".

3. **Cards de Acesso**:
   - Uma listagem de `Contextos/Tenant` associados a este usuário:
   - **Card 1**: "Edifício Solar" subtitulado como "SÍNDICO". Exibe ícone corporativo à esquerda, e um grafismo de edifício mesclado suavemente com a seta dourada à direita.
   - **Card 2**: "Residencial Park" subtitulado como "MORADOR". Exibe ícone de apartamento, com marca d'água de casa e seta dourada à direita.
   - Efeito visual importante: Os ícones da direita de cada card possuem gráficos (vetores .svg preferencialmente) compondo o contorno de background.

4. **Footer Link**:
   - Label ou botão Flat na parte inferior da tela "SAIR DA CONTA" junto de um ícone que demonstra egress.

## Regras de Negócio e Lógicas Necessárias
- A lista de Cards deve ser renderizada baseada na resposta de Autenticação/Login (que devolve um Array de "Permissões/Tenants").
- Cada item deve definir no Command duas variáveis para o framework: Qual o `CondominiumId` selecionado e qual a `Role/Context` escolhida (se a navegação difere muito de Morador vs. Síndico).
- Esta tela serve como App.MainPage na sequência do Identity. Ao clicar em um card, a Application Store atualizará o estado global com este "Condomínio Corrente" e navegará pro TabBar adequado (AppShell navigation base).
- O botão "Sair da Conta" realiza o Clear local dos Tokens de auth, Preferences e força o reset do Root de volta pra "TelaDeLogin".

# Análise da Tela: Dashboard Portaria

## Objetivo da Tela
Exibir métricas em tempo real e ações rápidas para o porteiro do condomínio poder atuar de forma ágil, com botões bem visíveis para facilitar o recebimento de encomendas, notificações de moradores, e registros de entradas.

## Componentes Visuais e Fluxos Identificados
1. **Header Principal**:
   - Foto ou Avatar do Perfil.
   - Logo/Título "Edifácil - O APP DO CONDOMÍNIO".
   - Botão de Notificações em formato de Sino.
   - Título principal: "Dashboard Portaria".
   - Subtítulo: Identificação do usuário logado (ex: "Porteiro Carlos - 15 Out 2024").

2. **Ações Primárias (Acesso Rápido)**:
   - Botão em destque de largura total (Largo): "RECEBER ENCOMENDAS" em fundo azul escuro com ícone de caixa.
   - Grid de duas colunas com 2 botões em formato de card:
     - "NOTIFICAR MORADORES" (fundo branco, ícone de sino com alerta).
     - "REGISTRAR VISITANTE" (fundo branco, ícone de adicionar pessoa).

3. **Seção "Resumo do Turno"**:
   - Indicador de tempo: label "TEMPO REAL" à direita.
   - Cards Estatísticos Grandes:
     - "ENCOMENDAS PENDENTES": Exibe o número grande "45" e um ícone em azul.
     - "NOVOS VISITANTES": Exibe o número grande "12" e um ícone em amarelo/laranja.

4. **Seção "Atividade Recente"**:
   - Link de ação: "Ver tudo" alinhado à direita.
   - Lista Vertical Simples de Eventos (Event Stream):
     - Exemplo 1: Card de entrega "Entrega Amazon - Apto 102" e subtítulo "Registrado há 15 min", ícone azul.
     - Exemplo 2: Card de visitante "Visitante: Marcos Silva (Apto 405)" e subtítulo "Entrada autorizada há 32 min", ícone laranja/dourado.

5. **Navegação Inferior (Bottom Tab Bar)**:
   - Abas disponíveis: INÍCIO (Ativa/Dark Blue), ENCOMENDAS, VISITANTES, SEGURANÇA.

## Regras de Negócio e Lógicas Necessárias
- A Dashboard deve buscar as informações resumidas (estatísticas e atividades) de modo síncrono ou com atualizações em tempo real (como via WebSockets/SignalR).
- As ações de "Receber Encomendas", "Notificar Moradores" e "Registrar Visitante" atuam como rotas primárias de navegação para fluxos complexos.
- Os cartões informativos no "Resumo do Turno" indicam quantos processos precisam de atenção neste turno específico.
- A "Atividade Recente" deve ordenar os eventos do mais recente para o mais antigo e ter limite visual de itens, redirecionando o fluxo inteiro pelo botão "Ver tudo".

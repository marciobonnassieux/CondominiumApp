# 🤖 Visão do PO (sk-03): Confirmação de Logout

## User Stories
*   **Como** usuário autenticado, **eu quero** visualizar uma tela de confirmação ao clicar em "Sair", **para que** eu evite deslogar por acidente e perca meu contexto de uso.

## Critérios de Aceite
*   A tela deve exibir uma mensagem clara: "Deseja realmente sair do sistema?".
*   Deve possuir dois botões: "Sim, Sair" e "Cancelar".
*   O botão "Sim, Sair" deve encerrar a sessão e redirecionar para a tela de Login.
*   O botão "Cancelar" deve fechar a tela de confirmação e manter o usuário na tela atual.
*   A interface deve seguir o padrão visual de diálogos do sistema (MAUI/Mobile).

## Fluxo de Exceção
*   Se houver erro ao encerrar a sessão (ex: falha de API), exibir notificação: "Erro ao realizar logout. Tente novamente.".

## Priorização
*   **Core**: Exibição da tela de confirmação e encerramento da sessão.
*   **Nice to have**: Animação de transição suave ao exibir o diálogo.

# 🤖 Visão do Arquiteto (sk-04): TASK-LOGOUT-001

## Desenho Técnico
*   **TASK-ID**: TASK-LOGOUT-001
*   **Layer/Stack**: Edifacil.Mobile (UI/MVVM) e Edifacil.Core (Se houver necessidade de salvar algum estado no log).
*   **Namespace**: Edifacil.Mobile.Views / Edifacil.Mobile.ViewModels

## Contratos e Interfaces
*   A confirmação deve ser um `Popup` (CommunityToolkit.Maui) ou apenas uma `LogoutConfirmationPage`.
*   A ViewModel deve expor dois Commands: `LogoutCommand` e `CancelCommand`.
*   O serviço de autenticação (`IAuthService`) deve ser injetado para realizar o logout.

## Semáforo de UI
*   A tela não existe em `Edifacil/Docs/Stitch/`. Marcar como `NEW`.

## Classificação (novas_telas.md)
*   `TASK-LOGOUT-001 | [PENDING_STITCH] | [TYPE: NEW] | [LogoutConfirmationView]`

## Estratégia de Desacoplamento
*   Utilizar Mensageria (Messenger) ou Navegação nativa para desacoplar a chamada de logout das páginas mestras.

## Mapeamento de Banco
*   Nenhuma alteração de banco necessária.

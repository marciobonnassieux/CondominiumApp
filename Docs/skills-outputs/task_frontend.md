# 🤖 Task Frontend (sk-05): TASK-LOGOUT-001

## Objetivo: Implementar tela de confirmação de logout.

### Especificações:
- **View**: Criar `LogoutConfirmationPopup.xaml` utilizando o CommunityToolkit.Maui.
- **Visual**: Seguir o design em `Edifacil/Docs/Stitch/task-logout-001.png`.
- **ViewModel**: Criar `LogoutConfirmationViewModel.cs`.
- **Bindings**:
  - `ConfirmLogoutCommand`: Chama o serviço de autenticação para invalidar o token e navegar para a `LoginPage`.
  - `CancelCommand`: Fecha o popup (ou navega de volta).
- **Injeção**: Garantir `IAuthService` via construtor.

### Localização:
- `paths.mobile` -> `Views/Popups/` e `ViewModels/`.

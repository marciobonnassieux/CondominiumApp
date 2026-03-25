⚙️ **Especificação Técnica: Backend & Lógica**
Caminho sugerido: `Pages/GestaoDeVinculosECargos/backend.md`

🧩 **Arquitetura da ViewModel (GestaoDeVinculosViewModel)**
- **Propriedades Observáveis (`[ObservableProperty]`)**:
  - `ObservableCollection<VinculoUsuarioModel> Equipe`: Lista de funcionários/usuários vinculados ao condomínio.
  - `ObservableCollection<CargoModel> CargosDisponiveis`: Lista de cargos possíveis para atribuição no Picker.
  - `string TermoBusca`: Texto digitado na barra de pesquisa (ícone de lupa) para filtrar a lista.
  - `bool IsBusy`: Indica carregamento de dados ou salvamento.
  - `bool TemAlteracoesNaoSalvas`: Controle de estado para habilitar/desabilitar o botão "SALVAR ALTERAÇÕES".
  - `int TotalUsuarios`: Propriedade calculada baseada na quantidade de itens em `Equipe`.
- **Comandos (`[RelayCommand]`)**:
  - `CarregarDadosAsync()`: Busca a lista de cargos disponíveis e a equipe vinculada atual.
  - `BuscarUsuarioCommand()`: Filtra a `Equipe` localmente com base no `TermoBusca`.
  - `VincularNovoCpfCommand()`: Abre diálogo de input ou navega para fluxo de novo vínculo.
  - `SalvarAlteracoesAsync()`: Envia as modificações de cargo em lote para a API.
  - `CargoAlteradoCommand(VinculoUsuarioModel usuario)`: Disparado quando o Picker altera um valor. Atualiza `TemAlteracoesNaoSalvas` para `true`.
- **Navegação**:
  - `INavigationService` para voltar (`GoBackAsync`) e possivelmente para navegar para a tela de "Vincular Novo Usuário" (se for uma página inteira e não um popup).
- **Lógica de Filtragem e Edição**:
  - A ViewModel deve manter uma lista original em memória para comparar os cargos alterados e enviar apenas o delta (modificados) no `SalvarAlteracoesAsync`.

🌐 **Integração de Dados**
- **Services Necessários**:
  - `IGestaoEquipeService`:
    - `Task<IEnumerable<CargoModel>> ObterCargosCondominioAsync(Guid condominioId)`: GET em `/api/condominio/{id}/cargos`.
    - `Task<IEnumerable<VinculoUsuarioModel>> ObterEquipeVinculadaAsync(Guid condominioId)`: GET em `/api/condominio/{id}/equipe`.
    - `Task AtualizarCargosEmLoteAsync(IEnumerable<AlteracaoCargoDto> alteracoes)`: PUT/PATCH em `/api/condominio/{id}/equipe/cargos` enviando UserId e novo CargoId.
    - `Task<UsuarioBasicoDto> BuscarUsuarioPorCpfAsync(string cpf)`: GET para validar a vinculação antes de acionar. (Possivelmente usado no command de VincularNovo).

🔒 **Regras de Negócio e Segurança**
- **Privacidade de Dados (LGPD)**: O Model que trafega do backend deve trazer o CPF já mascarado da API (ex: `***.452.118-**`); o front-end não deve ter o dado cru para mascarar localmente.
- **Autorização**: A tela e os endpoints correspondentes devem validar role claim de Administrador ou Síndico.

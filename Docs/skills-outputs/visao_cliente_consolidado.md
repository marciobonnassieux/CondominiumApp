# 📑 Visão Consolidada do Cliente (Edifácil)

# O Que Eu Vejo:
Atualmente, o aplicativo móvel do **Edifácil** apresenta uma interface moderna em tons escuros e dourados:
*   **Acesso Seguro:** Tela de login protegida onde moradores e funcionários podem entrar com email, CPF ou identificação única.
*   **Painel Central (Dashboard):** Tela inicial que organiza as principais ferramentas do condomínio.
*   **Gestão de Unidades:** Visualização de todos os blocos e apartamentos cadastrados no sistema.
*   **Monitor de Encomendas:** Interface dedicada para cadastro de novos pacotes que chegam à portaria.
*   **Scanner Inteligente:** Uma ferramenta avançada de câmera que utiliza visão computacional para ler etiquetas e códigos de barras automaticamente.

# O Que Eu Faço:
*   **Entrada de Usuário:** Realizo a verificação de credenciais para garantir que apenas pessoas autorizadas acessem as informações.
*   **Cadastro de Pacotes:** Permito que o porteiro capture o código de uma encomenda usando a câmera e vincule-a imediatamente a um morador.
*   **Consulta Rápida:** Facilitamos a busca e listagem de apartamentos e blocos para agilizar o atendimento na portaria.
*   **Navegação Segura:** Possibilitamos o encerramento da sessão com confirmação para evitar saídas acidentais.

# Regras de Negócio:
*   **Segurança de Dados:** O sistema exige usuário e senha válidos para liberar o acesso ao banco de dados das unidades.
*   **Vinculação de Encomendas:** Uma entrega só pode ser registrada se estiver corretamente associada a uma unidade (bloco e apartamento) existente.
*   **Confirmação de Ações:** Operações sensíveis, como sair da conta, requerem uma validação extra do usuário (Confirmar/Cancelar).

# Automações:
*   **Leitura Automática:** A câmera reconhece códigos de barras em tempo real, eliminando a necessidade de digitação manual de números longos e complexos.
*   **Sincronização na Nuvem:** Assim que um pacote é recebido, a informação é enviada instantaneamente para o servidor central, mantendo o histórico atualizado em todos os dispositivos.

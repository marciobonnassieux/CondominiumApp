---
## TASK-LOGOUT-001 | Prompt para Stitch
---

**Layout**: 
- Desenhe uma pequena tela de confirmação (Dialog/Popup) centralizada.
- Fundo semitransparente (overlay) para o restante da aplicação.
- Card principal com fundo branco, cantos arredondados (CornerRadius: 16).

**Componentes**:
- Um ícone de Logout no topo (ex: porta de saída ou exclamação).
- Título: "Confirmação de Saída" (Fonte: Bold, Cor: #2C3E50).
- Texto Descritivo: "Deseja realmente encerrar sua sessão e sair do aplicativo?".
- Dois botões lado a lado:
  - Botão "Sair": Cor de fundo Vermelho Suave (#E74C3C), texto Branco.
  - Botão "Cancelar": Cor de fundo Cinza (#95A5A6) ou apenas bordas, texto Cinza Escuro.

**Styles**:
- Sombras sutis (Elevation: 4).
- Espaçamento interno (Padding: 24).
- Alinhamento central de todos os elementos.

**Estados**:
- Quando o botão "Sair" for clicado, o ícone deve ser substituído por um ActivityIndicator (carregamento) antes da transição.
---

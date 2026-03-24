# Análise da Tela: Finalizar Recebimento (Resumo e Assinatura)

## Objetivo da Tela
Validar as informações consolidadas de uma jornada de entrega (lote de pacotes lidos e destinados aos moradores) e capturar a assinatura eletrônica do entregador ou responsável pela portaria para fechar o processo.

## Componentes Visuais e Fluxos Identificados
1. **Header**:
   - Botão Back (seta para esquerda).
   - Título "Finalizar Recebimento".
   - Logo EDIFÁCIL à direita.

2. **Seção "RESUMO DO LOTE"**:
   - Título menor e badge indicando o subtotal (ex: "2 ENTREGAS").
   - Lista de cards consolidando as entregas por morador:
     - Card do Apto 101: "Bloco A - Apto 101", subtítulo "2 volumes registrados", ícone de caixa e indicador de sucesso.
     - Card do Apto 304: "Bloco B - Apto 304", subtítulo "1 volume registrado", ícone de caixa e indicador de sucesso.

3. **Seção "ASSINATURA DO ENTREGADOR"**:
   - Subtítulo da seção.
   - Canvas/SignaturePad (Área de desenho livre):
     - Fundo estático pontilhado/texturizado para parecer papel ou frame especial.
     - Placeholder ou Hint no meio: ícone de rabisco com texto "Assine neste campo".
     - Botão "LIMPAR": Botão menor, embutido abaixo e à direita no canvas para resetar a assinatura.

4. **Painel de Instrução / Alerta**:
   - Card amarelo claro alertando "PROTOCOLO DE SEGURANÇA" com a instrução de que ao finalizar, os moradores vinculados receberão notificações push instantaneamente.

5. **Ação Primária e Footer**:
   - Botão grande e fixo no final (ou fim do scroll): "FINALIZAR LOTE E NOTIFICAR MORADORES", contendo ícone de decolagem de avião de papel apontando à direita.
   - Presença da navegação (Bottom Tab Bar), aba "VOLUMES" ativa.

## Regras de Negócio e Lógicas Necessárias
- A tela requer o uso de um controle customizado de SignaturePad capaz de capturar traços da UI e exportá-los para imagem (PNG/Base64) formatada a ser anexada no request de finalização do lote.
- O botão "Limpar" deve invocar a propriedade/evento "Clear()" do componente SignaturePad.
- A finalização do lote precisa submeter para a API uma lista de Ids de volumes agregada com as respectivas Unidades destino e a assinatura digitalizada.
- A API se encarrega de disparar as notificações Push para os moradores afetados. Somente exibir sucesso se a requisição de finalização voltar Status 200.

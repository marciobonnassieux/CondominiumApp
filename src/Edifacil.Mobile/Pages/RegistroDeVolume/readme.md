# Análise da Tela: Registro de Volume

## Objetivo da Tela
Habilitar a portaria (ou outro usuário habilitado) a utilizar a câmera do dispositivo móvel como scanner de pacotes/encomendas, otimizando o processo de entrada de volumes mediante leitura de etiquetas/código de barras/OCR.

## Componentes Visuais e Fluxos Identificados
1. **Header Principal**:
   - Botão de voltar (seta para esquerda).
   - Título: "Registro de Volume".
   - Botão secundário texto à direita: "PRÓXIMA UNIDADE" (provavelmente avança o fluxo sem escanear a placa ou passa pra o controle de qual apartamento esse pacote pertence).

2. **Visualização Central (Viewfinder da Câmera)**:
   - Feed ao vivo da câmera renderizado na tela toda.
   - Retângulo/frame transparente com bordas alaranjadas orientando ao escaneamento contínuo ("ALINHE A ETIQUETA"). Serve como guide visual paramétrico para o OCR ou leitor.

3. **Bottom Sheet "Pacotes Registrados"**:
   - Drawer na base da tela agindo como um log consolidado das leituras que acabaram de acontecer naquela sessão.
   - Lista os pacotes registrados até o momento:
     - Card do Volume 1 (Ícone, "Volume 123456", "Lido com sucesso", checkmark laranja/dourado).
     - Card do Volume 2 (Ícone, "Volume 987654", "Lido com sucesso", checkmark laranja/dourado).

## Regras de Negócio e Lógicas Necessárias
- Integração profunda com bibliotecas de visão computacional (ex: ML Kit da Cloud para leitura de código de rastreio, ou MAUI ZXing para barcodes).
- Captura contínua: Após alinhar uma etiqueta de sucesso, o dispositivo pode fazer um Haptic Feedback (vibração curta), registrar na lista de pacotes no background, e manter o visualizador ativo para a próxima leitura.
- O botão "Próxima unidade" provavelmente remete o lote de códigos lidos para uma próxima tela que vincula tudo isso ao respectivo apartamento, finalizando o recebimento em massa.

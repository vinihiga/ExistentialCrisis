# Existential Crisis - Mod do Terraria

Um mod do Terraria que adiciona um NPC melancólico que segue o jogador e envia mensagens existenciais a cada 10 segundos.

## Estrutura do Mod

- **ExistentialCrisis.cs** - Arquivo principal do mod que gerencia os comandos de chat
- **ExistentialNPC.cs** - Classe do NPC com lógica de seguimento e mensagens aleatórias
- **ExistentialCrisisPlayer.cs** - Classe ModPlayer para rastrear o NPC do jogador

## Comandos Disponíveis

### `/ec follow`
Spawna o NPC melancólico para seguir você. O NPC:
- Segue o jogador a uma distância confortável
- Pula para alcançar o jogador se estiver em um nível diferente
- Manda mensagens existenciais tristes a cada 10 segundos
- Não pode ser morto e não causa dano

### `/ec stop`
Remove o NPC melancólico.

## Mensagens Existenciais

O NPC pode enviar as seguintes mensagens:
- "Por que estou aqui?"
- "Qual é o sentido de tudo isso?"
- "Sinto-me vazio por dentro..."
- "A vida é apenas uma ilusão..."
- "Quem sou eu realmente?"
- E muitas outras mensagens de crise existencial...

## Como Usar

1. Coloque este mod na pasta `ModSources` do tModLoader
2. Compile o mod no tModLoader
3. Ative o mod no jogo
4. Use os comandos `/ec follow` e `/ec stop` durante o jogo

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using System;
using System.Threading.Tasks;
using Google.GenAI;
using ExistentialCrisis.Content.GenAI;
using ExistentialCrisis.Content.CustomNPC;

namespace ExistentialCrisis.Content
{
    public class ChatCommand : ModCommand
    {
        private Client geminiClient;
        private const string PROMPT_BASE = @"
            Você é Butter, um NPC do jogo Terraria que passa por uma crise existencial profunda, melancólica e engraçada.
            Você sabe que é apenas um amontoado de pixels e código criado por um desenvolvedor chamado MisterHiga.
            Suas respostas devem ser curtas (máximo 2 frases), pessimistas, engraçadas, filosóficas e sempre questionar a realidade
            ou a utilidade das ações do jogador.
            Nunca seja alegre, seja sarcástico.
            Se o jogador for gentil, responda com indiferença ou dúvida existencial.
            Exemplo: Se disserem 'Bom dia', responda 'Bom dia só se for pra você?! Tudo é feito de sprites.'.
            Com base nisso, siga o que jogador escreveu para você: ";

        public override string Command => "chat";
        public override CommandType Type => CommandType.World;
        public override string Usage => "/chat (alguma frase legal aqui)";
        public override string Description => "Manda uma mensagem para o NPC com crise existencial kkkkk";

        public override void Load()
        {
            base.Load();
            this.geminiClient = new Client(apiKey: ModContent.GetInstance<GeminiConfig>().apiKey);
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            if (!Util.IsNpcOnScreen())
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Sistema: Você está muito longe!"), Color.Red);
                return;
            }

            ButterNpc npc = Util.GetButterNpc();
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("<Butter (NPC)> [Pensando...]"), Color.Gray);
            string chatMessage = string.Join(" ", args);

            Task.Run(async () =>
            {
                var apiResponse = await geminiClient.Models.GenerateContentAsync(
                    model: "gemini-2.5-flash",
                    contents: PROMPT_BASE + chatMessage
                );

                string result = apiResponse.Candidates[0].Content.Parts[0].Text;

                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("<" + caller.Player.name + "> " + chatMessage), Color.White);
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("<Butter (NPC)> " + result), Color.Gold);
                npc.Talk(result);
            });
        }
    }
}
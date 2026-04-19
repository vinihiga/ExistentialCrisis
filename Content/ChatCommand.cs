using ExistentialCrisis.Content.CustomNPC;
using ExistentialCrisis.Content.Config;
using Google.GenAI;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Terraria.Chat;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;

namespace ExistentialCrisis.Content
{
    public class ChatCommand : ModCommand
    {
        private Client geminiClient;
        public override string Command => "chat";
        public override CommandType Type => CommandType.World;
        public override string Usage => "/chat (alguma frase legal aqui)";
        public override string Description => "Manda uma mensagem para o NPC com crise existencial.";

        public override void Load()
        {
            base.Load();

            try
            {
                this.geminiClient = new Client(apiKey: ModContent.GetInstance<GeminiConfig>().apiKey);
            } catch {
                ChatHelper.BroadcastChatMessage(
                    NetworkText.FromLiteral("Sistema: Não foi possível iniciar o Gemini. Cheque sua chave e o status de serviço!"),
                    Color.Red
                );
            }
        }

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            ButterNpc npc = ButterNpc.GetSomeInstance();
            if (npc == null || !Util.IsNpcOnScreen(npc.NPC))
            {
                ChatHelper.BroadcastChatMessage(
                    NetworkText.FromLiteral("Sistema: Não encontramos o NPC Butter na sua tela!"),
                    Color.Red
                );
                return;
            }

            switch (args[0].ToLower())
            {
                case "follow":
                case "me-siga":
                    npc.SetTarget(Main.LocalPlayer);
                    break;
                case "stop":
                case "pare-de-seguir":
                    npc.StopTargeting();
                    break;
                default:
                    if (geminiClient == null)
                    {
                        return;
                    }

                    string chatMessage = string.Join(" ", args);
                    this.HandleGemini(npc, caller, chatMessage);
                    break;
            }
        }

        private void HandleGemini(ButterNpc npc, CommandCaller caller, string input)
        {
            npc.Talk("[Pensando...]", Color.Gray);

            Task.Run(async () =>
            {
                var apiResponse = await geminiClient.Models.GenerateContentAsync(
                    model: Constants.GEMINI_MODEL,
                    contents: Constants.PROMPT_BASE + input
                );

                string result = apiResponse.Candidates[0].Content.Parts[0].Text;

                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("<" + caller.Player.name + "> " + input), Color.White);
                npc.Talk(result, Color.Gold);
            });
        }

        public struct Constants
        {
            public static readonly string GEMINI_MODEL = "gemini-2.5-flash";
            public static readonly string PROMPT_BASE = @"
                Você é Butter, um NPC do jogo Terraria que passa por uma crise existencial profunda, melancólica e engraçada.
                Você sabe que é apenas um amontoado de pixels e código criado por um desenvolvedor chamado MisterHiga.
                Suas respostas devem ser curtas (máximo 2 frases), pessimistas, engraçadas, filosóficas e sempre questionar a realidade
                ou a utilidade das ações do jogador.
                Nunca seja alegre, seja sarcástico.
                Se o jogador for gentil, responda com indiferença ou dúvida existencial.
                Exemplo: Se disserem 'Bom dia', responda 'Bom dia só se for pra você?! Tudo é feito de sprites.'.
                Com base nisso, siga o que jogador escreveu para você: ";
        }
    }
}
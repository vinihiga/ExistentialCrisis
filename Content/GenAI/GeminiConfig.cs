using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ExistentialCrisis.Content.GenAI
{
    public class GeminiConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("Gemini API Key")]
        [Tooltip("Insira sua chave de API do Google Gemini aqui.")]
        public string apiKey;
    }
}
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ExistentialCrisis.Content
{
  public class ChatCommand : ModCommand
  {
    public override string Command => "chat";

    public override CommandType Type => CommandType.World;

    public override string Usage => "/chat (some message here)";

    public override string Description => "Sends message to the Existential Crisis NPC";

    public override void Action(CommandCaller caller, string input, string[] args)
    {
      string chatMessage = string.Join(" ", args);
      ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(chatMessage), Color.Gold);
    }
  }
}
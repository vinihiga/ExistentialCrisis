using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace ExistentialCrisis.Content.CustomNPC
{
  public class ButterTrollingSystem
  {
    private ButterNpc butter;

    public ButterTrollingSystem(ButterNpc butter)
    {
      this.butter = butter;
    }

    public void Desintegrate(Player player)
    {
      var reason = PlayerDeathReason.ByCustomReason($"{player.name} foi desintegrado por Butter sem querer.");
      player.KillMe(reason, 9999, 0);
      butter.Talk("Ops, ativei o módulo 'desintegrar' em vez de ativar o módulo de 'seguir'.", Color.Gold);
    }

    public void MurderInDarkArea(Player player)
    {
      int butterTileX = (int)(butter.NPC.Center.X / 16f);
      int butterTileY = (int)(butter.NPC.Center.Y / 16f);

      Color npcTileColor = Lighting.GetColor(butterTileX, butterTileY);

      int playerTileX = (int)(player.Center.X / 16f);
      int playerTileY = (int)(player.Center.Y / 16f);

      Color playerTileColor = Lighting.GetColor(playerTileX, playerTileY);

      bool isButterInsideDarkArea = npcTileColor.R < 30 && npcTileColor.G < 30 && npcTileColor.B < 30;
      bool isPlayerInsideDarArea = playerTileColor.R < 30 && playerTileColor.G < 30 && playerTileColor.B < 30;

      if (isButterInsideDarkArea && isPlayerInsideDarArea)
      {
        var reason = PlayerDeathReason.ByCustomReason($"{player.name} foi assassinado por Butter em uma área escura. Será que ele é um psicopata? Aliás, robôs são psicopatas?!");
        player.KillMe(reason, 9999, 0);
        butter.Talk("Mwahaha.", Color.Gold);
      }
    }
  }
}
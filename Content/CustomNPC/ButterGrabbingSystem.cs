using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace ExistentialCrisis.Content.CustomNPC
{
  public class ButterGrabbingSystem
  {
    private ButterNpc butter;

    public ButterGrabbingSystem(ButterNpc butter)
    {
      this.butter = butter;
    }

    public Item GetNearestItem()
    {
      for (int i = 0; i < Main.maxItems; i++)
      {
        Item actualItem = Main.item[i];

        if (actualItem.active && actualItem.type > ItemID.None && actualItem.stack > 0)
        {
          float distance = Vector2.Distance(butter.NPC.Center, actualItem.Center);

          if (distance <= ButterNpc.Constants.MAX_ENTITY_DISTANCE)
          {
            return actualItem;
          }
        }
      }

      return null;
    }
  }
}
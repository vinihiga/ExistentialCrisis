using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace ExistentialCrisis.Content.CustomNPC
{
  public class ButterGrabbingSystem
  {
    private ButterNpc butter;
    private Queue<Item> bag;

    public ButterGrabbingSystem(ButterNpc butter)
    {
      this.butter = butter;
      this.bag = new Queue<Item>(3);
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

    public void AddToBag(Item item)
    {
      Item safeItem = item.Clone();
      item.active = false;
      item.type = ItemID.None;
      bag.Enqueue(safeItem);
    }

    public Item RemoveFromBag()
    {
      Item someItem = bag.Dequeue();
      someItem.active = true;
      return someItem;
    }

    public bool IsSomeItemInsideTheBag()
    {
      return bag.Count > 0;
    }
  }
}
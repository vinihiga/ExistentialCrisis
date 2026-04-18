using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ExistentialCrisis.Content
{
    public static class Util
    {
        public static bool IsNpcOnScreen()
        {
            int npcType = ModContent.NPCType<ButterNpc>();
            foreach (NPC npc in Main.npc)
            {
                if (!npc.active && npc.type != npcType)
                {
                    continue;
                }

                int w = NPC.sWidth + NPC.safeRangeX * 2;
                int h = NPC.sHeight + NPC.safeRangeY * 2;
                Rectangle npcScreenRect = new Rectangle((int)npc.Center.X - w / 2, (int)npc.Center.Y - h / 2, w, h);

                if (AnyPlayerInRect(npcScreenRect))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool AnyPlayerInRect(Rectangle rect)
        {
            foreach (Player player in Main.player)
            {
                if (player.active && player.getRect().Intersects(rect))
                {
                    return true;
                }
            }
            return false;
        }

        public static ButterNpc GetButterNpc()
        {
            int npcType = ModContent.NPCType<ButterNpc>();
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.type == npcType)
                {
                    return npc.ModNPC as ButterNpc;
                }
            }
            return null;
        }
    }
}
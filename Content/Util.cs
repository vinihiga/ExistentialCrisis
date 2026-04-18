using ExistentialCrisis.Content.CustomNPC;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ExistentialCrisis.Content
{
    public static class Util
    {
        public static bool IsNpcOnScreen(NPC npc)
        {
            int width = NPC.sWidth + NPC.safeRangeX;
            int height = NPC.sHeight + NPC.safeRangeY;

            Rectangle npcScreenRect = new Rectangle(
                (int)(npc.Center.X - (width / 2)),
                (int)(npc.Center.Y - (height / 2)),
                width,
                height
            );

            return IsLocaPlayerInRect(npcScreenRect);
        }

        private static bool IsLocaPlayerInRect(Rectangle rect)
        {
            return Main.LocalPlayer.getRect().Intersects(rect);
        }
    }
}
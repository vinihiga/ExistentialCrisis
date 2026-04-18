using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ExistentialCrisis.Content.CustomNPC;

namespace ExistentialCrisis
{
    public class ExistentialCrisis : ModSystem
    {
        public override void PostWorldLoad()
        {
            base.PostWorldGen();
            this.SpawnButter();
        }

        private void SpawnButter()
        {
            // Apenas o host pode spawnar o Butter.
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }

            int npcType = ModContent.NPCType<ButterNpc>();

            if (NPC.AnyNPCs(npcType))
            {
                return;
            }

            int npcIndex = NPC.NewNPC(null, Main.spawnTileX * 16, Main.spawnTileY * 16, npcType);

            if (Main.npc[npcIndex].active)
            {
                Main.npc[npcIndex].homeless = true;
            }
        }
    }
}
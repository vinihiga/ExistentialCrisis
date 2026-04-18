using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExistentialCrisis.Content.CustomNPC
{
    [AutoloadHead]
    public class ButterNpc : ModNPC
    {
        // Public props
        public override string Texture => "ExistentialCrisis/Content/CustomNPC/ButterNpc";

        // Private props
        private int chatTimer = 0;
        private Player target;

        // Methods

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 9;
            NPCID.Sets.AttackFrameCount[NPC.type] = 4;
            NPCID.Sets.DangerDetectRange[NPC.type] = 700;
            NPCID.Sets.AttackType[NPC.type] = 0;
            NPCID.Sets.AttackTime[NPC.type] = 90;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = 4;
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            AnimationType = NPCID.Guide;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>(["Butter"]);
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            // Isso impede que o sistema de "moradia" do jogo spawne um segundo NPC.
            return false;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            Main.npcChatText = Constants.TUTORIAL_INFO;
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shopName)
        {
            Main.npcChatText = Constants.TUTORIAL_INFO;
        }

        public override void AI()
        {
            base.AI();
            chatTimer++;

            if (chatTimer >= Constants.TIME_TO_SPEAK_WHEN_IDLE)
            {
                string message = Main.rand.Next(Constants.MESSAGES);
                Talk(message);
            }

            if (this.target != null)
            {
                this.Follow(this.target);
            }
        }

        public void Talk(string text)
        {
            chatTimer = 0;
            int index = CombatText.NewText(NPC.getRect(), Color.White, text);

            if (index >= 0 && index < Main.maxCombatText)
            {
                Main.combatText[index].lifeTime = Constants.COMBAT_TEXT_LIFESPAN;
            }
        }

        public void SetTarget(Player player)
        {
            this.target = player;
        }

        public void StopTargeting()
        {
            this.target = null;
        }

        private void Follow(Player player)
        {
            if (player == null || !player.active)
            {
                return;
            }

            Vector2 direction = player.Center - NPC.Center;
            float distance = direction.Length();

            if (distance > 50f)
            {
                direction.Normalize();
                NPC.velocity = direction * 2f;
            }
            else
            {
                NPC.velocity *= 0.9f;
            }
        }

        public static ButterNpc GetSomeInstance()
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

        public struct Constants
        {
            public const int TIME_TO_SPEAK_WHEN_IDLE = 1800; // 30s, pois consideramos geramos 60 quadros p/ segundo 
            public const int COMBAT_TEXT_LIFESPAN = 300;     // 5s

            public static readonly string TUTORIAL_INFO = "(Para falar comigo digite pelo /chat)";

            public static readonly List<string> MESSAGES = new List<string>
            {
                "O meu modder me abandonou?",
                "Mais um dia... ou seria o mesmo dia de ontem?",
                "Eu sou apenas um código em Java? Espero que não",
                "Sinto que minhas memórias foram escritas usando GPT.",
                "Butter... um nome engraçado para alguém tão vazio. O modder deve ser fã de Rick and Morty..."
            };
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
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
            NPC.aiStyle = -1;
            NPC.noTileCollide = false;
            NPC.noGravity = false;
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
                this.Talk(message, Color.Gold);
            }

            if (this.target != null)
            {
                this.MoveTo(this.target);
            }
        }

        public void Talk(string text, Color color)
        {
            chatTimer = 0;
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("<Butter (NPC)> " + text), color);
        }

        public void SetTarget(Player player)
        {
            this.target = player;
        }

        public void StopTargeting()
        {
            this.target = null;
        }

        private void MoveTo(Player player)
        {
            if (player == null || !player.active) return;

            float distance = Vector2.Distance(player.Center, NPC.Center);

            if (distance <= 100f)
            {
                NPC.velocity.X = 0f;
                return;
            }
            else if (distance > 800f)
            {
                NPC.position = player.position;
                this.Talk(
                    "Me espera, caramba...",
                    Color.Gold
                );
                return;
            }

            float directionX = (player.Center.X > NPC.Center.X) ? 1f : -1f;
            float maxSpeed = 2f;
            float acceleration = 0.1f;
            NPC.spriteDirection = (int)directionX;

            // Accelerates if we didn't met the maxSpeed
            if (Math.Abs(NPC.velocity.X) < maxSpeed)
            {
                NPC.velocity.X += directionX * acceleration;
            }

            // Forces jump if the NPC was collided
            if (NPC.collideX && NPC.velocity.Y == 0f)
            {
                NPC.velocity.Y = -6f; // Força do pulo
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
                "Eu sou apenas um código em Java? Espero que não. Espero que seja C# ou qualquer outra linguagem.",
                "Sinto que minhas memórias foram escritas usando GPT.",
                "Butter... um nome engraçado para alguém tão vazio. O modder deve ser fã de Rick and Morty..."
            };
        }
    }
}

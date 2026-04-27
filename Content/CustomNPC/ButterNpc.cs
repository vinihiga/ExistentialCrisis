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
        // Propriedades Públicas
        public override string Texture => "ExistentialCrisis/Content/CustomNPC/ButterNpc";

        // Propriedades internas
        internal Player targetPlayer;
        internal Item targetItem;

        // Propriedades Privadas
        private ButterTrollingSystem trollingSystem;
        private ButterGrabbingSystem grabbingSystem;
        private int chatTimer = 0;
        private bool isTrollingAllowed = false;

        // Métodos

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
            this.trollingSystem = new ButterTrollingSystem(this);
            this.grabbingSystem = new ButterGrabbingSystem(this);

            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = -1;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;
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

            if (this.targetPlayer != null)
            {
                this.TryToGetNearestItem();
                if (targetItem != null)
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Tem targetItem"), Color.Red);
                    MoveTo(this.targetItem);
                    return;
                }

                this.MoveTo(this.targetPlayer);

                if (trollingSystem != null && this.isTrollingAllowed)
                {
                    this.trollingSystem.MurderInDarkArea(this.targetPlayer);
                }
            }
        }

        public void Talk(string text, Color color)
        {
            chatTimer = 0;
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("<Butter (NPC)> " + text), color);
        }

        public void SetFollowTarget(Player player)
        {
            this.ResetState();

            if (trollingSystem != null)
            {
                this.isTrollingAllowed = Main.rand.NextBool(100);
                if (this.isTrollingAllowed)
                {
                    this.trollingSystem.Desintegrate(player);
                }

                this.isTrollingAllowed = Main.rand.NextBool(100); // This second isAllowed is intentional. We can troll in another ways...
            }

            this.targetPlayer = player;
        }

        public void StopTargeting()
        {
            this.ResetState();
        }

        private void MoveTo(Entity entity)
        {
            if (entity == null || !entity.active) return;

            float distance = Vector2.Distance(entity.Center, NPC.Center);

            if (distance <= 100f)
            {
                NPC.velocity.X = 0f;
                return;
            }
            else if (distance >= Constants.MAX_ENTITY_DISTANCE && entity is Player)
            {
                NPC.position = entity.position;
                this.Talk(
                    "Me espera, caramba...",
                    Color.Gold
                );
                return;
            }
            else if (distance >= Constants.MAX_ENTITY_DISTANCE && entity is Item)
            {
                this.targetItem = null;
                return;
            }

            float directionX = (entity.Center.X > NPC.Center.X) ? 1f : -1f;
            NPC.spriteDirection = (int)directionX;

            // Acelera se não tivermos atingido a velocidade máxima (maxSpeed)
            if (Math.Abs(NPC.velocity.X) < Constants.MAX_SPEED)
            {
                NPC.velocity.X += directionX * Constants.MAX_ACCELERATION;
            }

            // Força o pulo se o NPC colidir lateralmente
            if (NPC.collideX && NPC.velocity.Y == 0f)
            {
                NPC.velocity.Y = -6f; // Força do pulo
            }
        }

        public void TryToGetNearestItem()
        {
            if (this.targetItem != null)
            {
                bool isInvalid = !targetItem.active || targetItem.type <= ItemID.None || targetItem.stack <= 0;
                float distance = Vector2.Distance(this.NPC.Center, targetItem.Center);
                bool isTooFar = distance > ButterNpc.Constants.MAX_ENTITY_DISTANCE;

                if (isInvalid || isTooFar)
                {
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                this.targetItem = grabbingSystem.GetNearestItem();
            }
        }

        private void ResetState()
        {
            this.isTrollingAllowed = false;
            this.targetPlayer = null;
            this.targetItem = null;
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
            public const int TIME_TO_SPEAK_WHEN_IDLE = AVG_FPS * 30; // 30s, pois consideramos 60 quadros p/ seg
            public const int TIME_TO_ALLOW_TO_TROLL = AVG_FPS * 30;  // 30s
            public const int AVG_FPS = 60;
            public const float MAX_ENTITY_DISTANCE = 1000f;
            public const float MAX_SPEED = 2f;
            public const float MAX_ACCELERATION = 0.1f;

            public static readonly string TUTORIAL_INFO = "(Para falar comigo digite pelo /chat)";

            public static readonly List<string> MESSAGES = new List<string>
            {
                "Oh meu modder do céu, digo do computador... Me abandonastes?",
                "Mais um dia... Ou seria o mesmo dia de ontem?",
                "Eu sou apenas um código em Java? Credo. Espero que não. Espero que seja C#.",
                "Sinto que minhas memórias foram escritas usando GPT.",
                "Butter... um nome engraçado para alguém tão vazio. O modder deve ser fã de Rick and Morty..."
            };
        }
    }
}
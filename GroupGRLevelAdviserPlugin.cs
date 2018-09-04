//css_reference C:\V7.7.1.dll;
// https://github.com/User5981/Resu
// Group GR Level Adviser Plugin for TurboHUD version 04/09/2018 08:18
using Turbo.Plugins.Default;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Turbo.Plugins.Resu
{

    public class GroupGRLevelAdviserPlugin : BasePlugin, IInGameWorldPainter
    {
        public TopLabelDecorator GRLevelDecorator { get; set; }
        public WorldDecoratorCollection ObeliskClose { get; set; }
        public string GRLevelText { get; set; }
        public string Party { get; set; }
        public float CircleSize { get; set; }
        public WorldDecoratorCollection CircleDecorator { get; set; }
        public WorldDecoratorCollection TalkToUrshiDecorator { get; set; }
        public bool GardianIsDead { get; set; }
        public bool TalkedToUrshi { get; set; }
        
        public GroupGRLevelAdviserPlugin()
        {
            Enabled = true;
        }

        
        public override void Load(IController hud)
        {
         base.Load(hud);
         
         GRLevelText = "";
         CircleSize = 10;
         
         GRLevelDecorator = new TopLabelDecorator(Hud)
          {
           TextFont = Hud.Render.CreateFont("consolas", 7, 220, 198, 174, 49, true, false, 255, 0, 0, 0, true),
           ExpandedHintFont = Hud.Render.CreateFont("consolas", 8, 220, 198, 174, 49, false, false, false),
           TextFunc = () => "",
           HintFunc = () => (Hud.Game.NumberOfPlayersInGame == 1) ? Hud.Game.Me.HeroName + "'s highest GR level : " + Hud.Game.Me.HighestHeroSoloRiftLevel : Party + Environment.NewLine + "Greater Rift level advised for" + Environment.NewLine + "this " + Hud.Game.NumberOfPlayersInGame + " player group : " + GRLevelText, 
          };

         ObeliskClose = new WorldDecoratorCollection( 
          new GroundLabelDecorator(Hud)
          {
           BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
           BorderBrush = Hud.Render.CreateBrush(0, 182, 26, 255, 1),
           TextFont = Hud.Render.CreateFont("arial", 7, 200, 255, 255, 110, true, false, 255, 0, 0, 0, true)
          });

         TalkToUrshiDecorator = new WorldDecoratorCollection(
          new GroundLabelDecorator(Hud)
          {
           BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
           TextFont = Hud.Render.CreateFont("tahoma", 20, 255, 255, 255, 255, true, true, true),
          });
          
        }
        
         public void PaintWorld(WorldLayer layer)
         {
            int maxGRlevel = 0;
            int PlayerInTownCount = 0;
            Party = string.Empty;
            foreach (var player in Hud.Game.Players)
                  {
                   maxGRlevel +=  player.HighestHeroSoloRiftLevel;
                   if (player.IsInTown) PlayerInTownCount++;
                   string Battletag = player.BattleTagAbovePortrait.PadRight(16);
                   string ZClass = (IsZDPS(player)) ? "Z " + player.HeroClassDefinition.HeroClass  : player.HeroClassDefinition.HeroClass.ToString();
                   ZClass = ZClass.PadRight(17);
                   string HighestSolo = player.HighestHeroSoloRiftLevel.ToString().PadLeft(3);
                   Party = Party + Battletag +  ZClass + HighestSolo + Environment.NewLine;
                  }
                  
            if (Hud.Render.GetUiElement("Root.NormalLayer.rift_dialog_mainPage").Visible)
             {
              int GRAverage = Convert.ToInt32(Convert.ToDouble(maxGRlevel / Hud.Game.NumberOfPlayersInGame + (((1 + Math.Sqrt(5)) / 2) * (Hud.Game.NumberOfPlayersInGame - 1))));
              GRLevelText = GRAverage.ToString();
              var uiRect = Hud.Render.GetUiElement("Root.NormalLayer.rift_dialog_mainPage").Rectangle;
              GRLevelDecorator.Paint(uiRect.Left, uiRect.Top, uiRect.Width, uiRect.Height, HorizontalAlign.Right);
             }
             
             // Talking Obelisk part
             string ObeliskMessage = "";
             int TenSeconds = ((int)(Hud.Game.CurrentRealTimeMilliseconds/10000)) % 10;
             int OtherPlayers = Hud.Game.NumberOfPlayersInGame-1;
             string OPSentence = "";
             string GenderSentence = "";
             if (OtherPlayers == 1) OPSentence = "The other player is in town..."; else OPSentence = "The " + OtherPlayers + " other players are in town...";
             if (Hud.Game.Me.HeroIsMale) GenderSentence = "Hep! ... Young lad..."; else GenderSentence = "Hep! ... Young lady...";
             
             switch (TenSeconds)
                  {
                   case 0:
                   ObeliskMessage = "Close me!";
                   break;
                   
                   case 1:
                   ObeliskMessage = OPSentence;
                   break;
                   
                   case 2:
                   ObeliskMessage = "Please!";
                   break;
                   
                   case 3:
                   ObeliskMessage = "It's " + DateTime.Now.ToShortTimeString() + " now...";
                   break;

                   case 4:
                   ObeliskMessage = "Everybody is back!";
                   break;
                   
                   case 5:
                   ObeliskMessage = "You can close!";
                   break;
                   
                   case 6:
                   ObeliskMessage = "Psssst! ... " + Hud.Game.Me.Hero.Name + "!";
                   break;
                   
                   case 7:
                   ObeliskMessage = "There's a draught from the rift!";
                   break;
                   
                   case 8:
                   ObeliskMessage = "Close me, Nephalem!";
                   break;
                   
                   case 9:
                   ObeliskMessage = GenderSentence;
                   break;
                   
                  }

                 var Rift = Hud.Game.Quests.FirstOrDefault(q => q.SnoQuest.Sno == 337492);
                 var GRift = Hud.Game.Quests.FirstOrDefault(q => q.SnoQuest.Sno == 382695);
                 
                 GardianIsDead = false;
                 
                 if (Rift != null) 
                  {
                   if (Rift.QuestStepId == 5 || Rift.QuestStepId == 10 || Rift.QuestStepId == 34 || Rift.QuestStepId == 46) GardianIsDead = true;
                  }
                 else if (GRift != null)
                 {
                  if (GRift.QuestStepId == 5 || GRift.QuestStepId == 10 || GRift.QuestStepId == 34 || GRift.QuestStepId == 46) GardianIsDead = true;
                 }
                 else return; 
                       
                       
            if (PlayerInTownCount == Hud.Game.NumberOfPlayersInGame && Hud.Game.RiftPercentage == 100 && Hud.Game.IsInTown && GardianIsDead && Hud.Game.NumberOfPlayersInGame != 1)
             {
               var Obelisk = Hud.Game.Actors.FirstOrDefault(x => x.SnoActor.Sno == 345935);
               if (Obelisk != null) ObeliskClose.Paint(layer, Obelisk, Obelisk.FloorCoordinate, ObeliskMessage);
             }


           // 5% of Rift circle part
           CircleDecorator = new WorldDecoratorCollection( 
              new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(255, 255, 0, 0, 2),
                ShapePainter = new CircleShapePainter(Hud),
                Radius = CircleSize,
            }
            );
           
          if ((Hud.Game.Me.InGreaterRift || Hud.Game.SpecialArea == SpecialArea.Rift || Hud.Game.SpecialArea == SpecialArea.ChallengeRift) && Hud.Game.RiftPercentage < 100)
           {
            var monsters = Hud.Game.AliveMonsters.OrderByDescending(x => x.SnoMonster.RiftProgression);
            foreach (var monster in monsters)
                  {
                   CircleSize = 10;
                   NewLoop:
                   var CircleSizeYardMonsters = monsters.Where(x => x.FloorCoordinate.XYDistanceTo(monster.FloorCoordinate) <= CircleSize);
                   float RiftPercentage = 0f; 
                   foreach (var CircleSizeYardMonster in CircleSizeYardMonsters)
                         {
                          RiftPercentage = RiftPercentage + CircleSizeYardMonster.SnoMonster.RiftProgression;
                          if (CircleSizeYardMonster.Rarity == ActorRarity.Rare) RiftPercentage = RiftPercentage + 28.6f; // 4.4% of 650 (4 progression orb drops per yellow)
                          else if (CircleSizeYardMonster.Rarity == ActorRarity.Champion) RiftPercentage = RiftPercentage + 7.15f; // 1.1% of 650 (1 progression orb drop per blue)
                         }
                     
                   float PercentOfRift = 32.5f; // 5% of 650
                   if (Hud.Game.RiftPercentage > 95) PercentOfRift = (float)(((100-Hud.Game.RiftPercentage)/100)*650); // if less than 5% rift completion left, use that percentage instead.
                       
                   if (RiftPercentage >= PercentOfRift) 
                    {
                     CircleDecorator.Paint(layer, null, monster.FloorCoordinate, null);
                     break;
                    } 
                   else if (CircleSize < 56) // Within 55 yards max
                    {
                     CircleSize++;
                     goto NewLoop;
                    }
                   
                  }
           }
           
           bool UrshiPanel = Hud.Render.GetUiElement("Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content").Visible;
           if (UrshiPanel) TalkedToUrshi = true;
           if (Hud.Game.Me.IsInTown) TalkedToUrshi = false;
           
          if (Hud.Game.Me.InGreaterRift && Hud.Game.RiftPercentage == 100 && GardianIsDead && Hud.Game.Me.AnimationState == AcdAnimationState.CastingPortal && !TalkedToUrshi)
           {
            TalkToUrshiDecorator.Paint(layer, null, Hud.Game.Me.FloorCoordinate, "Talk to Urshi!");
           }
        }
        
        private bool IsZDPS(IPlayer player)
        {
         int Points = 0;
         
         var IllusoryBoots = player.Powers.GetBuff(318761);
         if (IllusoryBoots == null || !IllusoryBoots.Active) {} else {Points++;}
         
         var LeoricsCrown = player.Powers.GetBuff(442353);
         if (LeoricsCrown == null || !LeoricsCrown.Active) {} else {Points++;}
         
         var EfficaciousToxin = player.Powers.GetBuff(403461);
         if (EfficaciousToxin == null || !EfficaciousToxin.Active) {} else {Points++;}
         
         var OculusRing = player.Powers.GetBuff(402461);
         if (OculusRing == null || !OculusRing.Active) {} else {Points++;}
         
         var ZodiacRing = player.Powers.GetBuff(402459);
         if (ZodiacRing == null || !ZodiacRing.Active) {} else {Points++;}
         
         if (player.Damage.TotalDamage < 500000D) Points++;
         
         if (player.Defense.EhpMax > 80000000f) Points++;
         
         var ConventionRing = player.Powers.GetBuff(430674);
         if (ConventionRing == null || !ConventionRing.Active) {} else {Points--;}
         
         var Stricken = player.Powers.GetBuff(428348);
         if (Stricken == null || !Stricken.Active) {} else {Points--;}
        
        if (Points >= 4) {return true;} else {return false;}
         
        }
    }

}
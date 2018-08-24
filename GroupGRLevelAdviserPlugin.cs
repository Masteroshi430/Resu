//css_reference C:\V7.7.1.dll;
// https://github.com/User5981/Resu
// Group GR Level Adviser Plugin for TurboHUD version 21/08/2018 08:42
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
           TextFont = Hud.Render.CreateFont("arial", 7, 220, 198, 174, 49, true, false, 255, 0, 0, 0, true),
           TextFunc = () => "",
           HintFunc = () => Party + Environment.NewLine + "Greater Rift level advised for" + Environment.NewLine + "this " + Hud.Game.NumberOfPlayersInGame + " players group : " + GRLevelText
          };

         ObeliskClose = new WorldDecoratorCollection( 
          new GroundLabelDecorator(Hud)
          {
           BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
           BorderBrush = Hud.Render.CreateBrush(0, 182, 26, 255, 1),
           TextFont = Hud.Render.CreateFont("arial", 7, 200, 255, 255, 110, true, false, 255, 0, 0, 0, true)
          });

        }
        
         public void PaintWorld(WorldLayer layer)
         {
          if (Hud.Game.NumberOfPlayersInGame > 1)
           {
            int maxGRlevel = 0;
            int PlayerInTownCount = 0;
            Party = "";
            foreach (var player in Hud.Game.Players)
                  {
                   maxGRlevel +=  player.HighestHeroSoloRiftLevel;
                   if (player.IsInTown) PlayerInTownCount++;
                   Party = Party + player.BattleTagAbovePortrait + " " +  player.HeroClassDefinition.HeroClass + " " + player.HighestHeroSoloRiftLevel + Environment.NewLine;
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
                 
                 bool GardianIsDead = false;
                 
                 if (Rift != null) 
                  {
                   if (Rift.QuestStepId == 5 || Rift.QuestStepId == 10 || Rift.QuestStepId == 34 || Rift.QuestStepId == 46) GardianIsDead = true;
                  }
                 else if (GRift != null)
                 {
                  if (GRift.QuestStepId == 5 || GRift.QuestStepId == 10 || GRift.QuestStepId == 34 || GRift.QuestStepId == 46) GardianIsDead = true;
                 }
                 else return; 
                       
                       
            if (PlayerInTownCount == Hud.Game.NumberOfPlayersInGame && Hud.Game.RiftPercentage == 100 && Hud.Game.IsInTown && GardianIsDead)
             {
               var Obelisk = Hud.Game.Actors.FirstOrDefault(x => x.SnoActor.Sno == 345935);
               if (Obelisk != null) ObeliskClose.Paint(layer, Obelisk, Obelisk.FloorCoordinate, ObeliskMessage);
             }

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
        }
    }

}
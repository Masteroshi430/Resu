//css_reference C:\V7.7.1.dll;
// https://github.com/User5981/Resu
// Ariadne's Thread plugin for TurboHUD version 15/08/2018 14:48
// Shamelessly contains Xenthalon's AdvancedMarkerPlugin ^^;

using Turbo.Plugins.Default;
using System;
using System.Collections.Generic;
using System.Linq;




namespace Turbo.Plugins.Resu
{

    public class AriadnesThreadPlugin : BasePlugin, IInGameWorldPainter, ICustomizer
    {
        public TopLabelDecorator StrengthBuffDecorator { get; set; }
        public string StrengthBuffText{ get; set; }
        public string bossText{ get; set; }
        public IWorldCoordinate Other1 { get; set; }
        public IWorldCoordinate Other2 { get; set; }
        public IWorldCoordinate Other3 { get; set; }
        public IWorldCoordinate RealOther1 { get; set; }
        public IWorldCoordinate RealOther2 { get; set; }
        public IWorldCoordinate RealOther3 { get; set; }
        public string AreaOther1 { get; set; }
        public string AreaOther2 { get; set; }
        public string AreaOther3 { get; set; }
        public string NameOther1 { get; set; }
        public string NameOther2 { get; set; }
        public string NameOther3 { get; set; }
        public int StrengthBuff1 { get; set; }
        public int StrengthBuff2 { get; set; }
        public int StrengthBuff3 { get; set; }
        public IBrush WhiteBrush { get; set; }
        public WorldDecoratorCollection QuestDecorator { get; set; }
        public WorldDecoratorCollection KeywardenDecorator { get; set; }
        public WorldDecoratorCollection BossDecorator { get; set; }
        public WorldDecoratorCollection BannerDecorator { get; set; }
        private Dictionary<IWorldCoordinate, long> BannersList;
        private Dictionary<IWorldCoordinate, string> BannersAreas;
        public int BannerTimeSeconds { get; set; }
        public bool ThreadBetweenPlayers { get; set; }
                
        public AriadnesThreadPlugin()
        {
            Enabled = true;
            BannersList = new Dictionary<IWorldCoordinate,long>();
            BannersAreas = new Dictionary<IWorldCoordinate,string>();
            BannerTimeSeconds = 30;
            ThreadBetweenPlayers = true;
        }

        
        public override void Load(IController hud)
        {
         base.Load(hud);
         Other1 = Hud.Window.CreateWorldCoordinate(381.154f, 551.850f, 33.3f);
         Other2 = Hud.Window.CreateWorldCoordinate(381.154f, 551.850f, 33.3f);
         Other3 = Hud.Window.CreateWorldCoordinate(381.154f, 551.850f, 33.3f);
         RealOther1 = Hud.Window.CreateWorldCoordinate(381.154f, 551.850f, 33.3f);
         RealOther2 = Hud.Window.CreateWorldCoordinate(381.154f, 551.850f, 33.3f);
         RealOther3 = Hud.Window.CreateWorldCoordinate(381.154f, 551.850f, 33.3f);
         NameOther1 = "";
         NameOther2 = "";
         NameOther3 = "";
         AreaOther1 = ""; 
         AreaOther2 = "";
         AreaOther3 = "";
         StrengthBuff1 = 0;
         StrengthBuff2 = 0;
         StrengthBuff3 = 0;
         StrengthBuffText = "";
         WhiteBrush = Hud.Render.CreateBrush(125, 255, 255, 255, 1, SharpDX.Direct2D1.DashStyle.Dash, SharpDX.Direct2D1.CapStyle.Flat, SharpDX.Direct2D1.CapStyle.Triangle);
         
         StrengthBuffDecorator = new TopLabelDecorator(Hud)
          {
           TextFont = Hud.Render.CreateFont("arial", 7, 220, 198, 174, 49, true, false, 255, 0, 0, 0, true),
           TextFunc = () => StrengthBuffText,
          };
          
         QuestDecorator = new WorldDecoratorCollection
          (
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(192, 255, 255, 55, -1),
                    ShadowBrush = Hud.Render.CreateBrush(96, 0, 0, 0, 1),
                    Radius = 10.0f,
                    ShapePainter = new CircleShapePainter(Hud),
                },
                new MapLabelDecorator(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 200, 255, 255, 0, false, false, 128, 0, 0, 0, true),
                    RadiusOffset = 10,
                    Up = true,
                },
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(192, 255, 255, 55, -1),
                    ShapePainter = new LineFromMeShapePainter(Hud)
                }
          );
 
         KeywardenDecorator = new WorldDecoratorCollection
          (
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(192, 238, 130, 238, -1),
                    ShadowBrush = Hud.Render.CreateBrush(96, 0, 0, 0, 1),
                    Radius = 10.0f,
                    ShapePainter = new CircleShapePainter(Hud),
                },
                new MapLabelDecorator(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 200, 255, 20, 255, false, false, 128, 0, 0, 0, true),
                    RadiusOffset = 10,
                    Up = true,
                },
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(192, 238, 130, 238, -1),
                    ShapePainter = new LineFromMeShapePainter(Hud)
                }
          );
 
         BossDecorator = new WorldDecoratorCollection
          (
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(192, 0, 255, 255, -1),
                    ShadowBrush = Hud.Render.CreateBrush(96, 0, 0, 0, 1),
                    Radius = 10.0f,
                    ShapePainter = new CircleShapePainter(Hud),
                },
                new MapLabelDecorator(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 200,  0, 255, 255, false, false, 128, 0, 0, 0, true),
                    RadiusOffset = 10,
                    Up = true,
                },
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(192, 0, 255, 255, -1),
                    ShapePainter = new LineFromMeShapePainter(Hud)
                }
          );
          
         BannerDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(255, 127, 255, 0, 0),
                    BorderBrush = Hud.Render.CreateBrush(192, 0, 0, 0, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 6.5f, 255, 0, 0, 0, true, false, false),
                },
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(192, 27, 255, 0, -1),
                    ShadowBrush = Hud.Render.CreateBrush(96, 0, 0, 0, 1),
                    Radius = 10.0f,
                    ShapePainter = new CircleShapePainter(Hud),
                },
                new MapLabelDecorator(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 200,  27, 255, 0, false, false, 128, 0, 0, 0, true),
                    RadiusOffset = 10,
                    Up = true,
                },
                new MapShapeDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(192, 27, 255, 0, -1),
                    ShapePainter = new LineFromMeShapePainter(Hud)
                }
                );
          
        }
        
        
        
         public void PaintWorld(WorldLayer layer)
        {
              bool FirstQuestMarker = true;
              var Actors = Hud.Game.Actors.Where(a => a.SnoActor.Sno == 432770 || a.SnoActor.Sno == 430733 || a.SnoActor.Sno == 432885 || a.SnoActor.Sno == 433051 ||
              a.SnoActor.Sno == 432259 || a.SnoActor.Sno == 433246 || a.SnoActor.Sno == 433385 || a.SnoActor.Sno == 433295 || a.SnoActor.Sno == 433316 || 
              a.SnoActor.Sno == 434366 || a.SnoActor.Sno == 433124 || a.SnoActor.Sno == 433184 || a.SnoActor.Sno == 433402 || a.SnoActor.Sno == 432331 || 
              a.SnoActor.Sno == 435703);
              if (!Actors.Any())FirstQuestMarker = true;
              foreach (var Actor in Actors)
              {
               if (Actor == null) continue;
               if (Actor.FloorCoordinate == null) continue;
               if (Actor.IsDisabled || Actor.IsOperated) continue;
               
               uint ThatQuest = 0;
               string Name = "";
               switch (Actor.SnoActor.Sno)
               {
                case 432770: Name = "Royal Cocoon"; ThatQuest = 432784; break;
                case 430733: Name = "Captured Villager"; ThatQuest = 430723; break; 
                case 432885: Name = "Ancient Device"; ThatQuest = 433025; break;
                case 433051: Name = "Caldeum Villager"; ThatQuest = 433053; break;  
                case 435703: Name = "Captured Barbarian"; ThatQuest = 436280; break;
                case 432259: Name = "Triune Monument"; ThatQuest = 432293;  break;
                case 433246: Name = "Death Orb"; ThatQuest = 433256; break;
                case 433385: Name = "Catapult Winch"; ThatQuest = 433392; break;
                case 433295: Name = "Demon Gate"; ThatQuest = 433309; break;
                case 433316: Name = "Death Prison"; ThatQuest = 433339; break;
                case 434366: Name = "Tortured Wortham Militia"; ThatQuest = 434378; break;
                case 433184: Name = "Captured Guard"; ThatQuest = 433217; break;
                case 433124: Name = "Bone Cage"; ThatQuest = 433099; break;
                case 433402: Name = "Hell Portal"; ThatQuest = 433422; break;
                case 432331: Name = "Captured Iron Wolf"; ThatQuest = 432334; break;
                default:  Name = "";  break;
               }
                var quest = Hud.Game.Bounties.FirstOrDefault(x => x.SnoQuest.Sno == ThatQuest);
                if ((quest != null) && quest.State != QuestState.completed)
                 {
                  QuestDecorator.Paint(layer, null, Actor.FloorCoordinate, Name); FirstQuestMarker = false;
                 }
              }
         
         // modified Xenthalon's AdvancedMarkerPlugin
         var markers = Hud.Game.Markers.OrderBy(i => Hud.Game.Me.FloorCoordinate.XYDistanceTo(i.FloorCoordinate));
         if (markers != null) 
          {
           foreach (var marker in markers)
           {
            if (marker == null) continue;
            if (marker.FloorCoordinate == null) continue;
            if (marker.Name == null) continue;
            
            var OnScreen = marker.FloorCoordinate.IsOnScreen(1);
            QuestDecorator.ToggleDecorators<GroundLabelDecorator>(!OnScreen); // do not display ground labels when the marker is on the screen
            KeywardenDecorator.ToggleDecorators<GroundLabelDecorator>(!OnScreen);
            BossDecorator.ToggleDecorators<GroundLabelDecorator>(!OnScreen);

            if (marker.SnoQuest != null && FirstQuestMarker)
             {
              QuestDecorator.Paint(layer, null, marker.FloorCoordinate, marker.Name); FirstQuestMarker = false;
             }
            else if (marker.SnoActor != null)
             {
              if (marker.SnoActor.Code.Contains("_Boss_"))
               {
                BossDecorator.Paint(layer, null, marker.FloorCoordinate, marker.Name);
               }
              else if (marker.FloorCoordinate.XYDistanceTo(Hud.Game.Me.FloorCoordinate) < 500)
               {
                KeywardenDecorator.Paint(layer, null, marker.FloorCoordinate, marker.Name);
               }
             }
           }
          }
         
         
         if (Hud.Game.NumberOfPlayersInGame == 1) 
          {
           NameOther3 = ""; Other3 = Hud.Game.Me.FloorCoordinate; AreaOther3 = ""; StrengthBuff3 = 0;
           NameOther2 = ""; Other2 = Hud.Game.Me.FloorCoordinate; AreaOther2 = ""; StrengthBuff2 = 0;
           NameOther1 = ""; Other1 = Hud.Game.Me.FloorCoordinate; AreaOther1 = ""; StrengthBuff1 = 0;
           return;
          }
         
         // Strengh in numbers buff indicator
         int StrengthBuff = StrengthBuff1 + StrengthBuff2 + StrengthBuff3;
         if (StrengthBuff != 0 && !Hud.Game.Me.IsDead)
          {
              
           StrengthBuffText = "+" + StrengthBuff + "%";
           var uiRect = Hud.Render.GetUiElement("Root.NormalLayer.minimap_dialog_backgroundScreen.minimap_dialog_pve.BoostWrapper.BoostsDifficultyStackPanel.clock").Rectangle;
           StrengthBuffDecorator.Paint(uiRect.Left - uiRect.Width * 1.14f, uiRect.Top + uiRect.Height * 1f, uiRect.Width, uiRect.Height, HorizontalAlign.Right);
          }
          
         // Thread between players
         var players = Hud.Game.Players.Where(player => !player.IsMe).OrderBy(player => player.PortraitIndex);
         if (players != null)
          {
           foreach (var player in players)
           {
            if (player == null) continue;
            else if (player.BattleTagAbovePortrait == null) continue;
            else if (player.FloorCoordinate == null) continue;
            else if (player.SnoArea == null) continue;
            else if (player.SnoArea.NameEnglish == null) continue;
            else if (double.IsNaN(player.NormalizedXyDistanceToMe)) continue;
            else if (player.PortraitIndex == 1)
             { 
               NameOther1 = player.BattleTagAbovePortrait; RealOther1 = player.FloorCoordinate; AreaOther1 = player.SnoArea.NameEnglish;
               if (Hud.Game.NumberOfPlayersInGame == 2) NameOther3 = ""; Other3 = Hud.Game.Me.FloorCoordinate; AreaOther3 = ""; StrengthBuff3 = 0; NameOther2 = ""; Other2 = Hud.Game.Me.FloorCoordinate; AreaOther2 = ""; StrengthBuff2 = 0; 
               if (Hud.Game.Me.SnoArea.Sno == player.SnoArea.Sno)
                {
                 Other1 = player.FloorCoordinate;  
                 if (player.NormalizedXyDistanceToMe <= 186 && !player.IsDead) StrengthBuff1 = 10; else StrengthBuff1 = 0;
                }
                else
                {
                    
                 Other1 = Hud.Game.Me.FloorCoordinate; StrengthBuff1 = 0;
                }
             }
           
            else if (player.PortraitIndex == 2)
             { 
               NameOther2 = player.BattleTagAbovePortrait; RealOther2 = player.FloorCoordinate; AreaOther2 = player.SnoArea.NameEnglish;
               if (Hud.Game.NumberOfPlayersInGame == 3) NameOther3 = ""; Other3 = Hud.Game.Me.FloorCoordinate; AreaOther3 = ""; StrengthBuff3 = 0; 
               if (Hud.Game.Me.SnoArea.Sno == player.SnoArea.Sno)
                {
                 Other2 = player.FloorCoordinate; 
                 if (player.NormalizedXyDistanceToMe <= 186 && !player.IsDead) StrengthBuff2 = 10; else StrengthBuff2 = 0;
                }
                else
                {
                 Other2 = Hud.Game.Me.FloorCoordinate; StrengthBuff2 = 0;
                }
             }
           
            else if (player.PortraitIndex == 3)
             {
               NameOther3 = player.BattleTagAbovePortrait; RealOther3 = player.FloorCoordinate; AreaOther3 = player.SnoArea.NameEnglish;
               if (Hud.Game.Me.SnoArea.Sno == player.SnoArea.Sno)
                {
                 Other3 = player.FloorCoordinate; 
                 if (player.NormalizedXyDistanceToMe <= 186 && !player.IsDead) StrengthBuff3 = 10; else StrengthBuff3 = 0;
                }
                else
                {
                 Other3 = Hud.Game.Me.FloorCoordinate; StrengthBuff3 = 0;
                }
             }
           }
          }
         
         float Other1OnMapX, Other1OnMapY, Other2OnMapX, Other2OnMapY, Other3OnMapX, Other3OnMapY;
         
         Hud.Render.GetMinimapCoordinates(Other1.X, Other1.Y, out Other1OnMapX, out Other1OnMapY);
         Hud.Render.GetMinimapCoordinates(Other2.X, Other2.Y, out Other2OnMapX, out Other2OnMapY);
         Hud.Render.GetMinimapCoordinates(Other3.X, Other3.Y, out Other3OnMapX, out Other3OnMapY);
         
         if (!Hud.Game.IsInTown && ThreadBetweenPlayers)
          {
           WhiteBrush.DrawLine(Other1OnMapX, Other1OnMapY, Other2OnMapX, Other2OnMapY);
           WhiteBrush.DrawLine(Other1OnMapX, Other1OnMapY, Other3OnMapX, Other3OnMapY);
           WhiteBrush.DrawLine(Other3OnMapX, Other3OnMapY, Other2OnMapX, Other2OnMapY);
          }
          
         // banners
         var banners = Hud.Game.Banners;
         if (banners != null)
          {
           foreach (var banner in banners)
           {
              
              if (banner == null || banner.FloorCoordinate == null || !banner.FloorCoordinate.IsValid) continue;
              if (!BannersList.ContainsKey(banner.FloorCoordinate)) BannersList.Add(banner.FloorCoordinate, Hud.Game.CurrentRealTimeMilliseconds);
           }
          }
          
         float DistOther1;
         float DistOther2; 
         float DistOther3;
         float ZDistOther1;
         float ZDistOther2; 
         float ZDistOther3;
         float DistMe;
         float ZDistMe;
         
         if (BannersList != null)
          {
           foreach(var ListedBanner in BannersList.Where(b => Hud.Game.CurrentRealTimeMilliseconds < b.Value + 30000).OrderBy(b => b.Value).Take(5))
           {
              
              var BanCoord = ListedBanner.Key;
              long Timeleft = ((ListedBanner.Value + (BannerTimeSeconds*1000) - Hud.Game.CurrentRealTimeMilliseconds)/1000);
              string Countdown = " " + Timeleft.ToString("f0") + "s";
              
              
              var onScreen = BanCoord.IsOnScreen();
              BannerDecorator.ToggleDecorators<GroundLabelDecorator>(!onScreen);
              
              
              if (RealOther1 != null && Hud.Game.NumberOfPlayersInGame >= 2) {DistOther1 = RealOther1.XYDistanceTo(BanCoord); ZDistOther1 = RealOther1.ZDiffTo(BanCoord);} else {DistOther1 = float.MaxValue; ZDistOther1 = float.MaxValue;} 
              if (RealOther2 != null && Hud.Game.NumberOfPlayersInGame >= 3) {DistOther2 = RealOther2.XYDistanceTo(BanCoord); ZDistOther2 = RealOther2.ZDiffTo(BanCoord);} else {DistOther2 = float.MaxValue; ZDistOther2 = float.MaxValue;}
              if (RealOther3 != null && Hud.Game.NumberOfPlayersInGame == 4) {DistOther3 = RealOther3.XYDistanceTo(BanCoord); ZDistOther3 = RealOther3.ZDiffTo(BanCoord);} else {DistOther3 = float.MaxValue; ZDistOther3 = float.MaxValue;}
              DistMe = Hud.Game.Me.FloorCoordinate.XYDistanceTo(BanCoord);
              ZDistMe = Hud.Game.Me.FloorCoordinate.ZDiffTo(BanCoord);
           
              string NearestPlayer = "";
              string BannerArea = "";
              
              if (!BannersAreas.ContainsKey(BanCoord))
               {   
                if (DistOther1 < DistOther2 && DistOther1 < DistOther3 && DistOther1 < DistMe && DistOther1 < 200f && ZDistOther1 < 6f && NameOther1 != null) {NearestPlayer = "banner near " + NameOther1; BannerArea = AreaOther1;}
                else if (DistOther2 < DistOther1 && DistOther2 < DistOther3 && DistOther2 < DistMe && DistOther2 < 200f && ZDistOther2 < 6f && NameOther2 != null) {NearestPlayer = "banner near " + NameOther2; BannerArea = AreaOther2;}
                else if (DistOther3 < DistOther1 && DistOther3 < DistOther2 && DistOther3 < DistMe && DistOther3 < 200f && ZDistOther3 < 6f && NameOther3 != null) {NearestPlayer = "banner near " + NameOther3; BannerArea = AreaOther3;}
                else {NearestPlayer = "banner"; BannerArea = Hud.Game.Me.SnoArea.NameEnglish;}
               }
              else
               {
                if (DistOther1 < DistOther2 && DistOther1 < DistOther3 && DistOther1 < DistMe && DistOther1 < 200f && ZDistOther1 < 6f && BannersAreas[BanCoord] == AreaOther1 && NameOther1 != null) {NearestPlayer = "banner near " + NameOther1;}
                else if (DistOther2 < DistOther1 && DistOther2 < DistOther3 && DistOther2 < DistMe && DistOther2 < 200f && ZDistOther2 < 6f && BannersAreas[BanCoord] == AreaOther2 && NameOther2 != null) {NearestPlayer = "banner near " + NameOther2;}
                else if (DistOther3 < DistOther1 && DistOther3 < DistOther2 && DistOther3 < DistMe && DistOther3 < 200f && ZDistOther3 < 6f && BannersAreas[BanCoord] == AreaOther3 && NameOther3 != null) {NearestPlayer = "banner near " + NameOther3;}
                else {NearestPlayer = "banner";}
               }

               string RealBannerArea = "";
               if (!BannersAreas.ContainsKey(BanCoord)) BannersAreas.Add(BanCoord, BannerArea);
               else
                  {
                   RealBannerArea = BannersAreas[BanCoord];
                  }
              bool SameArea = false;
              if (Hud.Game.Me.SnoArea.NameEnglish == RealBannerArea) SameArea = true; else SameArea = false;
              BannerDecorator.ToggleDecorators<MapShapeDecorator>(SameArea);
              
              if (NearestPlayer == "banner near ") NearestPlayer = "banner";
              if (RealBannerArea.Contains("[TEMP]")) RealBannerArea = RealBannerArea.Replace("[TEMP]",string.Empty).Trim();
              if (RealBannerArea.Contains("Loot Run")) RealBannerArea = RealBannerArea.Replace("Loot Run", "Rift").Trim();
              
              if (NearestPlayer != null && RealBannerArea != "")
               { 
                 if (SameArea) BannerDecorator.Paint(layer, null, BanCoord, NearestPlayer + Countdown);
                 else if (NearestPlayer != "banner")
                  {
                    BannerDecorator.Paint(layer, null, BanCoord, NearestPlayer + " in " +  RealBannerArea + Environment.NewLine + "teleport!" + Countdown);
                  }
                 else  
                  {
                    BannerDecorator.Paint(layer, null, BanCoord, NearestPlayer + " in " +  RealBannerArea + Countdown);
                  }
               }
           }  
          }
          

        }
        
        public void OnNewArea(bool newGame, ISnoArea area)
        {
            if (newGame)
            {
                BannersList.Clear();
                BannersAreas.Clear();
            }
        }
            
            
         public void Customize()
        {
            Hud.TogglePlugin<MarkerPlugin>(false);  // disable default MarkerPlugin
            Hud.TogglePlugin<BannerPlugin>(false);  // disable default BannerPlugin
        }
        
    }

}
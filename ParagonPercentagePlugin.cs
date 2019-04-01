//css_reference C:\v9.0.dll;
// https://github.com/User5981/Resu
// Paragon Percentage Plugin for TurboHUD Version 29/03/2019 20:21

using System;
using System.Globalization;
using Turbo.Plugins.Default;
using System.Linq;

namespace Turbo.Plugins.Resu
{
    public class ParagonPercentagePlugin : BasePlugin, IInGameTopPainter, ICustomizer //, IChatLineChangedHandler
    {
        public bool ShowGreaterRiftMaxLevel { get; set; }
        public bool ParagonPercentageOnTheRight { get; set; }
        public bool DisplayParagonPercentage { get; set; }
        public string ParagonPercentage { get; set; }
        
        public TopLabelDecorator ParagonPercentageDecorator { get; set; }
        public TopLabelDecorator HighestSoloRiftLevelDecorator { get; set; }
        public TopLabelDecorator NemesisDecorator { get; set; }
        public TopLabelDecorator UnityDecorator { get; set; }
        public TopLabelDecorator ZDPSDecorator { get; set; }
        public TopLabelDecorator AFKDecorator { get; set; }
        
        public int GRlevel { get; set; }
        public float SheetDPS { get; set; }
        public float EHP { get; set; }
        public string Class { get; set; }
        public string Nemesis { get; set; }
        public string Unity { get; set; }
        public string TimeToNextParagon { get; set; }
        public IWorldCoordinate Player0pos { get; set; }
        public IWorldCoordinate Player1pos { get; set; }
        public IWorldCoordinate Player2pos { get; set; }
        public IWorldCoordinate Player3pos { get; set; }
        private IWatch _watch0;
        private IWatch _watch1;
        private IWatch _watch2;
        private IWatch _watch3;
        
        public ParagonPercentagePlugin()
        {
            Enabled = true;
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);
            _watch0 = Hud.Time.CreateWatch();
            _watch1 = Hud.Time.CreateWatch();
            _watch2 = Hud.Time.CreateWatch();
            _watch3 = Hud.Time.CreateWatch();
            
            ShowGreaterRiftMaxLevel = true;
            ParagonPercentageOnTheRight = true;
            ParagonPercentage = "0";
            DisplayParagonPercentage = true;
            TimeToNextParagon = String.Empty;

            
            ParagonPercentageDecorator = new TopLabelDecorator(Hud)
            {
                BackgroundTexture1 = Hud.Texture.Button2TextureBrown,
                BackgroundTextureOpacity1 = 0.8f,
                TextFont = Hud.Render.CreateFont("Segoe UI Light", 7, 250, 255, 255, 255, false, false, true),

                TextFunc = () => ParagonPercentage,

                HintFunc = () => "Paragon level " + (Hud.Game.Me.CurrentLevelParagon + 1) + " in " + TimeToNextParagon +  Environment.NewLine + "EXP/h : " + ValueToString(Hud.Game.CurrentHeroToday.GainedExperiencePerHourPlay, ValueFormat.ShortNumber),
            };

            
                        
            GRlevel = -1;
            SheetDPS = -1;
            EHP = -1;
            Class = "-1";
            Nemesis = "-1";
            Unity = "-1";
            
            HighestSoloRiftLevelDecorator = new TopLabelDecorator(Hud)
            {
                BackgroundTexture1 = Hud.Texture.Button2TextureBrown,
                BackgroundTextureOpacity1 = 0.9f,
                TextFont = Hud.Render.CreateFont("Segoe UI Light", 7, 250, 255, 255, 255, false, false, true),
                
                TextFunc = () =>  "      " + GRlevel,
                    
                HintFunc = () =>  Class + Nemesis + Unity +  Environment.NewLine + "Sheet DPS : " + ValueToString((long)SheetDPS, ValueFormat.LongNumber) + Environment.NewLine + "EHP : " + ValueToString((long)EHP, ValueFormat.LongNumber),
            };
            
            ZDPSDecorator = new TopLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                BorderBrush = Hud.Render.CreateBrush(0, 182, 26, 255, 1),
                TextFont = Hud.Render.CreateFont("Segoe UI Light", 30, 120, 0, 191, 255, true, false, false),
                
                TextFunc = () =>  "Z",
                    
                HintFunc = () => "",
            };
            
            AFKDecorator = new TopLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                BorderBrush = Hud.Render.CreateBrush(0, 182, 26, 255, 1),
                TextFont = Hud.Render.CreateFont("Segoe UI Light", 30, 255, 255, 0, 0, true, false, 255, 255, 255, 255, true),
                
                TextFunc = () =>  "AFK",
                    
                HintFunc = () => "",
            };
            
            NemesisDecorator = new TopLabelDecorator(Hud)
            {
                BackgroundTexture1 = Hud.Texture.Button2TextureBrown,
                BackgroundTextureOpacity1 = 0.8f,
                TextFont = Hud.Render.CreateFont("Segoe UI Light", 7, 250, 212, 144, 0, false, false, true),

                TextFunc = () => "[N]",
                HintFunc = () =>  "Nemesis equipped",
            };
            
            UnityDecorator = new TopLabelDecorator(Hud)
            {
                BackgroundTexture1 = Hud.Texture.Button2TextureBrown,
                BackgroundTextureOpacity1 = 0.8f,
                TextFont = Hud.Render.CreateFont("Segoe UI Light", 7, 250, 212, 144, 0, false, false, true),

                TextFunc = () => "[U]",
                HintFunc = () =>  "Unity equipped",
            };

        
        }

        public void PaintTopInGame(ClipState clipState)
        {
            if (Hud.Render.UiHidden) return;
            if (clipState != ClipState.BeforeClip) return;
            if (Hud.Game.Me.PortraitUiElement.Rectangle == null) return;
            var uiRect = Hud.Game.Me.PortraitUiElement.Rectangle;
            
            
            if (Hud.Game.Me.CurrentLevelNormal == 70)
                   {
                     ParagonPercentage = string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", (Hud.Game.Me.CurrentLevelParagonFloat - Hud.Game.Me.CurrentLevelParagon) * 100);
                   }
            else if (Hud.Game.Me.CurrentLevelNormal < 70)
                   {
                     ParagonPercentage = string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", Convert.ToSingle(Hud.Game.Me.CurrentLevelNormal)/70*100); 
                   }
            
            
            
            if (ParagonPercentageOnTheRight & DisplayParagonPercentage)
            {
            ParagonPercentageDecorator.Paint(uiRect.Left + uiRect.Width * 0.71f, uiRect.Top + uiRect.Height * 0.79f, uiRect.Width * 0.48f, uiRect.Height * 0.14f, HorizontalAlign.Center);
             }
            else if (DisplayParagonPercentage) 
            {   
            ParagonPercentageDecorator.Paint(uiRect.Left + uiRect.Width * -0.18f, uiRect.Top + uiRect.Height * 0.79f, uiRect.Width * 0.48f, uiRect.Height * 0.14f, HorizontalAlign.Center);
             }; 

             
              foreach (var player in Hud.Game.Players.OrderBy(p => p.PortraitIndex))
              {
                  if (player == null) continue;
                  if (player.PortraitUiElement == null) continue;
                  if (player.PortraitUiElement.Rectangle == null) continue;
                  var portrait = player.PortraitUiElement.Rectangle;

                  GRlevel = player.HighestHeroSoloRiftLevel; 
                  SheetDPS = player.Offense.SheetDps;
                  EHP = player.Defense.EhpCur;
                  Class = (IsZDPS(player)) ? "Z " + player.HeroClassDefinition.HeroClass  : player.HeroClassDefinition.HeroClass.ToString();;
                  var Nemo = player.Powers.GetBuff(318820);
                  if (Nemo == null || !Nemo.Active) {Nemesis = "";} else {Nemesis = " [Nemesis]";}
                  var Unit = player.Powers.GetBuff(318769);
                  if (Unit == null || !Unit.Active) {Unity = "";} else {Unity = " [Unity]";}
                  var MyUnit = Hud.Game.Me.Powers.GetBuff(318769);
                  
                  
                  if (player.CurrentLevelNormal == 70 && ShowGreaterRiftMaxLevel)
                   {
                      
                      var grk = Hud.Inventory.GetSnoItem(2835237830);
                      var texture = Hud.Texture.GetItemTexture(grk);
                     
                     HighestSoloRiftLevelDecorator.Paint(portrait.Left + portrait.Width * 0.26f, portrait.Top + portrait.Height * 0.2f, portrait.Width * 0.5f, portrait.Height * 0.1f, HorizontalAlign.Center);
                     texture.Draw(portrait.Left + portrait.Width * 0.26f, portrait.Top + portrait.Height * 0.17f, 20f, 20f, 1f); 
                   }
                  
                  
                  if (!player.IsMe)
                    {
                       if (Nemo == null || !Nemo.Active){}
                       else
                       {
                        if (ParagonPercentageOnTheRight)
                          { 
                        
                            NemesisDecorator.Paint(portrait.Left + portrait.Width * 0.71f, portrait.Top + portrait.Height * 0.79f, portrait.Width * 0.28f, portrait.Height * 0.14f, HorizontalAlign.Center);
                          }
                         else 
                          { 
                            NemesisDecorator.Paint(portrait.Left + portrait.Width * -0.18f, portrait.Top + portrait.Height * 0.79f, portrait.Width * 0.28f, portrait.Height * 0.14f, HorizontalAlign.Center);
                          };
                      
                       };
                    };
                
                
                
                  if (MyUnit == null || !MyUnit.Active){}
                  else {
                        
                         if (!player.IsMe)
                            {
                            
                              if (Unit == null || !Unit.Active){}
                              else {
                                  
                                      if (Nemo == null || !Nemo.Active)
                                         {
                                             if (ParagonPercentageOnTheRight)
                                                {   
                          
                                                 UnityDecorator.Paint(portrait.Left + portrait.Width * 0.71f, portrait.Top + portrait.Height * 0.79f, portrait.Width * 0.28f, portrait.Height * 0.14f, HorizontalAlign.Center);
                                                }
                                             else 
                                                {   
                                                 UnityDecorator.Paint(portrait.Left + portrait.Width * -0.18f, portrait.Top + portrait.Height * 0.79f, portrait.Width * 0.28f, portrait.Height * 0.14f, HorizontalAlign.Center);
                                                };
                                         }  
                                         else
                                         {
                                  
                                             if (ParagonPercentageOnTheRight)
                                                {   
                        
                                                  UnityDecorator.Paint(portrait.Left + portrait.Width * 0.71f, portrait.Top + portrait.Height * 0.93f, portrait.Width * 0.28f, portrait.Height * 0.14f, HorizontalAlign.Center);
                                                }
                                             else 
                                                {   
                                                   UnityDecorator.Paint(portrait.Left + portrait.Width * -0.18f, portrait.Top + portrait.Height * 0.93f, portrait.Width * 0.28f, portrait.Height * 0.14f, HorizontalAlign.Center);
                                                };
                    
                                         }
                                   }
                            
                            }
                
                       }
                       
                if (IsZDPS(player)) ZDPSDecorator.Paint(portrait.Left + portrait.Width * 0.26f, portrait.Top + portrait.Height * 0.4f, portrait.Width * 0.5f, portrait.Height * 0.1f, HorizontalAlign.Center);
                
                 
                if (player.AnimationState == AcdAnimationState.Idle && player.CoordinateKnown)
                 {
                  if (player.PortraitIndex == 0)
                  { 
                   if (player.FloorCoordinate != Player0pos) _watch0.Restart();
                   int AFK0 = (int)(_watch0.ElapsedMilliseconds/60000);
                   if (AFK0 > 3) AFKDecorator.Paint(portrait.Left + portrait.Width * 0.26f, portrait.Top + portrait.Height * 0.4f, portrait.Width * 0.5f, portrait.Height * 0.1f, HorizontalAlign.Center);
                  }
                  if (player.PortraitIndex == 1) 
                  { 
                   if (player.FloorCoordinate != Player1pos) _watch1.Restart();
                   int AFK1 = (int)(_watch1.ElapsedMilliseconds/60000);
                   if (AFK1 > 3) AFKDecorator.Paint(portrait.Left + portrait.Width * 0.26f, portrait.Top + portrait.Height * 0.4f, portrait.Width * 0.5f, portrait.Height * 0.1f, HorizontalAlign.Center);
                  }
                  if (player.PortraitIndex == 2) 
                  { 
                   if (player.FloorCoordinate != Player2pos) _watch2.Restart();
                   int AFK2 = (int)(_watch2.ElapsedMilliseconds/60000);
                   if (AFK2 > 3) AFKDecorator.Paint(portrait.Left + portrait.Width * 0.26f, portrait.Top + portrait.Height * 0.4f, portrait.Width * 0.5f, portrait.Height * 0.1f, HorizontalAlign.Center);
                  }
                  if (player.PortraitIndex == 3) 
                  { 
                   if (player.FloorCoordinate != Player3pos) _watch3.Restart();
                   int AFK3 = (int)(_watch3.ElapsedMilliseconds/60000);
                   if (AFK3 > 3) AFKDecorator.Paint(portrait.Left + portrait.Width * 0.26f, portrait.Top + portrait.Height * 0.4f, portrait.Width * 0.5f, portrait.Height * 0.1f, HorizontalAlign.Center);
                  }
                  
                 }
                else
                 {
                  if (player.PortraitIndex == 0) _watch0.Restart();
                  if (player.PortraitIndex == 1) _watch1.Restart();
                  if (player.PortraitIndex == 2) _watch2.Restart();
                  if (player.PortraitIndex == 3) _watch3.Restart();
                 }
               
                  if (player.PortraitIndex == 0) Player0pos = player.FloorCoordinate;
                  if (player.PortraitIndex == 1) Player1pos = player.FloorCoordinate;
                  if (player.PortraitIndex == 2) Player2pos = player.FloorCoordinate;
                  if (player.PortraitIndex == 3) Player3pos = player.FloorCoordinate;
            
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
         
         if (player.Offense.SheetDps < 500000f) Points++;
         if (player.Offense.SheetDps > 1500000f) Points--;
         
         if (player.Defense.EhpMax > 80000000f) Points++;
         
         var ConventionRing = player.Powers.GetBuff(430674);
         if (ConventionRing == null || !ConventionRing.Active) {} else {Points--;}
         
         var Stricken = player.Powers.GetBuff(428348);
         if (Stricken == null || !Stricken.Active) {} else {Points--;}
         
        
        if (Points >= 4) {return true;} else {return false;}
         
        }
        
/*         public void OnChatLineChanged(string currentLine, string previousLine)
        {
         string Message = currentLine;
            
         var PlayerOne = Hud.Game.Players.Where(x => x.PortraitIndex == 0).FirstOrDefault();
         var PlayerTwo = Hud.Game.Players.Where(x => x.PortraitIndex == 1).FirstOrDefault();
         var PlayerThree = Hud.Game.Players.Where(x => x.PortraitIndex == 2).FirstOrDefault();
         var PlayerFour = Hud.Game.Players.Where(x => x.PortraitIndex == 3).FirstOrDefault();
         
         
         if (PlayerOne != null && Message.Contains(PlayerOne.BattleTagAbovePortrait))
          {
           if(!Message.Contains("AFK")) _watch0.Restart();
          }
         else if (PlayerTwo != null && Message.Contains(PlayerTwo.BattleTagAbovePortrait))
          {
           if(!Message.Contains("AFK")) _watch1.Restart();
          }
         else if (PlayerThree != null && Message.Contains(PlayerThree.BattleTagAbovePortrait))
          {
           if(!Message.Contains("AFK")) _watch2.Restart();
          }
         else if (PlayerFour != null && Message.Contains(PlayerFour.BattleTagAbovePortrait))
          {
           if(!Message.Contains("AFK")) _watch3.Restart();
          }
        } */
        
        
        public void Customize()
        {
         var experiencePlugin = Hud.GetPlugin<TopExperienceStatistics>();
         TimeToNextParagon = experiencePlugin.TimeToParagonLevel(Hud.Game.Me.CurrentLevelParagon + 1, false);
        }
    }
}
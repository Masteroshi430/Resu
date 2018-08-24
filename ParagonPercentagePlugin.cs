//css_reference C:\V7.7.1.dll;
// https://github.com/User5981/Resu
// Paragon Percentage Plugin for TurboHUD Version 19/08/2018 10:50

using System;
using System.Globalization;
using Turbo.Plugins.Default;
using System.Linq;





namespace Turbo.Plugins.Resu
{
    public class ParagonPercentagePlugin : BasePlugin, IInGameTopPainter
    {
        public bool ShowGreaterRiftMaxLevel { get; set; }
        public bool ParagonPercentageOnTheRight { get; set; }
        public string ParagonPercentage { get; set; }
        
        public TopLabelDecorator ParagonPercentageDecorator { get; set; }
        public TopLabelDecorator HighestSoloRiftLevelDecorator { get; set; }
        public TopLabelDecorator NemesisDecorator { get; set; }
        public TopLabelDecorator UnityDecorator { get; set; }
        
        
        public int GRlevel { get; set; }
        public float SheetDPS { get; set; }
        public float EHP { get; set; }
        public string Class { get; set; }
        public string Nemesis { get; set; }
        public string Unity { get; set; }
                
        public ParagonPercentagePlugin()
        {
            Enabled = true;
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);

            ShowGreaterRiftMaxLevel = true;
            ParagonPercentageOnTheRight = true;
            ParagonPercentage = "0";
            
            var experiencePlugin = Hud.GetPlugin<TopExperienceStatistics>();
            
            ParagonPercentageDecorator = new TopLabelDecorator(Hud)
            {
                BackgroundTexture1 = Hud.Texture.Button2TextureBrown,
                BackgroundTextureOpacity1 = 0.8f,
                TextFont = Hud.Render.CreateFont("Segoe UI Light", 7, 250, 255, 255, 255, false, false, true),

                TextFunc = () => ParagonPercentage,  

                HintFunc = () => "Paragon level " + (Hud.Game.Me.CurrentLevelParagon + 1) + " in " + experiencePlugin.TimeToParagonLevel(Hud.Game.Me.CurrentLevelParagon + 1, false) +  Environment.NewLine + "EXP/h : " + ValueToString(Hud.Game.CurrentHeroToday.GainedExperiencePerHourPlay, ValueFormat.ShortNumber),
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

            var uiRect = Hud.Game.Me.PortraitUiElement.Rectangle;
            
            
            if (Hud.Game.Me.CurrentLevelNormal == 70)
                   {
                     ParagonPercentage = string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", (Hud.Game.Me.CurrentLevelParagonFloat - Hud.Game.Me.CurrentLevelParagon) * 100);
                   }
            else if (Hud.Game.Me.CurrentLevelNormal < 70)   
                   {
                     ParagonPercentage = string.Format(CultureInfo.InvariantCulture, "{0:0.##}%", Convert.ToSingle(Hud.Game.Me.CurrentLevelNormal)/70*100); 
                   }
            
            
            
            if (ParagonPercentageOnTheRight)
            {   
            ParagonPercentageDecorator.Paint(uiRect.Left + uiRect.Width * 0.71f, uiRect.Top + uiRect.Height * 0.79f, uiRect.Width * 0.48f, uiRect.Height * 0.14f, HorizontalAlign.Center);
             }
            else 
            {   
            ParagonPercentageDecorator.Paint(uiRect.Left + uiRect.Width * -0.18f, uiRect.Top + uiRect.Height * 0.79f, uiRect.Width * 0.48f, uiRect.Height * 0.14f, HorizontalAlign.Center);
             }; 

             
                 foreach (var player in Hud.Game.Players.OrderBy(p => p.PortraitIndex))
              {
                  if (player == null) continue;
                  var portrait = player.PortraitUiElement.Rectangle;

                        
                  GRlevel = player.HighestHeroSoloRiftLevel; 
                  SheetDPS = player.Offense.SheetDps;
                  EHP = player.Defense.EhpCur;
                  Class = player.HeroClassDefinition.HeroClass.ToString();
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
                       
                  
            
              };
             
        }
    }
}
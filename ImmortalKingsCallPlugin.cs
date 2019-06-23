// https://github.com/User5981/Resu
// Immortal King's Call Plugin for TurboHUD Version 09/07/2018 16:05

using System;
using System.Globalization;
using Turbo.Plugins.Default;
using System.Linq;

namespace Turbo.Plugins.Resu
{
    public class ImmortalKingsCallPlugin : BasePlugin, IInGameTopPainter
    {
        public TopLabelDecorator ImmortalKingsCallDecorator { get; set; }
        public string ImmortalKingsCall { get; set; }
                
        public ImmortalKingsCallPlugin()
        {
            Enabled = true;
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);
            ImmortalKingsCall = "-1";
            
            ImmortalKingsCallDecorator = new TopLabelDecorator(Hud)
            {
                BackgroundTexture1 = Hud.Texture.Button2TextureBrown,
                BackgroundTextureOpacity1 = 0.0f, 
                TextFont = Hud.Render.CreateFont("Segoe UI Light", 7, 250, 212, 144, 0, false, false, true),

                TextFunc = () => ImmortalKingsCall,
                HintFunc = () =>  "Immortal King's Call status",
            };
    
        }
                
        public void PaintTopInGame(ClipState clipState)
        {
            if (Hud.Render.UiHidden) return;
            if (clipState != ClipState.BeforeClip) return;
            if (Hud.Game.Me.HeroClassDefinition.HeroClass.ToString() != "Barbarian") return;

                  var uiRect = Hud.Game.Me.PortraitUiElement.Rectangle;
                  var WrathOfTheBerserker = Hud.Game.Me.Powers.GetBuff(79607);
                  var CallOfTheAncients = Hud.Game.Me.Powers.GetBuff(80049);
                  
                       if (WrathOfTheBerserker == null || !WrathOfTheBerserker.Active || CallOfTheAncients == null || !CallOfTheAncients.Active)
                       {
                        
                       }    
                       else
                       {
                            ImmortalKingsCall = "+1500% " + (int)WrathOfTheBerserker.TimeLeftSeconds[0] + "s"; 
                            
                            if (Hud.Game.NumberOfPlayersInGame == 1) 
                              {
                               ImmortalKingsCallDecorator.Paint(uiRect.Left + uiRect.Width * 2.40f, uiRect.Top + uiRect.Height * 0.50f, uiRect.Width * 0.28f, uiRect.Height * 0.14f, HorizontalAlign.Center);
                              }
                            else 
                              {
                               ImmortalKingsCallDecorator.Paint(uiRect.Left + uiRect.Width * 1.33f, uiRect.Top + uiRect.Height * 0.50f, uiRect.Width * 0.28f, uiRect.Height * 0.14f, HorizontalAlign.Center);   
                              }
                      
                       };
        
        }
        
    }
}
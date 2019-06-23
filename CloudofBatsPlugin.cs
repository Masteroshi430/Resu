// https://github.com/User5981/Resu
// Cloud of Bats Plugin for TurboHUD Version 22/01/2018 08:08

using System;
using System.Globalization;
using Turbo.Plugins.Default;
using System.Linq;

namespace Turbo.Plugins.Resu
{
    public class CloudofBatsPlugin : BasePlugin, IInGameTopPainter
    {
        
        public IBrush OutlineBrush { get; set; }
       
                
        public CloudofBatsPlugin()
        {
            Enabled = true;
            
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);
             OutlineBrush = Hud.Render.CreateBrush(30, 252, 126, 0, 3);
        }

        public void PaintTopInGame(ClipState clipState)
        {
          if (Hud.Game.Me.HeroClassDefinition.HeroClass!= HeroClass.WitchDoctor) return;
          
          var Skills = Hud.Game.Me.Powers.CurrentSkills;
          if (Skills == null) return;
          foreach (var skill in Skills)
          {
            if (skill.RuneNameEnglish == "Cloud of Bats") 
            {
              if (!Hud.Game.Me.InCombat) return;
              OutlineBrush.DrawWorldEllipse(15, -1, Hud.Game.Me.FloorCoordinate);   
            }               
        
          }       

        }
    }
}
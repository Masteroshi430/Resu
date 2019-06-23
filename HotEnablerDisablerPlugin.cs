// https://github.com/User5981/Resu
// Hot Enabler/Disabler Plugin for TurboHUD Version 24/12/2017 10:24

using System;
using System.Collections.Generic;
using Turbo.Plugins.Default;
using System.Linq;

namespace Turbo.Plugins.Resu
{

    public class HotEnablerDisablerPlugin : BasePlugin 
    {

       public Dictionary<string,string> DisableThatGameMode { get; set; }
       public Dictionary<string,string> DisableThatGameType { get; set; }
       public Dictionary<string,string> DisableTheseHeroClasses { get; set; }
       public Dictionary<string,string> DisableTheseHeroNames { get; set; }    
        
        public HotEnablerDisablerPlugin()
        {
            Enabled = true;
            DisableThatGameMode = new Dictionary<string,string>();
            DisableThatGameType = new Dictionary<string,string>();
            DisableTheseHeroClasses = new Dictionary<string,string>();
            DisableTheseHeroNames = new Dictionary<string,string>();
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            
        }
        
        
        
        public bool CanIRun(IPlayer me, string ThisPlugin)
             {
              bool Hardcore = me.HeroIsHardcore;
              bool Seasonal = me.Hero.Seasonal;
              string Heroclass = me.HeroClassDefinition.HeroClass.ToString();
              var ExcludeGameMode = "";
              var ExcludeGameType = "";
              var ExcludeHeroClasses = "";
              var ExcludeHeroNames = "";
              
              
        
              if (DisableThatGameMode.TryGetValue(ThisPlugin, out ExcludeGameMode)) 
                 {
                  if (Hardcore && ExcludeGameMode == "Hardcore") return false;
                  else if (!Hardcore && ExcludeGameMode == "Softcore") return false;
                  else goto NoGameMode;
                 }
              else goto NoGameMode;
              
             NoGameMode:
             if (DisableThatGameType.TryGetValue(ThisPlugin, out ExcludeGameType))
                {
                 if (Seasonal && ExcludeGameType == "Seasonal") return false;
                 else if (!Seasonal && ExcludeGameType == "NonSeasonal") return false;  
                 else goto NoGameType;
                } 
             else goto NoGameType;  
                
              NoGameType: 
              if (DisableTheseHeroClasses.TryGetValue(ThisPlugin, out ExcludeHeroClasses))
                 {
                  if (ExcludeHeroClasses.Contains(Heroclass)) return false;
                  else goto NoHeroClass;
                 }
              else goto NoHeroClass; 
                 
              NoHeroClass:                       
              if (DisableTheseHeroNames.TryGetValue(ThisPlugin, out ExcludeHeroNames))
                 {
                  if (ExcludeHeroNames.Contains(me.HeroName)) return false;  
                  else return true;  
                 }
              else return true;               
              }
    }

}
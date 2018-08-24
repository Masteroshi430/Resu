//css_reference C:\V7.7.1.dll;
// https://github.com/User5981/Resu
// Channeling Plugin for TurboHUD Version 15/08/2018 21:56

using System;
using System.Globalization;
using Turbo.Plugins.Default;
using System.Linq;
using System.Threading;


namespace Turbo.Plugins.Resu
{
    public class ChannelingPlugin : BasePlugin, IInGameTopPainter
    {
        
        public int ResourceMax { get; set; }
        public int ResourceMin { get; set; }
        public bool isOn { get; set; }
        public bool HighNotification { get; set; }
        public bool LowNotification { get; set; }
                
        public ChannelingPlugin()
        {
            Enabled = true;
            ResourceMax = 100;
            ResourceMin = 15;
            isOn = false;
            HighNotification = true;
            LowNotification = true;
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);
        }

        public void PaintTopInGame(ClipState clipState)
        {
            var hedPlugin = Hud.GetPlugin<HotEnablerDisablerPlugin>();
            bool GoOn = hedPlugin.CanIRun(Hud.Game.Me,this.GetType().Name); 
            if (!GoOn) return;
            
            float resource = 0f;
            
            switch (Hud.Game.Me.HeroClassDefinition.HeroClass)
            {
                case HeroClass.Barbarian:
                    resource = Hud.Game.Me.Stats.ResourcePctFury;
                    break;
                case HeroClass.Crusader:
                    resource = Hud.Game.Me.Stats.ResourcePctWrath;
                    break;
                case HeroClass.DemonHunter:
                    resource = Hud.Game.Me.Stats.ResourcePctHatred;
                    //resource = Hud.Game.Me.Stats.ResourcePctDiscipline;
                    break;
                case HeroClass.Monk:
                    resource = Hud.Game.Me.Stats.ResourcePctSpirit;
                    break;
                case HeroClass.Necromancer:
                    resource = Hud.Game.Me.Stats.ResourcePctEssence;
                    break;
                case HeroClass.WitchDoctor:
                    resource = Hud.Game.Me.Stats.ResourcePctMana;
                    break;
                case HeroClass.Wizard:
                    resource = Hud.Game.Me.Stats.ResourcePctArcane;
                    break;
            }               
            if (!Hud.Sound.IsIngameSoundEnabled) return;                      
            if (resource >= ResourceMax && isOn == true)
               {
                 if (Hud.Game.Me.IsDead || Hud.Game.IsInTown){ isOn = false; }
                 else if (HighNotification)
                         { 
                           
                           var highSound = Hud.Sound.LoadSoundPlayer("Resource-Full-By-Resu.wav");
            
                           ThreadPool.QueueUserWorkItem(state =>
                           {
                            try 
                               { 
                                highSound.PlaySync(); 
                               } 
                            catch (Exception) 
                              { 
                              } 
                         });  
                        
                           isOn = false;    
                         }
               }
            else if (resource <= ResourceMin && isOn == false)
                    {
                      if    (Hud.Game.Me.IsDead || Hud.Game.IsInTown){ isOn = true; }
                      else if (LowNotification)
                              {  
                                var lowSound = Hud.Sound.LoadSoundPlayer("Resource-Low-By-Resu.wav");
            
                                ThreadPool.QueueUserWorkItem(state =>
                                {
                                 try 
                                    { 
                                     lowSound.PlaySync(); 
                                    } 
                                 catch (Exception) 
                                   { 
                                   } 
                                  
                                }); 
                        
                                isOn = true;    
                              } 
    
                    }
            
                    

        }
    }
}
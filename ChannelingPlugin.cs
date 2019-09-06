// https://github.com/User5981/Resu
// Channeling Plugin for TurboHUD Version 06/09/2019 16:11

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
        public int DisciplineMin { get; set; }
        public int DisciplineMax { get; set; }
        public int HatredMin { get; set; }
        public int HatredMax { get; set; }
        public bool isOn { get; set; }
        public bool DisciplineIsOn { get; set; }
        public bool HatredIsOn { get; set; }
        public bool HighNotification { get; set; }
        public bool LowNotification { get; set; }

        public ChannelingPlugin()
        {
            Enabled = true;
            ResourceMax = 100;
            ResourceMin = 15;
            DisciplineMax = 100;
            DisciplineMin = 15;
            HatredMax = 100;
            HatredMin = 15;
            isOn = false;
            DisciplineIsOn = false;
            HatredIsOn = false;
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
            bool IsDemonHunter = false;

            switch (Hud.Game.Me.HeroClassDefinition.HeroClass)
            {
                case HeroClass.Barbarian:
                    resource = Hud.Game.Me.Stats.ResourcePctFury;
                    break;
                case HeroClass.Crusader:
                    resource = Hud.Game.Me.Stats.ResourcePctWrath;
                    break;
                case HeroClass.DemonHunter:
                    IsDemonHunter = true;
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

            if (!IsDemonHunter)
            {
                if (resource >= ResourceMax && isOn == true)
                {
                    if (Hud.Game.Me.IsDead || Hud.Game.IsInTown)
                    { isOn = false; }
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
                    else
                        isOn = false;
                }
                else if (resource <= ResourceMin && isOn == false)
                {
                    if (Hud.Game.Me.IsDead || Hud.Game.IsInTown)
                    { }
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
                    else
                        isOn = true;
                }
            }
           else
            {
             var Discipline = Hud.Game.Me.Stats.ResourcePctDiscipline;
             var Hatred = Hud.Game.Me.Stats.ResourcePctHatred;

                if (Discipline >= DisciplineMax && DisciplineIsOn == true)
                {
                    if (Hud.Game.Me.IsDead || Hud.Game.IsInTown)
                    { DisciplineIsOn = false; }
                    else if (HighNotification)
                    {
                        var highSound = Hud.Sound.LoadSoundPlayer("Discipline-Full-By-Resu.wav");
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
                        DisciplineIsOn = false;
                    }
                    else
                        DisciplineIsOn = false;
                }
                else if (Discipline <= DisciplineMin && DisciplineIsOn == false)
                {
                    if (Hud.Game.Me.IsDead || Hud.Game.IsInTown)
                    { }
                    else if (LowNotification)
                    {
                        var lowSound = Hud.Sound.LoadSoundPlayer("Discipline-Low-By-Resu.wav");
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
                        DisciplineIsOn = true;
                    }
                    else
                        DisciplineIsOn = true;
                }


                if (Hatred >= HatredMax && HatredIsOn == true)
                {
                    if (Hud.Game.Me.IsDead || Hud.Game.IsInTown)
                    { HatredIsOn = false; }
                    else if (HighNotification)
                    {
                        var highSound = Hud.Sound.LoadSoundPlayer("Hatred-Full-By-Resu.wav");
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
                        HatredIsOn = false;
                    }
                    else
                        HatredIsOn = false;
                }
                else if (Hatred <= HatredMin && HatredIsOn == false)
                {
                    if (Hud.Game.Me.IsDead || Hud.Game.IsInTown)
                    { }
                    else if (LowNotification)
                    {
                        var lowSound = Hud.Sound.LoadSoundPlayer("Hatred-Low-By-Resu.wav");
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
                        HatredIsOn = true;
                    }
                    else
                        HatredIsOn = true;
                }
            }
        }
    }
}

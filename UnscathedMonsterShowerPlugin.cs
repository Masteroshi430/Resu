//css_reference C:\V7.7.1.dll;
// https://github.com/User5981/Resu
// Unscathed Monster Shower plugin for TurboHUD version 14/10/2018 08:07
using Turbo.Plugins.Default;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Turbo.Plugins.Resu
{

    public class UnscathedMonsterShowerPlugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection UnscathedMonsterDecorator { get; set; }
        public static HashSet<string> DangerousMonsters = new HashSet<string>() {"Wood Wraith", "Highland Walker", "The Old Man", "Fallen Lunatic", "Deranged Fallen", "Fallen Maniac", "Frenzied Lunatic", "Herald of Pestilence", "Terror Demon", "Demented Fallen", "Savage Beast", "Tusked Bogan", "Punisher", "Anarch", "Corrupted Angel", "Winged Assassin", "Exarch"};
        
        
        public UnscathedMonsterShowerPlugin()
        {
            Enabled = true;
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);
            
            UnscathedMonsterDecorator = new WorldDecoratorCollection(
            new GroundLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 6, 255, 255, 0, 0, true, false, 0, 255, 255, 0, false)
            }
            );
            
        }
        
        public void PaintWorld(WorldLayer layer)
        {
         double MyMaxWeaponRange = 130D;
         var UnscathedMonsters = Hud.Game.AliveMonsters.Where(x => x.MaxHealth == x.CurHealth && x.Rarity == ActorRarity.Normal && !x.Untargetable && !x.Invisible && !DangerousMonsters.Contains(x.SnoActor.NameEnglish) &&x.NormalizedXyDistanceToMe < MyMaxWeaponRange);
         foreach (var UnscathedMonster in UnscathedMonsters)
         {
          if ((UnscathedMonster.AcdId + (Hud.Time.Now.Millisecond/2)) % 40 == 0){}
          else if ((UnscathedMonster.AcdId + (Hud.Time.Now.Millisecond/2)) % 99 == 0) UnscathedMonsterDecorator.Paint(layer, UnscathedMonster, UnscathedMonster.FloorCoordinate.Offset(0, 0, (UnscathedMonster.RadiusScaled*3)), "   \u2B2C");
          else UnscathedMonsterDecorator.Paint(layer, UnscathedMonster, UnscathedMonster.FloorCoordinate.Offset(0, 0, (UnscathedMonster.RadiusScaled*3)), "\u2B2C \u2B2C"); // round \u2022 eye \uD83D\uDC41
         }
        
        }

    }

}
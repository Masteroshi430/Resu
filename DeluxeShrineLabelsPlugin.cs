//css_reference C:\V7.7.1.dll;
// https://github.com/User5981/Resu
// Deluxe Shrine labels plugin for TurboHUD version 13/08/2018 08:19
// Psycho's Shrine labels plugin with new features 

using Turbo.Plugins.Default;
using System.Collections.Generic;
using System.Linq;
using System;



namespace Turbo.Plugins.Resu
{
    public class DeluxeShrineLabelsPlugin : BasePlugin, ICustomizer, IInGameWorldPainter
    {
        public Dictionary<ShrineType, WorldDecoratorCollection> ShrineDecorators { get; set; }
        public Dictionary<ShrineType, WorldDecoratorCollection> ShrineShortDecorators { get; set; }
        public Dictionary<ShrineType, string> ShrineCustomNames { get; set; }
        public Dictionary<ShrineType, string> ShrineCustomNamesShort { get; set;}
        public WorldDecoratorCollection PossibleRiftPylonDecorators { get; set; }
        public string PossibleRiftPylonName { get; set; }
        public bool ShowHealingWells { get; set;}
        public bool ShowPoolOfReflection { get; set;}
        public bool ShowAllWhenHealthIsUnder40 { get; set;}

        public DeluxeShrineLabelsPlugin()
        {
            Enabled = true;
            ShowAllWhenHealthIsUnder40 = true;
        }

        public WorldDecoratorCollection CreateMapDecorators(float size = 6f, int a = 192, int r = 255, int g = 255, int b = 55, float radiusOffset = 5f)
        {
            return new WorldDecoratorCollection(
                new MapLabelDecorator(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", size, a, r, g, b, false, false, 128, 0, 0, 0, true),
                    RadiusOffset = radiusOffset,
                });
        }

        public WorldDecoratorCollection CreateGroundLabelDecorators(float size = 6f, int a = 192, int r = 255, int g = 255, int b = 55,int bga = 255, int bgr = 0, int bgg = 0, int bgb = 0)
        {
            var grounLabelBackgroundBrush = Hud.Render.CreateBrush(bga, bgr, bgg, bgb, 0);
            return new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = grounLabelBackgroundBrush,
                    BorderBrush = Hud.Render.CreateBrush(a, r, g, b, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", size, a, r, g, b, false, false, 128, 0, 0, 0, true),
                });
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);

            ShrineDecorators = new Dictionary<ShrineType, WorldDecoratorCollection>();
            ShrineShortDecorators = new Dictionary<ShrineType, WorldDecoratorCollection>();
            ShrineCustomNames = new Dictionary<ShrineType, string>();
            ShrineCustomNamesShort = new Dictionary<ShrineType, string>();
            
            
            foreach (ShrineType shrine in Enum.GetValues(typeof(ShrineType)))
            {
                ShrineDecorators[shrine] = CreateMapDecorators();
                ShrineShortDecorators[shrine] = CreateGroundLabelDecorators();
                ShrineCustomNames[shrine] = string.Empty;
                ShrineCustomNamesShort[shrine] = string.Empty;
            }
            PossibleRiftPylonDecorators = CreateMapDecorators();
            PossibleRiftPylonName = string.Empty;
        }

        public void Customize()
        {
            Hud.TogglePlugin<ShrinePlugin>(false);
        }

        public void PaintWorld(WorldLayer layer)
        {
            string NemesisMessage = "";
            string MyNemesisMessage = "";
            int MyNemesisCount = 0;
            foreach (var player in Hud.Game.Players.OrderBy(p => p.PortraitIndex))
            {
             if (player == null) continue;
             var Nemo = player.Powers.GetBuff(318820);
             if (Nemo == null || !Nemo.Active) {} 
             else
                {
                 if (player.IsMe) MyNemesisCount++;
                 else
                  {
                   if (NemesisMessage == "") NemesisMessage += Environment.NewLine + player.BattleTagAbovePortrait;
                   else NemesisMessage += " or " + player.BattleTagAbovePortrait;
                  }
                }
            }
            
            if (MyNemesisCount == 1) MyNemesisMessage = " or HIT ME!";
            if (NemesisMessage != "") NemesisMessage = Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine + "\uD83D\uDEC7 Leave for \uD83D\uDEC7" + NemesisMessage + MyNemesisMessage;
            if (MyNemesisCount == 1 && NemesisMessage == "") NemesisMessage = Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine + "HIT ME!";
            
            var shrines = Hud.Game.Shrines.Where(x => !x.IsDisabled && !x.IsOperated);
            foreach (var shrine in shrines)
            {
                if (ShowAllWhenHealthIsUnder40 && Hud.Game.Me.Defense.HealthPct < (float)40) {ShowHealingWells = true; ShowPoolOfReflection = true;}
                else if (ShowAllWhenHealthIsUnder40 && Hud.Game.Me.Defense.HealthPct >= (float)40) {ShowHealingWells = false; ShowPoolOfReflection = false;}

                if (shrine.Type == ShrineType.HealingWell && ShowHealingWells == false) continue;
                if (shrine.Type == ShrineType.PoolOfReflection && ShowPoolOfReflection == false) continue;
                if (shrine.Type == ShrineType.HealingWell && ShowHealingWells == true || shrine.Type == ShrineType.PoolOfReflection && ShowPoolOfReflection == true) NemesisMessage ="";
                
                var shrineName = (ShrineCustomNames[shrine.Type] != string.Empty) ? ShrineCustomNames[shrine.Type] : shrine.SnoActor.NameLocalized;
                ShrineDecorators[shrine.Type].Paint(layer, shrine, shrine.FloorCoordinate, shrineName);

                var ShrineNameShort = (ShrineCustomNamesShort[shrine.Type] != string.Empty) ? ShrineCustomNamesShort[shrine.Type] : shrine.SnoActor.NameLocalized;
                
                switch (shrine.Type)
                {
                 case ShrineType.BlessedShrine:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine + "-25% damage recieved"  + NemesisMessage;
                      break;
                 case ShrineType.EnlightenedShrine:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "+25% EXP gain"  + NemesisMessage;
                      break;
                 case ShrineType.FortuneShrine:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "+25% magic & gold find"  + NemesisMessage;
                      break;
                 case ShrineType.FrenziedShrine:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "+25% attack speed"  + NemesisMessage;
                      break;
                 case ShrineType.EmpoweredShrine:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "+100% resource gain" + Environment.NewLine + "-50% cooldown time"  + NemesisMessage;
                      break;
                 case ShrineType.FleetingShrine:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "+25% movement speed" + Environment.NewLine + "+20yd pickup radius"  + NemesisMessage;
                      break;
                 case ShrineType.PowerPylon:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "+400% damage dealt" + NemesisMessage;
                      break;
                 case ShrineType.ConduitPylon:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "\u26A1 HIGH VOLTAGE \u26A1" + NemesisMessage;
                      break;
                 case ShrineType.ChannelingPylon:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "-75% cooldown"  + Environment.NewLine + "No resource cost"  + NemesisMessage;
                      break;
                 case ShrineType.ShieldPylon:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "60s of invulnerability" + NemesisMessage;
                      break;
                 case ShrineType.SpeedPylon:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "+30% attack speed" + Environment.NewLine + "+80% movement speed"  + NemesisMessage;
                      break;
                 case ShrineType.PoolOfReflection:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "+25% EXP gain";
                      break;
                 case ShrineType.BanditShrine:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "GOBLINS!" + NemesisMessage;
                      break;
                 case ShrineType.HealingWell:
                      ShrineNameShort = ShrineNameShort + Environment.NewLine + "━━━━━━━━━━━━━━" + Environment.NewLine  + "restores life";
                      break;
                }
                
                ShrineShortDecorators[shrine.Type].Paint(layer, shrine, shrine.FloorCoordinate, ShrineNameShort);
            }

            var riftPylonSpawnPoints = Hud.Game.Actors.Where(x => x.SnoActor.Sno == 428690);
            foreach (var actor in riftPylonSpawnPoints)
            {
                PossibleRiftPylonDecorators.Paint(layer, actor, actor.FloorCoordinate, (PossibleRiftPylonName != string.Empty) ? PossibleRiftPylonName : "Pylon?");
            }
        }
    }
}
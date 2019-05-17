// https://github.com/User5981/Resu
// Other Player's heads Plugin for TurboHUD Version 17/05/2019 15:44

using System.Collections.Generic;
using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Resu
{

    public class OtherPlayersHeadsPlugin : BasePlugin, IInGameWorldPainter, ICustomizer
   {

        public Dictionary<HeroClass, WorldDecoratorCollection> DecoratorByClass { get; set; }
        public float NameOffsetX { get; set; }
        public float NameOffsetY { get; set; }
        public float NameOffsetZ { get; set; }
        public bool ShowCompanions { get; set; }
        public WorldDecoratorCollection ZDPSDecorator { get; set; }
        public bool FemalePlayer1 { get; set; }
        public bool FemalePlayer2 { get; set; }
        public bool FemalePlayer3 { get; set; }

        public OtherPlayersHeadsPlugin()
       {
            Enabled = true;
            DecoratorByClass = new Dictionary<HeroClass, WorldDecoratorCollection>();
            NameOffsetX = 0.0f;
            NameOffsetY = 0.0f;
            NameOffsetZ = 12.0f;
            ShowCompanions = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            var grounLabelBackgroundBrush = Hud.Render.CreateBrush(120, 0, 0, 0, 0);
            
            ZDPSDecorator = new WorldDecoratorCollection(
                new MapLabelDecorator2(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 255, 255, 255, false, false, 128, 0, 0, 0, true),
                    UpUp = true,
                });
            
            DecoratorByClass.Add(HeroClass.Barbarian, new WorldDecoratorCollection(
                new MapLabelDecorator2(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 237, 20, 20, false, false, 128, 0, 0, 0, true),
                    Down = true,
                },
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = grounLabelBackgroundBrush,
                    BorderBrush = Hud.Render.CreateBrush(255, 237, 20, 20, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 237, 20, 20, false, false, 128, 0, 0, 0, true),
                }
                ));

            DecoratorByClass.Add(HeroClass.Crusader, new WorldDecoratorCollection(
                new MapLabelDecorator2(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 255, 204, 0, false, false, 128, 0, 0, 0, true),
                    Down = true,
                },
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = grounLabelBackgroundBrush,
                    BorderBrush = Hud.Render.CreateBrush(255, 255, 204, 0, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 255, 204, 0, false, false, 128, 0, 0, 0, false),
                }
                ));

            DecoratorByClass.Add(HeroClass.DemonHunter, new WorldDecoratorCollection(
                new MapLabelDecorator2(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 0, 168, 255, false, false, 128, 0, 0, 0, true),
                    Down = true,
                },
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = grounLabelBackgroundBrush,
                    BorderBrush = Hud.Render.CreateBrush(255, 0, 168, 255, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 0, 168, 255, false, false, 128, 0, 0, 0, true),
                }
                ));

            DecoratorByClass.Add(HeroClass.Monk, new WorldDecoratorCollection(
                new MapLabelDecorator2(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 255, 132, 0, false, false, 128, 0, 0, 0, true),
                    Down = true,
                },
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = grounLabelBackgroundBrush,
                    BorderBrush = Hud.Render.CreateBrush(255, 255, 132, 0, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 255, 132, 0, false, false, 128, 0, 0, 0, true),
                }
                ));

            DecoratorByClass.Add(HeroClass.Necromancer, new WorldDecoratorCollection(
                new MapLabelDecorator2(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 215, 201, 164, false, false, 128, 0, 0, 0, true),
                    Down = true,
                },
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = grounLabelBackgroundBrush,
                    BorderBrush = Hud.Render.CreateBrush(255, 215, 201, 164, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 215, 201, 164, false, false, 128, 0, 0, 0, true),
                }
                ));

            DecoratorByClass.Add(HeroClass.WitchDoctor, new WorldDecoratorCollection(
                new MapLabelDecorator2(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 43, 231, 6, false, false, 128, 0, 0, 0, true),
                    Down = true,
                },
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = grounLabelBackgroundBrush,
                    BorderBrush = Hud.Render.CreateBrush(255, 43, 231, 6, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 43, 231, 6, false, false, 128, 0, 0, 0, true),
                }
                ));

            DecoratorByClass.Add(HeroClass.Wizard, new WorldDecoratorCollection(
                new MapLabelDecorator2(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 6f, 255, 182, 26, 255, false, false, 128, 0, 0, 0, true),
                    Down = true,
                },
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = grounLabelBackgroundBrush,
                    BorderBrush = Hud.Render.CreateBrush(255, 182, 26, 255, 1),
                    TextFont = Hud.Render.CreateFont("tahoma", 6f, 255, 182, 26, 255, false, false, 128, 0, 0, 0, true),
                }
                ));
                
                FemalePlayer1 = false;
                FemalePlayer2 = false;
                FemalePlayer3 = false;
        }

        public void PaintWorld(WorldLayer layer)
        {
            var players = Hud.Game.Players.Where(player => !player.IsMe && player.CoordinateKnown && Hud.Game.Me.SnoArea.Sno == player.SnoArea.Sno && (player.HeadStone == null));
            foreach (var player in players)
            {
                
                if (player != null && player.IsOnScreen)
                 {
                  if (player.SnoActor != null && (uint)player.SnoActor.Sno != 0 && player.SnoActor.Sno.ToString().Contains("female")) //is female
                   {
                    switch (player.PortraitIndex)
                     {
                      case 1:
                             FemalePlayer1 = true;
                             break;
                      case 2:
                             FemalePlayer2 = true;
                             break;
                      case 3:
                             FemalePlayer3 = true;
                             break;
                     }
                   }
                  else // is male
                   {
                    switch (player.PortraitIndex)
                     {
                      case 1:
                             FemalePlayer1 = false;
                             break;
                      case 2:
                             FemalePlayer2 = false;
                             break;
                      case 3:
                             FemalePlayer3 = false;
                             break;
                     }
                   }
                 }
                 
                var HeroTexture = Hud.Texture.GetTexture(890155253);
                
                if (player.HeroClassDefinition.HeroClass.ToString() == "Barbarian")
                   {
                    if ((player.PortraitIndex == 1 && !FemalePlayer1) || (player.PortraitIndex == 2 && !FemalePlayer2) || (player.PortraitIndex == 3 && !FemalePlayer3)) HeroTexture = Hud.Texture.GetTexture(3921484788);
                    else HeroTexture = Hud.Texture.GetTexture(1030273087);
                   }
                else if (player.HeroClassDefinition.HeroClass.ToString() == "Crusader")
                   {
                    if ((player.PortraitIndex == 1 && !FemalePlayer1) || (player.PortraitIndex == 2 && !FemalePlayer2) || (player.PortraitIndex == 3 && !FemalePlayer3)) HeroTexture = Hud.Texture.GetTexture(3742271755);
                    else HeroTexture = Hud.Texture.GetTexture(3435775766);
                   } 
                else if (player.HeroClassDefinition.HeroClass.ToString() == "DemonHunter")
                   {
                    if ((player.PortraitIndex == 1 && !FemalePlayer1) || (player.PortraitIndex == 2 && !FemalePlayer2) || (player.PortraitIndex == 3 && !FemalePlayer3)) HeroTexture = Hud.Texture.GetTexture(3785199803);
                    else HeroTexture = Hud.Texture.GetTexture(2939779782);
                   } 
                else if (player.HeroClassDefinition.HeroClass.ToString() == "Monk")
                   {
                    if ((player.PortraitIndex == 1 && !FemalePlayer1) || (player.PortraitIndex == 2 && !FemalePlayer2) || (player.PortraitIndex == 3 && !FemalePlayer3)) HeroTexture = Hud.Texture.GetTexture(2227317895);
                    else HeroTexture = Hud.Texture.GetTexture(2918463890);
                   } 
                else if (player.HeroClassDefinition.HeroClass.ToString() == "Necromancer")
                   {
                    if ((player.PortraitIndex == 1 && !FemalePlayer1) || (player.PortraitIndex == 2 && !FemalePlayer2) || (player.PortraitIndex == 3 && !FemalePlayer3)) HeroTexture = Hud.Texture.GetTexture(3285997023); 
                    else HeroTexture = Hud.Texture.GetTexture(473831658);   
                   }
                else if (player.HeroClassDefinition.HeroClass.ToString() == "WitchDoctor")
                   {
                    if ((player.PortraitIndex == 1 && !FemalePlayer1) || (player.PortraitIndex == 2 && !FemalePlayer2) || (player.PortraitIndex == 3 && !FemalePlayer3)) HeroTexture = Hud.Texture.GetTexture(3925954876);
                    else HeroTexture = Hud.Texture.GetTexture(1603231623);
                   }
                else if (player.HeroClassDefinition.HeroClass.ToString() == "Wizard")
                   {
                    if ((player.PortraitIndex == 1 && !FemalePlayer1) || (player.PortraitIndex == 2 && !FemalePlayer2) || (player.PortraitIndex == 3 && !FemalePlayer3)) HeroTexture = Hud.Texture.GetTexture(44435619);
                    else HeroTexture = Hud.Texture.GetTexture(876580014);
                   }
                 float PlayersHeadOpacity = 1f;
                 var Elites = Hud.Game.Monsters.Where(M => M.IsAlive && M.Rarity != ActorRarity.Normal && M.Rarity != ActorRarity.RareMinion && M.Rarity != ActorRarity.Hireling && M.FloorCoordinate.XYDistanceTo(player.FloorCoordinate) <= 25);
                 if (Elites.Count() > 0) PlayersHeadOpacity = 0.20f;
                 
                 Hud.Render.GetMinimapCoordinates(player.FloorCoordinate.X, player.FloorCoordinate.Y, out float textureX, out float textureY);
                 HeroTexture.Draw(textureX-11, textureY-11, 22.3f, 24.1f, PlayersHeadOpacity);
                
                if (!DecoratorByClass.TryGetValue(player.HeroClassDefinition.HeroClass, out WorldDecoratorCollection decorator)) continue;

                decorator.Paint(layer, null, player.FloorCoordinate.Offset(NameOffsetX, NameOffsetY, NameOffsetZ), player.BattleTagAbovePortrait);
                if(IsZDPS(player)) ZDPSDecorator.Paint(layer, null, player.FloorCoordinate, "Z");
            }
            
            if (ShowCompanions && Hud.Game.NumberOfPlayersInGame == 1)
             {
               var companions = Hud.Game.Actors.Where(C => C.SnoActor.Sno == ActorSnoEnum._hireling_scoundrel || C.SnoActor.Sno == ActorSnoEnum._hireling_enchantress || C.SnoActor.Sno == ActorSnoEnum._hireling_templar);
               foreach (var companion in companions)
               {
                  var CompTexture = Hud.Texture.GetTexture(890155253);
                  if (companion.SnoActor.Sno == ActorSnoEnum._hireling_scoundrel) CompTexture = Hud.Texture.GetTexture(441912908); // scoundrel
                  else if (companion.SnoActor.Sno == ActorSnoEnum._hireling_enchantress) CompTexture = Hud.Texture.GetTexture(2807221403); // enchantress
                  else if (companion.SnoActor.Sno == ActorSnoEnum._hireling_templar) CompTexture = Hud.Texture.GetTexture(1094113362); // templar
                  else continue; 
                 
                 float CompanionsHeadOpacity = 1f;
                 var Elites = Hud.Game.Monsters.Where(M => M.IsAlive && M.Rarity != ActorRarity.Normal && M.Rarity != ActorRarity.RareMinion && M.Rarity != ActorRarity.Hireling && M.FloorCoordinate.XYDistanceTo(companion.FloorCoordinate) <= 25);
                 if (Elites.Count() > 0) CompanionsHeadOpacity = 0.20f;
                 
                 Hud.Render.GetMinimapCoordinates(companion.FloorCoordinate.X, companion.FloorCoordinate.Y, out float textureX, out float textureY);
                 CompTexture.Draw(textureX-11, textureY-11, 22.3f, 24.1f, CompanionsHeadOpacity);
                 
                 
               }

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
        
         public void Customize()
        {
            Hud.TogglePlugin<OtherPlayersPlugin>(false);  // disables OtherPlayersPlugin
        }
    }
    
    
     public class MapLabelDecorator2 : IWorldDecorator
    {

        public bool Enabled { get; set; }
        public WorldLayer Layer { get; private set; }
        public IController Hud { get; private set; }

        public IFont LabelFont { get; set; }
        public bool Up { get; set; }
        public bool UpUp { get; set; }
        public bool Down { get; set; }
        public float RadiusOffset { get; set; }

        public MapLabelDecorator2(IController hud)
        {
            Enabled = true;
            Layer = WorldLayer.Map;
            Hud = hud;
            UpUp = false;
            Up = false;
            Down = false;
        }

        public void Paint(IActor actor, IWorldCoordinate coord, string text)
        {
            if (!Enabled) return;
            if (LabelFont == null) return;
            if (string.IsNullOrEmpty(text)) return;

            Hud.Render.GetMinimapCoordinates(coord.X, coord.Y, out float mapx, out float mapy);

            var layout = LabelFont.GetTextLayout(text);
            if (Up)
            {
                LabelFont.DrawText(layout, mapx - layout.Metrics.Width / 2, mapy + RadiusOffset - layout.Metrics.Height);
            }
            else if (UpUp)
            {
                LabelFont.DrawText(layout, mapx - layout.Metrics.Width / 2, mapy + RadiusOffset - (layout.Metrics.Height)*1.7f);
            }
            else if (Down)
            {
                LabelFont.DrawText(layout, mapx - layout.Metrics.Width / 2, mapy + RadiusOffset + layout.Metrics.Height);
            }
            else
            {
                LabelFont.DrawText(layout, mapx - layout.Metrics.Width / 2, mapy - RadiusOffset);
            }
        }

        public IEnumerable<ITransparent> GetTransparents()
        {
            yield break;
        }

    }
}
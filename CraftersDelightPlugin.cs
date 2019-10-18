// https://github.com/User5981/Resu
// Crafter's Delight Plugin for TurboHUD Version 15/10/2019 05:10

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Resu
{
    public class CraftersDelightPlugin : BasePlugin, IInGameWorldPainter, ILootGeneratedHandler, ICustomizer, IInGameTopPainter
    {
        public Dictionary<uint, WorldDecoratorCollection> SnoMapping { get; set; }
        public WorldDecoratorCollection SlainFarmerDecorator { get; set; }
        public WorldDecoratorCollection ancientRankDecorator { get; set; }
        public WorldDecoratorCollection ancientRankSetDecorator { get; set; }
        public WorldDecoratorCollection CountDecorator { get; set; }
        public WorldDecoratorCollection HoradricCacheDecorator { get; set; }
        public WorldDecoratorCollection EquippedDecorator { get; set; }
        public WorldDecoratorCollection NormalDecorator { get; set; }
        public WorldDecoratorCollection MagicDecorator { get; set; }
        public WorldDecoratorCollection RareDecorator { get; set; }
        public bool ShowAncientRank { get; set; }
        public bool SlainFarmers { get; set; }
        public bool DeathsBreath { get; set; }
        public bool VeiledCrystal { get; set; }
        public bool ArcaneDust { get; set; }
        public bool Gems { get; set; }
        public bool ForgottenSoul { get; set; }
        public bool ReusableParts { get; set; }
        public bool GreaterRiftKeystone { get; set; }
        public bool BovineBardiche { get; set; }
        public bool PuzzleRing { get; set; }
        public bool BloodShards { get; set; }
        public bool RamaladnisGift { get; set; }
        public bool Potion { get; set; }
        public bool InfernalMachine { get; set; }
        public bool Bounty { get; set; }
        public bool HellFire { get; set; }
        public bool LegendaryGems { get; set; }
        public bool HoradricCaches { get; set; }
        public long HoradricTimer { get; set; }
        public long NextHoradricSound { get; set; }
        public bool Equipped { get; set; }
        public bool LoreChestsDisplay { get; set; }
        public bool NormalChestsDisplay { get; set; }
        public bool ResplendentChestsDisplay { get; set; }
        public bool GroupGems { get; set; }
        public bool NoobGearMode { get; set; }
        public IBrush EquippedBrush { get; set; }
        private bool init_mapping;

        public WorldDecoratorCollection GreaterRiftKeystoneDecorator
        {
            get { return SnoMapping.ContainsKey(2835237830) ? SnoMapping[2835237830] : null; }
            set { SnoMapping[2835237830] = value; }
        }

        public CraftersDelightPlugin()
        {
            Enabled = true;
        }

        public void Customize()
        {
            Hud.RunOnPlugin<ItemsPlugin>(plugin => plugin.DeathsBreathDecorator.Decorators.Clear()); // turn off death's breath on default item plugins
            Hud.GetPlugin<ItemsPlugin>().LegendaryDecorator.ToggleDecorators<MapShapeDecorator>(false);
            Hud.GetPlugin<ItemsPlugin>().AncientDecorator.ToggleDecorators<MapShapeDecorator>(false);
            Hud.GetPlugin<ItemsPlugin>().PrimalDecorator.ToggleDecorators<MapShapeDecorator>(false);
            Hud.GetPlugin<ItemsPlugin>().SetDecorator.ToggleDecorators<MapShapeDecorator>(false);
            Hud.GetPlugin<ItemsPlugin>().AncientSetDecorator.ToggleDecorators<MapShapeDecorator>(false);
            Hud.GetPlugin<ItemsPlugin>().PrimalSetDecorator.ToggleDecorators<MapShapeDecorator>(false);
            Hud.TogglePlugin<ChestPlugin>(false);  // disables ChestPlugin
        }

        private void init()
        {
            SnoMapping = new Dictionary<uint, WorldDecoratorCollection>();
            var blackBrush = Hud.Render.CreateBrush(160, 0, 0, 0, 1);
            var whiteBrush = Hud.Render.CreateBrush(160, 255, 255, 255, 1);
            var purpleBrush = Hud.Render.CreateBrush(160, 146, 32, 175, 1);
            var brownBrush = Hud.Render.CreateBrush(160, 91, 22, 12, 1);
            var mudBrush = Hud.Render.CreateBrush(160, 146, 29, 0, 1);
            var BlueGreenBrush = Hud.Render.CreateBrush(160, 14, 215, 195, 1);
            var grayBrush = Hud.Render.CreateBrush(160, 55, 61, 53, 1);
            var orangeBrush = Hud.Render.CreateBrush(160, 255, 232, 113, 1);
            var RoyalBlueBrush = Hud.Render.CreateBrush(160, 65, 105, 255, 1);
            var blackFont = Hud.Render.CreateFont("tahoma", 7, 255, 0, 0, 0, true, false, false);
            var whiteFont = Hud.Render.CreateFont("tahoma", 7, 255, 255, 255, 255, true, false, false);
            var purpleFont = Hud.Render.CreateFont("tahoma", 7, 255, 146, 32, 175, true, false, false);
            var orangeFont = Hud.Render.CreateFont("tahoma", 7, 255, 255, 232, 113, true, false, false);
            var brownFont = Hud.Render.CreateFont("tahoma", 7, 255, 91, 22, 12, true, false, false);
            var mudFont = Hud.Render.CreateFont("tahoma", 7, 255, 146, 29, 0, true, false, false);
            var BlueGreenFont = Hud.Render.CreateFont("tahoma", 7, 255, 14, 215, 195, true, false, false);
            var grayFont = Hud.Render.CreateFont("tahoma", 7, 255, 55, 61, 53, true, false, false);
            var RoyalBlueFont = Hud.Render.CreateFont("tahoma", 7, 255, 65, 105, 255, true, false, false);

            //Death Breath => already handled by ItemsPlugin but needed
            if (DeathsBreath) { AddDecorator(2087837753, Hud.Render.CreateBrush(160, 89, 178, 153, 0), whiteBrush, orangeFont); }
            //VeiledCrystal
            if (VeiledCrystal) { AddDecorator(3689019703, Hud.Render.CreateBrush(120, 223, 223, 1, 0), brownBrush, brownFont); }
            //ArcaneDust
            if (ArcaneDust) { AddDecorator(2709165134, Hud.Render.CreateBrush(160, 105, 105, 254, 0), BlueGreenBrush, BlueGreenFont); }
            //Gems
            if (Gems)
            {
                AddDecorator(2979276674, Hud.Render.CreateBrush(160, 82, 107, 173, 0), whiteBrush, whiteFont);
                AddDecorator(2979276673, Hud.Render.CreateBrush(160, 82, 107, 173, 0), whiteBrush, whiteFont);
                AddDecorator(3256663689, Hud.Render.CreateBrush(160, 82, 107, 173, 0), whiteBrush, whiteFont);
                AddDecorator(3256663690, Hud.Render.CreateBrush(160, 82, 107, 173, 0), whiteBrush, whiteFont);
                AddDecorator(3256663684, Hud.Render.CreateBrush(160, 82, 107, 173, 0), whiteBrush, whiteFont);
                AddDecorator(3256663685, Hud.Render.CreateBrush(160, 82, 107, 173, 0), whiteBrush, whiteFont);
                AddDecorator(3256663686, Hud.Render.CreateBrush(160, 82, 107, 173, 0), whiteBrush, whiteFont);
                AddDecorator(3256663687, Hud.Render.CreateBrush(160, 82, 107, 173, 0), whiteBrush, whiteFont);
                AddDecorator(3256663688, Hud.Render.CreateBrush(160, 82, 107, 173, 0), whiteBrush, whiteFont);
                
                AddDecorator(3446938396, Hud.Render.CreateBrush(160, 113, 0, 175, 0), whiteBrush, whiteFont);
                AddDecorator(3446938397, Hud.Render.CreateBrush(160, 113, 0, 175, 0), whiteBrush, whiteFont);
                AddDecorator(2883100436, Hud.Render.CreateBrush(160, 113, 0, 175, 0), whiteBrush, whiteFont);
                AddDecorator(2883100437, Hud.Render.CreateBrush(160, 113, 0, 175, 0), whiteBrush, whiteFont);
                AddDecorator(3446938391, Hud.Render.CreateBrush(160, 113, 0, 175, 0), whiteBrush, whiteFont);
                AddDecorator(3446938392, Hud.Render.CreateBrush(160, 113, 0, 175, 0), whiteBrush, whiteFont);
                AddDecorator(3446938393, Hud.Render.CreateBrush(160, 113, 0, 175, 0), whiteBrush, whiteFont);
                AddDecorator(3446938394, Hud.Render.CreateBrush(160, 113, 0, 175, 0), whiteBrush, whiteFont);
                AddDecorator(3446938395, Hud.Render.CreateBrush(160, 113, 0, 175, 0), whiteBrush, whiteFont);
                
                AddDecorator(2838965543, Hud.Render.CreateBrush(160, 14, 167, 12, 0), whiteBrush, whiteFont);
                AddDecorator(2838965544, Hud.Render.CreateBrush(160, 14, 167, 12, 0), whiteBrush, whiteFont);
                AddDecorator(2561578527, Hud.Render.CreateBrush(160, 14, 167, 12, 0), whiteBrush, whiteFont);
                AddDecorator(2561578528, Hud.Render.CreateBrush(160, 14, 167, 12, 0), whiteBrush, whiteFont);
                AddDecorator(2838965538, Hud.Render.CreateBrush(160, 14, 167, 12, 0), whiteBrush, whiteFont);
                AddDecorator(2838965539, Hud.Render.CreateBrush(160, 14, 167, 12, 0), whiteBrush, whiteFont);
                AddDecorator(2838965540, Hud.Render.CreateBrush(160, 14, 167, 12, 0), whiteBrush, whiteFont);
                AddDecorator(2838965541, Hud.Render.CreateBrush(160, 14, 167, 12, 0), whiteBrush, whiteFont);
                AddDecorator(2838965542, Hud.Render.CreateBrush(160, 14, 167, 12, 0), whiteBrush, whiteFont);
                
                AddDecorator(1019190639, Hud.Render.CreateBrush(160, 157, 21, 8, 0), whiteBrush, whiteFont);
                AddDecorator(1019190640, Hud.Render.CreateBrush(160, 157, 21, 8, 0), whiteBrush, whiteFont);
                AddDecorator(1603007847, Hud.Render.CreateBrush(160, 157, 21, 8, 0), whiteBrush, whiteFont);
                AddDecorator(1603007848, Hud.Render.CreateBrush(160, 157, 21, 8, 0), whiteBrush, whiteFont);
                AddDecorator(1603007812, Hud.Render.CreateBrush(160, 157, 21, 8, 0), whiteBrush, whiteFont);
                AddDecorator(1603007813, Hud.Render.CreateBrush(160, 157, 21, 8, 0), whiteBrush, whiteFont);
                AddDecorator(1603007816, Hud.Render.CreateBrush(160, 157, 21, 8, 0), whiteBrush, whiteFont);
                AddDecorator(1603007817, Hud.Render.CreateBrush(160, 157, 21, 8, 0), whiteBrush, whiteFont);
                AddDecorator(1603007843, Hud.Render.CreateBrush(160, 157, 21, 8, 0), whiteBrush, whiteFont);
                
                AddDecorator(4267641563, Hud.Render.CreateBrush(160, 163, 98, 7, 0), whiteBrush, whiteFont);
                AddDecorator(4267641564, Hud.Render.CreateBrush(160, 163, 98, 7, 0), whiteBrush, whiteFont);
                AddDecorator(2058771923, Hud.Render.CreateBrush(160, 163, 98, 7, 0), whiteBrush, whiteFont);
                AddDecorator(2058771924, Hud.Render.CreateBrush(160, 163, 98, 7, 0), whiteBrush, whiteFont);
                AddDecorator(2058771888, Hud.Render.CreateBrush(160, 163, 98, 7, 0), whiteBrush, whiteFont);
                AddDecorator(2058771889, Hud.Render.CreateBrush(160, 163, 98, 7, 0), whiteBrush, whiteFont);
                AddDecorator(2058771892, Hud.Render.CreateBrush(160, 163, 98, 7, 0), whiteBrush, whiteFont);
                AddDecorator(2058771893, Hud.Render.CreateBrush(160, 163, 98, 7, 0), whiteBrush, whiteFont);
                AddDecorator(2058771919, Hud.Render.CreateBrush(160, 163, 98, 7, 0), whiteBrush, whiteFont);
            }
            //ForgottenSoul
            if (ForgottenSoul) { AddDecorator(2073430088, Hud.Render.CreateBrush(160, 244, 122, 0, 0), mudBrush, mudFont); }
            //ReusableParts
            if (ReusableParts) { AddDecorator(3931359676, Hud.Render.CreateBrush(160, 193, 186, 102, 0), grayBrush, grayFont); }
            //GreaterRiftKeystone
            if (GreaterRiftKeystone) { AddDecorator(2835237830, Hud.Render.CreateBrush(180, 0, 0, 0, 0), purpleBrush, purpleFont); }
            //BovineBardiche
            if (BovineBardiche) { AddDecorator(2346057823); }
            //PuzzleRing
            if (PuzzleRing) { AddDecorator(3106130529); }
            //Blood shards
            if (BloodShards) { AddDecorator(2603730171, Hud.Render.CreateBrush(160, 234, 47, 0, 0), orangeBrush, orangeFont); }
            //Ramaladni's Gift
            if (RamaladnisGift) { AddDecorator(1844495708, Hud.Render.CreateBrush(160, 48, 187, 120, 0), orangeBrush, orangeFont); }
            //Potion
            if (Potion)
            {
                AddDecorator(2276259498, Hud.Render.CreateBrush(180, 0, 0, 0, 0), orangeBrush, orangeFont);
                AddDecorator(2276259499, Hud.Render.CreateBrush(180, 0, 0, 0, 0), orangeBrush, orangeFont);
                AddDecorator(2276259500, Hud.Render.CreateBrush(180, 0, 0, 0, 0), orangeBrush, orangeFont);
                AddDecorator(2276259501, Hud.Render.CreateBrush(180, 0, 0, 0, 0), orangeBrush, orangeFont);
                AddDecorator(2276259502, Hud.Render.CreateBrush(180, 0, 0, 0, 0), orangeBrush, orangeFont);
                AddDecorator(2276259503, Hud.Render.CreateBrush(180, 0, 0, 0, 0), orangeBrush, orangeFont);
                AddDecorator(2276259504, Hud.Render.CreateBrush(180, 0, 0, 0, 0), orangeBrush, orangeFont);
                AddDecorator(510979313, Hud.Render.CreateBrush(180, 0, 0, 0, 0), orangeBrush, orangeFont);
                AddDecorator(2276259505, Hud.Render.CreateBrush(180, 0, 0, 0, 0), orangeBrush, orangeFont);
                AddDecorator(2276259506, Hud.Render.CreateBrush(180, 0, 0, 0, 0), orangeBrush, orangeFont);
                AddDecorator(2276259530, Hud.Render.CreateBrush(180, 0, 0, 0, 0), orangeBrush, orangeFont);
            }

            //Infernal machine
            if (InfernalMachine)
            {
                AddDecorator(1054965529, Hud.Render.CreateBrush(120, 0, 0, 0, 0), whiteBrush, whiteFont);
                AddDecorator(2788723894, Hud.Render.CreateBrush(120, 0, 0, 0, 0), whiteBrush, whiteFont);
                AddDecorator(2622355732, Hud.Render.CreateBrush(120, 0, 0, 0, 0), whiteBrush, whiteFont);
                AddDecorator(1458185494, Hud.Render.CreateBrush(120, 0, 0, 0, 0), whiteBrush, whiteFont);
            }

            //Bounty crafts
            if (Bounty)
            {
                AddDecorator(1948629088, Hud.Render.CreateBrush(120, 146, 32, 175, 0), orangeBrush, orangeFont);
                AddDecorator(1948629089, Hud.Render.CreateBrush(120, 146, 32, 175, 0), orangeBrush, orangeFont);
                AddDecorator(1948629090, Hud.Render.CreateBrush(120, 146, 32, 175, 0), orangeBrush, orangeFont);
                AddDecorator(1948629091, Hud.Render.CreateBrush(120, 146, 32, 175, 0), orangeBrush, orangeFont);
                AddDecorator(1948629092, Hud.Render.CreateBrush(120, 146, 32, 175, 0), orangeBrush, orangeFont);
            }

            //HellFire crafts
            if (HellFire)
            {
                AddDecorator(1102953247, Hud.Render.CreateBrush(160, 234, 47, 0, 0), orangeBrush, orangeFont);
                AddDecorator(2029265596, Hud.Render.CreateBrush(160, 234, 47, 0, 0), orangeBrush, orangeFont);
                AddDecorator(2670343450, Hud.Render.CreateBrush(160, 234, 47, 0, 0), orangeBrush, orangeFont);
                AddDecorator(3336787100, Hud.Render.CreateBrush(160, 234, 47, 0, 0), orangeBrush, orangeFont);
            }

            //Legendary Gems
            if (LegendaryGems)
            {
                AddDecorator(3248511367, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3248547304, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3248583241, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3248619178, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3248655115, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3248691052, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3248726989, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3248762926, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3248798863, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3249661351, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3249697288, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3249733225, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3249769162, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3249805099, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3249841036, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3249876973, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3249912910, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3249948847, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3249984784, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3250847272, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3250883209, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3250919146, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
                AddDecorator(3250955083, Hud.Render.CreateBrush(120, 255, 255, 255, 0), RoyalBlueBrush, RoyalBlueFont);
            }

            init_mapping = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            ShowAncientRank = true;
            SlainFarmers = true;
            DeathsBreath = true;
            VeiledCrystal = true;
            ArcaneDust = true;
            Gems = true;
            ForgottenSoul = true;
            ReusableParts = true;
            GreaterRiftKeystone = true;
            BovineBardiche = true;
            PuzzleRing = true;
            BloodShards = true;
            RamaladnisGift = true;
            Potion = true;
            InfernalMachine = true;
            Bounty = true;
            HellFire = true;
            LegendaryGems = true;
            HoradricCaches = true;
            Equipped = true;
            LoreChestsDisplay = true;
            NormalChestsDisplay = true;
            ResplendentChestsDisplay = true;
            GroupGems = true;
            NoobGearMode = true;
            HoradricTimer = 0;
            NextHoradricSound = 0;
            EquippedBrush = Hud.Render.CreateBrush(200, 255, 54, 198, 2, SharpDX.Direct2D1.DashStyle.Solid, SharpDX.Direct2D1.CapStyle.Flat, SharpDX.Direct2D1.CapStyle.Flat);

            //Slain farmers (actors)
            SlainFarmerDecorator = new WorldDecoratorCollection(
            new MapLabelDecorator(Hud)
            {
                LabelFont = Hud.Render.CreateFont("tahoma", 5, 255, 146, 29, 0, true, false, true),
            },
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(100, 0, 0, 0, 0),
                BorderBrush = Hud.Render.CreateBrush(160, 146, 29, 0, 1),
                TextFont = Hud.Render.CreateFont("tahoma", 7, 255, 146, 29, 0, true, false, false)
            }
            );

            //Horadric Cache
            HoradricCacheDecorator = new WorldDecoratorCollection(
             new MapTextureDecorator(Hud)
             {
                 SnoItem = Hud.Inventory.GetSnoItem(2116952111),
                 Radius = 0.6f,
                 RadiusTransformator = new StandardPingRadiusTransformator(Hud, 500)
                 {
                     RadiusMinimumMultiplier = 0.8f,
                 }
             },
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(100, 0, 0, 0, 0),
                BorderBrush = Hud.Render.CreateBrush(160, 255, 255, 255, 1),
                TextFont = Hud.Render.CreateFont("tahoma", 7, 160, 255, 255, 255, true, false, false)
            }
            );

            //Ancient rank
            ancientRankDecorator = new WorldDecoratorCollection(
            new MapLabelDecorator(Hud)
            {
                LabelFont = Hud.Render.CreateFont("tahoma", 7, 255, 255, 255, 255, true, false, false),
            }

              );
            ancientRankSetDecorator = new WorldDecoratorCollection(
            new MapLabelDecorator(Hud)
            {
                LabelFont = Hud.Render.CreateFont("tahoma", 7, 255, 0, 170, 0, true, false, false),
            }
              );

            // item count minimap
            CountDecorator = new WorldDecoratorCollection(
            new MapLabelDecorator(Hud)
            {
                LabelFont = Hud.Render.CreateFont("tahoma", 7, 255, 255, 255, 255, true, false, 255, 0, 0, 100, true),
            }
              );

            // Equipped item minimap
            EquippedDecorator = new WorldDecoratorCollection(
            new MapLabelDecorator(Hud)
            {
                LabelFont = Hud.Render.CreateFont("tahoma", 7, 255, 255, 255, 255, true, false, true),
            },
            new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(200, 255, 54, 198, 2),
                ShapePainter = new CircleShapePainter(Hud),
                Radius = 20,
            }
              );

            NormalDecorator = new WorldDecoratorCollection(
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                    BorderBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                    TextFont = Hud.Render.CreateFont("tahoma", 10, 255, 255, 255, 255, true, false, false)
                },
                new MapLabelDecorator(Hud)
                {
                    LabelFont = Hud.Render.CreateFont("tahoma", 10, 255, 255, 255, 255, true, false, false),
                }
                );

            MagicDecorator = new WorldDecoratorCollection(
               new GroundLabelDecorator(Hud)
               {
                   BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                   BorderBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                   TextFont = Hud.Render.CreateFont("tahoma", 10, 255, 126, 122, 219, true, false, false)
               },
               new MapLabelDecorator(Hud)
               {
                   LabelFont = Hud.Render.CreateFont("tahoma", 10, 255, 126, 122, 219, true, false, false),
               }
               );

            RareDecorator = new WorldDecoratorCollection(
               new GroundLabelDecorator(Hud)
               {
                   BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                   BorderBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                   TextFont = Hud.Render.CreateFont("tahoma", 10, 255, 255, 255, 147, true, false, false)
               },
               new MapLabelDecorator(Hud)
               {
                   LabelFont = Hud.Render.CreateFont("tahoma", 10, 255, 255, 255, 147, true, false, false),
               }
               );
        }

        private void AddDecorator(uint sno)
        {
            SnoMapping.Add(sno, new WorldDecoratorCollection(
                new MapTextureDecorator(Hud)
                {
                    SnoItem = Hud.Inventory.GetSnoItem(sno),
                    Radius = 0.47f,
                }

            ));
        }

        private void AddDecorator(uint sno, IBrush bgBrush, IBrush borderBrush, IFont textFont)
        {
            SnoMapping.Add(sno, new WorldDecoratorCollection(
                new MapTextureDecorator(Hud)
                {
                    SnoItem = Hud.Inventory.GetSnoItem(sno),
                    Radius = 0.47f,
                },
                new GroundLabelDecorator(Hud)
                {
                    BackgroundBrush = bgBrush,
                    BorderBrush = borderBrush,
                    TextFont = textFont
                }
            ));
        }

        public void OnLootGenerated(IItem item, bool gambled)
        {
            if (!Hud.Sound.IsIngameSoundEnabled) return;
            if (SameAsEquipped(item, true) && Equipped)
            {
                var soundPlayer = Hud.Sound.LoadSoundPlayer("Equipped-Drop-By-Resu.wav");

                ThreadPool.QueueUserWorkItem(state =>
                  {
                      try
                      {
                          soundPlayer.PlaySync();
                      }
                      catch (Exception)
                      {
                      }
                  });
            }
           else if (SameAsArmory(item) && Equipped)
            {
                var soundPlayer = Hud.Sound.LoadSoundPlayer("Armory-Drop-By-Resu.wav");

                ThreadPool.QueueUserWorkItem(state =>
                  {
                      try
                      {
                          soundPlayer.PlaySync();
                      }
                      catch (Exception)
                      {
                      }
                  });
            }
            
            if (item.AncientRank < 1 ) { }
            else
            {
                var soundPlayer = item.AncientRank == 1 ? Hud.Sound.LoadSoundPlayer("Ancient-Drop-By-Resu.wav") : Hud.Sound.LoadSoundPlayer("Primal-Drop-By-Resu.wav");

                ThreadPool.QueueUserWorkItem(state =>
                  {
                      try
                      {
                          soundPlayer.PlaySync();
                      }
                      catch (Exception)
                      {
                      }
                  });
            }
        }

        public void PaintWorld(WorldLayer layer)
        {
            if (!init_mapping) { init(); }
            if (Hud.Render.UiHidden) return;

            var itemGroups = Hud.Game.Items.Where(item => item.Location == ItemLocation.Floor).GroupBy(item => item.SnoItem.Sno);

            if (GroupGems)
            {
                itemGroups = Hud.Game.Items.Where(item => item.Location == ItemLocation.Floor && item.SnoItem.MainGroupCode != "gems").GroupBy(item => item.SnoItem.Sno);
                var gemGroups = Hud.Game.Items.Where(item => item.Location == ItemLocation.Floor && item.SnoItem.MainGroupCode == "gems").GroupBy(item => item.SnoItem.MainGroupCode);

                foreach (var items in gemGroups)
                {
                    var orderedItems = items.OrderBy(i => i.NormalizedXyDistanceToMe);
                    var firstItem = orderedItems.FirstOrDefault();

                    if (firstItem == null) continue;

                    if (SnoMapping.ContainsKey(firstItem.SnoItem.Sno)) //2979276674
                    {
                        var count = orderedItems.Where(i => i.FloorCoordinate.XYDistanceTo(firstItem.FloorCoordinate) < 21).Sum(i => i.Quantity); // previously .XYDistanceTo(firstItem.FloorCoordinate) <= 40
                        if (count > 1)
                        {
                            var Qtt = " (" + count + ")";
                            SnoMapping[firstItem.SnoItem.Sno].Paint(layer, firstItem, firstItem.FloorCoordinate, "Gems" + Qtt);
                            CountDecorator.Paint(layer, firstItem, firstItem.FloorCoordinate, count.ToString());
                        }
                        else
                        {
                            SnoMapping[firstItem.SnoItem.Sno].Paint(layer, firstItem, firstItem.FloorCoordinate, firstItem.SnoItem.NameLocalized);
                        }
                    }
                }
            }

            foreach (var items in itemGroups)
            {
                var orderedItems = items.OrderBy(i => i.NormalizedXyDistanceToMe);
                var firstItem = orderedItems.FirstOrDefault();

                if (firstItem == null) continue;

                if (SnoMapping.ContainsKey(items.Key))
                {
                    var count = orderedItems.Where(i => i.FloorCoordinate.XYDistanceTo(firstItem.FloorCoordinate) < 21).Sum(i => i.Quantity); // previously .XYDistanceTo(firstItem.FloorCoordinate) <= 40
                    if (count > 1)
                    {
                        var Qtt = " (" + count + ")";
                        SnoMapping[firstItem.SnoItem.Sno].Paint(layer, firstItem, firstItem.FloorCoordinate, firstItem.SnoItem.NameLocalized + Qtt);
                        CountDecorator.Paint(layer, firstItem, firstItem.FloorCoordinate, count.ToString());
                    }
                    else
                    {
                        SnoMapping[firstItem.SnoItem.Sno].Paint(layer, firstItem, firstItem.FloorCoordinate, firstItem.SnoItem.NameLocalized);
                    }
                }

                if (!firstItem.IsLegendary && !NoobGearMode) continue;
                else if (!firstItem.IsLegendary && NoobGearMode && firstItem.Perfection != 0)
                {
                    if (DisplayItem((byte)firstItem.Quality))
                    {
                        if (firstItem.IsNormal) { NormalDecorator.Paint(layer, firstItem, firstItem.FloorCoordinate, "\u2605"); }
                        else if (firstItem.IsMagic) { MagicDecorator.Paint(layer, firstItem, firstItem.FloorCoordinate, "\u2605"); }
                        else if (firstItem.IsRare) { RareDecorator.Paint(layer, firstItem, firstItem.FloorCoordinate, "\u2605"); }
                    }
                }

                foreach (var item in items)
                {
                    var inKanaiCube = Hud.Game.Me.IsCubed(item.SnoItem);
                    var canKanaiCube = !inKanaiCube && item.SnoItem.CanKanaiCube;

                    if (canKanaiCube)
                    {
                        var cubeTexture = Hud.Texture.KanaiCubeTexture;
                        float radius;
                        Hud.Render.GetMinimapCoordinates(item.FloorCoordinate.X, item.FloorCoordinate.Y, out float mapX, out float mapY);
                        var RadiusTransformator = new StandardPingRadiusTransformator(Hud, 500)
                        {
                            RadiusMinimumMultiplier = 0.8f,
                        };
                        radius = 0.9f * Hud.Render.MinimapScale;
                        if (RadiusTransformator != null)
                        {
                            radius = RadiusTransformator.TransformRadius(radius);
                        }
                        var width = cubeTexture.Width * radius;
                        var height = cubeTexture.Height * radius;
                        cubeTexture.Draw(mapX - width / 2, mapY - height / 2, width, height);
                    }

                    if (SameAsEquipped(item, false) && Equipped)
                    {
                        EquippedDecorator.Paint(layer, item, item.FloorCoordinate, "E");
                    }
                    else if (SameAsArmory(item) && Equipped)
                    {
                        EquippedDecorator.Paint(layer, item, item.FloorCoordinate, "\u2694");
                    }

                    if (item.AncientRank < 1 || !ShowAncientRank) continue;
                    var ancientRankText = item.AncientRank == 1 ? "Ancient   ->                     <-   Ancient" : "Primal   ->                     <-   Primal";

                    if (item.SetSno != uint.MaxValue)
                    {
                        ancientRankSetDecorator.Paint(layer, item, item.FloorCoordinate, ancientRankText); // set color
                    }
                    else
                    {
                        ancientRankDecorator.Paint(layer, item, item.FloorCoordinate, ancientRankText); // legendary color
                    }
                }
            }

            /// Slain farmer
            if (SlainFarmers)
            {
                var SlainFarmer = Hud.Game.Actors.Where(x => !x.IsDisabled && !x.IsOperated && x.SnoActor.Sno >= ActorSnoEnum._loottype2_tristramvillager_male_a_corpse_01_farmer/*434676*/ && x.SnoActor.Sno <= ActorSnoEnum._tristramvillager_female_c_corpse_01_farmer /*434679*/);
                foreach (var actor in SlainFarmer)
                {
                    SlainFarmerDecorator.Paint(layer, actor, actor.FloorCoordinate, actor.SnoActor.NameLocalized);
                }
            }

            /// Horadric Cache
            if (HoradricCaches)
            {
                var HoradricCache = Hud.Game.Items.Where(item => item.Location == ItemLocation.Floor && item.SnoItem.MainGroupCode == "horadriccache");
                foreach (var cache in HoradricCache)
                {
                    HoradricCacheDecorator.Paint(layer, cache, cache.FloorCoordinate, cache.SnoItem.NameLocalized);
                    string HoradricCacheText = "Cache   ->                       <-   Cache";
                    ancientRankDecorator.Paint(layer, cache, cache.FloorCoordinate, HoradricCacheText);

                    HoradricTimer = NextHoradricSound - Hud.Game.CurrentRealTimeMilliseconds;
                    if (HoradricTimer < 0) HoradricTimer = 0;
                    if (cache.NormalizedXyDistanceToMe <= 50 && HoradricTimer == 0)
                    {
                        if (!Hud.Sound.IsIngameSoundEnabled) continue;
                        var soundPlayer = Hud.Sound.LoadSoundPlayer("Horadric-Cache-By-Resu.wav");

                        ThreadPool.QueueUserWorkItem(state =>
                        {
                            try
                            {
                                soundPlayer.PlaySync();
                            }
                            catch (Exception)
                            {
                            }
                        });
                        NextHoradricSound = Hud.Game.CurrentRealTimeMilliseconds + 20000;
                    }
                }
            }

            if (LoreChestsDisplay)
            {
                var loreChests = Hud.Game.Actors.Where(x => !x.IsDisabled && !x.IsOperated && x.GizmoType == GizmoType.LoreChest);
                foreach (var actor in loreChests)
                {
                    var LoreTexture = Hud.Texture.GetTexture(3651511087);
                    if (!Hud.Game.Me.IsInTown) LoreTexture.Draw(actor.FloorCoordinate.X, actor.FloorCoordinate.Y, 31.5f, 49.5f, 1f);
                }
            }

            var Glow = Hud.Texture.GetTexture(1981524232);

            if (NormalChestsDisplay)
            {
                var normalChests = Hud.Game.Actors.Where(x => !x.IsDisabled && !x.IsOperated && x.SnoActor.Kind == ActorKind.ChestNormal);
                foreach (var actor in normalChests)
                {
                    var NormalTexture = Hud.Texture.GetTexture(4061587565);
                    Hud.Render.GetMinimapCoordinates(actor.FloorCoordinate.X, actor.FloorCoordinate.Y, out float textureX, out float textureY);
                    Glow.Draw(textureX - 11, textureY - 13, 28f, 33f, 1f);
                    NormalTexture.Draw(textureX - 11, textureY - 13, 22.77f, 27.06f, 1f);
                }
            }

            if (ResplendentChestsDisplay)
            {
                var resplendentChests = Hud.Game.Actors.Where(x => !x.IsDisabled && !x.IsOperated && x.SnoActor.Kind == ActorKind.Chest);
                foreach (var actor in resplendentChests)
                {
                    var RespendentTexture = Hud.Texture.GetTexture(4029005773);
                    Hud.Render.GetMinimapCoordinates(actor.FloorCoordinate.X, actor.FloorCoordinate.Y, out float textureX, out float textureY);
                    Glow.Draw(textureX - 11, textureY - 13, 31f, 36f, 1f);
                    RespendentTexture.Draw(textureX - 11, textureY - 13, 22.77f, 27.06f, 1f);
                }
            }
       }

       private bool SameAsArmory(IItem item)
       {
            for (int i = 0; i < Hud.Game.Me.ArmorySets.Length; ++i)
            {
                var armorySet = Hud.Game.Me.ArmorySets[i];
                if (armorySet != null)
                {
                    if (armorySet.ContainsItem(item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SameAsEquipped(IItem item, bool includeArmory)
        {
            uint ThatItemSno = item.SnoItem.Sno;
            int ThatItemRank = item.AncientRank;

            if (ThatItemRank == 1) ThatItemRank = 2;
            bool Worn = Hud.Game.Items.Any(x => (int)x.Location > 0 && (int)x.Location < 14 && x.SnoItem.Sno == ThatItemSno && x.AncientRank <= ThatItemRank);
            bool Cubed1 = Hud.Game.Me.CubeSnoItem1?.Sno == ThatItemSno && ThatItemRank > 0;
            bool Cubed2 = Hud.Game.Me.CubeSnoItem2?.Sno == ThatItemSno && ThatItemRank > 0;
            bool Cubed3 = Hud.Game.Me.CubeSnoItem3?.Sno == ThatItemSno && ThatItemRank > 0;

            if (Worn) return true;
            else if (Cubed1) return true;
            else if (Cubed2) return true;
            else if (Cubed3) return true;

            /* Consider armory items as equipped */
            if (!includeArmory) return false;
            return SameAsArmory(item);
        }

        private bool DisplayItem(byte ItemQuality)
        {
            var EquippedItems = Hud.Game.Items.Where(x => (int)x.Location > 0 && (int)x.Location < 14);
            byte LowerQualityEquipped = 9;

            foreach (var EquippedItem in EquippedItems)
            {
                if ((byte)EquippedItem.Quality < LowerQualityEquipped) LowerQualityEquipped = (byte)EquippedItem.Quality;
            }

            if (ItemQuality >= LowerQualityEquipped) return true;
            else return false;
        }

        private int stashTabAbs;

        public void PaintTopInGame(ClipState clipState)
        {
            if (clipState == ClipState.Inventory && Equipped)
            {
                stashTabAbs = Hud.Inventory.SelectedStashTabIndex + Hud.Inventory.SelectedStashPageIndex * Hud.Inventory.MaxStashTabCountPerPage;

                foreach (var item in Hud.Game.Items)
                {
                    if ((Int32)item.Location > 0 && (Int32)item.Location < 14) continue;
                    if (SameAsEquipped(item, false))
                    {
                        if (item.Location == ItemLocation.Stash)
                        {
                            if ((item.InventoryY / 10) != stashTabAbs) continue;
                        }
                        if ((item.InventoryX < 0) || (item.InventoryY < 0)) continue;
                        var rect = Hud.Inventory.GetItemRect(item);
                        if (rect == System.Drawing.RectangleF.Empty) continue;
                        EquippedBrush.DrawLine(rect.Right, rect.Bottom, rect.Right, rect.Top);
                        EquippedBrush.DrawLine(rect.Left, rect.Bottom, rect.Left, rect.Top);
                        EquippedBrush.DrawLine(rect.Right, rect.Top, rect.Left, rect.Top);
                        EquippedBrush.DrawLine(rect.Right, rect.Bottom, rect.Left, rect.Bottom);
                    }
                }
            }
        }
    }
}

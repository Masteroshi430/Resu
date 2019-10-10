// https://github.com/User5981/Resu
// Danger Plugin for TurboHUD Version 10/10/2019 15:30
// Note : This plugin merges BM's DemonForgePlugin, ShockTowerPlugin, my BloodSpringsPlugin and adds many new features

using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Resu
{
    public class DangerPlugin : BasePlugin, IInGameWorldPainter, ICustomizer

    {
        public static Dictionary<IWorldCoordinate, int> BetrayedPoison { get; set; }
        public static Dictionary<IWorldCoordinate, int> GrotesqueBlow { get; set; }
        public WorldDecoratorCollection BloodSpringsDecoratorSmall { get; set; }
        public WorldDecoratorCollection BloodSpringsDecoratorMedium { get; set; }
        public WorldDecoratorCollection BloodSpringsDecoratorBig { get; set; }
        public WorldDecoratorCollection DemonicForgeDecorator { get; set; }
        public WorldDecoratorCollection UnknownDemonicForgeDecorator { get; set; }
        public WorldDecoratorCollection ShockTowerDecorator { get; set; }
        public WorldDecoratorCollection MoveWarningDecorator { get; set; }
        public WorldDecoratorCollection ArcaneDecorator { get; set; }
        public WorldDecoratorCollection ProjectileDecorator { get; set; }
        public WorldDecoratorCollection DemonMineDecorator { get; set; }
        public WorldDecoratorCollection OrbiterDecorator { get; set; }
        public WorldDecoratorCollection FastMummyDecorator { get; set; }
        public WorldDecoratorCollection GrotesqueDecorator { get; set; }
        public WorldDecoratorCollection BetrayedCountdownDecorator { get; set; }
        public WorldDecoratorCollection MorluMeteorCountdownDecorator { get; set; }
        public int offsetX { get; set; }
        public int offsetY { get; set; }
        public bool BloodSprings { get; set; }
        public bool DemonicForge { get; set; }
        public bool ShockTower { get; set; }
        public bool Desecrator { get; set; }
        public bool Thunderstorm { get; set; }
        public bool Plagued { get; set; }
        public bool Molten { get; set; }
        public bool ArcaneEnchanted { get; set; }
        public bool PoisonEnchanted { get; set; }
        public bool GasCloud { get; set; }
        public bool SandWaspProjectile { get; set; }
        public bool MorluSpellcasterMeteorPending { get; set; }
        public bool DemonMine { get; set; }
        public bool PoisonDeath { get; set; }
        public bool MoltenExplosion { get; set; }
        public bool Orbiter { get; set; }
        public bool BloodStar { get; set; }
        public bool ArrowProjectile { get; set; }
        public bool BogFamilyProjectile { get; set; }
        public bool bloodGolemProjectile { get; set; }
        public bool MoleMutantProjectile { get; set; }
        public bool IcePorcupineProjectile { get; set; }
        public float Health { get; set; }
        public float PrevHealth { get; set; }
        public int PrevSecond { get; set; }
        public bool RunForYourLife { get; set; }
        public bool Danger { get; set; }
        public bool GrotesqueExplosion { get; set; }
        public bool BetrayedPoisonCloud { get; set; }
        public IActor PoisonCloudActor { get; set; }

        public static HashSet<ActorSnoEnum> dangerIds = new HashSet<ActorSnoEnum>(new List<uint> { 174900, 185391, 332922, 332923, 332924, 322194, 84608, 341512, 108869, 3865, 219702, 221225, 340319, 95868, 93837, 5212, 159369, 118596, 4104, 4105, 4106, 4803, 343539, 164827, 312942, 337030, 353256, 349564, 117921, 117906, 150825, 468082, 430430 }.Select(x => (ActorSnoEnum)x));

        public static HashSet<ActorSnoEnum> BetrayedPoisonActors = new HashSet<ActorSnoEnum>(new[] { 4104, 4105, 4106 }.Select(x => (ActorSnoEnum)x));

        public static HashSet<ActorSnoEnum> GrotesqueExplosionActors = new HashSet<ActorSnoEnum>(new[]
        {
            3847,
            218307,
            218308,
            365450,
            3848,
            218405,
            3849,
            113994,
            3850,
            195639,
            365465,
            191592,
        }.Select(x => (ActorSnoEnum)x));

        public DangerPlugin()
        {
            Enabled = true;
            BloodSprings = true;
            DemonicForge = true;
            ShockTower = true;
            Desecrator = true;
            Thunderstorm = true;
            Plagued = true;
            Molten = true;
            ArcaneEnchanted = true;
            PoisonEnchanted = true;
            GasCloud = true;
            SandWaspProjectile = true;
            MorluSpellcasterMeteorPending = true;
            DemonMine = true;
            PoisonDeath = true;
            MoltenExplosion = true;
            Orbiter = true;
            BloodStar = true;
            ArrowProjectile = true;
            BogFamilyProjectile = true;
            bloodGolemProjectile = true;
            MoleMutantProjectile = true;
            IcePorcupineProjectile = true;
            GrotesqueExplosion = true;
            BetrayedPoisonCloud = true;
            BetrayedPoison = new Dictionary<IWorldCoordinate, int>();
            GrotesqueBlow = new Dictionary<IWorldCoordinate, int>();
        }

        public void Customize()
        {
            Hud.RunOnPlugin<StandardMonsterPlugin>(plugin => plugin.InvisibleDecorator.Decorators.Clear()); // turn off InvisibleDecorator on default StandardMonsterPlugin
            Hud.TogglePlugin<ExplosiveMonsterPlugin>(false);  // disables ExplosiveMonsterPlugin
        }

        public override void Load(IController hud)
        {
            base.Load(hud);

            Health = 100f;
            PrevHealth = 100f;
            PrevSecond = 0;
            RunForYourLife = false;
            BloodSpringsDecoratorSmall = new WorldDecoratorCollection(
            new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(100, 173, 220, 213, 0),
                Radius = 7.0f,
                ShapePainter = new CircleShapePainter(Hud),
            },
            new MapLabelDecorator(Hud)
            {
                LabelFont = Hud.Render.CreateFont("tahoma", 6, 255, 255, 255, 255, true, false, false),
            },
            new GroundCircleDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(150, 173, 220, 213, 5, SharpDX.Direct2D1.DashStyle.Dash),
                Radius = 7,
            },
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(160, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 79, 170, 245, true, false, false),
            }
            );

            BloodSpringsDecoratorMedium = new WorldDecoratorCollection(
            new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(100, 173, 220, 213, 0),
                Radius = 14.0f,
                ShapePainter = new CircleShapePainter(Hud),
            },
            new MapLabelDecorator(Hud)
            {
                LabelFont = Hud.Render.CreateFont("tahoma", 6, 255, 255, 255, 255, true, false, false),
            },
            new GroundCircleDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(150, 173, 220, 213, 5, SharpDX.Direct2D1.DashStyle.Dash),
                Radius = 14,
            },
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(160, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 79, 170, 245, true, false, false),
            }
            );

            BloodSpringsDecoratorBig = new WorldDecoratorCollection(
            new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(100, 173, 220, 213, 0),
                Radius = 20.0f,
                ShapePainter = new CircleShapePainter(Hud),
            },
            new MapLabelDecorator(Hud)
            {
                LabelFont = Hud.Render.CreateFont("tahoma", 6, 255, 255, 255, 255, true, false, false),
            },
            new GroundCircleDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(150, 173, 220, 213, 5, SharpDX.Direct2D1.DashStyle.Dash),
                Radius = 20,
            },
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(160, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 79, 170, 245, true, false, false),
            }
            );

            DemonicForgeDecorator = new WorldDecoratorCollection(
            new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(255, 255, 0, 0, 0),
                Radius = 10.0f,
                ShapePainter = new CircleShapePainter(Hud),
                RadiusTransformator = new StandardPingRadiusTransformator(Hud, 333),
            }
            );

            UnknownDemonicForgeDecorator = new WorldDecoratorCollection(
            new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(255, 255, 0, 0, 0),
                Radius = 10.0f,
                ShapePainter = new CircleShapePainter(Hud),
                RadiusTransformator = new StandardPingRadiusTransformator(Hud, 333),
            },
            new MapLabelDecorator(Hud)
            {
                LabelFont = Hud.Render.CreateFont("tahoma", 6, 255, 255, 255, 255, true, false, false),
            },
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(160, 255, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 255, 255, 220, true, false, false),
            }
            );

            ShockTowerDecorator = new WorldDecoratorCollection(
            new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(255, 79, 170, 245, 0),
                Radius = 10.0f,
                ShapePainter = new CircleShapePainter(Hud),
                RadiusTransformator = new StandardPingRadiusTransformator(Hud, 333),
            },
            new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(50, 79, 170, 245, 0),
                Radius = 30.0f,
                ShapePainter = new CircleShapePainter(Hud),
            },
        /*    new MapLabelDecorator(Hud)
            {
                LabelFont = Hud.Render.CreateFont("tahoma", 6, 255, 255, 255, 255, true, false, false),
            },*/
            new GroundCircleDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(100, 255, 255, 220, 5, SharpDX.Direct2D1.DashStyle.Dash),
                Radius = 30,
            }/*
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(160, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 79, 170, 245, true, false, false),
            }*/
            );

            ArcaneDecorator = new WorldDecoratorCollection(
            new GroundCircleDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(128, 255, 60, 255, 3, SharpDX.Direct2D1.DashStyle.Dash),
                Radius = 32f,
            },
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 20, 128, 255, 60, 255, true, false, false),
                OffsetY = 200f,
            },
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 20, 128, 255, 60, 255, true, false, false),
                OffsetY = -200f,
            },
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 20, 128, 255, 60, 255, true, false, false),

                OffsetX = -200f,
            },
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 20, 128, 255, 60, 255, true, false, false),
                OffsetX = 200f,
            }

            );

            MoveWarningDecorator = new WorldDecoratorCollection(
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 20, 255, 255, 255, 255, true, true, true),
            }
            );

            ProjectileDecorator = new WorldDecoratorCollection(
            new GroundLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
                TextFont = Hud.Render.CreateFont("tahoma", 20, 255, 0, 255, 0, true, false, false),
            }
            );

            DemonMineDecorator = new WorldDecoratorCollection(
            new GroundCircleDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(100, 255, 255, 220, 5, SharpDX.Direct2D1.DashStyle.Dash),
                Radius = 5,
            }
            );

            OrbiterDecorator = new WorldDecoratorCollection(
            new GroundCircleDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(255, 0, 255, 0, 5, SharpDX.Direct2D1.DashStyle.Solid),
                Radius = 4,
            }
            );

            FastMummyDecorator = new WorldDecoratorCollection(
             new GroundCircleDecorator(Hud)
             {
                 Brush = Hud.Render.CreateBrush(128, 255, 255, 255, 3, SharpDX.Direct2D1.DashStyle.Dash),
                 Radius = 9,
             }
             );

            GrotesqueDecorator = new WorldDecoratorCollection(
                new GroundCircleDecorator(Hud)
                {
                    Brush = Hud.Render.CreateBrush(160, 255, 50, 50, 3, SharpDX.Direct2D1.DashStyle.Dash),
                    Radius = 20f,
                },
                 new GroundLabelDecorator(Hud)
                 {
                     TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 255, 255, 255, true, false, 128, 0, 0, 0, true),
                 },
                new GroundTimerDecorator2(Hud)
                {
                    CountDownFrom = 1.3f,
                    BackgroundBrushEmpty = Hud.Render.CreateBrush(128, 0, 0, 0, 0),
                    BackgroundBrushFill = Hud.Render.CreateBrush(160, 255, 50, 50, 0),
                    Radius = 30,
                }
                );

            BetrayedCountdownDecorator = new WorldDecoratorCollection(
                  new GroundLabelDecorator(Hud)
                  {
                      TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 255, 255, 255, true, false, 128, 0, 0, 0, true),
                  },
                 new GroundTimerDecorator2(Hud)
                 {
                     CountDownFrom = 2.2f,
                     BackgroundBrushEmpty = Hud.Render.CreateBrush(128, 0, 0, 0, 0),
                     BackgroundBrushFill = Hud.Render.CreateBrush(160, 50, 255, 50, 0),
                     Radius = 30,
                 }
                 );

            MorluMeteorCountdownDecorator = new WorldDecoratorCollection(
                  new GroundLabelDecorator(Hud)
                  {
                      CountDownFrom = 2f,
                      TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 255, 255, 255, true, false, 128, 0, 0, 0, true),
                  },
                 new GroundTimerDecorator(Hud)
                 {
                     CountDownFrom = 2f,
                     BackgroundBrushEmpty = Hud.Render.CreateBrush(128, 0, 0, 0, 0),
                     BackgroundBrushFill = Hud.Render.CreateBrush(160, 255, 50, 50, 0),
                     Radius = 30,
                 }
                 );
        }

        public void PaintWorld(WorldLayer layer)
        {
            var hedPlugin = Hud.GetPlugin<HotEnablerDisablerPlugin>();
            bool GoOn = hedPlugin.CanIRun(Hud.Game.Me, this.GetType().Name);
            if (!GoOn) return;

            if (Hud.Game.Me.IsInTown && BetrayedPoison.Count != 0 || Hud.Game.Me.IsInTown && GrotesqueBlow.Count != 0)
            {
                BetrayedPoison.Clear();
                GrotesqueBlow.Clear();
            }
            else if (Hud.Game.IsInTown) return;

            var diff = Hud.Window.Size.Width / Hud.Window.Size.Height;
            offsetX = Convert.ToInt32(Hud.Window.Size.Width / Math.PI);
            offsetY = Convert.ToInt32(Hud.Window.Size.Height / (Math.PI / diff));

            Health = Hud.Game.Me.Defense.HealthPct;
            int Second = Hud.Time.Now.Second;

            if (Second != PrevSecond)
            {
                PrevHealth = Health;
                PrevSecond = Second;
            }

            if ((PrevHealth - Health) >= (Health / 10)) RunForYourLife = true;
            else RunForYourLife = false;

            var danger = Hud.Game.Actors.Where(x => dangerIds.Contains(x.SnoActor.Sno));
            foreach (var actor in danger)
            {
                if (actor.SnoActor.Sno == ActorSnoEnum._a3_battlefield_demonic_forge /*174900*/ && DemonicForge || actor.SnoActor.Sno == ActorSnoEnum._a3_crater_st_demonic_forge /*185391*/ && DemonicForge)
                {
                    var ActorPos = actor.FloorCoordinate.ToScreenCoordinate();
                    var ActorPosToMap = actor.FloorCoordinate;
                    var brush = Hud.Render.CreateBrush(100, 255, 255, 220, 5, SharpDX.Direct2D1.DashStyle.Dash, SharpDX.Direct2D1.CapStyle.Flat, SharpDX.Direct2D1.CapStyle.Flat);
                    var RedBrush = Hud.Render.CreateBrush(200, 255, 0, 0, 1, SharpDX.Direct2D1.DashStyle.Solid, SharpDX.Direct2D1.CapStyle.Flat, SharpDX.Direct2D1.CapStyle.Triangle);
                    var worldCoord1 = actor.FloorCoordinate;
                    var worldCoord2 = actor.FloorCoordinate;

                    switch (actor.FloorCoordinate.ToString())
                    {
                        case "811.115, 689.702, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(853.009f, 690.379f, 0.0f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(844.101f, 674.911f, 0.0f);
                            break;

                        case "781.829, 561.435, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(738.147f, 570.859f, 0.0f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(737.535f, 554.577f, 0.0f);
                            break;

                        case "502.417, 585.992, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(541.574f, 585.262f, 0.0f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(534.413f, 571.469f, 0.0f);
                            break;

                        case "1727.000, 680.000, 10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1754.450f, 651.427f, 10.0f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1741.038f, 644.561f, 10.1f);
                            break;

                        case "1820.000, 1295.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1819.980f, 1249.663f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1805.427f, 1247.586f, 0.1f);
                            break;

                        case "1607.000, 1264.000, 10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1606.558f, 1297.729f, 10.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1595.398f, 1293.642f, 10.1f);
                            break;

                        case "1727.000, 1160.000, 10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1758.430f, 1127.567f, 10.0f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1743.272f, 1121.456f, 10.1f);
                            break;

                        case "1586.000, 1772.000, -9.7":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1611.755f, 1794.798f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1618.903f, 1782.976f, -9.9f);
                            break;

                        case "925.000, 1112.500, -30.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(959.641f, 1102.643f, -29.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(951.034f, 1091.851f, -29.9f);
                            break;

                        case "945.000, 1160.000, -29.4":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(981.091f, 1140.616f, -29.6f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(968.648f, 1131.352f, -29.9f);
                            break;

                        case "985.000, 1207.500, -29.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1010.279f, 1177.264f, -29.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1001.511f, 1166.355f, -29.9f);
                            break;

                        case "1340.000, 1295.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1339.744f, 1246.202f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1322.890f, 1246.942f, 0.1f);
                            break;

                        case "1127.000, 1264.000, 10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1126.206f, 1304.633f, 10.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1108.522f, 1296.115f, 10.1f);
                            break;

                        case "1150.000, 1280.000, -39.2":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1134.904f, 1235.293f, -39.2f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1121.975f, 1239.220f, -39.8f);
                            break;

                        case "1122.500, 1287.500, -59.7":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1102.278f, 1245.708f, -59.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1116.381f, 1240.770f, -59.3f);
                            break;

                        case "1220.000, 1237.500, -59.2":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1183.445f, 1208.253f, -59.4f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1194.199f, 1197.634f, -59.4f);
                            break;

                        case "1287.500, 1040.000, -69.2":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1242.896f, 1048.652f, -69.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1240.559f, 1037.631f, -69.6f);
                            break;

                        case "1245.000, 1172.500, -39.4":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1272.411f, 1178.274f, -39.2f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1267.978f, 1186.448f, -39.1f);
                            break;

                        case "1237.500, 1225.000, -39.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1219.680f, 1203.061f, -39.4f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1211.956f, 1210.992f, -39.4f);
                            break;

                        case "1187.500, 1235.000, -40.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1205.183f, 1255.794f, -39.5f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1196.936f, 1257.215f, -43.3f);
                            break;

                        case "1760.000, 1291.000, -9.9":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1718.161f, 1292.128f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1716.758f, 1277.894f, -9.8f);
                            break;

                        case "1707.000, 1043.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1677.532f, 1065.701f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1671.413f, 1053.276f, 0.1f);
                            break;

                        case "1373.607, 1186.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1372.853f, 1223.386f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1360.826f, 1219.108f, -1.4f);
                            break;

                        case "1376.000, 1272.000, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1372.921f, 1231.106f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1362.072f, 1229.695f, -2.6f);
                            break;

                        case "993.510, 1171.200, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1031.848f, 1159.829f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1032.348f, 1172.640f, 0.1f);
                            break;

                        case "1075.000, 907.500, -19.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1032.924f, 909.476f, -19.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1031.414f, 901.485f, -18.9f);
                            break;

                        case "1167.500, 910.000, -29.2":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1176.092f, 885.503f, -29.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1167.423f, 879.680f, -29.0f);
                            break;

                        case "1025.000, 1125.000, -69.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1050.322f, 1154.410f, -69.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1057.073f, 1142.646f, -69.8f);
                            break;

                        case "1035.000, 1022.500, 0.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1057.083f, 1060.427f, 0.4f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1066.855f, 1051.411f, 0.4f);
                            break;

                        case "1010.000, 940.000, 0.3":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1029.523f, 970.888f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1020.438f, 978.767f, 0.8f);
                            break;

                        case "962.500, 977.500, 0.3":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(998.513f, 996.272f, 0.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(990.262f, 1005.881f, 0.8f);
                            break;

                        case "995.000, 1122.500, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1023.505f, 1098.805f, -7.7f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1026.503f, 1108.515f, -9.5f);
                            break;

                        case "1280.000, 1291.000, -9.9":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1237.393f, 1292.912f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1236.358f, 1274.019f, -9.9f);
                            break;

                        case "1227.000, 1043.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1193.665f, 1067.520f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1189.534f, 1051.004f, 0.1f);
                            break;

                        case "1247.000, 680.000, 10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1272.953f, 651.860f, 10.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1257.523f, 642.197f, 10.1f);
                            break;

                        case "1025.000, 1147.500, 10.7":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(988.003f, 1122.394f, 10.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(998.977f, 1113.907f, 10.6f);
                            break;

                        case "1150.000, 1280.000, -19.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1141.777f, 1242.112f, -19.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1127.747f, 1245.466f, -19.9f);
                            break;

                        case "1082.500, 1125.000, -29.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1093.025f, 1157.112f, -29.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1080.533f, 1159.822f, -29.9f);
                            break;

                        case "1162.000, 870.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1198.629f, 869.412f, 0.5f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1193.494f, 852.606f, 0.5f);
                            break;

                        case "1032.000, 750.000, 0.7":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1031.181f, 707.104f, 0.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1014.178f, 713.400f, 0.8f);
                            break;

                        case "1231.000, 1583.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1265.307f, 1584.545f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1266.898f, 1566.579f, -9.9f);
                            break;

                        case "1074.000, 1271.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1098.761f, 1298.055f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1107.496f, 1286.721f, 0.1f);
                            break;

                        case "1215.000, 1038.000, 0.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1180.545f, 1074.266f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1173.368f, 1060.616f, 0.1f);
                            break;

                        case "1195.000, 1257.500, -60.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1160.482f, 1221.160f, -59.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1172.020f, 1212.780f, -59.9f);
                            break;

                        case "1218.034, 1513.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1176.761f, 1502.721f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1174.960f, 1518.705f, 0.1f);
                            break;

                        case "1258.000, 1525.000, -9.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1240.757f, 1558.512f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1254.084f, 1559.995f, -9.9f);
                            break;

                        case "1350.000, 675.001, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1329.027f, 709.186f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1314.738f, 696.295f, 0.1f);
                            break;

                        case "1314.000, 702.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1321.168f, 656.072f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1303.477f, 653.470f, 0.1f);
                            break;

                        case "1310.000, 656.000, 0.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1271.569f, 684.623f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1260.174f, 665.743f, 0.1f);
                            break;

                        case "1263.640, 667.709, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1288.150f, 632.079f, 0.6f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1267.869f, 619.950f, 0.6f);
                            break;

                        case "1260.000, 609.001, 0.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1236.962f, 646.999f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1222.410f, 630.882f, 0.1f);
                            break;

                        case "1214.553, 627.469, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1223.920f, 583.704f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1208.465f, 578.848f, 0.1f);
                            break;

                        case "1698.034, 1033.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1657.670f, 1023.867f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1656.701f, 1037.720f, 0.1f);
                            break;

                        case "1711.000, 1103.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1745.617f, 1101.863f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1745.750f, 1083.358f, -9.9f);
                            break;

                        case "1738.000, 1045.000, -9.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1735.119f, 1079.881f, -9.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1714.385f, 1069.432f, -9.3f);
                            break;

                        case "1820.000, 1775.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1819.907f, 1729.212f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1803.550f, 1730.318f, 0.1f);
                            break;

                        case "1694.000, 1273.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1693.361f, 1308.948f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1678.297f, 1303.182f, -9.9f);
                            break;

                        case "1627.000, 1227.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1626.570f, 1263.914f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1611.689f, 1256.506f, -9.9f);
                            break;

                        case "1699.639, 864.021, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1653.231f, 863.687f, 0.6f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1656.379f, 843.384f, 0.6f);
                            break;

                        case "1980.785, 1267.248, 0.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1979.710f, 1220.692f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1962.928f, 1221.554f, 0.1f);
                            break;

                        case "1193.000, 1193.000, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1217.199f, 1220.293f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1206.404f, 1227.620f, 0.1f);
                            break;

                        case "1215.000, 914.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1181.688f, 948.568f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1171.451f, 930.820f, 0.4f);
                            break;

                        case "1830.000, 675.001, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1807.157f, 707.507f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1793.867f, 696.147f, 0.1f);
                            break;

                        case "1794.000, 702.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1800.281f, 654.520f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1781.604f, 653.602f, 0.1f);
                            break;

                        case "1790.000, 656.000, 0.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1759.080f, 680.990f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1748.663f, 666.368f, 0.1f);
                            break;

                        case "1743.640, 667.709, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1769.370f, 631.103f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1749.803f, 619.944f, 0.1f);
                            break;

                        case "1740.000, 609.001, 0.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1718.760f, 642.140f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1705.238f, 628.125f, 0.1f);
                            break;

                        case "1694.553, 627.469, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1704.518f, 586.267f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1688.520f, 580.049f, 0.1f);
                            break;

                        case "1500.785, 787.248, 0.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1497.669f, 738.562f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1482.833f, 739.652f, 0.1f);
                            break;

                        case "1147.000, 784.000, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1182.706f, 794.898f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1179.399f, 776.099f, 0.1f);
                            break;

                        case "1338.000, 1167.000, 0.2":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1335.216f, 1115.787f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1317.877f, 1118.732f, 0.1f);
                            break;

                        case "1227.000, 1523.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1185.564f, 1531.316f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1193.854f, 1548.594f, 0.1f);
                            break;

                        case "1107.500, 1245.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1107.239f, 1291.502f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1123.941f, 1286.082f, -9.9f);
                            break;

                        case "1262.500, 1142.500, -39.3":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1284.768f, 1153.468f, -39.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1290.541f, 1137.356f, -39.1f);
                            break;

                        case "1190.000, 955.000, -60.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1218.600f, 922.797f, -59.6f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1202.211f, 909.561f, -59.6f);
                            break;

                        case "1218.034, 1033.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1174.871f, 1021.767f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1172.179f, 1037.639f, 0.1f);
                            break;

                        case "1231.000, 1103.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1266.215f, 1102.728f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1267.122f, 1083.696f, -9.9f);
                            break;

                        case "1258.000, 1045.000, -9.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1255.382f, 1081.482f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1237.470f, 1076.252f, -9.9f);
                            break;

                        case "1324.945, 1650.731, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1346.505f, 1675.775f, 0.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1354.882f, 1665.782f, 0.3f);
                            break;

                        case "1247.000, 1160.000, 10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1273.861f, 1129.394f, 10.0f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1258.688f, 1119.937f, 10.0f);
                            break;

                        case "1077.500, 1010.000, 0.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1037.038f, 1017.848f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1036.736f, 1002.085f, 0.1f);
                            break;

                        case "1179.500, 1001.500, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1207.010f, 1021.364f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1210.541f, 1007.415f, 0.1f);
                            break;

                        case "1500.785, 1747.248, 0.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1498.434f, 1699.949f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1484.603f, 1700.406f, 0.1f);
                            break;

                        case "1163.000, 1358.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1163.345f, 1314.953f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1147.099f, 1314.363f, -9.9f);
                            break;

                        case "1052.000, 558.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1052.211f, 592.506f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1036.774f, 585.589f, 0.1f);
                            break;

                        case "2333.607, 1186.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(2334.267f, 1220.821f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(2317.482f, 1217.472f, 0.1f);
                            break;

                        case "1953.510, 1171.200, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1984.772f, 1156.345f, 0.2f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1990.419f, 1171.063f, 0.2f);
                            break;

                        case "1147.000, 1227.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1145.734f, 1262.716f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1131.223f, 1255.076f, -9.9f);
                            break;

                        case "1280.000, 1771.000, -9.9":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1236.327f, 1770.075f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1233.498f, 1754.835f, -9.9f);
                            break;

                        case "1214.000, 1273.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1212.704f, 1311.488f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1198.539f, 1306.196f, -9.9f);
                            break;

                        case "1353.000, 1270.000, 0.2":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1382.493f, 1235.654f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1367.642f, 1222.414f, 0.1f);
                            break;

                        case "1218.034, 1993.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1172.737f, 1993.799f, 0.6f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1172.844f, 1976.634f, 0.6f);
                            break;

                        case "1038.000, 705.000, 0.4":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1036.615f, 745.526f, 0.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1017.877f, 740.176f, 0.9f);
                            break;

                        case "1711.000, 2063.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1746.875f, 2062.477f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1742.869f, 2050.794f, -9.9f);
                            break;

                        case "1738.000, 2005.000, -9.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1732.862f, 2041.387f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1714.499f, 2028.747f, -9.9f);
                            break;

                        case "1623.500, 1110.500, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1660.592f, 1094.305f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1648.556f, 1079.045f, 0.1f);
                            break;

                        case "1238.000, 601.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1219.048f, 634.069f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1235.820f, 639.875f, 0.1f);
                            break;

                        case "751.000, 1103.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(786.825f, 1101.840f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(782.223f, 1089.787f, -9.9f);
                            break;

                        case "778.000, 1045.000, -9.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(775.377f, 1080.793f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(755.382f, 1067.687f, -9.9f);
                            break;

                        case "1856.000, 1272.000, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1852.454f, 1230.363f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1836.384f, 1232.420f, 0.1f);
                            break;

                        case "1473.510, 1171.200, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1512.074f, 1170.969f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1510.695f, 1150.941f, 0.1f);
                            break;

                        case "1069.000, 1225.000, 0.7":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1103.989f, 1226.769f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1099.504f, 1211.722f, 0.1f);
                            break;

                        case "1734.000, 1041.000, 0.5":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1702.419f, 1071.380f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1690.118f, 1056.925f, 0.1f);
                            break;

                        case "1643.000, 1358.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1642.340f, 1308.638f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1626.709f, 1309.371f, -9.9f);
                            break;

                        case "1711.000, 1583.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1741.414f, 1571.936f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1745.252f, 1585.075f, -9.9f);
                            break;

                        case "1853.607, 1186.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1853.425f, 1221.118f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1836.242f, 1217.067f, 0.1f);
                            break;

                        case "1182.500, 1612.500, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1181.482f, 1571.988f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1159.592f, 1570.790f, -9.9f);
                            break;

                        case "1063.000, 1615.000, 0.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1098.740f, 1614.953f, -1.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1092.827f, 1599.562f, -1.3f);
                            break;

                        case "1219.639, 864.021, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1172.021f, 861.046f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1172.825f, 841.668f, 0.1f);
                            break;

                        case "1586.000, 1292.000, -9.7":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1612.872f, 1313.190f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1616.216f, 1300.485f, -9.9f);
                            break;

                        case "1642.000, 870.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1680.158f, 869.684f, 0.5f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1676.094f, 854.772f, 0.5f);
                            break;

                        case "1373.607, 706.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1374.358f, 744.028f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1356.375f, 739.247f, 0.1f);
                            break;

                        case "993.510, 691.200, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1033.744f, 690.147f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1027.099f, 677.289f, 0.1f);
                            break;

                        case "1830.000, 1155.001, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1806.783f, 1188.490f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1794.464f, 1175.351f, 0.1f);
                            break;

                        case "1794.000, 1182.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1799.684f, 1134.512f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1781.490f, 1132.778f, 0.1f);
                            break;

                        case "1790.000, 1136.000, 0.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1757.392f, 1162.921f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1746.697f, 1147.660f, 0.1f);
                            break;

                        case "1743.640, 1147.708, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1765.807f, 1113.081f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1751.051f, 1099.607f, 0.1f);
                            break;

                        case "1740.000, 1089.001, 0.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1716.325f, 1123.497f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1705.235f, 1110.743f, 0.1f);
                            break;

                        case "1694.553, 1107.469, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1704.706f, 1063.489f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1689.579f, 1060.016f, 0.1f);
                            break;

                        case "1856.000, 792.000, 0.1":
                        case "1853.607, 706.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1835.618f, 743.403f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1853.792f, 747.476f, 0.1f);
                            break;

                        case "2336.000, 792.000, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(2333.038f, 748.019f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(2315.177f, 746.222f, 0.1f);
                            break;

                        case "1250.000, 1284.000, -20.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1248.383f, 1238.843f, -19.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1234.601f, 1238.144f, -19.8f);
                            break;

                        case "1106.000, 1292.000, -9.7":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1139.747f, 1302.398f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1134.796f, 1313.575f, -9.9f);
                            break;

                        case "738.034, 1993.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(692.410f, 1992.857f, 0.2f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(694.415f, 1979.533f, 0.2f);
                            break;

                        case "751.000, 2063.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(785.622f, 2064.573f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(784.747f, 2051.893f, -9.9f);
                            break;

                        case "1500.785, 1267.248, 0.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1498.978f, 1221.712f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1483.300f, 1220.289f, 0.1f);
                            break;

                        case "1253.001, 1145.999, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1286.419f, 1130.511f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1274.154f, 1117.183f, 0.1f);
                            break;

                        case "835.000, 897.500, -60.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(800.756f, 863.261f, -59.2f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(811.196f, 856.925f, -59.2f);
                            break;

                        case "927.500, 680.000, -69.2":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(881.944f, 674.190f, -69.0f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(881.895f, 687.170f, -69.0f);
                            break;

                        case "1607.000, 1744.000, 10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1605.623f, 1783.320f, 10.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1588.742f, 1776.022f, 10.1f);
                            break;

                        case "1833.000, 1270.000, 0.2":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1860.265f, 1238.670f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1845.802f, 1226.004f, 0.1f);
                            break;

                        case "1818.000, 1167.000, 0.2":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1816.096f, 1119.558f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1797.291f, 1116.181f, 0.1f);
                            break;

                        case "877.500, 865.000, -39.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(859.190f, 843.511f, -39.4f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(852.344f, 851.775f, -39.4f);
                            break;

                        case "827.500, 875.000, -40.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(846.080f, 895.617f, -39.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(833.047f, 903.756f, -39.9f);
                            break;

                        case "1324.945, 1170.731, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1351.955f, 1200.774f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1357.874f, 1190.310f, 0.1f);
                            break;

                        case "885.000, 812.500, -39.4":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(912.895f, 819.495f, -39.0f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(906.972f, 830.953f, -39.0f);
                            break;

                        case "747.500, 885.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(765.168f, 931.680f, -9.0f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(748.516f, 933.296f, -9.0f);
                            break;

                        case "902.500, 782.500, -39.3":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(927.406f, 794.035f, -39.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(929.039f, 784.236f, -38.8f);
                            break;

                        case "830.000, 595.000, -60.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(859.446f, 561.527f, -59.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(846.472f, 550.931f, -59.9f);
                            break;

                        case "762.500, 927.500, -59.7":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(757.220f, 880.017f, -59.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(740.785f, 883.564f, -59.3f);
                            break;

                        case "1695.000, 914.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1663.675f, 945.355f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1653.017f, 926.270f, 0.1f);
                            break;

                        case "1518.000, 705.000, 0.4":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1517.723f, 743.360f, 0.6f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1499.606f, 741.004f, 0.6f);
                            break;

                        case "1512.000, 750.000, 0.7":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1509.630f, 698.601f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1497.233f, 705.774f, 0.1f);
                            break;

                        case "665.000, 787.500, 10.7":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(628.334f, 761.527f, 10.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(637.583f, 752.272f, 10.8f);
                            break;

                        case "790.000, 920.000, -19.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(763.759f, 879.785f, -19.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(779.832f, 874.955f, -19.8f);
                            break;

                        case "1718.000, 601.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1700.923f, 633.282f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1715.711f, 637.723f, 0.1f);
                            break;

                        case "1532.000, 558.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1531.762f, 599.938f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1516.049f, 594.653f, 0.1f);
                            break;

                        case "1730.000, 1284.000, -20.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1729.562f, 1233.752f, -19.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1713.666f, 1236.566f, -19.8f);
                            break;

                        case "565.000, 752.500, -30.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(603.371f, 740.587f, -29.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(597.649f, 725.779f, -29.9f);
                            break;

                        case "585.000, 800.000, -29.4":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(620.953f, 781.000f, -29.6f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(609.879f, 766.239f, -29.6f);
                            break;

                        case "625.000, 847.500, -29.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(636.387f, 803.288f, -29.6f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(651.761f, 817.361f, -29.6f);
                            break;

                        case "715.000, 547.500, -19.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(673.938f, 552.332f, -19.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(670.547f, 539.062f, -19.2f);
                            break;

                        case "807.500, 550.000, -29.2":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(806.216f, 518.899f, -29.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(818.463f, 525.613f, -29.3f);
                            break;

                        case "665.000, 765.000, -69.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(690.322f, 791.526f, -69.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(696.963f, 781.937f, -69.9f);
                            break;

                        case "1376.000, 792.000, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1361.278f, 749.009f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1377.149f, 749.373f, 0.1f);
                            break;

                        case "1182.500, 1132.500, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1182.970f, 1075.510f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1161.672f, 1091.661f, -9.9f);
                            break;

                        case "1473.510, 691.200, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1508.859f, 669.130f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1511.500f, 691.192f, 0.1f);
                            break;

                        case "1122.936, 549.417, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1151.337f, 575.905f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1139.722f, 587.115f, 0.1f);
                            break;

                        case "339.276, 906.676, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(365.023f, 932.040f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(350.773f, 940.882f, 0.1f);
                            break;

                        case "213.000, 1135.000, 0.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(243.265f, 1109.739f, 0.7f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(254.062f, 1134.910f, 0.7f);
                            break;

                        case "682.000, 1038.000, 0.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(668.557f, 1068.441f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(686.036f, 1072.790f, 0.1f);
                            break;

                        case "1695.000, 1038.000, 0.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1666.130f, 1070.112f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1673.467f, 1051.452f, 0.1f);
                            break;

                        case "1822.500, 1160.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1848.118f, 1193.514f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1852.573f, 1187.104f, 0.1f);
                            break;

                        case "717.500, 650.000, 0.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(678.290f, 666.247f, 0.2f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(679.381f, 649.375f, 0.5f);
                            break;

                        case "1554.000, 1271.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1585.932f, 1289.330f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1580.782f, 1296.168f, 0.1f);
                            break;

                        case "1698.034, 1513.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1663.058f, 1504.468f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1661.347f, 1515.322f, 0.1f);
                            break;

                        case "1063.000, 1135.000, 0.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1105.451f, 1140.433f, 1.4f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1106.510f, 1130.367f, -0.9f);
                            break;

                        case "1980.785, 787.248, 0.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1984.896f, 741.813f, 0.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1972.306f, 746.412f, 0.1f);
                            break;

                        case "1738.000, 1525.000, -9.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1742.726f, 1568.543f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1728.978f, 1560.071f, -9.6f);
                            break;

                        case "868.000, 1081.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(870.017f, 1129.642f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(846.471f, 1131.752f, 0.1f);
                            break;

                        case "682.000, 1038.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(685.691f, 1085.453f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(666.405f, 1088.739f, 0.1f);
                            break;

                        case "650.000, 580.000, 0.3":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(674.027f, 612.886f, 0.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(657.211f, 620.335f, 0.8f);
                            break;

                        case "635.000, 762.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(660.533f, 738.268f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(664.457f, 751.970f, -10.9f);
                            break;

                        case "1730.000, 1764.000, -20.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1737.904f, 1719.187f, -19.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1719.671f, 1718.834f, -19.7f);
                            break;

                        case "1127.000, 1744.000, 10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1133.214f, 1784.894f, 10.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1118.811f, 1776.153f, 10.9f);
                            break;

                        case "1340.000, 1775.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1328.011f, 1732.206f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1347.369f, 1732.206f, 0.1f);
                            break;

                        case "1673.000, 1673.000, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1686.445f, 1714.270f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1700.836f, 1698.847f, 0.1f);
                            break;

                        case "675.000, 662.500, 0.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(709.480f, 687.973f, 0.6f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(691.848f, 698.717f, 0.1f);
                            break;

                        case "1760.000, 1771.000, -9.9":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1713.408f, 1759.265f, -9.7f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1711.355f, 1775.395f, -9.7f);
                            break;

                        case "1532.000, 1038.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1518.603f, 1072.998f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1533.450f, 1078.962f, 0.1f);
                            break;

                        case "1718.000, 1081.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1698.273f, 1114.907f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1716.821f, 1119.037f, 0.1f);
                            break;

                        case "1707.000, 1523.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1677.378f, 1561.943f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1666.671f, 1543.314f, 0.1f);
                            break;

                        case "1342.500, 1160.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1365.745f, 1201.802f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1381.714f, 1183.739f, 0.1f);
                            break;

                        case "1122.936, 1029.417, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1159.013f, 1057.721f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1142.207f, 1073.693f, 0.1f);
                            break;

                        case "860.000, 877.500, -59.2":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(836.091f, 832.327f, -59.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(821.458f, 847.681f, -59.3f);
                            break;

                        case "790.000, 920.000, -39.2":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(783.226f, 869.030f, -39.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(760.955f, 879.303f, -39.3f);
                            break;

                        case "1258.000, 2005.000, -9.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1263.917f, 2051.145f, -9.8f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1244.825f, 2042.773f, -9.8f);
                            break;

                        case "1074.000, 1751.000, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1119.127f, 1769.263f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1103.280f, 1786.780f, 0.1f);
                            break;

                        case "1215.000, 1518.000, 0.6":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1189.881f, 1560.483f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1161.773f, 1540.047f, 0.1f);
                            break;

                        case "702.500, 1132.500, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(713.399f, 1088.343f, -9.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(686.296f, 1085.490f, -10.0f);
                            break;

                        case "602.500, 617.500, 0.3":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(644.272f, 632.519f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(628.288f, 647.943f, 0.1f);
                            break;

                        case "1254.000, 1041.000, 0.5":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1228.808f, 1082.657f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1212.302f, 1060.645f, 0.1f);
                            break;

                        case "1673.000, 1193.000, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1705.416f, 1221.782f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1689.244f, 1240.489f, 0.1f);
                            break;

                        case "635.000, 762.500, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(671.025f, 755.950f, -9.6f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(663.514f, 739.846f, -8.9f);
                            break;

                        case "722.500, 765.000, -29.8":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(718.606f, 796.127f, -29.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(733.245f, 794.067f, -29.9f);
                            break;

                        case "332.500, 1132.000, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(314.806f, 1088.549f, -10.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(329.783f, 1082.354f, -10.3f);
                            break;

                        case "2336.000, 1272.000, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(2333.957f, 1224.421f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(2316.752f, 1220.511f, 0.1f);
                            break;

                        case "332.500, 1132.500, -10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(325.572f, 1083.857f, -9.6f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(309.119f, 1082.317f, -9.6f);
                            break;

                        case "474.945, 1179.731, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(505.947f, 1188.002f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(496.516f, 1199.508f, 0.1f);
                            break;

                        case "1280.000, 2251.000, -9.9":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1236.898f, 2251.329f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1236.719f, 2235.266f, -9.9f);
                            break;

                        case "1602.936, 549.417, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1618.053f, 584.589f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1630.469f, 576.974f, 0.1f);
                            break;

                        case "1627.000, 784.000, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1662.510f, 794.012f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1661.674f, 778.172f, 0.1f);
                            break;

                        case "474.945, 1170.731, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(507.679f, 1190.924f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(501.005f, 1200.877f, 0.1f);
                            break;

                        case "1302.503, 1288.000, -20.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1260.444f, 1305.028f, -17.6f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1257.252f, 1289.873f, -19.9f);
                            break;

                        case "1231.000, 2543.000, 10.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1268.364f, 2531.598f, -9.9f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1266.420f, 2544.175f, -9.9f);
                            break;

                        case "1669.276, 1386.676, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1690.258f, 1417.582f, 0.3f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1696.484f, 1403.538f, 0.3f);
                            break;

                        case "739.639, 1824.021, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(703.058f, 1814.118f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(703.696f, 1834.336f, 0.5f);
                            break;

                        case "1143.500, 1590.500, 0.0":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1178.362f, 1575.043f, 0.1f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1165.389f, 1557.444f, 0.1f);
                            break;

                        case "1953.510, 691.200, 0.1":
                            worldCoord1 = Hud.Window.CreateWorldCoordinate(1988.417f, 683.042f, 0.2f);
                            worldCoord2 = Hud.Window.CreateWorldCoordinate(1990.678f, 696.895f, 0.1f);
                            break;

                        default:
                            UnknownDemonicForgeDecorator.Paint(layer, actor, actor.FloorCoordinate, "!!! Not repertoriated !!! " + actor.FloorCoordinate);
                            var cursorScreen = Hud.Window.CreateScreenCoordinate(Hud.Window.CursorX, Hud.Window.CursorY);
                            var World = cursorScreen.ToWorldCoordinate();
                            string worldstring = World.ToString();
                            int visibleX = Hud.Window.CursorX;
                            int visibleY = Hud.Window.CursorY;
                            var TextFont = Hud.Render.CreateFont("tahoma", 12, 255, 255, 255, 0, true, false, true);
                            var layout = TextFont.GetTextLayout(worldstring);
                            TextFont.DrawText(layout, visibleX, visibleY - 50);
                            break;
                    }

                    var ScreenCoord1 = Hud.Window.WorldToScreenCoordinate(worldCoord1.X, worldCoord1.Y, worldCoord1.Z, false, false);
                    var ScreenCoord2 = Hud.Window.WorldToScreenCoordinate(worldCoord2.X, worldCoord2.Y, worldCoord2.Z, false, false);

                    brush.DrawLine(ActorPos.X, ActorPos.Y, ScreenCoord1.X, ScreenCoord1.Y);
                    brush.DrawLine(ActorPos.X, ActorPos.Y, ScreenCoord2.X, ScreenCoord2.Y);
                    brush.DrawLine(ScreenCoord1.X, ScreenCoord1.Y, ScreenCoord2.X, ScreenCoord2.Y);
                    DemonicForgeDecorator.Paint(layer, actor, actor.FloorCoordinate, actor.SnoActor.NameLocalized);

                    Hud.Render.GetMinimapCoordinates(ActorPosToMap.X, ActorPosToMap.Y, out float ActorOnMapX, out float ActorOnMapY);
                    Hud.Render.GetMinimapCoordinates(worldCoord1.X, worldCoord1.Y, out float worldCoord1OnMapX, out float worldCoord1OnMapY);
                    Hud.Render.GetMinimapCoordinates(worldCoord2.X, worldCoord2.Y, out float worldCoord2OnMapX, out float worldCoord2OnMapY);

                    RedBrush.DrawLine(ActorOnMapX, ActorOnMapY, worldCoord1OnMapX, worldCoord1OnMapY);
                    RedBrush.DrawLine(ActorOnMapX, ActorOnMapY, worldCoord2OnMapX, worldCoord2OnMapY);
                    RedBrush.DrawLine(worldCoord1OnMapX, worldCoord1OnMapY, worldCoord2OnMapX, worldCoord2OnMapY);
                }

                if (actor.SnoActor.Sno == ActorSnoEnum._x1_pand_ext_ordnance_tower_shock_a /*322194*/ && ShockTower) ShockTowerDecorator.Paint(layer, actor, actor.FloorCoordinate, actor.SnoActor.NameLocalized); 
                if (actor.SnoActor.Sno == ActorSnoEnum._x1_bog_bloodspring_medium /*332922*/ && BloodSprings) BloodSpringsDecoratorMedium.Paint(layer, actor, actor.FloorCoordinate, actor.SnoActor.NameLocalized);
                if (actor.SnoActor.Sno == ActorSnoEnum._x1_bog_bloodspring_large /*332923*/ && BloodSprings) BloodSpringsDecoratorBig.Paint(layer, actor, actor.FloorCoordinate, actor.SnoActor.NameLocalized);
                if (actor.SnoActor.Sno == ActorSnoEnum._x1_bog_bloodspring_small /*332924*/ && BloodSprings) BloodSpringsDecoratorSmall.Paint(layer, actor, actor.FloorCoordinate, actor.SnoActor.NameLocalized);
                if (!Hud.Game.Me.IsDead)
                {
                    if (actor.SnoActor.Sno == ActorSnoEnum._monsteraffix_desecrator_damage_aoe /*84608*/ && actor.NormalizedXyDistanceToMe <= 8 && Desecrator && RunForYourLife
                        || actor.SnoActor.Sno == ActorSnoEnum._x1_monsteraffix_thunderstorm_impact /*341512*/ && actor.NormalizedXyDistanceToMe <= 16 && Thunderstorm && RunForYourLife
                        || actor.SnoActor.Sno == ActorSnoEnum._monsteraffix_plagued_endcloud /*108869*/ && actor.NormalizedXyDistanceToMe <= 12 && Plagued && RunForYourLife
                        || actor.SnoActor.Sno == ActorSnoEnum._creepmobarm /*3865*/ && actor.NormalizedXyDistanceToMe <= 12 && Plagued && RunForYourLife
                        || actor.SnoActor.Sno == ActorSnoEnum._monsteraffix_molten_trail /*95868*/ && actor.NormalizedXyDistanceToMe <= 5 && Molten && RunForYourLife
                        || actor.SnoActor.Sno == ActorSnoEnum._gluttony_gascloud_proxy /*93837*/ && actor.NormalizedXyDistanceToMe <= 20 && GasCloud && RunForYourLife
                        || actor.SnoActor.Sno >= ActorSnoEnum._fastmummy_a/*4104*/ && actor.SnoActor.Sno <= ActorSnoEnum._fastmummy_c/*4106*/ && actor.NormalizedXyDistanceToMe <= 5 && PoisonDeath && RunForYourLife) //|| actor.SnoActor.Sno == ActorSnoEnum._monsteraffix_molten_deathstart_proxy/*4803*/ && actor.NormalizedXyDistanceToMe <= 13f && MoltenExplosion) || actor.SnoActor.Sno == ActorSnoEnum._morluspellcaster_meteor_pending /*159369*/ && actor.NormalizedXyDistanceToMe <= 20 && MorluSpellcasterMeteorPending
                    {
                        MoveWarningDecorator.Paint(layer, actor, actor.FloorCoordinate, "Moveth!");
                        Danger = false;
                    }
                    else Danger = true;
                }
                if (ArcaneEnchanted)
                {
                    if (actor.SnoActor.Sno == ActorSnoEnum._monsteraffix_arcaneenchanted_petsweep /*219702*/) ArcaneDecorator.Paint(layer, actor, actor.FloorCoordinate, "\u21BA");
                    if (actor.SnoActor.Sno == ActorSnoEnum._monsteraffix_arcaneenchanted_petsweep_reverse /*221225*/) ArcaneDecorator.Paint(layer, actor, actor.FloorCoordinate, "\u21BB");
                }

                if (actor.SnoActor.Sno == ActorSnoEnum._x1_monsteraffix_corpsebomber_bomb_start /*340319*/ && PoisonEnchanted)
                {
                    var ActorPos = actor.FloorCoordinate.ToScreenCoordinate();
                    var brush = Hud.Render.CreateBrush(128, 160, 255, 160, 3, SharpDX.Direct2D1.DashStyle.Dash, SharpDX.Direct2D1.CapStyle.Flat, SharpDX.Direct2D1.CapStyle.Flat);
                    brush.DrawLine(ActorPos.X + offsetX, ActorPos.Y + offsetY, ActorPos.X - offsetX, ActorPos.Y - offsetY); // antislash
                    brush.DrawLine(ActorPos.X + offsetX, ActorPos.Y - offsetY, ActorPos.X - offsetX, ActorPos.Y + offsetY); // slash
                }

                if (actor.SnoActor.Sno == ActorSnoEnum._sandwasp_projectile /*5212*/ && SandWaspProjectile
                    || actor.SnoActor.Sno == ActorSnoEnum._x1_skeletonarcher_arrow_cold /*312942*/ && ArrowProjectile
                    || actor.SnoActor.Sno == ActorSnoEnum._x1_bogfamily_ranged_quill_proj /*337030*/ && BogFamilyProjectile
                    || actor.SnoActor.Sno == ActorSnoEnum._x1_bloodgolem_shaman_bloodball /*353256*/ && bloodGolemProjectile
                    || actor.SnoActor.Sno == ActorSnoEnum._x1_molemutant_ranged_projectile /*349564*/ && MoleMutantProjectile
                    || actor.SnoActor.Sno == ActorSnoEnum._p4_ice_porcupine_nova_projectile /*430430*/ && IcePorcupineProjectile)
                {
                    ProjectileDecorator.Paint(layer, actor, actor.FloorCoordinate, "O");
                }
                if (DemonMine)
                {
                    if (actor.SnoActor.Sno == ActorSnoEnum._a3_battlefield_demonmine_c /*118596*/ ||
                        actor.SnoActor.Sno == ActorSnoEnum._a3_battlefield_demonmine_a_energy /*117921*/ ||
                        actor.SnoActor.Sno == ActorSnoEnum._a3_battlefield_demonmine_a_rune /*117906*/ ||
                        actor.SnoActor.Sno == ActorSnoEnum._a3_battlefield_demonmine_c_snow /*150825*/ ||
                        actor.SnoActor.Sno == ActorSnoEnum._ls_a3_battlefield_demonmine_c /*468082*/)
                    {
                        DemonMineDecorator.Paint(layer, actor, actor.FloorCoordinate, null);
                    }
                }

                if (actor.SnoActor.Sno == ActorSnoEnum._x1_monsteraffix_orbiter_projectile /*343539*/ && actor.NormalizedXyDistanceToMe <= 10 && Orbiter ||
                    actor.SnoActor.Sno == ActorSnoEnum._succubus_bloodstar_projectile /*164827*/ && actor.NormalizedXyDistanceToMe <= 12 && BloodStar)
                {
                    OrbiterDecorator.Paint(layer, actor, actor.FloorCoordinate, null);
                }

                // toggles default EliteMonsterSkillPlugin decorators if you step on it and damage is null or not life threatening
                if (actor.SnoActor.Sno == ActorSnoEnum._monsteraffix_desecrator_damage_aoe /*84608*/ && actor.NormalizedXyDistanceToMe <= 8 && !RunForYourLife) Hud.GetPlugin<EliteMonsterSkillPlugin>().DesecratorDecorator.ToggleDecorators<GroundCircleDecorator>(false);
                if (actor.SnoActor.Sno == ActorSnoEnum._x1_monsteraffix_thunderstorm_impact /*341512*/ && actor.NormalizedXyDistanceToMe <= 16 && !RunForYourLife) Hud.GetPlugin<EliteMonsterSkillPlugin>().ThunderstormDecorator.ToggleDecorators<GroundCircleDecorator>(false);
                if ((actor.SnoActor.Sno == ActorSnoEnum._monsteraffix_plagued_endcloud /*108869*/ || actor.SnoActor.Sno == ActorSnoEnum._creepmobarm /*3865*/) && actor.NormalizedXyDistanceToMe <= 12 && !RunForYourLife) Hud.GetPlugin<EliteMonsterSkillPlugin>().PlaguedDecorator.ToggleDecorators<GroundCircleDecorator>(false);
                if (actor.SnoActor.Sno == ActorSnoEnum._monsteraffix_molten_trail /*95868*/ && actor.NormalizedXyDistanceToMe <= 5 && !RunForYourLife) Hud.GetPlugin<EliteMonsterSkillPlugin>().MoltenDecorator.ToggleDecorators<GroundCircleDecorator>(false);
                if (actor.SnoActor.Sno == ActorSnoEnum._gluttony_gascloud_proxy /*93837*/ && actor.NormalizedXyDistanceToMe <= 20 && !RunForYourLife) Hud.GetPlugin<EliteMonsterSkillPlugin>().GhomDecorator.ToggleDecorators<GroundCircleDecorator>(false);
                if (actor.SnoActor.Sno == ActorSnoEnum._x1_monsteraffix_frozenpulse_monster /*349774*/ && actor.NormalizedXyDistanceToMe <= 14 && !RunForYourLife) Hud.GetPlugin<EliteMonsterSkillPlugin>().FrozenPulseDecorator.ToggleDecorators<GroundCircleDecorator>(false);
                if (actor.SnoActor.Sno == ActorSnoEnum._morluspellcaster_meteor_pending /*159369*/ && MorluSpellcasterMeteorPending) MorluMeteorCountdownDecorator.Paint(layer, actor, actor.FloorCoordinate, "nothing");
            }
            if (RunForYourLife && Danger && !Hud.Game.Me.IsDead) MoveWarningDecorator.Paint(layer, null, Hud.Game.Me.FloorCoordinate, "Danger! " + (int)Hud.Game.Me.Defense.HealthPct + "%");

            var invisibleMonsters = Hud.Game.AliveMonsters.Where(M => M.Invisible);
            var invisibleTexture = Hud.Texture.GetTexture(3123731847);

            foreach (var invisibleMonster in invisibleMonsters)
            {
                var MonsterScreen = invisibleMonster.FloorCoordinate.ToScreenCoordinate();
                invisibleTexture.Draw(MonsterScreen.X, MonsterScreen.Y, 150f, 75f, opacityMultiplier: 0.5f);
            }

            // Explosive monster plugin mod
            var deadMonsters = Hud.Game.Monsters.Where(x => !x.IsAlive);
            foreach (var monster in deadMonsters)
            {
                if (BetrayedPoisonCloud && BetrayedPoisonActors.Contains(monster.SnoActor.Sno))
                {
                    if (!BetrayedPoison.ContainsKey(monster.FloorCoordinate)) BetrayedPoison.Add(monster.FloorCoordinate, Hud.Game.CurrentGameTick);
                    PoisonCloudActor = monster;
                }
                else if (GrotesqueExplosion && GrotesqueExplosionActors.Contains(monster.SnoActor.Sno))
                {
                    if (!GrotesqueBlow.ContainsKey(monster.FloorCoordinate))
                        GrotesqueBlow.Add(monster.FloorCoordinate, Hud.Game.CurrentGameTick);

                    GrotesqueBlow.TryGetValue(monster.FloorCoordinate, out var CreatedAtGameTick);

                    var remaining = 1.3 - ((Hud.Game.CurrentGameTick - CreatedAtGameTick) / 60.0f);
                    if (remaining < 0)
                        remaining = 0;

                    var vf = (remaining > 1.0f) ? "F0" : "F1";
                    var text = remaining.ToString(vf, CultureInfo.InvariantCulture);

                    GrotesqueDecorator.Paint(layer, monster, monster.FloorCoordinate, text);
                }
            }

            foreach (var Cloud in BetrayedPoison)
            {
                var remaining = 7 - ((Hud.Game.CurrentGameTick - Cloud.Value) / 60.0f);
                if (remaining < 0) remaining = 0;
                var countdown = remaining - 4.8;
                var vf = (countdown > 1.0f) ? "F0" : "F1";
                var text = countdown.ToString(vf, CultureInfo.InvariantCulture);
                if (remaining > 0)
                {
                    FastMummyDecorator.Paint(layer, null, Cloud.Key, null);
                    if (countdown > 0) BetrayedCountdownDecorator.Paint(layer, PoisonCloudActor, Cloud.Key, text);
                }
            }
        }
    }

    public class GroundTimerDecorator2 : IWorldDecorator
    {
        public bool Enabled { get; set; }
        public WorldLayer Layer { get; } = WorldLayer.Ground;
        public IController Hud { get; }

        public IBrush BackgroundBrushEmpty { get; set; }
        public IBrush BackgroundBrushFill { get; set; }

        public float Radius { get; set; }
        public float CountDownFrom { get; set; }

        public GroundTimerDecorator2(IController hud)
        {
            Enabled = true;
            Hud = hud;
        }

        public void Paint(IActor actor, IWorldCoordinate coord, string text)
        {
            if (!Enabled) return;
            if (actor == null) return;

            var rad = Radius / 1200.0f * Hud.Window.Size.Height;
            var max = CountDownFrom;

            int CreatedAtGameTick;

            if (DangerPlugin.BetrayedPoisonActors.Contains(actor.SnoActor.Sno))
            {
                DangerPlugin.BetrayedPoison.TryGetValue(coord, out CreatedAtGameTick);
            }
            else if (DangerPlugin.GrotesqueExplosionActors.Contains(actor.SnoActor.Sno))
            {
                DangerPlugin.GrotesqueBlow.TryGetValue(coord, out CreatedAtGameTick);
            }
            else
            {
                CreatedAtGameTick = Hud.Game.CurrentGameTick;
            }

            var elapsed = (Hud.Game.CurrentGameTick - CreatedAtGameTick) / 60.0f;

            if (elapsed < 0) return;
            if (elapsed > max) elapsed = max;

            var screenCoord = coord.ToScreenCoordinate();
            var startAngle = Convert.ToInt32(360 / max * elapsed) - 90;
            var endAngle = 360 - 90;

            using (var pg = Hud.Render.CreateGeometry())
            {
                using (var gs = pg.Open())
                {
                    gs.BeginFigure(new Vector2(screenCoord.X, screenCoord.Y), FigureBegin.Filled);
                    for (int angle = startAngle; angle <= endAngle; angle++)
                    {
                        var mx = rad * (float)Math.Cos(angle * Math.PI / 180.0f);
                        var my = rad * (float)Math.Sin(angle * Math.PI / 180.0f);
                        var vector = new Vector2(screenCoord.X + mx, screenCoord.Y + my);
                        gs.AddLine(vector);
                    }

                    gs.EndFigure(FigureEnd.Closed);
                    gs.Close();
                }

                BackgroundBrushFill.DrawGeometry(pg);
            }

            using (var pg = Hud.Render.CreateGeometry())
            {
                using (var gs = pg.Open())
                {
                    gs.BeginFigure(new Vector2(screenCoord.X, screenCoord.Y), FigureBegin.Filled);
                    for (int angle = endAngle; angle <= startAngle + 360; angle++)
                    {
                        var mx = rad * (float)Math.Cos(angle * Math.PI / 180.0f);
                        var my = rad * (float)Math.Sin(angle * Math.PI / 180.0f);
                        var vector = new Vector2(screenCoord.X + mx, screenCoord.Y + my);
                        gs.AddLine(vector);
                    }

                    gs.EndFigure(FigureEnd.Closed);
                    gs.Close();
                }

                BackgroundBrushEmpty.DrawGeometry(pg);
            }
        }

        public IEnumerable<ITransparent> GetTransparents()
        {
            yield return BackgroundBrushEmpty;
            yield return BackgroundBrushFill;
        }
    }
}
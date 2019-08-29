// https://github.com/User5981/Resu
// Custom Life Warning Plugin for TurboHUD Version 29/08/2019 23:24
// The health globes part was stolen from Xewl's HealthGlobePlugin

using System;
using System.Globalization;
using Turbo.Plugins.Default;
using System.Linq;
using System.Collections.Generic;

namespace Turbo.Plugins.Resu
{
    public class CustomLifeWarningPlugin : BasePlugin, IInGameTopPainter, IInGameWorldPainter
    {

        public int lifePercentage { get; set; }
        public int lifePercentageToDisplayGlobes { get; set; }
        public int maxX { get; set; }
        public int maxY { get; set; }
        public TopLabelDecorator CustomLifeWarningDecorator { get; set; }
        public TopLabelDecorator ShieldDecorator { get; set; }
        public TopLabelDecorator InfiniteShieldDecorator { get; set; }
        public WorldDecoratorCollection HealthGlobeDecorator { get; set; }
        public float opacity { get; set; }
        public IBrush ShieldBrush { get; set; }
        public string SPTL { get; set; }

        private readonly List<float> _steps;

        public CustomLifeWarningPlugin()
        {
            Enabled = true;
            lifePercentage = 50;
            lifePercentageToDisplayGlobes = 40;

            _steps = new List<float>();
            for (var i = 195f; i <= 360f; i += 15f) // 210
            {
                _steps.Add(i);
            }
            for (var i = 15f; i <= 120f; i += 15f)
            {
                _steps.Add(i);
            }
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            opacity = 0.0f;
            maxX = Hud.Window.Size.Width;
            maxY = Hud.Window.Size.Height;

            ShieldBrush = Hud.Render.CreateBrush(255, 160, 160, 215, 3);

            HealthGlobeDecorator = new WorldDecoratorCollection(
            new MapShapeDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(255, 255, 0, 0, 0),
                ShadowBrush = Hud.Render.CreateBrush(96, 0, 0, 0, 1),
                Radius = 4.0f,
                ShapePainter = new CircleShapePainter(Hud),
            },
            new GroundCircleDecorator(Hud)
            {
                Brush = Hud.Render.CreateBrush(255, 255, 0, 0, 3f),
                Radius = 1f,
            });

            ShieldDecorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 12, 200, 160, 160, 215, true, false, 255, 100, 0, 0, true),
                TextFunc = () => "\u25CF",
            };

            CustomLifeWarningDecorator = new TopLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(25, 255, 165, 0, 0),
                TextFont = Hud.Render.CreateFont("Segoe UI Light", 7, 250, 255, 255, 255, false, false, true), // it doesn't work without that line
            };
            
            InfiniteShieldDecorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 12, 255, 160, 160, 215, true, false, 255, 100, 0, 0, true),
                TextFunc = () => " ∞" + SPTL,
            };
            
        }

        public void PaintTopInGame(ClipState clipState)
        {
            var hedPlugin = Hud.GetPlugin<HotEnablerDisablerPlugin>();
            bool GoOn = hedPlugin.CanIRun(Hud.Game.Me, this.GetType().Name);
            if (!GoOn) return;

            if (Hud.Render.UiHidden) return;
            if (clipState != ClipState.BeforeClip) return;

            CustomLifeWarningDecorator.BackgroundBrush.Opacity = opacity;

            var percentLife = Hud.Game.Me.Defense.HealthPct;
            if (percentLife < (float)lifePercentage && percentLife > 25f)
            {
                if (opacity < 1.0f) { opacity += 0.04f; }
                CustomLifeWarningDecorator.Paint(0f, 0f, (float)maxX, (float)maxY, HorizontalAlign.Center);
            }
            else
            {
                if (opacity > 0.0f)
                {
                    opacity -= 0.04f;
                    CustomLifeWarningDecorator.Paint(0f, 0f, (float)maxX, (float)maxY, HorizontalAlign.Center);
                }
            }
            

            var uiRect = Hud.Render.GetUiElement("Root.NormalLayer.game_dialog_backgroundScreenPC.game_progressBar_healthBall").Rectangle;
            var CircleCenter = Hud.Window.CreateScreenCoordinate(uiRect.Left + (uiRect.Width / 2.15f), uiRect.Bottom - (uiRect.Height / 1.41f));
            float CircleRadius = 55f;

            //ShieldBrush.DrawEllipse(uiRect.Left + (uiRect.Width/2), uiRect.Bottom - (uiRect.Height/2), 60, 60); // test circle
            //ShieldDecorator.Paint(CircleCenter.X, CircleCenter.Y, 50f, 50f, HorizontalAlign.Left); // center

            int ShieldPer19 = (int)Math.Round((Hud.Game.Me.Defense.CurShield / Hud.Game.Me.Defense.HealthMax) * 19);
            
            var glowTexture = Hud.Texture.GetTexture(1981524232);
            

            
            var ShieldPylon = Hud.Game.Me.Powers.GetBuff(266254);
             if (ShieldPylon == null || !ShieldPylon.Active)
              {
                SPTL = String.Empty;

                Hud.RunOnPlugin<ResourceOverGlobePlugin>(plugin =>
                {
                    plugin.ShieldDecorator.Enabled = true;
                });
            }
             else {

                   Hud.RunOnPlugin<ResourceOverGlobePlugin>(plugin =>
                   {
                       plugin.ShieldDecorator.Enabled = false;
                   }); 

                    int ShieldPylonTimeLeft = (int)ShieldPylon.TimeLeftSeconds[0];
                    ShieldPer19 = 19;
                     if (ShieldPylonTimeLeft < 10) {SPTL = " " + ShieldPylonTimeLeft.ToString();}
                     else {SPTL = ShieldPylonTimeLeft.ToString();}
                     InfiniteShieldDecorator.Paint(uiRect.Left + uiRect.Width * 0.2f, uiRect.Top + uiRect.Height * 0.60f, uiRect.Width * 0.63f, uiRect.Height * 0.12f, HorizontalAlign.Center);

                  }

            for (int i = 1; i <= ShieldPer19 && i < _steps.Count; i++)
            {
                float Angle = _steps[i];
                var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
                glowTexture.Draw(PointOne.Item1 - 15, PointOne.Item2, 50f, 50f, opacityMultiplier: 0.5f);
                ShieldDecorator.Paint(PointOne.Item1, PointOne.Item2, 50f, 50f, HorizontalAlign.Left);
            }

        }

        public static Tuple<float, float> PointOnCircle(float radius, float angleInDegrees, float CircleCenterX, float CircleCenterY)
        {
            float x = (float)(radius * Math.Cos(angleInDegrees * Math.PI / 180F)) + CircleCenterX;
            float y = (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + CircleCenterY;
            var NewPoint = Tuple.Create(x, y);
            return NewPoint;
        }



        public void PaintWorld(WorldLayer layer)
        {
            var percentLife = Hud.Game.Me.Defense.HealthPct;
            if (percentLife <= (float)lifePercentageToDisplayGlobes && percentLife > 0f)
            {
                var actors = Hud.Game.Actors.Where(x => x.SnoActor.Kind == ActorKind.HealthGlobe);
                foreach (var actor in actors)
                {
                    // HealthGlobeDecorator.ToggleDecorators<GroundLabelDecorator>(!actor.IsOnScreen); // do not display ground labels when the actor is on the screen
                    HealthGlobeDecorator.Paint(layer, actor, actor.FloorCoordinate, "health globe");
                }
            }
        }





    }
}
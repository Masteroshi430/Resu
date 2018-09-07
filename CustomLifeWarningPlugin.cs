//css_reference C:\V7.7.1.dll;
// https://github.com/User5981/Resu
// Custom Life Warning Plugin for TurboHUD Version 07/09/2018 17:30
// The health globes part was stolen from Xewl's HealthGlobePlugin 

using System;
using System.Globalization;
using Turbo.Plugins.Default;
using System.Linq;

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
        public WorldDecoratorCollection HealthGlobeDecorator { get; set; }
        public int opacity { get; set; }
        public IBrush ShieldBrush { get; set; }
        
                
        public CustomLifeWarningPlugin()
        {
            Enabled = true;
            lifePercentage = 50;
            lifePercentageToDisplayGlobes = 40;
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);
            opacity = 0;
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
                 TextFont = Hud.Render.CreateFont("tahoma", 8, 255, 160, 160, 215, true, false, 255, 100, 0, 0, true),
                 TextFunc = () => "\u25CF",
            };
           
           
           
        }

        public void PaintTopInGame(ClipState clipState)
        {
            var hedPlugin = Hud.GetPlugin<HotEnablerDisablerPlugin>();
            bool GoOn = hedPlugin.CanIRun(Hud.Game.Me,this.GetType().Name); 
            if (!GoOn) return;
            
            if (Hud.Render.UiHidden) return;
            if (clipState != ClipState.BeforeClip) return;

             CustomLifeWarningDecorator = new TopLabelDecorator(Hud)
            {
                 BackgroundBrush = Hud.Render.CreateBrush(opacity, 255, 165, 0, 0),
                 TextFont = Hud.Render.CreateFont("Segoe UI Light", 7, 250, 255, 255, 255, false, false, true), // it doesn't work without that line
            };
            
            var percentLife = Hud.Game.Me.Defense.HealthPct; 
            if (percentLife < (float)lifePercentage && percentLife > 25f)
             {
              if (opacity < 25) {opacity++;}  
              CustomLifeWarningDecorator.Paint(0f, 0f, (float)maxX, (float)maxY, HorizontalAlign.Center);
             }
            else
             {
              if (opacity != 0) 
               {
                opacity--;
                CustomLifeWarningDecorator.Paint(0f, 0f, (float)maxX, (float)maxY, HorizontalAlign.Center);
               }
             }
             
             
             var uiRect = Hud.Render.GetUiElement("Root.NormalLayer.game_dialog_backgroundScreenPC.game_progressBar_healthBall").Rectangle;
             var CircleCenter = Hud.Window.CreateScreenCoordinate(uiRect.Left + (uiRect.Width/2.15f), uiRect.Bottom - (uiRect.Height/1.41f));
             float CircleRadius = 59f;
             
              //ShieldBrush.DrawEllipse(uiRect.Left + (uiRect.Width/2), uiRect.Bottom - (uiRect.Height/2), 60, 60); // test circle
             //ShieldDecorator.Paint(CircleCenter.X, CircleCenter.Y, 50f, 50f, HorizontalAlign.Left); // center
             
             int ShieldPer19 = (int)Math.Round((Hud.Game.Me.Defense.CurShield / Hud.Game.Me.Defense.HealthMax)*19);
             
             if (ShieldPer19 >= 1)
              {
               float Angle = 210f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
              
             if (ShieldPer19 >= 2)
              {
               float Angle = 225f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
              
             if (ShieldPer19 >= 3)
              {
               float Angle = 240f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
             
             if (ShieldPer19 >= 4)
              {
               float Angle = 255f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
             
             if (ShieldPer19 >= 5)
              {
               float Angle = 270f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
             
             if (ShieldPer19 >= 6)
              {
               float Angle = 285f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
             
             if (ShieldPer19 >= 7)
              {
               float Angle = 300f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
             
             if (ShieldPer19 >= 8)
              {
               float Angle = 315f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
             
             if (ShieldPer19 >= 9)
              {
               float Angle = 330f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
             
             if (ShieldPer19 >= 10)
              {
               float Angle = 345f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
             
             if (ShieldPer19 >= 11)
              {
               float Angle = 360f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
              
             if (ShieldPer19 >= 12)
              {
               float Angle = 15f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
              
             if (ShieldPer19 >= 13)
              {
               float Angle = 30f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
              
             if (ShieldPer19 >= 14)
              {
               float Angle = 45f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
              
             if (ShieldPer19 >= 15)
              {
               float Angle = 60f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
              
             if (ShieldPer19 >= 16)
              {
               float Angle = 75f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
              
             if (ShieldPer19 >= 17)
              {
               float Angle = 90f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
              
             if (ShieldPer19 >= 18)
              {
               float Angle = 105f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
              
             if (ShieldPer19 >= 19)
              {
               float Angle = 120f;
               var PointOne = PointOnCircle(CircleRadius, Angle, CircleCenter.X, CircleCenter.Y);
               var PointOneToScreen = Hud.Window.CreateScreenCoordinate(PointOne.Item1,PointOne.Item2);
               ShieldDecorator.Paint(PointOneToScreen.X,PointOneToScreen.Y, 50f, 50f, HorizontalAlign.Left);
              }
             
        }
        
         public static Tuple<float,float> PointOnCircle(float radius, float angleInDegrees, float CircleCenterX, float CircleCenterY)
        {
         
         float x = (float)(radius * Math.Cos(angleInDegrees  * Math.PI / 180F)) + CircleCenterX;
         float y = (float)(radius * Math.Sin(angleInDegrees * Math.PI / 180F)) + CircleCenterY;
         var NewPoint = Tuple.Create(x,y); 

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
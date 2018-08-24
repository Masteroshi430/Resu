//css_reference C:\V7.7.1.dll;
// https://github.com/User5981/Resu
// Custom Life Warning Plugin for TurboHUD Version 13/08/2018 08:22
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
        public WorldDecoratorCollection HealthGlobeDecorator { get; set; }
        public int opacity { get; set; }
        
                
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
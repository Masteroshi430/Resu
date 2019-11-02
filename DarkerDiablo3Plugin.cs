// https://github.com/User5981/Resu
// Darker diablo 3 Plugin for TurboHUD Version 02/11/2019 23:18

using System;
using Turbo.Plugins.Default;



namespace Turbo.Plugins.Resu
{
    public class DarkerDiablo3Plugin : BasePlugin, IInGameTopPainter
    {


        public int maxX { get; set; }
        public int maxY { get; set; }
        public TopLabelDecorator DarknessDecorator { get; set; }

        public DarkerDiablo3Plugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            maxX = Hud.Window.Size.Width;
            maxY = Hud.Window.Size.Height;

            DarknessDecorator = new TopLabelDecorator(Hud)
            {
                BackgroundBrush = Hud.Render.CreateBrush(70, 0, 18, 52, 0),
                TextFont = Hud.Render.CreateFont("Segoe UI Light", 7, 250, 255, 255, 255, false, false, true), // it doesn't work without that line
            };

            
        }

        public void PaintTopInGame(ClipState clipState)
        {
            if (Hud.Render.UiHidden) return;
            if (clipState != ClipState.BeforeClip) return;

            DarknessDecorator.Paint(0f, 0f, (float)maxX, (float)maxY, HorizontalAlign.Center);
        }

    }
}
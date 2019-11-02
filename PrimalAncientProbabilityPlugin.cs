// https://github.com/User5981/Resu
// Primal Ancient Probability Plugin for TurboHUD Version 25/10/2019 14:39

using System;
using System.Globalization;
using Turbo.Plugins.Default;
using System.Linq;
using System.Collections.Generic;

namespace Turbo.Plugins.Resu
{
    public class PrimalAncientProbabilityPlugin : BasePlugin, IInGameTopPainter
    {
      
        public TopLabelDecorator ancientDecorator { get; set; }
        public TopLabelDecorator primalDecorator { get; set; }
        public string ancientText{ get; set; }
        public string primalText{ get; set; }
        
        public PrimalAncientProbabilityPlugin()
        {
            Enabled = true;
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);
            ancientText = string.Empty;
            primalText = string.Empty;
            
        }
        
        
        public void PaintTopInGame(ClipState clipState)
        {
                
            if (Hud.Game.Me.CurrentLevelNormal != 70 && Hud.Game.Me.CurrentLevelNormal > 0)  { return;}
            if (Hud.Game.Me.HighestSoloRiftLevel < 70 && Hud.Game.Me.HighestSoloRiftLevel > 0){ return; }

            long PrimalAncientTotal = Hud.Tracker.CurrentAccountTotal.DropPrimalAncient;
            long AncientTotal = Hud.Tracker.CurrentAccountTotal.DropAncient;
            long LegendariesTotal = Hud.Tracker.CurrentAccountTotal.DropLegendary;
            string TotalPercPrimal = ((float)PrimalAncientTotal / (float)LegendariesTotal).ToString("0.00%");
            string TotalPercAncient = ((float)AncientTotal / (float)LegendariesTotal).ToString("0.00%");
                    
             ancientDecorator = new TopLabelDecorator(Hud)
            {
                 TextFont = Hud.Render.CreateFont("arial", 7, 220, 227, 153, 25, true, false, 255, 0, 0, 0, true),
                 TextFunc = () => ancientText,
                 HintFunc = () => "Chance for the next Legendary drop to be Ancient." + Environment.NewLine + "Total Ancient drops : " + AncientTotal + " (" + TotalPercAncient + ") of Legendary drops",
                 BackgroundBrush = Hud.Render.CreateBrush(50, 0, 0, 0, 0),
             };
            
             primalDecorator = new TopLabelDecorator(Hud)
            {

                 TextFont = Hud.Render.CreateFont("arial", 7, 180, 255, 64, 64, true, false, 255, 0, 0, 0, true),
                 TextFunc = () => primalText,
                 HintFunc = () => "Chance for the next Legendary drop to be Primal Ancient." + Environment.NewLine + "Total Primal Ancient drops : " + PrimalAncientTotal + " (" + TotalPercPrimal + ") of Legendary drops",
                 BackgroundBrush = Hud.Render.CreateBrush(50, 0, 0, 0, 0),
             };


            double RNGprobaAncient = 9.7753333;
            double RNGprobaPrimal = 0.2246666;
            double probaAncient = ((float)(AncientTotal) / (float)(LegendariesTotal)) * 100;
            double probaPrimal = ((float)(PrimalAncientTotal) / (float)(LegendariesTotal)) * 100;
            double DiffProbaAncient = (RNGprobaAncient - probaAncient);
            double DiffProbaPrimal = (RNGprobaPrimal - probaPrimal);

            probaAncient += (DiffProbaAncient * 2);
            probaPrimal += (DiffProbaPrimal * 2);

             probaAncient = Math.Round(probaAncient, 4);
             probaPrimal = Math.Round(probaPrimal, 5);


            ancientText = "A: " + probaAncient  + "%";
            primalText =  "P: " + probaPrimal  + "%";

            var uiRect = Hud.Render.GetUiElement("Root.NormalLayer.game_dialog_backgroundScreenPC.game_progressBar_healthBall").Rectangle;
            
            ancientDecorator.Paint(uiRect.Right - (uiRect.Width / 0.35f), uiRect.Top + (uiRect.Height / 1.168f), 75f, 25f, HorizontalAlign.Left);

            if (Hud.Game.Me.HighestSoloRiftLevel >= 70)
            {
            primalDecorator.Paint(uiRect.Right - (uiRect.Width / 0.42f), uiRect.Top + (uiRect.Height / 1.168f), 75f, 25f, HorizontalAlign.Left);
            }
            
            

        }
    }
}
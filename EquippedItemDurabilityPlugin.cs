// https://github.com/User5981/Resu
// Equipped Item Durability plugin for TurboHUD version 12/02/2019 11:27

using Turbo.Plugins.Default;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Turbo.Plugins.Resu
{

    public class EquippedItemDurabilityPlugin : BasePlugin, IInGameTopPainter
    {

        public TopLabelDecorator RedDecorator { get; set; }
        public TopLabelDecorator YellowDecorator { get; set; }
        public TopLabelDecorator GreenDecorator { get; set; }
        public TopLabelDecorator F3Decorator { get; set; }
        public TopLabelDecorator F3RepairDecorator { get; set; }
        public decimal Percentage { get; set; }
        public decimal Oldpercentage { get; set; }
        public bool F3pressed { get; set; }


        public EquippedItemDurabilityPlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            Oldpercentage = -1;

            RedDecorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 10, 255, 255, 100, 100, true, false, 255, 0, 0, 0, true),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 0.0f,
                BackgroundTextureOpacity2 = 0.0f,
                TextFunc = () => Percentage.ToString("F0"),
                HintFunc = () => "durability left in %",
            };

            YellowDecorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 200, 205, 50, true, false, 255, 0, 0, 0, true),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 0.0f,
                BackgroundTextureOpacity2 = 0.0f,
                TextFunc = () => Percentage.ToString("F0"),
                HintFunc = () => "durability left in %",
            };

            GreenDecorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 9, 255, 100, 130, 100, true, false, 255, 0, 0, 0, true),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 0.0f,
                BackgroundTextureOpacity2 = 0.0f,
                TextFunc = () => Percentage > 98 ? String.Empty : Percentage.ToString("F0"),
                HintFunc = () => "durability left in %",
            };
            
            F3Decorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 30, 255, 255, 100, 100, true, false, 255, 0, 0, 0, true),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 0.0f,
                BackgroundTextureOpacity2 = 0.0f,
                TextFunc = () => "Press F3",
            };
            
            F3RepairDecorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 25, 255, 255, 100, 100, true, false, 255, 0, 0, 0, true),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 0.0f,
                BackgroundTextureOpacity2 = 0.0f,
                TextFunc = () => "Repair & Press F3",
            };
        }

        public void PaintTopInGame(ClipState clipState)
        {
            
            if (Hud.Render.UiHidden) return;
            // if (clipState != ClipState.BeforeClip) return;
            if ((Hud.Game.MapMode == MapMode.WaypointMap) || (Hud.Game.MapMode == MapMode.ActMap) || (Hud.Game.MapMode == MapMode.Map)) return;
            
              var uiRect = Hud.Render.InGameBottomHudUiElement.Rectangle;

              decimal TotalCurrentDurability = 0;
              decimal TotalMaxDurability = 0;
              
              var Items = Hud.Game.Items.Where(i => i.Location == ItemLocation.Head || i.Location == ItemLocation.Torso || i.Location == ItemLocation.Torso || i.Location == ItemLocation.RightHand || i.Location == ItemLocation.LeftHand || i.Location == ItemLocation.Hands || i.Location == ItemLocation.Waist || i.Location == ItemLocation.Feet || i.Location == ItemLocation.Shoulders || i.Location == ItemLocation.Legs || i.Location == ItemLocation.Bracers || i.Location == ItemLocation.LeftRing || i.Location == ItemLocation.RightRing || i.Location == ItemLocation.Neck);
              foreach (var Item in Items)
              {
               var ObjectCurrentDurability = Item.StatList.FirstOrDefault(i => i.Id.Contains("Durability_Cur"));
               var ObjectMaxDurability = Item.StatList.FirstOrDefault(i => i.Id.Contains("Durability_Max"));
               var CurrentDurability = (double)ObjectCurrentDurability.Value;
               var MaxDurability = (double)ObjectMaxDurability.Value;
               TotalCurrentDurability = TotalCurrentDurability + (decimal)CurrentDurability;
               TotalMaxDurability = TotalMaxDurability + (decimal)MaxDurability;
              }

               if (TotalMaxDurability == 0) TotalMaxDurability = 1;
               Percentage = (TotalCurrentDurability / TotalMaxDurability)* 100;
               
               var decorator = Percentage < 21 ? RedDecorator : Percentage < 51 ? YellowDecorator : GreenDecorator;
               
                decorator.Paint((uiRect.Left + uiRect.Width * 0.640f) + (uiRect.Width * 0.002f), uiRect.Top + uiRect.Height * 0.66f, uiRect.Width * 0.024f, uiRect.Height * 0.12f, HorizontalAlign.Center);
               
               var BlackSmith = Hud.Game.Actors.Where(x =>  x.SnoActor.Sno == ActorSnoEnum._pt_blacksmith && x.FloorCoordinate.XYDistanceTo(Hud.Game.Me.FloorCoordinate) <= 22); // 56947
               
               bool Indestructible = false;
               var Indes = Hud.Game.Me.Powers.GetBuff(318858);
               if (Indes == null || !Indes.Active) {Indestructible = false;} else {Indestructible = true;}
               
               if (Oldpercentage != Percentage)
                {
                 F3pressed = true;
                 Oldpercentage = Percentage;
                }
                else
                {
                 if (Hud.Game.Me.IsDead)
                  {
                   if (!F3pressed && !Indestructible) F3Decorator.Paint(Hud.Window.CursorX, Hud.Window.CursorY, 50, 50, HorizontalAlign.Center);
                  }
                 else if (clipState == ClipState.Inventory && BlackSmith.Any())
                  {
                  if (TotalMaxDurability == TotalCurrentDurability && TotalCurrentDurability != 0) {}
                   else if (!F3pressed) F3RepairDecorator.Paint(Hud.Window.CursorX, Hud.Window.CursorY, 50, 50, HorizontalAlign.Center);
                  }
                 else  F3pressed = false;
                }
        }
    }
}
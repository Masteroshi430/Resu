// https://github.com/User5981/Resu
// Urshi's gift plugin for TurboHUD version 02/10/2019 07:44

using System;
using System.Globalization;
using System.Linq;
using Turbo.Plugins.Default;
using System.Collections.Generic;

namespace Turbo.Plugins.Resu
{
    public class UrshisGiftPlugin : BasePlugin, IInGameTopPainter
    {
        public bool InventoryNumbers { get; set; }
        public bool HoveredNumbers { get; set; }

        public IBrush ShadowBrush { get; set; }
        public IBrush InventoryLockBorderBrush { get; set; }
        public IFont GRupgradeChanceFont { get; set; }
        public IFont GRupgradeNoChanceFont { get; set; }
        public int GRlevel { get; set; }
        public int ChanceWantedPercentage { get; set; }
        public int NumberOfAttempts { get; set; }
        public int AddPerc
        {
            get
            {
                if (ChanceWantedPercentage == 100)
                    return 10;
                if (ChanceWantedPercentage == 90)
                    return 9;
                if (ChanceWantedPercentage == 80)
                    return 8;
                if (ChanceWantedPercentage == 70)
                    return 7;
                if (ChanceWantedPercentage == 60)
                    return 0;
                if (ChanceWantedPercentage == 30)
                    return -1;
                if (ChanceWantedPercentage == 15)
                    return -2;
                if (ChanceWantedPercentage == 8)
                    return -3;
                if (ChanceWantedPercentage == 4)
                    return -4;
                if (ChanceWantedPercentage == 2)
                    return -5;
                if (ChanceWantedPercentage == 1)
                    return -15;

                return 500;
            }
        }

        private readonly HashSet<int> Chances = new HashSet<int> { 100, 90, 80, 70, 60, 30, 15, 8, 4, 2, 1 };
        private readonly List<string> Slots = new List<string> {"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow0._item0.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow0._item1.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow0._item2.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow0._item3.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow0._item4.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow1._item10.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow1._item6.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow1._item7.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow1._item8.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow1._item9.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow2._item12.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow2._item13.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow2._item14.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow2._item15.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow2._item16.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow3._item18.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow3._item19.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow3._item20.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow3._item21.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow3._item22.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow4._item24.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow4._item25.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow4._item26.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow4._item27.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow4._item28.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow5._item30.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow5._item31.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow5._item32.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow5._item33.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow5._item34.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow6._item36.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow6._item37.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow6._item38.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow6._item39.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow6._item40.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow7._item42.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow7._item43.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow7._item44.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow7._item45.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow7._item46.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow8._item48.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow8._item49.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow8._item50.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow8._item51.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow8._item52.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow9._item54.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow9._item55.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow9._item56.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow9._item57.Item",
"Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content._stackpanel._tilerow9._item58.Item"};

        public Func<float> LeftFunc { get; set; }
        public Func<float> TopFunc { get; set; }

        public IFont UpgradeFont { get; set; }
        public string UpgradeText { get; set; }
        public string UpgradeTextMax { get; set; }
        public string UpgradeTextOne { get; set; }
        public string LGemName { get; set; }

        public UrshisGiftPlugin()
        {
            Enabled = true;

            InventoryNumbers = true;        //show numbers in inventory, stash
            HoveredNumbers = true;          //show upgrade hint on item hovered
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            ChanceWantedPercentage = 100;
            NumberOfAttempts = 3;


            ShadowBrush = Hud.Render.CreateBrush(175, 0, 0, 0, -1.6f);
            GRupgradeChanceFont = Hud.Render.CreateFont("arial", 7, 255, 0, 0, 0, true, false, false);
            GRupgradeChanceFont.SetShadowBrush(128, 39, 229, 224, true);


            GRupgradeNoChanceFont = Hud.Render.CreateFont("arial", 7, 255, 0, 0, 0, true, false, false);
            GRupgradeNoChanceFont.SetShadowBrush(128, 255, 0, 0, true);

            LeftFunc = () =>
            {
                var uicMain = Hud.Inventory.GetHoveredItemMainUiElement();
                return uicMain.Rectangle.X + uicMain.Rectangle.Width * 0.23f;
            };
            TopFunc = () =>
            {
                var uicTop = Hud.Inventory.GetHoveredItemTopUiElement();
                return uicTop.Rectangle.Bottom + (40f / 1200.0f * Hud.Window.Size.Height);
            };

            UpgradeFont = Hud.Render.CreateFont("tahoma", 7, 255, 200, 200, 200, false, false, false);
            UpgradeTextOne = "{2}GR {0}+ for one attempt at a {1}% upgrade chance";
            UpgradeText = "{3}GR {0}+ for {1} attempts at a {2}% upgrade chance";
            UpgradeTextMax = "{0}Max level!";
        }

        private int stashTabAbs;

        public void PaintTopInGame(ClipState clipState)
        {
            if (!Chances.Contains(ChanceWantedPercentage))
                return;      //false settings
            if (NumberOfAttempts < 1 || NumberOfAttempts > 5)
                return;   //false settings

            var UpgradeGemPanel =  Hud.Render.GetUiElement("Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content");
            if (UpgradeGemPanel.Visible)
             {
                var UpgradeRect = UpgradeGemPanel.Rectangle;

                foreach (var Slot in Slots)
                {
                 var datSlot = Hud.Render.GetUiElement(Slot);
                 var Item = Hud.Game.Items.FirstOrDefault(x => x.AcdId == datSlot.LegendaryGemAcdId);
                 if (Item == null || !datSlot.Visible) continue;
                 else DrawItemChancePercentage(Item, datSlot.Rectangle);
                }
            }


            if (clipState == ClipState.Inventory && InventoryNumbers)
            {
                stashTabAbs = Hud.Inventory.SelectedStashTabIndex + Hud.Inventory.SelectedStashPageIndex * Hud.Inventory.MaxStashTabCountPerPage;

                foreach (var item in Hud.Game.Items)
                {
                    if (item.SnoItem.MainGroupCode != "gems_unique")
                        continue;
                    if (item.Location == ItemLocation.Stash)
                    {
                        if ((item.InventoryY / 10) != stashTabAbs)
                            continue;
                    }
                    if ((item.InventoryX < 0) || (item.InventoryY < 0))
                        continue;

                    var rect = Hud.Inventory.GetItemRect(item);
                    if (rect == System.Drawing.RectangleF.Empty)
                        continue;

                    DrawItemGRupgradeChance(item, rect);
                }
            }

            if (clipState == ClipState.AfterClip && HoveredNumbers)
                DrawTextLine();


        }

        private bool IsMaxedGem(IItem item, int jewelRank)
        {
            switch (item.SnoItem.Sno)
            {
                case 3248762926: // 3248762926 - Gogok of Swiftness
                    return jewelRank == 150;
                case 3249876973: // 3249876973 - Esoteric Alteration
                case 3249984784: // 3249984784 - Mutilation Guard
                    return jewelRank == 100;
                case 3250883209: // 3250883209 - Iceblink 
                case 3249805099: // 3249805099 - Boon of the Hoarder
                    return jewelRank == 50;
                case 3250955083: // 3250955083 - Legacy of dreams
                    return jewelRank == 99;
                default:
                    return false;
            }
        }

        private void DrawItemGRupgradeChance(IItem item, System.Drawing.RectangleF rect)
        {
            var jewelRank = item.JewelRank;
            if (jewelRank == -1)
            { jewelRank = 0; }
            bool Max = IsMaxedGem(item, jewelRank);

            if (Max)
            {
                var layout = GRupgradeChanceFont.GetTextLayout("max");
                GRupgradeChanceFont.DrawText(layout, rect.Right - layout.Metrics.Width - 3, rect.Bottom - layout.Metrics.Height - 3);

                return;
            }

            else
            {

                GRlevel = jewelRank + AddPerc + (NumberOfAttempts - 1);
                if (GRlevel < 1)
                { GRlevel = 1; }
                var text = GRlevel.ToString("D", CultureInfo.InvariantCulture);
                var layout = GRupgradeChanceFont.GetTextLayout(text);
                GRupgradeChanceFont.DrawText(layout, rect.Right - layout.Metrics.Width - 3, rect.Bottom - layout.Metrics.Height - 3);

            }
        }

        private void DrawItemChancePercentage(IItem item, System.Drawing.RectangleF rect)
        {
            var jewelRank = item.JewelRank;
            if (jewelRank == -1)
            { jewelRank = 0; }
            bool Max = IsMaxedGem(item, jewelRank);
            var UpgradeGemPanelRect = Hud.Render.GetUiElement("Root.NormalLayer.vendor_dialog_mainPage.riftReward_dialog.LayoutRoot.gemUpgradePane.items_list._content").Rectangle;

           if (Max)
            {
                var layout = GRupgradeNoChanceFont.GetTextLayout("max");
                if ((rect.Bottom - layout.Metrics.Height - 3) > UpgradeGemPanelRect.Top && (rect.Bottom - layout.Metrics.Height - 3) < UpgradeGemPanelRect.Bottom)
                 {
                  GRupgradeNoChanceFont.DrawText(layout, rect.Right - layout.Metrics.Width - 3, rect.Bottom - layout.Metrics.Height - 3);
                 }
                return;
            }

            else
            {
                uint DatGRlevel = Hud.Game.Me.InGreaterRiftRank;
                int Diff = (int)(DatGRlevel - jewelRank);

                if (Diff > 10) Diff = 10;
                else if (Diff < -7) Diff = -7;

                string Percentage = string.Empty;

                switch (Diff)
                {
                    case 10:
                        Percentage = "100%";
                        break;
                    case 9:
                        Percentage = "90%";
                        break;
                    case 8:
                        Percentage = "80%";
                        break;
                    case 7:
                        Percentage = "70%";
                        break;
                    case 6:
                    case 5:
                    case 4:
                    case 3:
                    case 2:
                    case 1:
                    case 0:
                        Percentage = "60%";
                        break;
                    case -1:
                        Percentage = "30%";
                        break;
                    case -2:
                        Percentage = "15%";
                        break;
                    case -3:
                        Percentage = "8%";
                        break;
                    case -4:
                        Percentage = "4%";
                        break;
                    case -5:
                        Percentage = "2%";
                        break;
                    case -6:
                        Percentage = "1%";
                        break;
                    case -7:
                        Percentage = "0%";
                        break;
                }

                if (Percentage == "0%")
                 {
                  var layout = GRupgradeNoChanceFont.GetTextLayout(Percentage);
                  if ((rect.Bottom - layout.Metrics.Height - 3) > UpgradeGemPanelRect.Top && (rect.Bottom - layout.Metrics.Height - 3) < UpgradeGemPanelRect.Bottom)
                   {
                    GRupgradeNoChanceFont.DrawText(layout, rect.Right - layout.Metrics.Width - 3, rect.Bottom - layout.Metrics.Height - 3);
                   }
                  }
                 else
                  {
                   var layout = GRupgradeChanceFont.GetTextLayout(Percentage);
                   if ((rect.Bottom - layout.Metrics.Height - 3) > UpgradeGemPanelRect.Top && (rect.Bottom - layout.Metrics.Height - 3) < UpgradeGemPanelRect.Bottom)
                    {
                     GRupgradeChanceFont.DrawText(layout, rect.Right - layout.Metrics.Width - 3, rect.Bottom - layout.Metrics.Height - 3);
                    }
                  }
            }
        } 

        private void DrawTextLine()
        {
            var item = Hud.Inventory.HoveredItem;
            if (item == null)
                return;
            var jewelRank = item.JewelRank;
            if (item.ItemsInSocket == null)
            {
                if (!item.IsLegendary)
                    return;
                if (item.SnoItem.MainGroupCode != "gems_unique")
                    return;
                LGemName = "";
            }
            else
            {
                var legendaryGem = item.ItemsInSocket.FirstOrDefault(x => x.Quality == ItemQuality.Legendary && x.JewelRank > -1);
                if (legendaryGem == null)
                    return;
                jewelRank = legendaryGem.JewelRank;

                if (item.SnoItem.MainGroupCode == "2h" || item.SnoItem.MainGroupCode == "1h")
                {
                    if (!item.IsLegendary || item.SetSno != uint.MaxValue)
                    {
                        LGemName = item.AncientRank + "GoE: ";
                    }
                    else
                    {
                        LGemName = Environment.NewLine + "GoE: ";
                    }
                }
                else if (item.SnoItem.MainGroupCode == "helm")
                {
                    if (item.AncientRank < 1 || item.SetSno != uint.MaxValue)
                    {
                        LGemName = "RSS: ";
                    }
                    else
                    {
                        LGemName = Environment.NewLine + "RSS: ";
                    }

                }
                else if (item.AncientRank < 1 || item.SetSno != uint.MaxValue)
                {
                    LGemName = legendaryGem.SnoItem.NameLocalized + ":" + Environment.NewLine;
                }
                else
                {
                    LGemName = Environment.NewLine + legendaryGem.SnoItem.NameLocalized + ":" + Environment.NewLine;
                }


                if (jewelRank == -1)
                    jewelRank = 0;
                bool Max = IsMaxedGem(legendaryGem, jewelRank);

                if (Max)
                {
                    var textMax = string.Format(UpgradeTextMax, LGemName);
                    var layoutMax = UpgradeFont.GetTextLayout(textMax);
                    UpgradeFont.DrawText(layoutMax, LeftFunc(), TopFunc());

                    return;
                }
                else
                {
                    GRlevel = jewelRank + AddPerc + (NumberOfAttempts - 1);
                    if (GRlevel < 1)
                    { GRlevel = 1; }
                    if (NumberOfAttempts == 1)
                    {
                        var textOne = string.Format(UpgradeTextOne, GRlevel, ChanceWantedPercentage, LGemName);
                        var layoutOne = UpgradeFont.GetTextLayout(textOne);
                        UpgradeFont.DrawText(layoutOne, LeftFunc(), TopFunc());

                    }
                    else
                    {
                        var text = string.Format(UpgradeText, GRlevel, NumberOfAttempts, ChanceWantedPercentage, LGemName);
                        var layout = UpgradeFont.GetTextLayout(text);
                        UpgradeFont.DrawText(layout, LeftFunc(), TopFunc());
                    }
                }

            }

        }

    }
}
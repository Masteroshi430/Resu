// https://github.com/User5981/Resu
// Deluxe Inventory Free Space plugin for TurboHUD version 25/07/2019 20:17
// It's the default Inventory Free Space plugin with new features 

using Turbo.Plugins.Default;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Turbo.Plugins.Resu
{

    public class DeluxeInventoryFreeSpacePlugin : BasePlugin, IInGameTopPainter, ICustomizer, IItemPickedHandler
    {

        public TopLabelDecorator RedDecorator { get; set; }
        public TopLabelDecorator YellowDecorator { get; set; }
        public TopLabelDecorator GreenDecorator { get; set; }
        public TopLabelDecorator RedDecoratorTwo { get; set; }
        public TopLabelDecorator YellowDecoratorTwo { get; set; }
        public TopLabelDecorator GreenDecoratorTwo { get; set; }
        public TopLabelDecorator BloodRedDecorator { get; set; }
        public TopLabelDecorator BloodYellowDecorator { get; set; }
        public TopLabelDecorator BloodGreenDecorator { get; set; }
        public bool ShowRemaining { get; set; }
        public float SquareSide { get; set; }
        public int freeSpaceTwo { get; set; }
        public Dictionary<string,string> InventorySlots;
        public bool InventoryOpen { get; set; }
        public int CachesCount { get; set; }
        public int CachesLoopCount { get; set; }
        public string HeroName { get; set; }
        public bool Go { get; set; }

        public DeluxeInventoryFreeSpacePlugin()
        {
            Enabled = true;
        }

        public override void Load(IController hud)
        {
            base.Load(hud);
            
            HeroName = string.Empty;

            string textFunc()
            {
                return (ShowRemaining ? 500 + (Hud.Game.Me.HighestSoloRiftLevel * 10) - Hud.Game.Me.Materials.BloodShard : Hud.Game.Me.Materials.BloodShard).ToString("D", CultureInfo.InvariantCulture);
            }

            string hintFunc()
            {
                return ShowRemaining ? "amount of blood shards remaining" : "amount of blood shards";
            }

            InventorySlots = new Dictionary<string,string>
            {
             {"C0R0", string.Empty}, {"C0R1", string.Empty}, {"C0R2", string.Empty}, {"C0R3", string.Empty}, {"C0R4", string.Empty}, {"C0R5", string.Empty},
             {"C1R0", string.Empty}, {"C1R1", string.Empty}, {"C1R2", string.Empty}, {"C1R3", string.Empty}, {"C1R4", string.Empty}, {"C1R5", string.Empty},
             {"C2R0", string.Empty}, {"C2R1", string.Empty}, {"C2R2", string.Empty}, {"C2R3", string.Empty}, {"C2R4", string.Empty}, {"C2R5", string.Empty},
             {"C3R0", string.Empty}, {"C3R1", string.Empty}, {"C3R2", string.Empty}, {"C3R3", string.Empty}, {"C3R4", string.Empty}, {"C3R5", string.Empty},
             {"C4R0", string.Empty}, {"C4R1", string.Empty}, {"C4R2", string.Empty}, {"C4R3", string.Empty}, {"C4R4", string.Empty}, {"C4R5", string.Empty},
             {"C5R0", string.Empty}, {"C5R1", string.Empty}, {"C5R2", string.Empty}, {"C5R3", string.Empty}, {"C5R4", string.Empty}, {"C5R5", string.Empty},
             {"C6R0", string.Empty}, {"C6R1", string.Empty}, {"C6R2", string.Empty}, {"C6R3", string.Empty}, {"C6R4", string.Empty}, {"C6R5", string.Empty},
             {"C7R0", string.Empty}, {"C7R1", string.Empty}, {"C7R2", string.Empty}, {"C7R3", string.Empty}, {"C7R4", string.Empty}, {"C7R5", string.Empty},
             {"C8R0", string.Empty}, {"C8R1", string.Empty}, {"C8R2", string.Empty}, {"C8R3", string.Empty}, {"C8R4", string.Empty}, {"C8R5", string.Empty},
             {"C9R0", string.Empty}, {"C9R1", string.Empty}, {"C9R2", string.Empty}, {"C9R3", string.Empty}, {"C9R4", string.Empty}, {"C9R5", string.Empty}
            };

            RedDecorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 100, 100, true, false, 255, 0, 0, 0, true),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 1.0f,
                BackgroundTextureOpacity2 = 1.0f,
                TextFunc = () => "\u25A0 " + (Hud.Game.Me.InventorySpaceTotal - Hud.Game.InventorySpaceUsed).ToString("D", CultureInfo.InvariantCulture),
                HintFunc = () => "free space in inventory for one slot items",
            };

            YellowDecorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 7, 255, 200, 205, 50, true, false, false),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 1.0f,
                BackgroundTextureOpacity2 = 1.0f,
                TextFunc = () => "\u25A0 " + (Hud.Game.Me.InventorySpaceTotal - Hud.Game.InventorySpaceUsed).ToString("D", CultureInfo.InvariantCulture),
                HintFunc = () => "free space in inventory for one slot items",
            };

            GreenDecorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 7, 255, 100, 130, 100, false, false, false),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 1.0f,
                BackgroundTextureOpacity2 = 1.0f,
                TextFunc = () => "\u25A0 " + (Hud.Game.Me.InventorySpaceTotal - Hud.Game.InventorySpaceUsed).ToString("D", CultureInfo.InvariantCulture),
                HintFunc = () => "free space in inventory for one slot items",
            };
            
            RedDecoratorTwo = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 7, 255, 255, 100, 100, true, false, 255, 0, 0, 0, true),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 1.0f,
                BackgroundTextureOpacity2 = 1.0f,
                TextFunc = () => "\u2588 " + freeSpaceTwo.ToString("D", CultureInfo.InvariantCulture),
                HintFunc = () => "free space in inventory for two slot items",
            };

            YellowDecoratorTwo = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 6, 255, 200, 205, 50, true, false, false),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 1.0f,
                BackgroundTextureOpacity2 = 1.0f,
                TextFunc = () => "\u2588 " + freeSpaceTwo.ToString("D", CultureInfo.InvariantCulture),
                HintFunc = () => "free space in inventory for two slot items",
            };

            GreenDecoratorTwo = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 6, 255, 100, 130, 100, false, false, false),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 1.0f,
                BackgroundTextureOpacity2 = 1.0f,
                TextFunc = () => freeSpaceTwo == int.MaxValue ? "\u25AE ?" : "\u25AE" + freeSpaceTwo.ToString("D", CultureInfo.InvariantCulture),
                HintFunc = () => "free space in inventory for two slot items",
            };

            BloodRedDecorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 8, 255, 255, 100, 100, true, false, 255, 0, 0, 0, true),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 1.0f,
                BackgroundTextureOpacity2 = 1.0f,
                TextFunc = textFunc,
                HintFunc = hintFunc,
            };

            BloodYellowDecorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 7, 255, 200, 205, 50, true, false, false),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 1.0f,
                BackgroundTextureOpacity2 = 1.0f,
                TextFunc = textFunc,
                HintFunc = hintFunc,
            };

            BloodGreenDecorator = new TopLabelDecorator(Hud)
            {
                TextFont = Hud.Render.CreateFont("tahoma", 7, 255, 100, 130, 100, false, false, false),
                BackgroundTexture1 = Hud.Texture.ButtonTextureGray,
                BackgroundTexture2 = Hud.Texture.BackgroundTextureOrange,
                BackgroundTextureOpacity1 = 1.0f,
                BackgroundTextureOpacity2 = 1.0f,
                TextFunc = textFunc,
                HintFunc = hintFunc,
            };
        }

        public void PaintTopInGame(ClipState clipState)
        {
            if (Hud.Render.UiHidden) return;
            // if (clipState != ClipState.BeforeClip) return;
            if ((Hud.Game.MapMode == MapMode.WaypointMap) || (Hud.Game.MapMode == MapMode.ActMap) || (Hud.Game.MapMode == MapMode.Map)) return;
            
              
            var uiRect = Hud.Render.InGameBottomHudUiElement.Rectangle;
            var freeSpace = Hud.Game.Me.InventorySpaceTotal - Hud.Game.InventorySpaceUsed;

            var BloodRemaining = 500 + (Hud.Game.Me.HighestSoloRiftLevel * 10) - Hud.Game.Me.Materials.BloodShard;

            var BloodDecorator = BloodRemaining < 100 ? BloodRedDecorator : (BloodRemaining < 200 ? BloodYellowDecorator : BloodGreenDecorator);
            BloodDecorator.Paint(uiRect.Left + (uiRect.Width * 0.647f), uiRect.Top + (uiRect.Height * 0.88f), uiRect.Width * 0.038f, uiRect.Height * 0.12f, HorizontalAlign.Center);
           // BloodDecorator.Paint(uiRect.Left + (uiRect.Width * 0.664f), uiRect.Top + (uiRect.Height * 0.88f), uiRect.Width * 0.038f, uiRect.Height * 0.12f, HorizontalAlign.Center);
            var itemSno = 2603730171;
            var BloodShard = Hud.Inventory.GetSnoItem(itemSno);
            var texture = Hud.Texture.GetItemTexture(BloodShard);
            var remaining = 500 + (Hud.Game.Me.HighestSoloRiftLevel * 10) - Hud.Game.Me.Materials.BloodShard;
            if (remaining < 100) ;
            else if (Hud.Game.Me.Materials.BloodShard < 1000)
                texture.Draw(uiRect.Left + (uiRect.Width * 0.648f), uiRect.Top + (uiRect.Height * 0.90f), uiRect.Height * 0.09f, uiRect.Height * 0.09f, 0.5f);
            else
                texture.Draw(uiRect.Left + (uiRect.Width * 0.6475f), uiRect.Top + (uiRect.Height * 0.90f), uiRect.Height * 0.08f, uiRect.Height * 0.08f, 0.5f);


            if (HeroName != Hud.Game.Me.HeroName)
              {
               freeSpaceTwo = int.MaxValue;
               foreach (var key in InventorySlots.Keys.ToList()) // empty dictionary values
                {
                 InventorySlots[key] = string.Empty;
                }
               HeroName = Hud.Game.Me.HeroName;
               Go = false;
              }
            if (clipState == ClipState.Inventory) Go = true;
            if (!Go) goto SwitchCharacter;
            
            var ContainerRect = Hud.Inventory.InventoryItemsUiElement.Rectangle;

            var ItemCheck = Hud.Game.Items.FirstOrDefault(i => i.Location == ItemLocation.Inventory);
            if (ItemCheck == null)
             {
              freeSpaceTwo = int.MaxValue;
             }
            else
             {
              var Rect = Hud.Inventory.GetItemRect(ItemCheck);
              SquareSide = Rect.Width;
             }
            
            if (SquareSide == 0f || ContainerRect == null) 
             {
              freeSpaceTwo = int.MaxValue;
             }
            else if (clipState != ClipState.Inventory)
             {
              InventoryOpen = false;
             }
            else if (clipState == ClipState.Inventory)
             {
              InventoryOpen = true;
              var FirstSquareTop = ContainerRect.Top;
              var FirstSquareLeft = ContainerRect.Left;
              
              var Items = Hud.Game.Items.Where(i => i.Location == ItemLocation.Inventory);
              
              foreach (var key in InventorySlots.Keys.ToList()) // empty dictionary values before filling
              {
               InventorySlots[key] = string.Empty;
              }
              
              foreach (var Item in Items)
              {
                var ItemRect = Hud.Inventory.GetItemRect(Item);
               
                for (var c = 0; c < 10; c++) // 10 columns
                 {
                   for (var r = 0; r < 6; r++) // 6 rows
                    {
                     var DatSquareTop = FirstSquareTop + (SquareSide * r);
                     var DatSquareLeft = FirstSquareLeft + (SquareSide * c);
                     
                     
                     if ( Math.Abs(ItemRect.Height - SquareSide) < 1)
                      {
                       if ( Math.Abs(ItemRect.Top - DatSquareTop) < 4 && Math.Abs(ItemRect.Left - DatSquareLeft) < 4) //populate inventory slot
                        {
                         var DatKey = "C" + c + "R" + r;
                         InventorySlots[DatKey] = Item.SnoItem.Sno.ToString() + Item.CreatedAtInGameTick.ToString(); //Item.ItemUniqueId;
                        }
                       
                      }
                     else
                      {
                       if (Math.Abs(ItemRect.Top - DatSquareTop) < 4 && Math.Abs(ItemRect.Left - DatSquareLeft) < 4) //populate 2 inventory slots
                        {
                         var DatKey = "C" + c + "R" + r;
                         InventorySlots[DatKey] = Item.SnoItem.Sno.ToString() + Item.CreatedAtInGameTick.ToString(); //Item.ItemUniqueId;
                         var DatKeyTwo = "C" + c + "R" + (r+1);
                         InventorySlots[DatKeyTwo] = Item.SnoItem.Sno.ToString() + Item.CreatedAtInGameTick.ToString(); //Item.ItemUniqueId;
                        }
                      }
                    }
                 }
              }
             }
             
            if (SquareSide != 0f && ContainerRect != null) // calculate the freespacetwo value
             {
              var TwoSlotsCount = 0;
              for (var c = 0; c < 10; c++) // 10 columns
                 {
                   for (var r = 0; r < 6; r++) // 6 rows
                    {
                         var DatKey = "C" + c + "R" + r;
                         var DatValue = InventorySlots[DatKey];
                         if (DatValue == string.Empty && r < 5)
                          {
                           var DatKeyTwo = "C" + c + "R" + (r+1);
                           var DatValueTwo = InventorySlots[DatKeyTwo];
                           if (DatValueTwo == string.Empty)
                            {
                             TwoSlotsCount++;
                             r++;
                            }
                          }
                    }
                 }
                 
                freeSpaceTwo = TwoSlotsCount;
                
                //  Horadric Cache Workaround
                if (clipState != ClipState.Inventory)
                 {
                  CachesLoopCount = 0;
                  var HoradricCaches = Hud.Inventory.ItemsInInventory.Where(x => x.SnoItem.MainGroupCode == "horadriccache" );
                  foreach (var item in HoradricCaches)
                   {
                    CachesLoopCount++;
                   }
                 
                  CachesLoopCount = Math.Abs(CachesLoopCount);
                 
                  if (CachesLoopCount > CachesCount)
                   {
                    var HoradricCachesToAdd = CachesLoopCount - CachesCount;
                    for (var h = 0; h < HoradricCachesToAdd; h++)
                     {   
                       for (var r = 0; r < 5; r++) // 5 rows (we don't need the last one)
                        {
                          for (var c = 0; c < 10; c++) // 10 columns
                           {
                            var DatKey = "C" + c + "R" + r;
                            var DatValue = InventorySlots[DatKey];
                            var DatKeyTwo = "C" + c + "R" + (r+1);
                            var DatValueTwo = InventorySlots[DatKeyTwo];
                            if (DatValue == string.Empty && DatValueTwo == string.Empty)
                             {
                              InventorySlots[DatKey] = "HoradricWorkaroundFromHell";
                              InventorySlots[DatKeyTwo] = "HoradricWorkaroundFromHell";
                              goto OuterFromHell;
                             }
                           }
                        }
                       OuterFromHell:;
                     }
                   }
                  
                 }

             }
             CachesCount = CachesLoopCount;
             
            SwitchCharacter:;
            var decorator = freeSpace < 2 ? RedDecorator : freeSpace < 20 ? YellowDecorator : GreenDecorator;
            var decoratorTwo = freeSpaceTwo < 2 ? RedDecoratorTwo : freeSpaceTwo < 10 ? YellowDecoratorTwo : GreenDecoratorTwo;
            
            decorator.Paint(uiRect.Left + (uiRect.Width * 0.595f), uiRect.Top + (uiRect.Height * 0.88f), uiRect.Width * 0.028f, uiRect.Height * 0.12f, HorizontalAlign.Center);
            decoratorTwo.Paint(uiRect.Left + (uiRect.Width * 0.625f), uiRect.Top + (uiRect.Height * 0.88f), uiRect.Width * 0.021f, uiRect.Height * 0.12f, HorizontalAlign.Center);

        }
        
        public void Customize()
        {
            Hud.TogglePlugin<InventoryFreeSpacePlugin>(false);
            Hud.TogglePlugin<BloodShardPlugin>(false);
        }
        
        public void OnItemPicked(IItem item)
        {
         if (!InventoryOpen)
         {
                var ItemID = item.SnoItem.Sno.ToString() + item.CreatedAtInGameTick.ToString();
           if (item.SnoItem.MainGroupCode == "helm" || item.SnoItem.MainGroupCode == "chestarmor" ||
               item.SnoItem.MainGroupCode == "gloves" || item.SnoItem.MainGroupCode == "boots" || item.SnoItem.MainGroupCode == "shoulders" ||
               item.SnoItem.MainGroupCode == "pants" || item.SnoItem.MainGroupCode == "bracers" ||item.SnoItem.MainGroupCode == "crusadershield" ||
               item.SnoItem.MainGroupCode == "quiver" || item.SnoItem.MainGroupCode == "source" ||item.SnoItem.MainGroupCode == "mojo" ||
               item.SnoItem.MainGroupCode == "shield" || item.SnoItem.MainGroupCode == "necromanceroffhand" ||item.SnoItem.MainGroupCode == "1h" ||
               item.SnoItem.MainGroupCode == "2h") // 2 slot items
            {
             for (var r = 0; r < 5; r++) // 5 rows (we don't need the last one)
                 {
                   for (var c = 0; c < 10; c++) // 10 columns
                    {
                            var DatKey = "C" + c + "R" + r;
                            var DatValue = InventorySlots[DatKey];
                            var DatKeyTwo = "C" + c + "R" + (r+1);
                            var DatValueTwo = InventorySlots[DatKeyTwo];
                     if (DatValue == string.Empty && DatValueTwo == string.Empty)
                      {
                       InventorySlots[DatKey] = ItemID;
                       InventorySlots[DatKeyTwo] = ItemID;
                       goto Outer;
                      }
                    }
                 }
            }
           else if (item.SnoItem.MainGroupCode != "horadriccache")// 1 slot item
            {
             for (var c = 0; c < 10; c++) // 10 columns
                 {
                   for (var r = 0; r < 6; r++) // 6 rows
                    {
                            var DatKey = "C" + c + "R" + r;
                            var DatValue = InventorySlots[DatKey];
                     if (DatValue == string.Empty)
                      {
                       InventorySlots[DatKey] = ItemID;
                       goto Outer;
                      }
                    }
                 }
            }
            Outer:;
         }
        }
    }

}
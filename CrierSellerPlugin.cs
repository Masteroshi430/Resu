//css_reference C:\V7.7.1.dll;
// https://github.com/User5981/Resu
// Crier Seller Plugin for TurboHUD version 21/08/2018 08:42
using Turbo.Plugins.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;




namespace Turbo.Plugins.Resu
{

    public class CrierSellerPlugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection SellerDecorator { get; set; }
        public int PrevSeconds { get; set; }
        public int KadalaSentence { get; set; }
        public int JewellerSentence { get; set; }
        public int MysticSentence { get; set; }
        
        public CrierSellerPlugin()
        {
            Enabled = true;
        }

        
        public override void Load(IController hud)
        {
         base.Load(hud);
         
         SellerDecorator = new WorldDecoratorCollection( 
          new GroundLabelDecorator(Hud)
          {
           BackgroundBrush = Hud.Render.CreateBrush(0, 0, 0, 0, 0),
           BorderBrush = Hud.Render.CreateBrush(0, 182, 26, 255, 1),
           TextFont = Hud.Render.CreateFont("arial", 7, 200, 255, 255, 110, true, false, 255, 0, 0, 0, true)
          });
         
         PrevSeconds = 0;
         KadalaSentence = 0;
         JewellerSentence = 0;
         MysticSentence = 0;
         
        }
        
         public void PaintWorld(WorldLayer layer)
         {
           if (!Hud.Game.IsInTown) return;
           if (Hud.Inventory.InventoryMainUiElement.Visible) return;
           var Sellers = Hud.Game.Actors.Where(x => x.SnoActor.Sno == 361241 || x.SnoActor.Sno == 56949 || x.SnoActor.Sno == 56948 || x.SnoActor.Sno == 56947); //  429005 Kulle
           string SellerMessage = string.Empty;
           int Seconds = (int)(Hud.Game.CurrentRealTimeMilliseconds/1000);
           
           
           if (Seconds % 5 == 0 && PrevSeconds != Seconds)
            {
             KadalaSentence++;
             if (KadalaSentence == 5) KadalaSentence = 0;
             JewellerSentence++;
             if (JewellerSentence == 4) JewellerSentence = 0;
             MysticSentence++;
             if (MysticSentence == 5) MysticSentence = 0;
             PrevSeconds = Seconds;
            }
           
           Dictionary<string, long> GemsCount = new Dictionary<string, long>();
           
           var GemsStash = Hud.Inventory.ItemsInStash.Where(x => x.SnoItem.MainGroupCode == "gems" && x.Quantity != 0);
           foreach (var Gem in GemsStash)
           {
            if (Gem == null) continue;
            if (Gem.SnoItem == null) continue;
            if (Gem.SnoItem.NameEnglish == null) continue;
            
            
                        
            string GemNameStash = Gem.SnoItem.NameEnglish;
            long GemQuantityStash = Gem.Quantity;
            
            if (!GemsCount.ContainsKey(GemNameStash)) GemsCount.Add(GemNameStash,GemQuantityStash);
            else 
                {
                 long DictQuantity = 0;    
                 GemsCount.TryGetValue(GemNameStash, out DictQuantity);   
                 GemsCount[GemNameStash] = DictQuantity + GemQuantityStash;    
                } 
           }
           
           var GemsInventory = Hud.Inventory.ItemsInInventory.Where(x => x.SnoItem.MainGroupCode == "gems" && x.Quantity != 0);
           foreach (var Gem in GemsInventory)
           {
            if (Gem == null) continue;
            if (Gem.SnoItem == null) continue;
            if (Gem.SnoItem.NameEnglish == null) continue;
                        
            string GemNameInventory = Gem.SnoItem.NameEnglish;
            long GemQuantityInventory = Gem.Quantity;
            
            if (!GemsCount.ContainsKey(GemNameInventory)) GemsCount.Add(GemNameInventory,GemQuantityInventory);
            else 
                {
                 long DictQuantity2 = 0;    
                 GemsCount.TryGetValue(GemNameInventory, out DictQuantity2);  
                 GemsCount[GemNameInventory] = DictQuantity2 + GemQuantityInventory;    
                } 
              
           }
           
           
           foreach (var Seller in Sellers)
           {
             if (Seller == null) continue;
             if (Seller.SnoActor == null) continue;
             if (Seller.SnoActor.Sno == null) continue;
             if (Seller.FloorCoordinate == null) continue;
             
             if (Seller.SnoActor.Sno == 361241) // Kadala
              {
                string BloodShardsPlural = "s";
                if (Hud.Game.Me.Materials.BloodShard == 1) BloodShardsPlural = string.Empty;
                string BloodShardsAmmount = "- You have " + Hud.Game.Me.Materials.BloodShard + " Blood Shard" + BloodShardsPlural + " out of " + (500 + (Hud.Game.Me.HighestSoloRiftLevel*10)) + ".";
                if (Hud.Game.Me.Materials.BloodShard == 0) BloodShardsAmmount = "You have no Blood Shards.";
                
                int TwentyFive = (int)(Hud.Game.Me.Materials.BloodShard/25);
                string TwentyFivePlural = "s";
                string PhylacteryPlural = "phylacteries ";
                if (TwentyFive == 1) {TwentyFivePlural = string.Empty; PhylacteryPlural = "phylactery ";}
                
                int Fifty = (int)(Hud.Game.Me.Materials.BloodShard/50);
                string FiftyPlural = "s";
                if (Fifty == 1) FiftyPlural = string.Empty;
                
                int SeventyFive = (int)(Hud.Game.Me.Materials.BloodShard/75);
                string SeventyFivePlural = "s";
                if (SeventyFive == 1) SeventyFivePlural = string.Empty;
                
                int Hundred = (int)(Hud.Game.Me.Materials.BloodShard/100);
                string HundredPlural = "s";
                if (Hundred == 1) HundredPlural = string.Empty;
                
                
                string TwentyFiveClass = string.Empty;
                string FiftyClass = string.Empty;
                string SeventyFiveClass = string.Empty;
                string HundredClass = string.Empty;
           
                switch (Hud.Game.Me.HeroClassDefinition.HeroClass.ToString())
                {
                 case "Barbarian" :
                 break;
                 case "DemonHunter" :
                 TwentyFiveClass = "mystery quiver" + TwentyFivePlural + ", " + "helm" + TwentyFivePlural + ", " + "pair of gloves" + "," + Environment.NewLine + "pair of boots" + ", " + "chest armor" + TwentyFivePlural + ", " + "belt" + TwentyFivePlural + ", " + "pair of shoulders" + "," + Environment.NewLine + "pair of pants" + ", " + "pair of bracers" + " or " + "shield" + TwentyFivePlural + "."  ;
                 break;
                 case "Wizard" :
                 TwentyFiveClass = "mystery orb" + TwentyFivePlural + ", " + "helm" + TwentyFivePlural + ", " + "pair of gloves" + "," + Environment.NewLine + "pair of boots" + ", " + "chest armor" + TwentyFivePlural + ", " + "belt" + TwentyFivePlural + ", " + "pair of shoulders" + "," + Environment.NewLine + "pair of pants" + ", " + "pair of bracers" + " or " + "shield" + TwentyFivePlural + "."  ;
                 break;
                 case "WitchDoctor" :
                 TwentyFiveClass = "mystery mojo" + TwentyFivePlural + ", " + "helm" + TwentyFivePlural + ", " + "pair of gloves" + "," + Environment.NewLine + "pair of boots" + ", " + "chest armor" + TwentyFivePlural + ", " + "belt" + TwentyFivePlural + ", " + "pair of shoulders" + "," + Environment.NewLine + "pair of pants" + ", " + "pair of bracers" + " or " + "shield" + TwentyFivePlural + "."  ;
                 break;
                 case "Monk" :
                 case "Crusader" :
                 TwentyFiveClass = "helm" + TwentyFivePlural + ", " + "pair of gloves" + "," + Environment.NewLine + "pair of boots" + ", " + "chest armor" + TwentyFivePlural + ", " + "belt" + TwentyFivePlural + ", " + "pair of shoulders" + "," + Environment.NewLine + "pair of pants" + ", " + "pair of bracers" + " or " + "shield" + TwentyFivePlural + "."  ;
                 break;
                 case "Necromancer" :
                 TwentyFiveClass = "mystery " + PhylacteryPlural + ", " + "helm" + TwentyFivePlural + ", " + "pair of gloves" + "," + Environment.NewLine + "pair of boots" + ", " + "chest armor" + TwentyFivePlural + ", " + "belt" + TwentyFivePlural + ", " + "pair of shoulders" + "," + Environment.NewLine + "pair of pants" + ", " + "pair of bracers" + " or " + "shield" + TwentyFivePlural + "."  ;
                 break;
                }
                
                FiftyClass = "mystery ring" + FiftyPlural + ".";
                SeventyFiveClass = "mystery weapon" + SeventyFivePlural + ".";
                HundredClass = "mystery amulet" + HundredPlural + ".";
                
                string TwentyFiveMessage = string.Empty;
                if (TwentyFive == 0) TwentyFiveMessage = "- There's nothing I can sell you.";
                else if (TwentyFive == 1) TwentyFiveMessage = "- You can get one " + TwentyFiveClass;
                else TwentyFiveMessage = "- You can get " + TwentyFive + " " + TwentyFiveClass;
                
                string FiftyMessage = string.Empty;
                if (Fifty == 0) {FiftyMessage = string.Empty; if (KadalaSentence == 2) KadalaSentence = 0;}
                else if (Fifty == 1) FiftyMessage = "- I have also one " + FiftyClass;
                else FiftyMessage = "- I have also " + Fifty + " " + FiftyClass;
                
                string SeventyFiveMessage = string.Empty;
                if (SeventyFive == 0) {SeventyFiveMessage = string.Empty; if (KadalaSentence == 3) KadalaSentence = 0;}
                else if (SeventyFive == 1) SeventyFiveMessage = "- ... And one " + SeventyFiveClass;
                else SeventyFiveMessage = "- ... And " + SeventyFive + " " + SeventyFiveClass;
                
                string HundredMessage = string.Empty;
                if (Hundred == 0) {HundredMessage = string.Empty; if (KadalaSentence == 4) KadalaSentence = 0;}
                else if (Hundred == 1) HundredMessage = "- ... And to finish: One " + HundredClass;
                else HundredMessage = "- ... And to finish: " + Hundred + " " + HundredClass;
                
               
               switch (KadalaSentence)
                  {
                   case 0:
                   SellerMessage = BloodShardsAmmount;
                   break;
                   
                   case 1:
                   SellerMessage = TwentyFiveMessage;
                   break;
                   
                   case 2:
                   SellerMessage = FiftyMessage;
                   break;
                   
                   case 3:
                   SellerMessage = SeventyFiveMessage;
                   break;
                   
                   case 4:
                   SellerMessage = HundredMessage;
                   break;
                  }
              }
              
             else if (Seller.SnoActor.Sno == 56949) // Jeweler
              {
               string GemSentence = string.Empty;
                              
                               
                foreach (var Gem in GemsCount.OrderByDescending(i => i.Value).Take(1)) 
                {
                 if (Gem.Key.Contains("Marquise") && Gem.Value > 3)
                    {
                     int ImperialNumbers = (int)(Gem.Value/3);
                     string ImperialColor = Gem.Key.Replace("Marquise",string.Empty).Trim();
                     string MarquisePlural = "s";
                     if (Gem.Value == 1) MarquisePlural = string.Empty;
                     string ImperialPlural = "s";
                     if (ImperialNumbers == 1) ImperialPlural = string.Empty;
                     if (ImperialColor == "Ruby" && ImperialNumbers > 1) {ImperialColor = "Rubie";}
                     else if (ImperialColor == "Topaz" && ImperialNumbers > 1) {ImperialColor = "Topaze";}
                     long Gold = (int)(ImperialNumbers*200000);
                     if (Hud.Game.Me.Materials.Gold < Gold) {ImperialNumbers = (int)(Hud.Game.Me.Materials.Gold/200000); Gold = (int)(ImperialNumbers*200000);}
                     string GoldString = Gold.ToString("N0",CultureInfo.InvariantCulture);
                     if (Hud.Game.Me.Materials.Gold >= Gold) GemSentence = "- You have " + Gem.Value + " Marquise " + ImperialColor + MarquisePlural + Environment.NewLine + "  I can combine them into " + ImperialNumbers + " Imperial " + ImperialColor + ImperialPlural + Environment.NewLine + "  for " + GoldString + " gold.";   
                    }
                 else if (Gem.Key.Contains("Flawless Imperial") && Gem.Value > 3)
                    {
                     int RoyalNumbers = (int)(Gem.Value/3);
                     string RoyalColor = Gem.Key.Replace("Fawless Imperial",string.Empty).Trim();
                     string FlawlessImperialPlural = "s";
                     if (Gem.Value == 1) FlawlessImperialPlural = string.Empty;
                     string RoyalPlural = "s";
                     if (RoyalNumbers == 1) RoyalPlural = string.Empty;
                     if (RoyalColor == "Ruby" && RoyalNumbers > 1) {RoyalColor = "Rubie";}
                     else if (RoyalColor == "Topaz" && RoyalNumbers > 1) {RoyalColor = "Topaze";}
                     long DeathBreaths = RoyalNumbers;
                     long Gold = (int)(RoyalNumbers*400000);
                     if (Hud.Game.Me.Materials.Gold < Gold) {RoyalNumbers = (int)(Hud.Game.Me.Materials.Gold/400000); Gold = (int)(RoyalNumbers*400000); DeathBreaths = RoyalNumbers;}
                     if (Hud.Game.Me.Materials.DeathsBreath < DeathBreaths) {RoyalNumbers = (int)(Hud.Game.Me.Materials.DeathsBreath); Gold = (int)(RoyalNumbers*400000); DeathBreaths = RoyalNumbers;}
                     string GoldString = Gold.ToString("N0",CultureInfo.InvariantCulture);
                     
                     
                     if (Hud.Game.Me.Materials.Gold >= Gold && Hud.Game.Me.Materials.DeathsBreath >= DeathBreaths) GemSentence = "- You have " + Gem.Value + " Fawless Imperial " + RoyalColor + FlawlessImperialPlural + Environment.NewLine + "  I can combine them into " + RoyalNumbers + " Royal " + RoyalColor + RoyalPlural + Environment.NewLine + "  for " + GoldString + " gold and " + DeathBreaths + "Death's Breath" + RoyalPlural + ".";   
                    }
                 else if (Gem.Key.Contains("Imperial") && Gem.Value > 3)
                    {
                     int FlawlessImperialNumbers = (int)(Gem.Value/3);
                     string FlawlessImperialColor = Gem.Key.Replace("Imperial",string.Empty).Trim();
                     string ImperialPlural = "s";
                     if (Gem.Value == 1) ImperialPlural = string.Empty;
                     string FlawlessImperialPlural = "s";
                     if (FlawlessImperialNumbers == 1) FlawlessImperialPlural = string.Empty;
                     if (FlawlessImperialColor == "Ruby" && FlawlessImperialNumbers > 1) {FlawlessImperialColor = "Rubie";}
                     else if (FlawlessImperialColor == "Topaz" && FlawlessImperialNumbers > 1) {FlawlessImperialColor = "Topaze";}
                     long Gold = (int)(FlawlessImperialNumbers*300000);
                     if (Hud.Game.Me.Materials.Gold < Gold) {FlawlessImperialNumbers = (int)(Hud.Game.Me.Materials.Gold/300000); Gold = (int)(FlawlessImperialNumbers*300000);}
                     string GoldString = Gold.ToString("N0",CultureInfo.InvariantCulture);
                     
                     if (Hud.Game.Me.Materials.Gold >= Gold) GemSentence = "- You have " + Gem.Value + " Imperial " + FlawlessImperialColor + ImperialPlural + Environment.NewLine + "  I can combine them into " + FlawlessImperialNumbers + " Flawless Imperial " + FlawlessImperialColor + FlawlessImperialPlural + Environment.NewLine + "  for " + GoldString + " gold.";   
                    }
                 else if (Gem.Key.Contains("Royal") && Gem.Value > 3)
                    {
                     int FlawlessRoyalNumbers = (int)(Gem.Value/3);
                     string FlawlessRoyalColor = Gem.Key.Replace("Imperial",string.Empty).Trim();
                     string RoyalPlural = "s";
                     if (Gem.Value == 1) RoyalPlural = string.Empty;
                     string FlawlessRoyalPlural = "s";
                     if (FlawlessRoyalNumbers == 1) FlawlessRoyalPlural = string.Empty;
                     if (FlawlessRoyalColor == "Ruby" && FlawlessRoyalNumbers > 1) {FlawlessRoyalColor = "Rubie";}
                     else if (FlawlessRoyalColor == "Topaz" && FlawlessRoyalNumbers > 1) {FlawlessRoyalColor = "Topaze";}
                     long Gold = (int)(FlawlessRoyalNumbers*500000);
                     long DeathBreaths = FlawlessRoyalNumbers;
                     if (Hud.Game.Me.Materials.Gold < Gold) {FlawlessRoyalNumbers = (int)(Hud.Game.Me.Materials.Gold/500000); Gold = (int)(FlawlessRoyalNumbers*500000); DeathBreaths = FlawlessRoyalNumbers;}
                     if (Hud.Game.Me.Materials.DeathsBreath < DeathBreaths) {FlawlessRoyalNumbers = (int)(Hud.Game.Me.Materials.DeathsBreath); Gold = (int)(FlawlessRoyalNumbers*500000); DeathBreaths = FlawlessRoyalNumbers;}
                     string GoldString = Gold.ToString("N0",CultureInfo.InvariantCulture);
                     
                     
                     if (Hud.Game.Me.Materials.Gold >= Gold && Hud.Game.Me.Materials.DeathsBreath >= DeathBreaths) GemSentence = "- You have " + Gem.Value + " Royal " + FlawlessRoyalColor + RoyalPlural + Environment.NewLine + "  I can combine them into " + FlawlessRoyalNumbers + " Flawless Royal " + FlawlessRoyalColor + FlawlessRoyalPlural + Environment.NewLine + "  for " + GoldString + " gold and " + DeathBreaths + "Death's Breath" + FlawlessRoyalPlural + ".";   
                    }
                }
                if (GemSentence == string.Empty && JewellerSentence == 0) JewellerSentence = 1;
                string HellfireRingSentence = string.Empty;
                string HellfireAmuletSentence = string.Empty;
                if (Hud.Game.Me.Materials.LeoricsRegret > 0 && Hud.Game.Me.Materials.VialOfPutridness > 0 && Hud.Game.Me.Materials.IdolOfTerror > 0 && Hud.Game.Me.Materials.HeartOfFright > 0)
                 {
                   if (Hud.Game.Me.Materials.ForgottenSoul >= 10)
                    {
                      long ForgottenSouls = (int)(Hud.Game.Me.Materials.ForgottenSoul/10);
                      var HellfireAmuletPlural = "s";
                      long HellfireAmuletNumber = 0;
                      if (ForgottenSouls <= Hud.Game.Me.Materials.LeoricsRegret && ForgottenSouls <= Hud.Game.Me.Materials.VialOfPutridness && ForgottenSouls <= Hud.Game.Me.Materials.IdolOfTerror && ForgottenSouls <= Hud.Game.Me.Materials.HeartOfFright)
                       {
                        HellfireAmuletNumber = ForgottenSouls;
                       }
                      else if (Hud.Game.Me.Materials.LeoricsRegret <= ForgottenSouls && Hud.Game.Me.Materials.LeoricsRegret <= Hud.Game.Me.Materials.VialOfPutridness && Hud.Game.Me.Materials.LeoricsRegret <= Hud.Game.Me.Materials.IdolOfTerror && Hud.Game.Me.Materials.LeoricsRegret <= Hud.Game.Me.Materials.HeartOfFright)
                       {
                        HellfireAmuletNumber = Hud.Game.Me.Materials.LeoricsRegret;
                       }
                      else if (Hud.Game.Me.Materials.VialOfPutridness <= ForgottenSouls && Hud.Game.Me.Materials.VialOfPutridness <= Hud.Game.Me.Materials.LeoricsRegret && Hud.Game.Me.Materials.VialOfPutridness <= Hud.Game.Me.Materials.IdolOfTerror && Hud.Game.Me.Materials.VialOfPutridness <= Hud.Game.Me.Materials.HeartOfFright)
                       {
                        HellfireAmuletNumber = Hud.Game.Me.Materials.VialOfPutridness;
                       }
                       else if (Hud.Game.Me.Materials.IdolOfTerror <= ForgottenSouls && Hud.Game.Me.Materials.IdolOfTerror <= Hud.Game.Me.Materials.LeoricsRegret && Hud.Game.Me.Materials.IdolOfTerror <= Hud.Game.Me.Materials.VialOfPutridness && Hud.Game.Me.Materials.IdolOfTerror <= Hud.Game.Me.Materials.HeartOfFright)
                       {
                        HellfireAmuletNumber = Hud.Game.Me.Materials.IdolOfTerror;
                       }
                      else if (Hud.Game.Me.Materials.HeartOfFright <= ForgottenSouls && Hud.Game.Me.Materials.HeartOfFright <= Hud.Game.Me.Materials.LeoricsRegret && Hud.Game.Me.Materials.HeartOfFright <= Hud.Game.Me.Materials.VialOfPutridness && Hud.Game.Me.Materials.HeartOfFright <= Hud.Game.Me.Materials.IdolOfTerror)
                       {
                        HellfireAmuletNumber = Hud.Game.Me.Materials.HeartOfFright;
                       }
                       
                      if (HellfireAmuletNumber == 1) HellfireAmuletPlural = string.Empty;  
                      HellfireAmuletSentence = "- I can craft you " + HellfireAmuletNumber + " HellFire Amulet" + HellfireAmuletPlural + ".";
                    }
                   
                   long HellfireRingNumber = 0;
                   var HellfireRingPlural = "s";
                   if (Hud.Game.Me.Materials.LeoricsRegret <= Hud.Game.Me.Materials.VialOfPutridness && Hud.Game.Me.Materials.LeoricsRegret <= Hud.Game.Me.Materials.IdolOfTerror && Hud.Game.Me.Materials.LeoricsRegret <= Hud.Game.Me.Materials.HeartOfFright)
                    {
                     HellfireRingNumber = Hud.Game.Me.Materials.LeoricsRegret;
                    }
                   else if (Hud.Game.Me.Materials.VialOfPutridness <= Hud.Game.Me.Materials.LeoricsRegret && Hud.Game.Me.Materials.VialOfPutridness <= Hud.Game.Me.Materials.IdolOfTerror && Hud.Game.Me.Materials.VialOfPutridness <= Hud.Game.Me.Materials.HeartOfFright)
                    {
                     HellfireRingNumber = Hud.Game.Me.Materials.VialOfPutridness;
                    }
                    else if (Hud.Game.Me.Materials.IdolOfTerror <= Hud.Game.Me.Materials.LeoricsRegret && Hud.Game.Me.Materials.IdolOfTerror <= Hud.Game.Me.Materials.VialOfPutridness && Hud.Game.Me.Materials.IdolOfTerror <= Hud.Game.Me.Materials.HeartOfFright)
                    {
                     HellfireRingNumber = Hud.Game.Me.Materials.IdolOfTerror;
                    }
                   else if (Hud.Game.Me.Materials.HeartOfFright <= Hud.Game.Me.Materials.LeoricsRegret && Hud.Game.Me.Materials.HeartOfFright <= Hud.Game.Me.Materials.VialOfPutridness && Hud.Game.Me.Materials.HeartOfFright <= Hud.Game.Me.Materials.IdolOfTerror)
                    {
                     HellfireRingNumber = Hud.Game.Me.Materials.HeartOfFright;
                    }
                  
                  if (HellfireRingNumber == 1) HellfireRingPlural = string.Empty;  
                      HellfireRingSentence = "- ...And also " + HellfireRingNumber + " HellFire Ring" + HellfireRingPlural + ".";

                    
                 }
                if (HellfireAmuletSentence == string.Empty && JewellerSentence == 1) JewellerSentence = 2;
                if (HellfireRingSentence == string.Empty && JewellerSentence == 2) JewellerSentence = 3; 
                
                switch (JewellerSentence)
                  {
                   case 0:
                   SellerMessage = GemSentence;
                   break;
                   case 1:
                   SellerMessage = HellfireAmuletSentence;
                   break;
                   case 2:
                   SellerMessage = HellfireRingSentence;
                   break;
                   case 3:
                   SellerMessage = "...And extracting gems from your gear is free of course... *sigh*.";
                   break;
                  } 
              }
              else if (Seller.SnoActor.Sno == 56948) // Mystik
              {
               long ArcaneDust = (int)(Hud.Game.Me.Materials.ArcaneDust/5);
               long VeiledCrystal = (int)(Hud.Game.Me.Materials.VeiledCrystal/15);
               long ForgottenSoul = Hud.Game.Me.Materials.ForgottenSoul;
               long DeathsBreath = Hud.Game.Me.Materials.DeathsBreath;
               long ImperialRubies = GemsCount["Imperial Ruby"];
               long ImperialTopaz = GemsCount["Imperial Topaz"];
               long ImperialEmerald = GemsCount["Imperial Emerald"];
               long ImperialDiamond = GemsCount["Imperial Diamond"];
               long ImperialAmethyst = GemsCount["Imperial Amethyst"];
               
               long EnchantTrinketsCount = 0;
               
               if (ArcaneDust <= VeiledCrystal && ArcaneDust <= ForgottenSoul && ArcaneDust <= DeathsBreath && ArcaneDust <= ImperialRubies && ArcaneDust <= ImperialTopaz
                   && ArcaneDust <= ImperialEmerald && ArcaneDust <= ImperialDiamond && ArcaneDust <= ImperialAmethyst)
                   {
                    EnchantTrinketsCount = ArcaneDust;    
                   }
               else if (VeiledCrystal <= ArcaneDust && VeiledCrystal <= ForgottenSoul && VeiledCrystal <= DeathsBreath && VeiledCrystal <= ImperialRubies &&
                        VeiledCrystal <= ImperialTopaz && VeiledCrystal <= ImperialEmerald && VeiledCrystal <= ImperialDiamond && VeiledCrystal <= ImperialAmethyst)
                   {
                    EnchantTrinketsCount = VeiledCrystal;    
                   }
               else if (ForgottenSoul <= ArcaneDust && ForgottenSoul <= VeiledCrystal && ForgottenSoul <= DeathsBreath && ForgottenSoul <= ImperialRubies &&
                        ForgottenSoul <= ImperialTopaz && ForgottenSoul <= ImperialEmerald && ForgottenSoul <= ImperialDiamond && ForgottenSoul <= ImperialAmethyst)
                   {
                    EnchantTrinketsCount = ForgottenSoul;    
                   }
               else if (DeathsBreath <= ArcaneDust && DeathsBreath <= VeiledCrystal && DeathsBreath <= ForgottenSoul && DeathsBreath <= ImperialRubies &&
                        DeathsBreath <= ImperialTopaz && DeathsBreath <= ImperialEmerald && DeathsBreath <= ImperialDiamond && DeathsBreath <= ImperialAmethyst)
                   {
                    EnchantTrinketsCount = DeathsBreath;    
                   }
               else if (ImperialRubies <= ArcaneDust && ImperialRubies <= VeiledCrystal && ImperialRubies <= ForgottenSoul && ImperialRubies <= DeathsBreath &&
                        ImperialRubies <= ImperialTopaz && ImperialRubies <= ImperialEmerald && ImperialRubies <= ImperialDiamond && ImperialRubies <= ImperialAmethyst
                        && ImperialRubies != 0)
                   {
                    EnchantTrinketsCount = ImperialRubies;    
                   }
               else if (ImperialTopaz <= ArcaneDust && ImperialTopaz <= VeiledCrystal && ImperialTopaz <= ForgottenSoul && ImperialTopaz <= DeathsBreath &&
                        ImperialTopaz <= ImperialRubies && ImperialTopaz <= ImperialEmerald && ImperialTopaz <= ImperialDiamond && ImperialTopaz <= ImperialAmethyst
                        && ImperialTopaz != 0)
                   {
                    EnchantTrinketsCount = ImperialTopaz;    
                   }
               else if (ImperialEmerald <= ArcaneDust && ImperialEmerald <= VeiledCrystal && ImperialEmerald <= ForgottenSoul && ImperialEmerald <= DeathsBreath &&
                        ImperialEmerald <= ImperialRubies && ImperialEmerald <= ImperialTopaz && ImperialEmerald <= ImperialDiamond && ImperialEmerald <= ImperialAmethyst
                        && ImperialEmerald != 0)
                   {
                    EnchantTrinketsCount = ImperialEmerald;    
                   }
               else if (ImperialDiamond <= ArcaneDust && ImperialDiamond <= VeiledCrystal && ImperialDiamond <= ForgottenSoul && ImperialDiamond <= DeathsBreath &&
                        ImperialDiamond <= ImperialRubies && ImperialDiamond <= ImperialTopaz && ImperialDiamond <= ImperialEmerald && ImperialDiamond <= ImperialAmethyst
                        && ImperialDiamond != 0)
                   {
                    EnchantTrinketsCount = ImperialDiamond;    
                   }
               else if (ImperialAmethyst <= ArcaneDust && ImperialAmethyst <= VeiledCrystal && ImperialAmethyst <= ForgottenSoul && ImperialAmethyst <= DeathsBreath &&
                        ImperialAmethyst <= ImperialRubies && ImperialAmethyst <= ImperialTopaz && ImperialAmethyst <= ImperialEmerald && ImperialAmethyst <= ImperialDiamond
                        && ImperialAmethyst != 0)
                   {
                    EnchantTrinketsCount = ImperialAmethyst;    
                   }
               
               long EnchantCount = 0;
               
                    if (ArcaneDust <= VeiledCrystal && ArcaneDust <= ForgottenSoul && ArcaneDust <= DeathsBreath)
                   {
                    EnchantCount = ArcaneDust;    
                   }
               else if (VeiledCrystal <= ArcaneDust && VeiledCrystal <= ForgottenSoul && VeiledCrystal <= DeathsBreath)
                   {
                    EnchantCount = VeiledCrystal;    
                   }
               else if (ForgottenSoul <= ArcaneDust && ForgottenSoul <= VeiledCrystal && ForgottenSoul <= DeathsBreath)
                   {
                    EnchantCount = ForgottenSoul;    
                   }
               else if (DeathsBreath <= ArcaneDust && DeathsBreath <= VeiledCrystal && DeathsBreath <= ForgottenSoul)
                   {
                    EnchantCount = DeathsBreath;    
                   }


               string EnchantTrinketsSentence = string.Empty;
               string EnchantTrinketsPlural = "s";
               if (EnchantTrinketsCount == 1) EnchantTrinketsPlural = string.Empty;
               else if (EnchantTrinketsCount == 0 && MysticSentence == 0) MysticSentence = 1;           
               else EnchantTrinketsSentence = "- I sense from your materials that I can approximately enchant" + Environment.NewLine + "  " + EnchantTrinketsCount + " time" + EnchantTrinketsPlural + " one of your rings or Amulets."; 
               
               string EnchantSentence = string.Empty;
               string EnchantPlural = "s";
               if (EnchantCount == 1) EnchantPlural = string.Empty;
               else if (EnchantCount == 0 && MysticSentence == 1) MysticSentence = 2;           
               else EnchantSentence = "- I can enchant at least " + EnchantCount + " time" + EnchantPlural + " one of your gear items.";
               
               long TransmogCount = (int)(Hud.Game.Me.Materials.Gold/50000);
               string TransmogPlural = "s";
               if (TransmogCount == 1) TransmogPlural = string.Empty;
               string TransmogSentence = "- You can have " + TransmogCount + " of the most expensive" + Environment.NewLine + "  transmog" + TransmogPlural + " with your gold.";            

               
               long DyeCount = (int)(Hud.Game.Me.Materials.Gold/5040);
               string DyePlural = "s";
               if (DyeCount == 1) DyePlural = string.Empty;
               string DyeSentence = "- ...And you can dye " + DyeCount + " item" + TransmogPlural + " with your gold.";            

               
               switch (MysticSentence)
                  {
                   case 0:
                   SellerMessage = EnchantTrinketsSentence;
                   break;
                   case 1:
                   SellerMessage = EnchantSentence;
                   break;
                   case 2:
                   SellerMessage = "- Remember to keep Imperial gems of any type," + Environment.NewLine + "  I need some to enchant rings and amulets." ;
                   break;
                   case 3:
                   SellerMessage = TransmogSentence;
                   break;
                   case 4:
                   SellerMessage = DyeSentence;
                   break;
                  }    
              }
              
              else if (Seller.SnoActor.Sno == 56947) // Blacksmith
              {
              // 20 reusable 20 arcane  30 vieled 1 Khanduran 1 corrupted
              // 15 reusable 15 arcane 20 vieled
              // staff of herding : 1 wirt's bell 1 Black Mushroom 1 liquid rainbow 1 gibbering gemstone 1 leoric's shinbone
              // 30 30 50 1 1 (weapon)

              
              
              
              
              }
              
            if (Seller.FloorCoordinate.IsOnScreen() && Seller != null) SellerDecorator.Paint(layer, Seller, Seller.FloorCoordinate.Offset(0,0,-2), SellerMessage);
           }


           }

        }
}

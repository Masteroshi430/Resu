// https://github.com/User5981/Resu
// Crier Seller Plugin for TurboHUD version 12/02/2019 11:27
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Turbo.Plugins.Default;

namespace Turbo.Plugins.Resu
{
    public class CrierSellerPlugin : BasePlugin, IInGameWorldPainter
    {
        public WorldDecoratorCollection SellerDecorator { get; set; }
        public int PrevSeconds { get; set; }
        public int KadalaSentence { get; set; }
        public int JewellerSentence { get; set; }
        public int MysticSentence { get; set; }
        public int BlackSmithSentence { get; set; }
        public int KulleSentence { get; set; }

        private readonly HashSet<ActorSnoEnum> _actorList = new HashSet<ActorSnoEnum>(new List<uint>()
        {
            361241, 56949, 56948, 56947, 429005
        }.Select(x => (ActorSnoEnum)x));

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
            BlackSmithSentence = 0;
            KulleSentence = 0;
        }

        public void PaintWorld(WorldLayer layer)
        {
            if (!Hud.Game.IsInTown) return;
            if (Hud.Inventory.InventoryMainUiElement.Visible) return;
            var Sellers = Hud.Game.Actors.Where(x => _actorList.Contains(x.SnoActor.Sno));
            string SellerMessage = string.Empty;
            int Seconds = Hud.Time.Now.Second;

            if (Seconds % 5 == 0 && PrevSeconds != Seconds)
            {
                KadalaSentence++;
                if (KadalaSentence == 5) KadalaSentence = 0;
                JewellerSentence++;
                if (JewellerSentence == 4) JewellerSentence = 0;
                MysticSentence++;
                if (MysticSentence == 5) MysticSentence = 0;
                BlackSmithSentence++;
                if (BlackSmithSentence == 3) BlackSmithSentence = 0;
                KulleSentence++;
                if (KulleSentence == 6) KulleSentence = 0;
                PrevSeconds = Seconds;
            }

            Dictionary<string, long> GemsCount = new Dictionary<string, long>();

            var GemsStash = Hud.Inventory.ItemsInStash.Where(x => x.SnoItem.MainGroupCode == "gems");
            foreach (var Gem in GemsStash)
            {
                if (Gem == null) continue;
                if (Gem.SnoItem == null) continue;
                if (Gem.SnoItem.NameEnglish == null) continue;

                string GemNameStash = Gem.SnoItem.NameEnglish;
                long GemQuantityStash = Gem.Quantity;

                if (!GemsCount.ContainsKey(GemNameStash)) GemsCount.Add(GemNameStash, GemQuantityStash);
                else
                {
                    long DictQuantity = 0;
                    if (GemsCount.TryGetValue(GemNameStash, out DictQuantity))
                    {
                        GemsCount.TryGetValue(GemNameStash, out DictQuantity);
                        GemsCount[GemNameStash] = DictQuantity + GemQuantityStash;
                    }
                }
            }

            var GemsInventory = Hud.Inventory.ItemsInInventory.Where(x => x.SnoItem.MainGroupCode == "gems");
            foreach (var Gem in GemsInventory)
            {
                if (Gem == null) continue;
                if (Gem.SnoItem == null) continue;
                if (Gem.SnoItem.NameEnglish == null) continue;

                string GemNameInventory = Gem.SnoItem.NameEnglish;
                long GemQuantityInventory = Gem.Quantity;

                if (!GemsCount.ContainsKey(GemNameInventory)) GemsCount.Add(GemNameInventory, GemQuantityInventory);
                else
                {
                    long DictQuantity2 = 0;
                    if (GemsCount.TryGetValue(GemNameInventory, out DictQuantity2))
                    {
                        GemsCount.TryGetValue(GemNameInventory, out DictQuantity2);
                        GemsCount[GemNameInventory] = DictQuantity2 + GemQuantityInventory;
                    }
                }
            }

            long Khanduran = Hud.Game.Me.Materials.KhanduranRune;
            long Caldeum = Hud.Game.Me.Materials.CaldeumNightShade;
            long Arreat = Hud.Game.Me.Materials.ArreatWarTapestry;
            long AngelFlesh = Hud.Game.Me.Materials.CorruptedAngelFlesh;
            long HolyWater = Hud.Game.Me.Materials.WestmarchHolyWater;
            long DeathsBreath = Hud.Game.Me.Materials.DeathsBreath;
            long ArcaneDust = Hud.Game.Me.Materials.ArcaneDust;
            long VeiledCrystal = Hud.Game.Me.Materials.VeiledCrystal;
            long ReusableParts = Hud.Game.Me.Materials.ReusableParts;
            long ForgottenSoul = Hud.Game.Me.Materials.ForgottenSoul;
            long Regret = Hud.Game.Me.Materials.LeoricsRegret;
            long Vial = Hud.Game.Me.Materials.VialOfPutridness;
            long Idol = Hud.Game.Me.Materials.IdolOfTerror;
            long Heart = Hud.Game.Me.Materials.HeartOfFright;

            foreach (var Seller in Sellers)
            {
                if (Seller == null) continue;
                if (Seller.SnoActor == null) continue;
                if (Seller.SnoActor.Sno == 0) continue;
                if (Seller.FloorCoordinate == null) continue;

                if (Seller.SnoActor.Sno == ActorSnoEnum._x1_randomitemnpc /*361241*/) // Kadala
                {
                    string BloodShardsPlural = "s";
                    if (Hud.Game.Me.Materials.BloodShard == 1) BloodShardsPlural = string.Empty;
                    string BloodShardsAmmount = "- You have " + Hud.Game.Me.Materials.BloodShard + " Blood Shard" + BloodShardsPlural + " out of " + (500 + (Hud.Game.Me.HighestSoloRiftLevel * 10)) + ".";
                    if (Hud.Game.Me.Materials.BloodShard == 0) BloodShardsAmmount = "You have no Blood Shards.";

                    int TwentyFive = (int)(Hud.Game.Me.Materials.BloodShard / 25);
                    string TwentyFivePlural = "s";
                    string PhylacteryPlural = "phylacteries ";
                    if (TwentyFive == 1) { TwentyFivePlural = string.Empty; PhylacteryPlural = "phylactery "; }

                    int Fifty = (int)(Hud.Game.Me.Materials.BloodShard / 50);
                    string FiftyPlural = "s";
                    if (Fifty == 1) FiftyPlural = string.Empty;

                    int SeventyFive = (int)(Hud.Game.Me.Materials.BloodShard / 75);
                    string SeventyFivePlural = "s";
                    if (SeventyFive == 1) SeventyFivePlural = string.Empty;

                    int Hundred = (int)(Hud.Game.Me.Materials.BloodShard / 100);
                    string HundredPlural = "s";
                    if (Hundred == 1) HundredPlural = string.Empty;

                    string TwentyFiveClass = string.Empty;
                    string FiftyClass = string.Empty;
                    string SeventyFiveClass = string.Empty;
                    string HundredClass = string.Empty;

                    switch (Hud.Game.Me.HeroClassDefinition.HeroClass.ToString())
                    {
                        case "Barbarian":
                            break;

                        case "DemonHunter":
                            TwentyFiveClass = "mystery quiver" + TwentyFivePlural + ", " + "helm" + TwentyFivePlural + ", " + "pair of gloves" + "," + Environment.NewLine + "pair of boots" + ", " + "chest armor" + TwentyFivePlural + ", " + "belt" + TwentyFivePlural + ", " + "pair of shoulders" + "," + Environment.NewLine + "pair of pants" + ", " + "pair of bracers" + " or " + "shield" + TwentyFivePlural + ".";
                            break;

                        case "Wizard":
                            TwentyFiveClass = "mystery orb" + TwentyFivePlural + ", " + "helm" + TwentyFivePlural + ", " + "pair of gloves" + "," + Environment.NewLine + "pair of boots" + ", " + "chest armor" + TwentyFivePlural + ", " + "belt" + TwentyFivePlural + ", " + "pair of shoulders" + "," + Environment.NewLine + "pair of pants" + ", " + "pair of bracers" + " or " + "shield" + TwentyFivePlural + ".";
                            break;

                        case "WitchDoctor":
                            TwentyFiveClass = "mystery mojo" + TwentyFivePlural + ", " + "helm" + TwentyFivePlural + ", " + "pair of gloves" + "," + Environment.NewLine + "pair of boots" + ", " + "chest armor" + TwentyFivePlural + ", " + "belt" + TwentyFivePlural + ", " + "pair of shoulders" + "," + Environment.NewLine + "pair of pants" + ", " + "pair of bracers" + " or " + "shield" + TwentyFivePlural + ".";
                            break;

                        case "Monk":
                        case "Crusader":
                            TwentyFiveClass = "helm" + TwentyFivePlural + ", " + "pair of gloves" + "," + Environment.NewLine + "pair of boots" + ", " + "chest armor" + TwentyFivePlural + ", " + "belt" + TwentyFivePlural + ", " + "pair of shoulders" + "," + Environment.NewLine + "pair of pants" + ", " + "pair of bracers" + " or " + "shield" + TwentyFivePlural + ".";
                            break;

                        case "Necromancer":
                            TwentyFiveClass = "mystery " + PhylacteryPlural + ", " + "helm" + TwentyFivePlural + ", " + "pair of gloves" + "," + Environment.NewLine + "pair of boots" + ", " + "chest armor" + TwentyFivePlural + ", " + "belt" + TwentyFivePlural + ", " + "pair of shoulders" + "," + Environment.NewLine + "pair of pants" + ", " + "pair of bracers" + " or " + "shield" + TwentyFivePlural + ".";
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
                    if (Fifty == 0) { FiftyMessage = string.Empty; if (KadalaSentence == 2) KadalaSentence = 0; }
                    else if (Fifty == 1) FiftyMessage = "- I have also one " + FiftyClass;
                    else FiftyMessage = "- I have also " + Fifty + " " + FiftyClass;

                    string SeventyFiveMessage = string.Empty;
                    if (SeventyFive == 0) { SeventyFiveMessage = string.Empty; if (KadalaSentence == 3) KadalaSentence = 0; }
                    else if (SeventyFive == 1) SeventyFiveMessage = "- ... And one " + SeventyFiveClass;
                    else SeventyFiveMessage = "- ... And " + SeventyFive + " " + SeventyFiveClass;

                    string HundredMessage = string.Empty;
                    if (Hundred == 0) { HundredMessage = string.Empty; if (KadalaSentence == 4) KadalaSentence = 0; }
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
                    if (Seller.FloorCoordinate.IsOnScreen() && Seller.FloorCoordinate.XYDistanceTo(Hud.Game.Me.FloorCoordinate) <= 40 && Seller != null) SellerDecorator.Paint(layer, Seller, Seller.FloorCoordinate.Offset(0, 0, -2), SellerMessage);
                }
                else if (Seller.SnoActor.Sno == ActorSnoEnum._pt_jeweler/*56949*/) // Jeweler
                {
                    string GemSentence = string.Empty;

                    foreach (var Gem in GemsCount.OrderByDescending(i => i.Value).Take(1))
                    {
                        if (Gem.Key.Contains("Marquise") && Gem.Value > 3)
                        {
                            int ImperialNumbers = (int)(Gem.Value / 3);
                            string ImperialColor = Gem.Key.Replace("Marquise", string.Empty).Trim();
                            string MarquisePlural = "s";
                            if (Gem.Value == 1) MarquisePlural = string.Empty;
                            string ImperialPlural = "s";
                            if (ImperialNumbers == 1) ImperialPlural = string.Empty;
                            if (ImperialColor == "Ruby" && ImperialNumbers > 1) { ImperialColor = "Rubie"; }
                            else if (ImperialColor == "Topaz" && ImperialNumbers > 1) { ImperialColor = "Topaze"; }
                            long Gold = (int)(ImperialNumbers * 200000);
                            if (Hud.Game.Me.Materials.Gold < Gold) { ImperialNumbers = (int)(Hud.Game.Me.Materials.Gold / 200000); Gold = (int)(ImperialNumbers * 200000); }
                            string GoldString = Gold.ToString("N0", CultureInfo.InvariantCulture);
                            if (Hud.Game.Me.Materials.Gold >= Gold) GemSentence = "- You have " + Gem.Value + " Marquise " + ImperialColor + MarquisePlural + Environment.NewLine + "  I can combine them into " + ImperialNumbers + " Imperial " + ImperialColor + ImperialPlural + Environment.NewLine + "  for " + GoldString + " gold.";
                        }
                        else if (Gem.Key.Contains("Flawless Imperial") && Gem.Value > 3)
                        {
                            int RoyalNumbers = (int)(Gem.Value / 3);
                            string RoyalColor = Gem.Key.Replace("Fawless Imperial", string.Empty).Trim();
                            string FlawlessImperialPlural = "s";
                            if (Gem.Value == 1) FlawlessImperialPlural = string.Empty;
                            string RoyalPlural = "s";
                            if (RoyalNumbers == 1) RoyalPlural = string.Empty;
                            if (RoyalColor == "Ruby" && RoyalNumbers > 1) { RoyalColor = "Rubie"; }
                            else if (RoyalColor == "Topaz" && RoyalNumbers > 1) { RoyalColor = "Topaze"; }
                            long DeathBreaths = RoyalNumbers;
                            long Gold = (int)(RoyalNumbers * 400000);
                            if (Hud.Game.Me.Materials.Gold < Gold) { RoyalNumbers = (int)(Hud.Game.Me.Materials.Gold / 400000); Gold = (int)(RoyalNumbers * 400000); DeathBreaths = RoyalNumbers; }
                            if (Hud.Game.Me.Materials.DeathsBreath < DeathBreaths) { RoyalNumbers = (int)(Hud.Game.Me.Materials.DeathsBreath); Gold = (int)(RoyalNumbers * 400000); DeathBreaths = RoyalNumbers; }
                            string GoldString = Gold.ToString("N0", CultureInfo.InvariantCulture);

                            if (Hud.Game.Me.Materials.Gold >= Gold && Hud.Game.Me.Materials.DeathsBreath >= DeathBreaths) GemSentence = "- You have " + Gem.Value + " Fawless Imperial " + RoyalColor + FlawlessImperialPlural + Environment.NewLine + "  I can combine them into " + RoyalNumbers + " Royal " + RoyalColor + RoyalPlural + Environment.NewLine + "  for " + GoldString + " gold and " + DeathBreaths + "Death's Breath" + RoyalPlural + ".";
                        }
                        else if (Gem.Key.Contains("Imperial") && Gem.Value > 3)
                        {
                            int FlawlessImperialNumbers = (int)(Gem.Value / 3);
                            string FlawlessImperialColor = Gem.Key.Replace("Imperial", string.Empty).Trim();
                            string ImperialPlural = "s";
                            if (Gem.Value == 1) ImperialPlural = string.Empty;
                            string FlawlessImperialPlural = "s";
                            if (FlawlessImperialNumbers == 1) FlawlessImperialPlural = string.Empty;
                            if (FlawlessImperialColor == "Ruby" && FlawlessImperialNumbers > 1) { FlawlessImperialColor = "Rubie"; }
                            else if (FlawlessImperialColor == "Topaz" && FlawlessImperialNumbers > 1) { FlawlessImperialColor = "Topaze"; }
                            long Gold = (int)(FlawlessImperialNumbers * 300000);
                            if (Hud.Game.Me.Materials.Gold < Gold) { FlawlessImperialNumbers = (int)(Hud.Game.Me.Materials.Gold / 300000); Gold = (int)(FlawlessImperialNumbers * 300000); }
                            string GoldString = Gold.ToString("N0", CultureInfo.InvariantCulture);

                            if (Hud.Game.Me.Materials.Gold >= Gold) GemSentence = "- You have " + Gem.Value + " Imperial " + FlawlessImperialColor + ImperialPlural + Environment.NewLine + "  I can combine them into " + FlawlessImperialNumbers + " Flawless Imperial " + FlawlessImperialColor + FlawlessImperialPlural + Environment.NewLine + "  for " + GoldString + " gold.";
                        }
                        else if (Gem.Key.Contains("Royal") && Gem.Value > 3)
                        {
                            int FlawlessRoyalNumbers = (int)(Gem.Value / 3);
                            string FlawlessRoyalColor = Gem.Key.Replace("Imperial", string.Empty).Trim();
                            string RoyalPlural = "s";
                            if (Gem.Value == 1) RoyalPlural = string.Empty;
                            string FlawlessRoyalPlural = "s";
                            if (FlawlessRoyalNumbers == 1) FlawlessRoyalPlural = string.Empty;
                            if (FlawlessRoyalColor == "Ruby" && FlawlessRoyalNumbers > 1) { FlawlessRoyalColor = "Rubie"; }
                            else if (FlawlessRoyalColor == "Topaz" && FlawlessRoyalNumbers > 1) { FlawlessRoyalColor = "Topaze"; }
                            long Gold = (int)(FlawlessRoyalNumbers * 500000);
                            long DeathBreaths = FlawlessRoyalNumbers;
                            if (Hud.Game.Me.Materials.Gold < Gold) { FlawlessRoyalNumbers = (int)(Hud.Game.Me.Materials.Gold / 500000); Gold = (int)(FlawlessRoyalNumbers * 500000); DeathBreaths = FlawlessRoyalNumbers; }
                            if (Hud.Game.Me.Materials.DeathsBreath < DeathBreaths) { FlawlessRoyalNumbers = (int)(Hud.Game.Me.Materials.DeathsBreath); Gold = (int)(FlawlessRoyalNumbers * 500000); DeathBreaths = FlawlessRoyalNumbers; }
                            string GoldString = Gold.ToString("N0", CultureInfo.InvariantCulture);

                            if (Hud.Game.Me.Materials.Gold >= Gold && Hud.Game.Me.Materials.DeathsBreath >= DeathBreaths) GemSentence = "- You have " + Gem.Value + " Royal " + FlawlessRoyalColor + RoyalPlural + Environment.NewLine + "  I can combine them into " + FlawlessRoyalNumbers + " Flawless Royal " + FlawlessRoyalColor + FlawlessRoyalPlural + Environment.NewLine + "  for " + GoldString + " gold and " + DeathBreaths + "Death's Breath" + FlawlessRoyalPlural + ".";
                        }
                    }
                    if (GemSentence == string.Empty && JewellerSentence == 0) JewellerSentence = 1;
                    string HellfireRingSentence = string.Empty;
                    string HellfireAmuletSentence = string.Empty;

                    if (ForgottenSoul >= 10)
                    {
                        var HellfireAmuletPlural = "s";
                        long HellfireAmuletNumber = Math.Min((int)(ForgottenSoul / 10), Math.Min(Regret, Math.Min(Vial, Math.Min(Idol, Heart))));
                        if (HellfireAmuletNumber == 1) HellfireAmuletPlural = string.Empty;
                        else if (HellfireAmuletNumber == 0 && JewellerSentence == 1) JewellerSentence = 2;
                        else HellfireAmuletSentence = "- I can craft you " + HellfireAmuletNumber + " HellFire Amulet" + HellfireAmuletPlural + ".";
                    }

                    long HellfireRingNumber = Math.Min(Regret, Math.Min(Vial, Math.Min(Idol, Heart)));

                    var HellfireRingPlural = "s";
                    if (HellfireRingNumber == 1) HellfireRingPlural = string.Empty;
                    else if (HellfireRingNumber == 0 && JewellerSentence == 2) JewellerSentence = 3;
                    else HellfireRingSentence = "- ...And also " + HellfireRingNumber + " HellFire Ring" + HellfireRingPlural + ".";

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
                    if (Seller.FloorCoordinate.IsOnScreen() && Seller.FloorCoordinate.XYDistanceTo(Hud.Game.Me.FloorCoordinate) <= 40 && Seller != null) SellerDecorator.Paint(layer, Seller, Seller.FloorCoordinate.Offset(0, 0, -2), SellerMessage);
                }
                else if (Seller.SnoActor.Sno == ActorSnoEnum._pt_mystic /*56948*/) // Mystik
                {
                    long ImperialRubies = 0;
                    if (GemsCount.TryGetValue("Imperial Ruby", out ImperialRubies)) ImperialRubies = GemsCount["Imperial Ruby"];
                    long ImperialTopaz = 0;
                    if (GemsCount.TryGetValue("Imperial Topaz", out ImperialTopaz)) ImperialTopaz = GemsCount["Imperial Topaz"];
                    long ImperialEmerald = 0;
                    if (GemsCount.TryGetValue("Imperial Emerald", out ImperialEmerald)) ImperialEmerald = GemsCount["Imperial Emerald"];
                    long ImperialDiamond = 0;
                    if (GemsCount.TryGetValue("Imperial Diamond", out ImperialDiamond)) ImperialDiamond = GemsCount["Imperial Diamond"];
                    long ImperialAmethyst = 0;
                    if (GemsCount.TryGetValue("Imperial Amethyst", out ImperialAmethyst)) ImperialAmethyst = GemsCount["Imperial Amethyst"];

                    long EnchantTrinketsCount = Math.Min((int)(ArcaneDust / 5), Math.Min((int)(VeiledCrystal / 15), Math.Min(ForgottenSoul, Math.Min(DeathsBreath, Math.Min(ImperialRubies, Math.Min(ImperialTopaz, Math.Min(ImperialEmerald, Math.Min(ImperialDiamond, ImperialAmethyst))))))));
                    long EnchantCount = Math.Min((int)(ArcaneDust / 5), Math.Min((int)(VeiledCrystal / 15), Math.Min(ForgottenSoul, DeathsBreath)));

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

                    string TransmogSentence = string.Empty;
                    long TransmogCount = (int)(Hud.Game.Me.Materials.Gold / 50000);
                    string TransmogPlural = "s";
                    if (TransmogCount == 1) TransmogPlural = string.Empty;
                    else if (TransmogCount == 0 && MysticSentence == 3) MysticSentence = 4;
                    else TransmogSentence = "- You can have " + TransmogCount + " of the most expensive" + Environment.NewLine + "  transmog" + TransmogPlural + " with your gold.";

                    string DyeSentence = string.Empty;
                    long DyeCount = (int)(Hud.Game.Me.Materials.Gold / 5040);
                    string DyePlural = "s";
                    if (DyeCount == 1) DyePlural = string.Empty;
                    else if (DyeCount == 0 && MysticSentence == 4) MysticSentence = 0;
                    else DyeSentence = "- ...And you can dye " + DyeCount + " item" + TransmogPlural + " with your gold.";

                    switch (MysticSentence)
                    {
                        case 0:
                            SellerMessage = EnchantTrinketsSentence;
                            break;

                        case 1:
                            SellerMessage = EnchantSentence;
                            break;

                        case 2:
                            SellerMessage = "- Remember to keep Imperial gems of any type," + Environment.NewLine + "  I need some to enchant rings and amulets.";
                            break;

                        case 3:
                            SellerMessage = TransmogSentence;
                            break;

                        case 4:
                            SellerMessage = DyeSentence;
                            break;
                    }
                    if (Seller.FloorCoordinate.IsOnScreen() && Seller.FloorCoordinate.XYDistanceTo(Hud.Game.Me.FloorCoordinate) <= 40 && Seller != null) SellerDecorator.Paint(layer, Seller, Seller.FloorCoordinate.Offset(0, 0, -2), SellerMessage);
                }
                else if (Seller.SnoActor.Sno == ActorSnoEnum._pt_blacksmith/*56947*/) // Blacksmith
                {
                    int BellCount = 0;
                    int MushroomCount = 0;
                    int RainbowCount = 0;
                    int GemstoneCount = 0;
                    int ShinboneCount = 0;

                    var StaffStash = Hud.Inventory.ItemsInStash.Where(x => x.SnoItem.MainGroupCode == "pony" && x.Quantity != 0);
                    foreach (var Pony in StaffStash)
                    {
                        if (Pony == null) continue;
                        if (Pony.SnoItem == null) continue;
                        if (Pony.SnoItem.Sno == 0) continue;

                        if (Pony.SnoItem.Sno == 3495098760) BellCount++;
                        else if (Pony.SnoItem.Sno == 2301417192) MushroomCount++;
                        else if (Pony.SnoItem.Sno == 725082635) RainbowCount++;
                        else if (Pony.SnoItem.Sno == 3494992897) GemstoneCount++;
                        else if (Pony.SnoItem.Sno == 3665447244) ShinboneCount++;
                    }

                    var StaffInventory = Hud.Inventory.ItemsInInventory.Where(x => x.SnoItem.MainGroupCode == "pony" && x.Quantity != 0);
                    foreach (var Pony in StaffInventory)
                    {
                        if (Pony == null) continue;
                        if (Pony.SnoItem == null) continue;
                        if (Pony.SnoItem.Sno == 0) continue;

                        if (Pony.SnoItem.Sno == 3495098760) BellCount++;
                        else if (Pony.SnoItem.Sno == 2301417192) MushroomCount++;
                        else if (Pony.SnoItem.Sno == 725082635) RainbowCount++;
                        else if (Pony.SnoItem.Sno == 3494992897) GemstoneCount++;
                        else if (Pony.SnoItem.Sno == 3665447244) ShinboneCount++;
                    }
                    int StaffCount = Math.Min(BellCount, Math.Min(MushroomCount, Math.Min(RainbowCount, Math.Min(GemstoneCount, ShinboneCount))));
                    long CraftItemCount = Math.Min((int)(ArcaneDust / 30), Math.Min((int)(VeiledCrystal / 50), Math.Min((int)(ReusableParts / 30), Math.Min(Khanduran, Math.Min(Caldeum, Math.Min(Arreat, Math.Min(AngelFlesh, HolyWater)))))));
                    long HallowedBreach = Math.Min((int)(ReusableParts / 20), Math.Min((int)(ArcaneDust / 20), Math.Min((int)(VeiledCrystal / 30), Math.Min(Khanduran, Math.Min(AngelFlesh, (int)(Hud.Game.Me.Materials.Gold / 72000))))));

                    var freeSpace = Hud.Game.Me.InventorySpaceTotal - Hud.Game.InventorySpaceUsed;
                    string InventorySentence = string.Empty;
                    if (freeSpace <= 10) InventorySentence = "- Time to empty your inventory is coming!" + Environment.NewLine + "  Let me salvage those...";
                    else if (BlackSmithSentence == 2) BlackSmithSentence = 0;

                    string StaffSentence = string.Empty;
                    string StaffPlural = "s";
                    if (StaffCount == 1) StaffPlural = string.Empty;
                    else if (StaffCount == 0 && BlackSmithSentence == 0) BlackSmithSentence = 1;
                    else StaffSentence = "- I can craft you " + StaffCount + " Staff" + StaffPlural + " of Herding!";

                    string BestItemSentence = string.Empty;
                    if (CraftItemCount == 0 && BlackSmithSentence == 1) BlackSmithSentence = 2;
                    else BestItemSentence = "- You have enough materials to have " + CraftItemCount + " of my best items crafted!";

                    switch (BlackSmithSentence)
                    {
                        case 0:
                            SellerMessage = StaffSentence;
                            break;

                        case 1:
                            SellerMessage = BestItemSentence;
                            break;

                        case 2:
                            SellerMessage = InventorySentence;
                            break;
                    }
                    if (Seller.FloorCoordinate.IsOnScreen() && Seller.FloorCoordinate.XYDistanceTo(Hud.Game.Me.FloorCoordinate) <= 40 && Seller != null) SellerDecorator.Paint(layer, Seller, Seller.FloorCoordinate.Offset(0, 0, -2), SellerMessage);
                }
                else if (Seller.SnoActor.Sno == ActorSnoEnum._p2_hq_zoltunkulle/*429005*/) // Kulle
                {
                    var puzzleRingCount = 0;
                    var bovineBardicheCount = 0;

                    foreach (var item in Hud.Inventory.ItemsInStash)
                    {
                        if (item.SnoItem.Sno == 2346057823) bovineBardicheCount += (int)item.Quantity; // Bovine Bardiche
                        if (item.SnoItem.Sno == 3106130529) puzzleRingCount += (int)item.Quantity; // Puzzle Ring
                    }

                    foreach (var item in Hud.Inventory.ItemsInInventory)
                    {
                        if (item.SnoItem.Sno == 2346057823) bovineBardicheCount += (int)item.Quantity; // Bovine Bardiche
                        if (item.SnoItem.Sno == 3106130529) puzzleRingCount += (int)item.Quantity; // Puzzle Ring
                    }

                    puzzleRingCount = Math.Abs(puzzleRingCount);
                    bovineBardicheCount = Math.Abs(bovineBardicheCount);

                    long Extract = Math.Min(Khanduran, Math.Min(Caldeum, Math.Min(Arreat, Math.Min(AngelFlesh, Math.Min(HolyWater, (int)(DeathsBreath / 5))))));
                    long Reforge = Math.Min((int)(Khanduran / 5), Math.Min((int)(Caldeum / 5), Math.Min((int)(Arreat / 5), Math.Min((int)(AngelFlesh / 5), Math.Min((int)(HolyWater / 5), (int)(ForgottenSoul / 50))))));
                    long Upgrade = Math.Min((int)(VeiledCrystal / 50), Math.Min((int)(ArcaneDust / 50), Math.Min((int)(ReusableParts / 50), (int)(DeathsBreath / 25))));
                    long Convert = Math.Min((int)(DeathsBreath / 10), (int)(ForgottenSoul / 10));

                    string ConvertSentence = string.Empty;
                    string ConvertPlural = "s";
                    if (Convert == 1) ConvertPlural = string.Empty;
                    else if (Convert == 0 && KulleSentence == 5) KulleSentence = 0;
                    else ConvertSentence = "- You can convert a set item " + Convert + " time" + ConvertPlural + ".";

                    string UpgradeSentence = string.Empty;
                    string UpgradePlural = "s";
                    if (Upgrade == 1) UpgradePlural = string.Empty;
                    else if (Upgrade == 0 && KulleSentence == 4) KulleSentence = 5;
                    else UpgradeSentence = "- You can upgrade " + Upgrade + " rare item" + UpgradePlural + ".";

                    string ReforgeSentence = string.Empty;
                    string ReforgePlural = "s";
                    if (Reforge == 1) ReforgePlural = string.Empty;
                    else if (Reforge == 0 && KulleSentence == 3) KulleSentence = 4;
                    else ReforgeSentence = "- You can reforge " + Reforge + " legendary item" + ReforgePlural + ".";

                    string ExtractSentence = string.Empty;
                    string ExtractPlural = "s";
                    if (Extract == 1) ExtractPlural = string.Empty;
                    else if (Extract == 0 && KulleSentence == 2) KulleSentence = 3;
                    else ExtractSentence = "- You can extract " + Extract + " legendary power" + ExtractPlural + ".";

                    string GoblinSentence = string.Empty;
                    string GoblinPlural = "s";
                    if (puzzleRingCount == 1) GoblinPlural = string.Empty;
                    else if (puzzleRingCount == 0 && KulleSentence == 1) KulleSentence = 2;
                    else GoblinSentence = "- You have " + puzzleRingCount + " ticket" + GoblinPlural + " to Greed's realm.";

                    string BovineSentence = string.Empty;
                    string BovinePlural = "s";
                    if (bovineBardicheCount == 1) BovinePlural = string.Empty;
                    else if (bovineBardicheCount == 0 && KulleSentence == 0) KulleSentence = 1;
                    else BovineSentence = "- I can help you access to a place that doesn't exist " + bovineBardicheCount + " time" + BovinePlural + ".";

                    switch (KulleSentence)
                    {
                        case 0:
                            SellerMessage = BovineSentence;
                            break;

                        case 1:
                            SellerMessage = GoblinSentence;
                            break;

                        case 2:
                            SellerMessage = ExtractSentence;
                            break;

                        case 3:
                            SellerMessage = ReforgeSentence;
                            break;

                        case 4:
                            SellerMessage = UpgradeSentence;
                            break;

                        case 5:
                            SellerMessage = ConvertSentence;
                            break;
                    }
                    if (Convert == 0 && Upgrade == 0 && Reforge == 0 && Extract == 0 && puzzleRingCount == 0 && bovineBardicheCount == 0) { }
                    else if (Seller.FloorCoordinate.IsOnScreen() && Seller.FloorCoordinate.XYDistanceTo(Hud.Game.Me.FloorCoordinate) <= 40 && Seller != null) SellerDecorator.Paint(layer, Seller, Seller.FloorCoordinate.Offset(0, 0, -2), SellerMessage);
                }
            }
        }
    }
}
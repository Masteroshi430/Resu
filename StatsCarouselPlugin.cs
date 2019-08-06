// https://github.com/User5981/Resu
// Stats Carousel plugin for TurboHUD version 06/08/2019 11:47
using Turbo.Plugins.Default;
using System;

namespace Turbo.Plugins.Resu
{

    public class StatsCarouselPlugin : BasePlugin, IInGameTopPainter
    {

        public TopLabelDecorator CurrentStatDecorator { get; set; }
        public string CurrentStats { get; set; }
        private IWatch _Timer;
        public int WhichStat;
        public bool ExperienceStats { get; set; }
        public bool MonsterKillStats { get; set; }
        public bool EliteKillStats { get; set; }
        public bool ItemDropStats { get; set; }
        public bool WhiteDropStats { get; set; }
        public bool MagicDropStats { get; set; }
        public bool RareDropStats { get; set; }
        public bool LegendaryDropStats { get; set; }
        public bool AncientDropStats { get; set; }
        public bool PrimalDropStats { get; set; }
        public bool GameTimeStats { get; set; }
        public bool GoldStats { get; set; }
        public bool DeathStats { get; set; }
        public bool BloodShardStats { get; set; }



        public StatsCarouselPlugin()
        {
            Enabled = true;

            ExperienceStats = true;
            MonsterKillStats = true;
            EliteKillStats = true;
            ItemDropStats = true;
            WhiteDropStats = true;
            MagicDropStats = true;
            RareDropStats = true;
            LegendaryDropStats = true;
            AncientDropStats = true;
            PrimalDropStats = true;
            GameTimeStats = true;
            GoldStats = true;
            DeathStats = true;
            BloodShardStats = true;
        }
        
        
        
        public override void Load(IController hud)
        {
            base.Load(hud);
            _Timer = Hud.Time.CreateWatch();
            _Timer.Start();
            WhichStat = 0;

            CurrentStatDecorator = new TopLabelDecorator(Hud)
            {
                 BackgroundBrush = Hud.Render.CreateBrush(0, 255, 234, 137, 30), 
                 TextFont = Hud.Render.CreateFont("Segoe UI Light", 9, 255, 255, 234, 137, false, false, true),
                 
                 TextFunc = () => CurrentStats,
            };
            
        }
        
        
        public void PaintTopInGame(ClipState clipState)
        {

            if (Hud.Render.UiHidden)
                return;
            if (clipState != ClipState.BeforeClip)
                return;
            var uiInv = Hud.Inventory.InventoryMainUiElement;
            if (uiInv.Visible)
                return;

            int RextY = (int)((Hud.Window.Size.Height / 3) * 2);
            int RextX = (int)((Hud.Window.Size.Width / 8) * 7);
            int Height = (int)(Hud.Window.Size.Height / 3);
            int Width = (int)(Hud.Window.Size.Width / 8);

            if (Hud.Window.CursorInsideRect(RextX, RextY, Width, Height))
                return;


            string TotalTime = ToReadableTime(Hud.Tracker.CurrentAccountTotal.ElapsedMilliseconds);
            string TodayTime = ToReadableTime(Hud.Tracker.CurrentAccountToday.ElapsedMilliseconds);
            string SessionTime = ToReadableTime(Hud.Tracker.Session.ElapsedMilliseconds);
            string TotalTimePlayed = ToReadableTime(Hud.Tracker.CurrentAccountTotal.PlayElapsedMilliseconds);
            string TodayTimePlayed = ToReadableTime(Hud.Tracker.CurrentAccountToday.PlayElapsedMilliseconds);
            string SessionTimePlayed = ToReadableTime(Hud.Tracker.Session.PlayElapsedMilliseconds);
            string TotalTimeTown = ToReadableTime(Hud.Tracker.CurrentAccountTotal.TownElapsedMilliseconds);
            string TodayTimeTown = ToReadableTime(Hud.Tracker.CurrentAccountToday.TownElapsedMilliseconds);
            string SessionTimeTown = ToReadableTime(Hud.Tracker.Session.TownElapsedMilliseconds);


            double TotalGainedExperience = Hud.Tracker.CurrentAccountTotal.GainedExperience;
            double TotalGainedExperiencePerHour = Hud.Tracker.CurrentAccountTotal.GainedExperiencePerHourPlay;
            double TodayGainedExperience = Hud.Tracker.CurrentAccountToday.GainedExperience;
            double TodayGainedExperiencePerHour = Hud.Tracker.CurrentAccountToday.GainedExperiencePerHourPlay;
            double SessionGainedExperience = Hud.Tracker.Session.GainedExperience;
            double SessionGainedExperiencePerHour = Hud.Tracker.Session.GainedExperiencePerHourPlay;

            long TotalMonsterKill = Hud.Tracker.CurrentAccountTotal.MonsterKill;
            double TotalMonsterKillPerHour = Hud.Tracker.CurrentAccountTotal.MonsterKillPerHour;
            long TodayMonsterKill = Hud.Tracker.CurrentAccountToday.MonsterKill;
            double TodayMonsterKillPerHour = Hud.Tracker.CurrentAccountToday.MonsterKillPerHour;
            long SessionMonsterKill = Hud.Tracker.Session.MonsterKill;
            double SessionMonsterKillPerHour = Hud.Tracker.Session.MonsterKillPerHour;

            long TotalEliteKill = Hud.Tracker.CurrentAccountTotal.EliteKill;
            double TotalEliteKillPerHour = Hud.Tracker.CurrentAccountTotal.EliteKillPerHour;
            long TodayEliteKill = Hud.Tracker.CurrentAccountToday.EliteKill;
            double TodayEliteKillPerHour = Hud.Tracker.CurrentAccountToday.EliteKillPerHour;
            long SessionEliteKill = Hud.Tracker.Session.EliteKill;
            double SessionEliteKillPerHour = Hud.Tracker.Session.EliteKillPerHour;

            long TotalDropAll = Hud.Tracker.CurrentAccountTotal.DropAll;
            double TotalDropAllPerHour = Hud.Tracker.CurrentAccountTotal.DropAllPerHour;
            long TodayDropAll = Hud.Tracker.CurrentAccountToday.DropAll;
            double TodayDropAllPerHour = Hud.Tracker.CurrentAccountToday.DropAllPerHour;
            long SessionDropAll = Hud.Tracker.Session.DropAll;
            double SessionDropAllPerHour = Hud.Tracker.Session.DropAllPerHour;

            long TotalWhite = Hud.Tracker.CurrentAccountTotal.DropWhite;
            double TotalWhitePerHour = Hud.Tracker.CurrentAccountTotal.DropWhitePerHour;
            long TodayWhite = Hud.Tracker.CurrentAccountToday.DropWhite;
            double TodayWhitePerHour = Hud.Tracker.CurrentAccountToday.DropWhitePerHour;
            long SessionWhite = Hud.Tracker.Session.DropWhite;
            double SessionWhitePerHour = Hud.Tracker.Session.DropWhitePerHour;

            long TotalMagic = Hud.Tracker.CurrentAccountTotal.DropMagic;
            double TotalMagicPerHour = Hud.Tracker.CurrentAccountTotal.DropMagicPerHour;
            long TodayMagic = Hud.Tracker.CurrentAccountToday.DropMagic;
            double TodayMagicPerHour = Hud.Tracker.CurrentAccountToday.DropMagicPerHour;
            long SessionMagic = Hud.Tracker.Session.DropMagic;
            double SessionMagicPerHour = Hud.Tracker.Session.DropMagicPerHour;

            long TotalRare = Hud.Tracker.CurrentAccountTotal.DropRare;
            double TotalRarePerHour = Hud.Tracker.CurrentAccountTotal.DropRarePerHour;
            long TodayRare = Hud.Tracker.CurrentAccountToday.DropRare;
            double TodayRarePerHour = Hud.Tracker.CurrentAccountToday.DropRarePerHour;
            long SessionRare = Hud.Tracker.Session.DropRare;
            double SessionRarePerHour = Hud.Tracker.Session.DropRarePerHour;

            long TotalLegendary = Hud.Tracker.CurrentAccountTotal.DropLegendary;
            double TotalLegendaryPerHour = Hud.Tracker.CurrentAccountTotal.DropLegendaryPerHour;
            long TodayLegendary = Hud.Tracker.CurrentAccountToday.DropLegendary;
            double TodayLegendaryPerHour = Hud.Tracker.CurrentAccountToday.DropLegendaryPerHour;
            long SessionLegendary = Hud.Tracker.Session.DropLegendary;
            double SessionLegendaryPerHour = Hud.Tracker.Session.DropLegendaryPerHour;

            long TotalAncient = Hud.Tracker.CurrentAccountTotal.DropAncient;
            double TotalAncientPerHour = Hud.Tracker.CurrentAccountTotal.DropAncientPerHour;
            long TodayAncient = Hud.Tracker.CurrentAccountToday.DropAncient;
            double TodayAncientPerHour = Hud.Tracker.CurrentAccountToday.DropAncientPerHour;
            long SessionAncient = Hud.Tracker.Session.DropAncient;
            double SessionAncientPerHour = Hud.Tracker.Session.DropAncientPerHour;

            long TotalPrimal = Hud.Tracker.CurrentAccountTotal.DropPrimalAncient;
            double TotalPrimalPerHour = Hud.Tracker.CurrentAccountTotal.DropPrimalAncientPerHour;
            long TodayPrimal = Hud.Tracker.CurrentAccountToday.DropPrimalAncient;
            double TodayPrimalPerHour = Hud.Tracker.CurrentAccountToday.DropPrimalAncientPerHour;
            long SessionPrimal = Hud.Tracker.Session.DropPrimalAncient;
            double SessionPrimalPerHour = Hud.Tracker.Session.DropPrimalAncientPerHour;

            long TotalDropGold = Hud.Tracker.CurrentAccountTotal.DropGold; //always 0
            long TotalGainedGold = Hud.Tracker.CurrentAccountTotal.GainedGold;
            double TotalGainedGoldPerHour = Hud.Tracker.CurrentAccountTotal.GainedGoldPerHour;
            long TodayDropGold = Hud.Tracker.CurrentAccountToday.DropGold; //always 0
            long TodayGainedGold = Hud.Tracker.CurrentAccountToday.GainedGold;
            double TodayGainedGoldPerHour = Hud.Tracker.CurrentAccountToday.GainedGoldPerHour;
            long SessionDropGold = Hud.Tracker.Session.DropGold; //always 0
            long SessionGainedGold = Hud.Tracker.Session.GainedGold;
            double SessionGainedGoldPerHour = Hud.Tracker.Session.GainedGoldPerHour;

            long TotalDeath = Hud.Tracker.CurrentAccountTotal.Death;
            double TotalDeathPerHour = Hud.Tracker.CurrentAccountTotal.DeathPerHour;
            long TodayDeath = Hud.Tracker.CurrentAccountToday.Death;
            double TodayDeathPerHour = Hud.Tracker.CurrentAccountToday.DeathPerHour;
            long SessionDeath = Hud.Tracker.Session.Death;
            double SessionDeathPerHour = Hud.Tracker.Session.DeathPerHour;

            long TotalDropBloodShard = Hud.Tracker.CurrentAccountTotal.DropBloodShard;
            double TotalDropBloodShardPerHour = Hud.Tracker.CurrentAccountTotal.DropBloodShardPerHour;
            long TodayDropBloodShard = Hud.Tracker.CurrentAccountToday.DropBloodShard;
            double TodayDropBloodShardPerHour = Hud.Tracker.CurrentAccountToday.DropBloodShardPerHour;
            long SessionDropBloodShard = Hud.Tracker.Session.DropBloodShard;
            double SessionDropBloodShardPerHour = Hud.Tracker.Session.DropBloodShardPerHour;


            if (_Timer.ElapsedMilliseconds > 6000)
            {
                ++WhichStat;
                _Timer.Restart();
            }

            if (WhichStat > 15)
                WhichStat = 1;


            switch (WhichStat)
            {
                case 1:
                    if (ExperienceStats)
                    {
                        CurrentStats = "━━━━━ Experience Gained ━━━━━" + Environment.NewLine +
                                       "Run: " + ValueToString(SessionGainedExperience, ValueFormat.ShortNumber) + " (" + ValueToString(SessionGainedExperiencePerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                        "Today: " + ValueToString(TodayGainedExperience, ValueFormat.ShortNumber) + " (" + ValueToString(TodayGainedExperiencePerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                        "Total: " + ValueToString(TotalGainedExperience, ValueFormat.ShortNumber) + " (" + ValueToString(TotalGainedExperiencePerHour, ValueFormat.ShortNumber) + "/h)";
                        break;
                    }
                    else
                    {
                       ++WhichStat;
                        break;
                    }
                case 2:
                    if (MonsterKillStats)
                    {
                        CurrentStats = "━━━━━ Monster Killed ━━━━━" + Environment.NewLine +
                                    "Run: " + ValueToString(SessionMonsterKill, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(SessionMonsterKillPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Today: " + ValueToString(TodayMonsterKill, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(TodayMonsterKillPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Total: " + ValueToString(TotalMonsterKill, ValueFormat.ShortNumber) + " (" + ValueToString(TotalMonsterKillPerHour, ValueFormat.ShortNumber) + "/h)";
                    break;
                    }
                    else
                    {
                       ++WhichStat;
                       break;
                    }
                case 3:
                    if (EliteKillStats)
                    {
                        CurrentStats = "━━━━━ Elite Killed ━━━━━" + Environment.NewLine +
                                    "Run: " + ValueToString(SessionEliteKill, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(SessionEliteKillPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Today: " + ValueToString(TodayEliteKill, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(TodayEliteKillPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Total: " + ValueToString(TotalEliteKill, ValueFormat.ShortNumber) + " (" + ValueToString(TotalEliteKillPerHour, ValueFormat.ShortNumber) + "/h)";
                    break;
                    }
                    else
                    {
                       ++WhichStat;
                       break;
                    }
                case 4:
                    if (ItemDropStats)
                    {
                        CurrentStats = "━━━━━ Item Drop ━━━━━" + Environment.NewLine +
                                    "Run: " + ValueToString(SessionDropAll, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(SessionDropAllPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Today: " + ValueToString(TodayDropAll, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(TodayDropAllPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Total: " + ValueToString(TotalDropAll, ValueFormat.ShortNumber) + " (" + ValueToString(TotalDropAllPerHour, ValueFormat.ShortNumber) + "/h)";
                    break;
                    }
                    else
                    {
                       ++WhichStat;
                       break;
                    }
                case 5:
                    if (WhiteDropStats)
                    {
                        CurrentStats = "━━━━━ White Drop ━━━━━" + Environment.NewLine +
                                    "Run: " + ValueToString(SessionWhite, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(SessionWhitePerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Today: " + ValueToString(TodayWhite, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(TodayWhitePerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Total: " + ValueToString(TotalWhite, ValueFormat.ShortNumber) + " (" + ValueToString(TotalWhitePerHour, ValueFormat.ShortNumber) + "/h)";
                    break;
                    }
                    else
                    {
                       ++WhichStat;
                       break;
                    }
                case 6:
                    if (MagicDropStats)
                    {
                        CurrentStats = "━━━━━ Magic Drop ━━━━━" + Environment.NewLine +
                                    "Run: " + ValueToString(SessionMagic, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(SessionMagicPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Today: " + ValueToString(TodayMagic, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(TodayMagicPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Total: " + ValueToString(TotalMagic, ValueFormat.ShortNumber) + " (" + ValueToString(TotalMagicPerHour, ValueFormat.ShortNumber) + "/h)";
                    break;
                    }
                    else
                    {
                       ++WhichStat;
                       break;
                    }
                case 7:
                    if (RareDropStats)
                    {
                        CurrentStats = "━━━━━ Rare Drop ━━━━━" + Environment.NewLine +
                                    "Run: " + ValueToString(SessionRare, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(SessionRarePerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Today: " + ValueToString(TodayRare, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(TodayRarePerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Total: " + ValueToString(TotalRare, ValueFormat.ShortNumber) + " (" + ValueToString(TotalRarePerHour, ValueFormat.ShortNumber) + "/h)";
                    break;
                    }
                    else
                    {
                       ++WhichStat;
                       break;
                    }
                case 8:
                    if (LegendaryDropStats)
                    {
                        CurrentStats = "━━━━━ Legendary Drop ━━━━━" + Environment.NewLine +
                                    "Run: " + ValueToString(SessionLegendary, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(SessionLegendaryPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Today: " + ValueToString(TodayLegendary, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(TodayLegendaryPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Total: " + ValueToString(TotalLegendary, ValueFormat.ShortNumber) + " (" + ValueToString(TotalLegendaryPerHour, ValueFormat.ShortNumber) + "/h)";
                    break;
                    }
                    else
                    {
                       ++WhichStat;
                       break;
                    }
                case 9:
                    if (AncientDropStats)
                    {
                        CurrentStats = "━━━━━ Ancient Drop ━━━━━" + Environment.NewLine +
                                    "Run: " + ValueToString(SessionAncient, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(SessionAncientPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Today: " + ValueToString(TodayAncient, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(TodayAncientPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Total: " + ValueToString(TotalAncient, ValueFormat.ShortNumber) + " (" + ValueToString(TotalAncientPerHour, ValueFormat.ShortNumber) + "/h)";
                    break;
                    }
                    else
                    {
                       ++WhichStat;
                       break;
                    }
                case 10:
                    if (PrimalDropStats)
                    {
                        CurrentStats = "━━━━━ Primal Ancient Drop ━━━━━" + Environment.NewLine +
                                    "Run: " + ValueToString(SessionPrimal, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(SessionPrimalPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Today: " + ValueToString(TodayPrimal, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(TodayPrimalPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Total: " + ValueToString(TotalPrimal, ValueFormat.ShortNumber) + " (" + ValueToString(TotalPrimalPerHour, ValueFormat.ShortNumber) + "/h)";
                    break;
                    }
                    else
                    {
                       ++WhichStat;
                       break;
                    }
                case 11:
                case 12:
                    if (GameTimeStats)
                    {
                        CurrentStats = "━━━━━ Time in Game ━━━━━" + Environment.NewLine +
                                    "Run: " + SessionTime + Environment.NewLine +
                                    "         (" + SessionTimePlayed + " played)" + Environment.NewLine +
                                    "Today: " + TodayTime + Environment.NewLine +
                                    "           (" + TodayTimePlayed + " played)" + Environment.NewLine +
                                    "Total: " + TotalTime + Environment.NewLine +
                                    "          (" + TotalTimePlayed + " played)";
                    break;
                    }
                    else
                    {
                       WhichStat += 2;
                       break;
                    }
                case 13:
                    if (GoldStats)
                    {
                        CurrentStats = "━━━━━ Gold ━━━━━" + Environment.NewLine +
                                    "Run: " +  ValueToString(SessionGainedGold, ValueFormat.ShortNumber) + " (" + ValueToString(SessionGainedGoldPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Today: " + ValueToString(TodayGainedGold, ValueFormat.ShortNumber) + " (" + ValueToString(TodayGainedGoldPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Total: " + ValueToString(TotalGainedGold, ValueFormat.ShortNumber) + " (" + ValueToString(TotalGainedGoldPerHour, ValueFormat.ShortNumber) + "/h)";
                    break;
                    }
                    else
                    {
                       ++WhichStat;
                       break;
                    }
                case 14:
                    if (DeathStats)
                    {
                        CurrentStats = "━━━━━ Death ━━━━━" + Environment.NewLine +
                                    "Run: " + ValueToString(SessionDeath, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(SessionDeathPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Today: " + ValueToString(TodayDeath, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(TodayDeathPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Total: " + ValueToString(TotalDeath, ValueFormat.ShortNumber) + " (" + ValueToString(TotalDeathPerHour, ValueFormat.ShortNumber) + "/h)";
                    break;
                    }
                    else
                    {
                       ++WhichStat;
                       break;
                    }
                case 15:
                    if (BloodShardStats)
                    {
                        CurrentStats = "━━━━━ BloodShard Drop ━━━━━" + Environment.NewLine +
                                    "Run: " + ValueToString(SessionDropBloodShard, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(SessionDropBloodShardPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Today: " + ValueToString(TodayDropBloodShard, ValueFormat.NormalNumberNoDecimal) + " (" + ValueToString(TodayDropBloodShardPerHour, ValueFormat.ShortNumber) + "/h)" + Environment.NewLine +
                                    "Total: " + ValueToString(TotalDropBloodShard, ValueFormat.ShortNumber) + " (" + ValueToString(TotalDropBloodShardPerHour, ValueFormat.ShortNumber) + "/h)";
                    break;
                    }
                    else
                    {
                       WhichStat = 1;
                       break;
                    }


            }








            CurrentStatDecorator.Paint(RextX, RextY, Width, Height, HorizontalAlign.Left);







            /*
             
              HorizontalTopLabelList, VerticalTopLabelList ? 

             CurrentAccountLastMonth
             CurrentAccountLastWeek
             CurrentAccountYesterday

             long DamageDealtAll { get; }
             long DamageDealtCrit { get; }
             long DamageTaken { get; }
             long Healing { get; }
             double WalkYards { get; }

             double GainedExperiencePerHourFull { get; }


             double DamageDealtAllPerSecond { get; }
             double DamageDealtCritPerSecond { get; }
             double DamageTakenPerSecond { get; }
             double HealingPerSecond { get; }

             double MonsterKillPerLegendary { get; }
             double EliteKillPerLegendary { get; }
             */

        }


        private string ToReadableTime(long Time)
        {
         TimeSpan t = TimeSpan.FromMilliseconds(Time);
         string TotalTime = String.Empty;

            if (t.Hours == 0 && t.Minutes != 0)
                TotalTime = string.Format("{0:D1}m {1:D1}s", t.Minutes, t.Seconds);
            else if (t.Hours == 0 && t.Minutes == 0)
                TotalTime = string.Format("{0:D1}s", t.Seconds);
            else
                TotalTime = string.Format("{0:D1}h {1:D1}m {2:D1}s", t.Hours, t.Minutes, t.Seconds);

            return TotalTime;
        }





    }

}
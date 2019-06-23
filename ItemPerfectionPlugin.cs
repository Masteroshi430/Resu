// https://github.com/User5981/Resu
// Item Perfection plugin for TurboHUD version 10/05/2019 15:11
 
using System;
using System.Globalization;
using System.Linq;
using Turbo.Plugins.Default;
using System.Collections.Generic;

namespace Turbo.Plugins.Resu
{
    public class ItemPerfectionPlugin : BasePlugin, IInGameTopPainter
    {
        public Func<float> LeftFunc { get; set; }
        public Func<float> TopFunc { get; set; }
       
        public IFont PerfectFont { get; set; }
        public string PerfectText { get; set; }
        public IBrush ShadowBrush { get; set; }
        public float height { get; set; }
        
       
        public ItemPerfectionPlugin()
        {
            Enabled = true;
        }
 
        public override void Load(IController hud)
        {
            base.Load(hud);
            height = -80f;
                       
            LeftFunc = () =>
            {
                var uicMain = Hud.Inventory.GetHoveredItemMainUiElement();
                return uicMain.Rectangle.X + uicMain.Rectangle.Width * 0.75f; 
            };
            TopFunc = () =>
            {
                var uicTop = Hud.Inventory.GetHoveredItemTopUiElement(); 
                return uicTop.Rectangle.Bottom + (height / 1200.0f * Hud.Window.Size.Height); 
            };
                 
            PerfectText = "{0}";  
        }
 
         public void PaintTopInGame(ClipState clipState)
        {
        
         DrawTextLine();
        } 
       
       
        public bool IsPluginNameLoaded(string pluginName)
        {
            if (string.IsNullOrWhiteSpace(pluginName)) return false;

            var pluginNames = Hud.AllPlugins.Select(p => p.GetType().Name.ToLower()).ToList();
            return pluginNames.Contains(pluginName.ToLower());
        }
           
       
        private void DrawTextLine()
        {
            var item = Hud.Inventory.HoveredItem;
            if (item == null) return;
            bool WeaponDamageRerollCalculatorPlugin = IsPluginNameLoaded("WeaponDamageRerollCalculatorPlugin");
            if (WeaponDamageRerollCalculatorPlugin && item.SnoItem.HasGroupCode("weapons")) return;
            var Perfection = Math.Round(item.Perfection, 2);
            
            if (Perfection == 0) return;
            ShadowBrush = Hud.Render.CreateBrush(255, 0, 0, 0, 0.1f); 
            
            PerfectFont = Hud.Render.CreateFont("tahoma", 8, 255, 154, 105, 24, false, false, false);
            
            height = 80f;
            
            
            
            
            
            var affixes = "";
            var countAffixes = item.Affixes.Count();
            float advCount = 0f;
            float resCount = 0f;
            float lifeCount = 0f;
            float defCount = 0f;
            float offCount = 0f;
            var myClass = Hud.Game.Me.HeroClassDefinition.Name;
            var coreStat = "";
            if (myClass == "Monk" || myClass == "Demon Hunter") coreStat = "Dexterity";  
            if (myClass == "Witch Doctor" || myClass == "Wizard" || myClass == "Necromancer") coreStat = "Intelligence";
            if (myClass == "Barbarian" || myClass == "Crusader") coreStat = "Strength";
            
            
            
                
            foreach (var affix in item.Affixes)
                
                {
                    if (affix.ToString().Contains("Experience_Bonus") || affix.ToString().Contains("Item_Level_Requirement_Reduction") || affix.ToString().Contains("Magic_Find") || affix.ToString().Contains("Gold_Find") || affix.ToString().Contains("Movement_Scalar")) advCount += 1f; 
                    else if (affix.ToString().Contains("Resource_Max_Bonus") || affix.ToString().Contains("Resource_Regen_Per_Second") || affix.ToString().Contains("Resource_Cost_Reduction_Percent_All") || affix.ToString().Contains("Resource_On_Crit")) resCount += 1f;
                    else if (affix.ToString().Contains("Hitpoints_Max_Percent_Bonus_Item") || affix.ToString().Contains("Vitality_Item") || affix.ToString().Contains("Spending_Resource_Heals_Percent") || affix.ToString().Contains("Hitpoints_Regen_Per_Second") || affix.ToString().Contains("Hitpoints_On_Hit") || affix.ToString().Contains("Health_Globe_Bonus") || affix.ToString().Contains("Hitpoints_On_Kill") || affix.ToString().Contains("Gold_PickUp_Radius")) lifeCount += 1f;
                    else if (affix.ToString().Contains("Armor") || affix.ToString().Contains("Damage_Percent_Reduction_From_Elites") || affix.ToString().Contains("Damage_Percent_Reduction_From_Melee") || affix.ToString().Contains("Block_Chance_Bonus_Item") || affix.ToString().Contains("Dodge_Chance_Bonus_Item") || affix.ToString().Contains("CrowdControl_Reduction")|| affix.ToString().Contains("Damage_Percent_Reduction_From_Ranged") || affix.ToString().Contains("Thorns")|| affix.ToString().Contains("Resistance")|| affix.ToString().Contains("Item_Indestructible")) defCount += 1f;
                    else if (affix.ToString().Contains(coreStat) || affix.ToString().Contains("Damage_Percent_Bonus_Vs_Monster_Type")|| affix.ToString().Contains("Power_Cooldown_Reduction_Percent_All")  || affix.ToString().Contains("Damage_Percent_Bonus_Vs_Elites") || affix.ToString().Contains("Weapon_On_Hit_Percent_Bleed") || affix.ToString().Contains("Item_Power_Passive")|| affix.ToString().Contains("Splash_Damage_Effect_Percent")|| affix.ToString().Contains("Damage_Weapon_Bonus_Delta") || affix.ToString().Contains("Damage_Delta") || affix.ToString().Contains("Damage_Weapon_Percent_All") || affix.ToString().Contains("Damage_Weapon_Delta") || affix.ToString().Contains("Damage_Dealt_Percent_Bonus") || affix.ToString().Contains("Power_Damage_Percent_Bonus")|| affix.ToString().Contains("Crit_Damage_Percent") || affix.ToString().Contains("Attacks_Per_Second_Item_Percent") || affix.ToString().Contains("Crit_Percent_Bonus_Capped") ||affix.ToString().Contains("Power_Cooldown_Reduction_Percent") || affix.ToString().Contains("Attacks_Per_Second_Percent") || affix.ToString().Contains("On_Hit_Blind_Proc_Chance") || affix.ToString().Contains("On_Hit_Stun_Proc_Chance") || affix.ToString().Contains("On_Hit_Fear_Proc_Chance") || affix.ToString().Contains("On_Hit_Immobilize_Proc_Chance") || affix.ToString().Contains("On_Hit_Slow_Proc_Chance") || affix.ToString().Contains("On_Hit_Knockback_Proc_Chance") || affix.ToString().Contains("On_Hit_Freeze_Proc_Chance")|| affix.ToString().Contains("On_Hit_Chill_Proc_Chance")) offCount += 1f;
                    else if (affix.ToString().Contains("Sockets")) countAffixes -= 1;
                    else if (coreStat != "Dexterity" && affix.ToString().Contains("Intelligence") || coreStat != "Dexterity" && affix.ToString().Contains("Strength")) defCount += 1f;
                    else if (coreStat != "Intelligence" && affix.ToString().Contains("Dexterity") || coreStat != "Intelligence" && affix.ToString().Contains("Strength")) defCount += 1f;
                    else if (coreStat != "Strength" && affix.ToString().Contains("Intelligence") || coreStat != "Strength" && affix.ToString().Contains("Dexterity")) defCount += 1f;
                    else {affixes += Environment.NewLine + "Do a partial screenshot of this to help improving the plugin :" + Environment.NewLine + affix;}  // I'll remove this one day... 
                }
                
            
            var offPercent = Math.Round(offCount/countAffixes*100, 2).ToString();
            var offSymbol = "\u2694";
            var offSpace = "";
            if (offPercent == "0") {offPercent = "";} 
            else {
                  if (offPercent.Length == 1) offSpace = "     ";
                  if (offPercent.Length == 2) offSpace = "     ";
                  if (offPercent.Length == 3) offSpace = "   ";
                  if (offPercent.Length == 4) offSpace = "  ";
                  if (offPercent.Length == 5) offSpace = " ";
                  offPercent = Environment.NewLine + offSymbol + offSpace + offPercent + "%";
                 } 
                 
            var defPercent = Math.Round(defCount/countAffixes*100, 2).ToString();
            var defSymbol = "\u26e8";
            var defSpace = "";
            if (defPercent == "0") {defPercent = "";} 
            else {
                  if (defPercent.Length == 1) defSpace = "      ";
                  if (defPercent.Length == 2) defSpace = "      ";
                  if (defPercent.Length == 3) defSpace = "    ";
                  if (defPercent.Length == 4) defSpace = "   ";
                  if (defPercent.Length == 5) defSpace = "  ";
                  defPercent = Environment.NewLine + defSymbol + defSpace + defPercent + "%";
                 }
            
            var lifePercent = Math.Round(lifeCount/countAffixes*100, 2).ToString();
            var lifeSymbol = "\u2764";
            var lifeSpace = "";
            if (lifePercent == "0") {lifePercent = "";} 
            else {
                  if (lifePercent.Length == 1) lifeSpace = "     ";
                  if (lifePercent.Length == 2) lifeSpace = "     ";
                  if (lifePercent.Length == 3) lifeSpace = "   ";
                  if (lifePercent.Length == 4) lifeSpace = "  ";
                  if (lifePercent.Length == 5) lifeSpace = " ";
                  lifePercent = Environment.NewLine + lifeSymbol + lifeSpace + lifePercent + "%";
                 }
            
            var resPercent = Math.Round(resCount/countAffixes*100, 2).ToString();
            var resSymbol = "\ud83d\udd2e";
            var resSpace = "";
            if (resPercent == "0") {resPercent = "";}
            else {
                  if (resPercent.Length == 1) resSpace = "     ";
                  if (resPercent.Length == 2) resSpace = "     ";
                  if (resPercent.Length == 3) resSpace = "   ";
                  if (resPercent.Length == 4) resSpace = "  ";
                  if (resPercent.Length == 5) resSpace = " ";
                  resPercent = Environment.NewLine + resSymbol + resSpace + resPercent + "%";
                 }
            
            var advPercent = Math.Round(advCount/countAffixes*100, 2).ToString();
            var advSymbol = "\ud83d\udc62";
            var advSpace = "";
            if (advPercent == "0") {advPercent = "";} 
            else {
                  if (advPercent.Length == 1) advSpace = "     ";
                  if (advPercent.Length == 2) advSpace = "     ";
                  if (advPercent.Length == 3) advSpace = "   ";
                  if (advPercent.Length == 4) advSpace = "  ";
                  if (advPercent.Length == 5) advSpace = " ";
                  advPercent = Environment.NewLine + advSymbol + advSpace + advPercent + "%";
                 }

            var perfSymbol = "\u2713";
            var perfSpace = "";
            var perfPercent = Perfection.ToString();
                  if (perfPercent.Length == 1) perfSpace = "     ";
                  if (perfPercent.Length == 2) perfSpace = "       ";
                  if (perfPercent.Length == 3) perfSpace = "   ";
                  if (perfPercent.Length == 4) perfSpace = "    ";
                  if (perfPercent.Length == 5) perfSpace = "   ";
                  
            PerfectFont.SetShadowBrush(50, 255, 234, 137, false);
            var text = string.Format(PerfectText, perfSymbol + perfSpace + Perfection + "%" + offPercent + defPercent + lifePercent + resPercent + advPercent + Environment.NewLine + affixes);
            var layout = PerfectFont.GetTextLayout(text); 
            PerfectFont.DrawText(layout, LeftFunc(), TopFunc());
            
        }
        
    }
}
            
            
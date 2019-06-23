// https://github.com/User5981/Resu
// Next Hero Plugin for TurboHUD Version 25/09/2019 06:36

using System;
using System.Globalization;
using Turbo.Plugins.Default;
using System.Linq;

namespace Turbo.Plugins.Resu
{
    public class NextHeroPlugin : BasePlugin, INewAreaHandler, IInGameTopPainter
    {
        private IWatch _watch; 
        public TopLabelDecorator NextHeroDecorator { get; set; }
        public string NextHeroText{ get; set; }
        public int maxX { get; set; }
        public int maxY { get; set; }
        

        
        public NextHeroPlugin()
        {
            Enabled = true;
            NextHeroText = string.Empty;
            
        }
        
        public override void Load(IController hud)
        {
            base.Load(hud);
            _watch = Hud.Time.CreateWatch();
            maxX = Hud.Window.Size.Width; 
            maxY = Hud.Window.Size.Height;
            
            NextHeroDecorator = new TopLabelDecorator(Hud)
            {
                 TextFont = Hud.Render.CreateFont("Microsoft Sans Serif", 9, 225, 255, 255, 255, false, false, 100, 0, 0, 0, true), 
                 TextFunc = () => NextHeroText,
            };
        
        }

       
       public void PaintTopInGame(ClipState clipState)
        {

             if (Hud.Render.UiHidden) return;
             if (clipState != ClipState.BeforeClip) return;
             var uiInv = Hud.Inventory.InventoryMainUiElement; 
             if (uiInv.Visible) return;
             if (!Hud.Game.IsInTown) return;
             NextHeroText = string.Empty;
                
                
                 var PosY = (maxY/4)*3-80;
                 var PosX = (maxX/8)*7-80;
                 var timeInGame = _watch.ElapsedMilliseconds;
                 var Heroes = Hud.AccountHeroes.OrderBy(Hero => Hero.PlayedSeconds);
                 var TimePlayedMe = Hud.Game.Me.Hero.PlayedSeconds + (int)(timeInGame/1000); 
                
             foreach (var Hero in Heroes.Where(t => t.PlayedSeconds < TimePlayedMe && t.Hardcore == Hud.Game.Me.Hero.Hardcore && t.Seasonal == Hud.Game.Me.Hero.Seasonal && t.Name != Hud.Game.Me.Hero.Name).Take(1))
             {
                var Difference = (TimePlayedMe - Hero.PlayedSeconds);
                
                 TimeSpan t = TimeSpan.FromSeconds(Difference);
                 string Diff = string.Format("{0:D1}h {1:D1}m {2:D1}s", (int)t.TotalHours, t.Minutes, t.Seconds);
                 
                NextHeroText = "━━━━━━━ Next Hero to play ━━━━━━━" + Environment.NewLine + Hero.Name + " [" + Hero.ClassDefinition.HeroClass + "]" + Environment.NewLine + Diff + " behind " + Hud.Game.Me.Hero.Name;
                
                var HeroTexture = Hud.Texture.GetTexture(890155253);
                var SeasonTexture = Hud.Texture.GetTexture(1944779632);
                var HardcoreTexture = Hud.Texture.GetTexture(2946806416);
                
                if (Hero.ClassDefinition.HeroClass.ToString() == "Barbarian")
                   {
                    if (Hero.IsMale) HeroTexture = Hud.Texture.GetTexture(3921484788);
                    else HeroTexture = Hud.Texture.GetTexture(1030273087);  
                   }
                else if (Hero.ClassDefinition.HeroClass.ToString() == "Crusader")
                   {
                    if (Hero.IsMale) HeroTexture = Hud.Texture.GetTexture(3742271755);
                    else HeroTexture = Hud.Texture.GetTexture(3435775766);  
                   } 
                else if (Hero.ClassDefinition.HeroClass.ToString() == "DemonHunter")
                   {
                    if (Hero.IsMale) HeroTexture = Hud.Texture.GetTexture(3785199803);
                    else HeroTexture = Hud.Texture.GetTexture(2939779782);  
                   } 
                else if (Hero.ClassDefinition.HeroClass.ToString() == "Monk")
                   {
                    if (Hero.IsMale) HeroTexture = Hud.Texture.GetTexture(2227317895);
                    else HeroTexture = Hud.Texture.GetTexture(2918463890);  
                   } 
                else if (Hero.ClassDefinition.HeroClass.ToString() == "Necromancer")
                   {
                    if (Hero.IsMale) HeroTexture = Hud.Texture.GetTexture(3285997023); 
                    else HeroTexture = Hud.Texture.GetTexture(473831658);   
                   }
                else if (Hero.ClassDefinition.HeroClass.ToString() == "WitchDoctor")
                   {
                    if (Hero.IsMale) HeroTexture = Hud.Texture.GetTexture(3925954876);
                    else HeroTexture = Hud.Texture.GetTexture(1603231623);  
                   }
                else if (Hero.ClassDefinition.HeroClass.ToString() == "Wizard")
                   {
                    if (Hero.IsMale) HeroTexture = Hud.Texture.GetTexture(44435619);
                    else HeroTexture = Hud.Texture.GetTexture(876580014);
                   }
                
                
                HeroTexture.Draw(PosX-50, PosY, 44.6f, 48.2f, 0.59f);
                if (Hero.Seasonal) SeasonTexture.Draw(PosX-50, PosY-30, 42f, 84.666f, 0.59f);
                if (Hero.Hardcore) HardcoreTexture.Draw(PosX-20, PosY+35, 22f, 28.444f, 0.59f);
                NextHeroDecorator.Paint(PosX, PosY, 50f, 50f, HorizontalAlign.Left);
                
             }
              
    
        }
        
        public void OnNewArea(bool newGame, ISnoArea area)
        {
            if (newGame)
                _watch.Restart();
        }
        
        
    }
}
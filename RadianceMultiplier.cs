using Modding;
using Satchel.BetterMenus;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SFCore.Utils;

namespace RadianceMultiplier {
    public class RadianceMultiplier: Mod, ICustomMenuMod, ILocalSettings<LocalSettings> {
        private Menu menuRef = null;
        public static RadianceMultiplier instance;

        public RadianceMultiplier() : base("Radiance Multiplier") => instance = this;
        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();


        public static LocalSettings localSettings { get; private set; } = new();
        public void OnLoadLocal(LocalSettings s) => localSettings = s;
        public LocalSettings OnSaveLocal() => localSettings;

        public bool ToggleButtonInsideMenu => false;
        
        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates) {
            menuRef ??= new Menu(
                name: "Radiance Multiplier",
                elements: new Element[] {
                    new CustomSlider(
                        name: "Number of Radiances",
                        storeValue: val => localSettings.numRadiances = (int)val,
                        loadValue: () => localSettings.numRadiances,
                        minValue: 1,
                        maxValue: 10,
                        wholeNumbers: true
                    )
                }
            );
            
            return menuRef.GetMenuScreen(modListMenu);
        }

        public override void Initialize() {
            Log("Initializing");

            ModHooks.NewGameHook += AddFinder;
            ModHooks.SavegameLoadHook += Load;

            Log("Initialized");
        }

        private void Load(int _) => AddFinder();

        private void AddFinder() => GameManager.instance.gameObject.AddComponent<AbsFinder>();
    }

    public class LocalSettings {
        public int numRadiances = 1;
    }
}
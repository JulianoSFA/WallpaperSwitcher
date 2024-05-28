using System;
using Force.DeepCloner;
using Microsoft.Xna.Framework;
using Netcode;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;

namespace YourProjectName
{
    internal sealed class ModEntry : Mod
    {
        Dictionary<string, NetString> floors = new Dictionary<string, NetString>();
        Dictionary<string, NetString> wallpapers = new Dictionary<string, NetString>();

        private void Print(string e)
        {
            this.Monitor.Log(e, LogLevel.Debug);
        }

        public override void Entry(IModHelper helper)
        {
            Helper.Events.GameLoop.SaveLoaded += this.OnLocationListChanged;
            Helper.Events.Player.Warped += this.OnLocationListChanged;
            Helper.Events.Player.InventoryChanged += this.GetUsedDecorationAndRetrieve;
        }

        private void GetUsedDecorationAndRetrieve(object? sender, InventoryChangedEventArgs e)
        {
            if (!e.IsLocalPlayer) return;

            DecoratableLocation location = (DecoratableLocation)Game1.player.currentLocation;
            if (!location.GetType().IsSubclassOf(typeof(DecoratableLocation))) return;

            List<Item> usedDecorations = new List<Item>();
            foreach (Item item in e.Removed)
            {
                if (item.Name == "Wallpaper" || item.Name == "Flooring") usedDecorations.Add(item);
            }

            if (usedDecorations.Count < 1) return;

            

            foreach(Item item in usedDecorations)
            {
                if (item.Name == "Wallpaper")
                {
                    var difference = this.wallpapers.Except(location.appliedWallpaper.FieldDict);
                    foreach (var item2 in difference) Game1.player.addItemToInventory(new Wallpaper(Int32.Parse(item2.Value), false));
                    this.SaveWallpapers(location);

                }

                if (item.Name == "Flooring")
                {
                    var difference = this.floors.Except(location.appliedFloor.FieldDict);
                    foreach (var item2 in difference) Game1.player.addItemToInventory(new Wallpaper(Int32.Parse(item2.Value), true));
                    this.SaveFloors(location);
                }
            }
        }


        private void OnLocationListChanged(object? sender, EventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            var currentLocation = Game1.player.currentLocation;
            var isDecoratable = currentLocation.GetType().IsSubclassOf(typeof(DecoratableLocation));
            if (isDecoratable) {
                this.SaveFloors((DecoratableLocation)currentLocation);
                this.SaveWallpapers((DecoratableLocation)currentLocation);
            }
        }

        private void SaveFloors(DecoratableLocation farmHouse)
        {
            this.floors = farmHouse.appliedFloor.FieldDict.DeepClone();
        }

        private void SaveWallpapers(DecoratableLocation farmHouse)
        {
            this.wallpapers = farmHouse.appliedWallpaper.FieldDict.DeepClone();
        }
    }
}
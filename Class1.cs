using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace YourProjectName
{
    internal sealed class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            Helper.Events.GameLoop.SaveLoaded += this.OnLocationListChanged;
            Helper.Events.Player.Warped += this.OnLocationListChanged;
        }


        private void OnLocationListChanged(object? sender, EventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            // print button presses to the console window
            this.Monitor.Log($"{Game1.player.currentLocation.GetType()}", LogLevel.Debug);
        }
    }
}
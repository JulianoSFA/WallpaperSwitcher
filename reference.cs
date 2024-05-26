using StardewModdingAPI.Events;
using StardewModdingAPI;
using StardewValley.Characters;
using StardewValley.Locations;
using StardewValley.Objects;
using StardewValley;

public class MainModClass : Mod
{
    // Fields
    private static int NO_CHANGE = -333_000;
    private List<int> wallPaper;
    private List<int> floors;

    // Methods
    public override void Entry(params object[] objects)
    {
        GameEvents.add_OneSecondTick(new EventHandler(this.OneSecondTick));
        PlayerEvents.add_InventoryChanged(new EventHandler<EventArgsInventoryChanged>(this.InventoryChanged));
    }

    private int FloorsChangedFor(FarmHouse farmHouse)
    {
        int num;
        List<int> floor = farmHouse.floor;
        if (((floor == null) || (floor.Count == 0)) || ((this.floors == null) || (this.floors.Count == 0)))
        {
            num = 0;
        }
        else
        {
            int num2 = 0;
            while (true)
            {
                if (num2 >= floor.Count)
                {
                    num = NO_CHANGE;
                }
                else
                {
                    int num3 = this.floors[num2];
                    if (num3.Equals(floor[num2]))
                    {
                        num2++;
                        continue;
                    }
                    num = this.floors[num2];
                }
                break;
            }
        }
        return num;
    }

    private void getAndAddOldPaper(int which, bool isFloor)
    {
        Wallpaper wallpaper = new Wallpaper(which, isFloor);
        Game1.player.addItemToInventory(wallpaper);
        string str = "wallpaper ";
        if (isFloor)
        {
            str = "floor ";
        }
        object[] objArray1 = new object[] { "Added ", str, wallpaper.parentSheetIndex, " to player" };
        Log.Info(string.Concat(objArray1));
    }

    private Wallpaper GetRemovedWallpaper(List<ItemStackChange> changedItems)
    {
        using (List<ItemStackChange>.Enumerator enumerator = changedItems.GetEnumerator())
        {
            while (true)
            {
                if (!enumerator.MoveNext())
                {
                    break;
                }
                ItemStackChange current = enumerator.Current;
                if ((current.get_Item().GetType() == typeof(Wallpaper)) && (current.get_ChangeType() == 0))
                {
                    return (Wallpaper)current.get_Item();
                }
            }
        }
        return null;
    }

    private void InventoryChanged(object sender, EventArgs e)
    {
        if ((Game1.hasLoadedGame && (Game1.player.currentLocation != null)) && ((Game1.player.currentLocation.GetType() == typeof(FarmHouse)) && (Game1.activeClickableMenu == null)))
        {
            FarmHouse currentLocation = Game1.player.currentLocation as FarmHouse;
            Wallpaper removedWallpaper = this.GetRemovedWallpaper(((EventArgsInventoryChanged)e).get_Removed());
            if (removedWallpaper != null)
            {
                int num;
                if (removedWallpaper.isFloor)
                {
                    num = this.FloorsChangedFor(currentLocation);
                    this.saveFloors(currentLocation);
                }
                else
                {
                    num = this.WallPapersChangedFor(currentLocation);
                    this.saveWallPapers(currentLocation);
                }
                if (num != NO_CHANGE)
                {
                    this.getAndAddOldPaper(num, removedWallpaper.isFloor);
                }
                else
                {
                    Game1.player.addItemToInventory(removedWallpaper);
                }
            }
        }
    }

    private void OneSecondTick(object sender, EventArgs e)
    {
        if ((Game1.hasLoadedGame && (Game1.player.currentLocation != null)) && (Game1.player.currentLocation.GetType() == typeof(FarmHouse)))
        {
            FarmHouse currentLocation = Game1.player.currentLocation as FarmHouse;
            if (((this.wallPaper == null) || (this.wallPaper.Count == 0)) ? (currentLocation.wallPaper.Count != 0) : false)
            {
                Log.Info("Initializing wallpapers");
                this.saveWallPapers(currentLocation);
            }
            if (!(((this.floors == null) || (this.floors.Count == 0)) ? (currentLocation.floor.Count == 0) : true))
            {
                Log.Info("Initializing floors");
                this.saveFloors(currentLocation);
            }
        }
    }

    private void saveFloors(FarmHouse farmHouse)
    {
        List<int> floor = farmHouse.floor;
        this.floors = new List<int>();
        for (int i = 0; i < floor.Count; i++)
        {
            this.floors.Add(floor[i]);
        }
    }

    private void saveWallPapers(FarmHouse farmHouse)
    {
        List<int> wallPaper = farmHouse.wallPaper;
        this.wallPaper = new List<int>();
        for (int i = 0; i < wallPaper.Count; i++)
        {
            this.wallPaper.Add(wallPaper[i]);
        }
    }

    private int WallPapersChangedFor(FarmHouse farmHouse)
    {
        int num;
        List<int> wallPaper = farmHouse.wallPaper;
        if (((wallPaper == null) || (wallPaper.Count == 0)) || ((this.wallPaper == null) || (this.wallPaper.Count == 0)))
        {
            num = 0;
        }
        else
        {
            int num2 = 0;
            while (true)
            {
                if (num2 >= wallPaper.Count)
                {
                    num = NO_CHANGE;
                }
                else
                {
                    int num3 = this.wallPaper[num2];
                    if (num3.Equals(wallPaper[num2]))
                    {
                        num2++;
                        continue;
                    }
                    num = this.wallPaper[num2];
                }
                break;
            }
        }
        return num;
    }
}


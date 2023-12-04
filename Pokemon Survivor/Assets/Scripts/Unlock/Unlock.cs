using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Unlock
{
    public bool locked;
    [TextArea(3, 5)]
    public string unlockMessage;
    public string unlockSave;

    public void CheckUnlock(List<Player> players, List<LockedPlayer> lockedPlayers)
    {
        foreach(LockedPlayer player in lockedPlayers)
        {
            switch(player.name)
            {
                case "Mew":

                    int totalEvolutions = 0;

                    foreach(Player ply in players)
                    {
                        if(ply.mainPlayer.name != player.name && ply.evoloved.Equals(ply.totalEvolutions))
                            totalEvolutions++;
                    }

                    if(totalEvolutions.Equals(players.Count - 1))
                        PlayerPrefs.SetInt(DataController.mewLocked, 1);

                break;
            }
        }
    }

    public void CheckUnlock(List<GameObject> stages, List<LockedStage> lockedStages)
    {
        foreach(LockedStage lockedStage in lockedStages)
        {
            switch(lockedStage.name)
            {
                case "Dark Crater":

                    // we only take this 2 levels we only need those levels to be completed.

                    int stagesCompleted = PlayerPrefs.GetInt(DataController.cerulean) + PlayerPrefs.GetInt(DataController.jungle);

                    if(stagesCompleted == 2)
                        PlayerPrefs.SetInt(lockedStage.unlock.unlockSave, 1);

                break;
            }
        }
    }
}

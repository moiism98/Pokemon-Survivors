using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectPlayer : MonoBehaviour
{
    public Button selectPlayer;
    public Text blocked;
    public Text unlockText;
    public Text evoStones;
    public Button evoButton;
    public Text shinyStones;
    public Button shinyButton;
    public List<GameObject> selectablePlayers = new List<GameObject>();
    public List<Player> players = new List<Player>();
    public List<LockedPlayer> lockedPlayers = new List<LockedPlayer>();
    private int playerCount = 0;
    bool reseted = false;
    bool update = false;
    bool locked = false;

    private void Start()
    {
        AddPlayers(); 
    }

    private void Update()
    {
        CheckUnlocks();

        // if we went back on the menu
        if(reseted)
        {
            LoopPlayers(); // loop the players again to show as default

            reseted = false;
        }

        // we show the evolution button when we have evo stones and pokemon still have evolutions remaining

        if(int.TryParse(evoStones.text, out int stones))
        {
            if(stones != 0 && players[playerCount].evoloved != players[playerCount].totalEvolutions && !locked)
                evoButton.gameObject.SetActive(true);
            else
                evoButton.gameObject.SetActive(false);
        }

        if(int.TryParse(shinyStones.text, out int shinyStone))
        {
            if(shinyStone != 0 && !players[playerCount].shiny && !locked)
                shinyButton.gameObject.SetActive(true);
            else
                shinyButton.gameObject.SetActive(false);
        }

        if(update)
        {
            if(selectablePlayers.Count > 0)
                ResetSelectableList();

            AddPlayers();
        }
    }

    public void AddPlayers()
    { 
        // we have to load the evolutions done first from the player prefs.

        int pikachuStones = PlayerPrefs.GetInt(DataController.pikachuStones);
        int machopStones = PlayerPrefs.GetInt(DataController.machopStones);
        int eeveeStones = PlayerPrefs.GetInt(DataController.eeveeStones);

        // we have to load the shiny value  from the player prefs.

        int pikachuShiny = PlayerPrefs.GetInt(DataController.pikachuShiny);
        int machopShiny = PlayerPrefs.GetInt(DataController.machopShiny);
        int eeveeShiny = PlayerPrefs.GetInt(DataController.eeveeShiny);
        int mewShiny = PlayerPrefs.GetInt(DataController.mewShiny);

        // load the locked value from player prefs

        int mewLocked = PlayerPrefs.GetInt(DataController.mewLocked);


        // at the beggining we add the selectable players to the list to show

        foreach (Player player in players)
        {
            // we load the respectives stone values

            switch (player.mainPlayer.name)
            {
                case "Pikachu": 

                    player.evoloved = pikachuStones; 
                    player.shiny = pikachuShiny == 1 ? true : false;
                    
                break;

                case "Machop": 
                
                    player.evoloved = machopStones; 
                    player.shiny = machopShiny == 1 ? true : false;

                break;

                case "Eevee": 
                
                    player.evoloved = eeveeStones; 
                    player.shiny = eeveeShiny == 1 ? true : false;
                    
                break;

                case "Mew":  player.shiny = mewShiny == 1 ? true : false; break;
            }

            if(!player.shiny) // check ig the pokemon it's shiny or not
            {
                // if not

                if(player.evoloved != 0) // if the pokemon has evolved before, we select the current evolution
                {
                    selectablePlayers.Add(player.evolutions[player.evoloved - 1]);
                }
                else // if the pokemon has not evolved yet, so doesn't have evolutions actives, we select the main player, in other words the pokemon base
                    selectablePlayers.Add(player.mainPlayer);
            }
            else // if it is, we simply take the pokemon on shiny list
                selectablePlayers.Add(player.shinies[player.evoloved]);
        }

        // we load the unlocks values

        foreach(LockedPlayer lPlayer in lockedPlayers)
        {
            switch(lPlayer.name)
            {
                case "Mew": lPlayer.unlock.locked = mewLocked == 1 ? false: true; break;
            }
        }
        
        selectablePlayers[playerCount].SetActive(true);

        update = false;
    }

    private void CheckUnlocks()
    {
        foreach(LockedPlayer lockedPlayer in lockedPlayers)
        {
            lockedPlayer.unlock.CheckUnlock(players, lockedPlayers);
        }
    }

    // separate method for shinies

    public void ShinyPokemon()
    {
        int shinyStonesCount = 0;

        if(int.TryParse(shinyStones.text, out int stones))
            shinyStonesCount = stones;

        if(shinyStonesCount > 0)
        {
            if(!players[playerCount].shiny)
            {

                // pokemon it's now shiny

                players[playerCount].shiny = true;

                // use the stone and update the UI and the values
                
                int savedStones = PlayerPrefs.GetInt(DataController.shinyStoneSave);

                --savedStones;

                PlayerPrefs.SetInt(DataController.shinyStoneSave, savedStones);

                shinyStones.text = savedStones.ToString();

                // save the evolved values

                switch (players[playerCount].mainPlayer.name)
                {
                    case "Pikachu": PlayerPrefs.SetInt(DataController.pikachuShiny, 1); break;
                    case "Machop": PlayerPrefs.SetInt(DataController.machopShiny, 1); break;
                    case "Eevee": PlayerPrefs.SetInt(DataController.eeveeShiny, 1); break;
                    case "Mew": PlayerPrefs.SetInt(DataController.mewShiny, 1); break;

                }
            }
        }

        update = true;
    }

    public void EvolvePokemon()
    {
        int evoStonesCount = 0;

        // we check if we have or not evolution stones

        if(int.TryParse(evoStones.text, out int stones))
            evoStonesCount = stones;

        // if we do

        if(evoStonesCount > 0)
        {   
            // check if that pokemon that we want to evolve has evolutions remainings

            if(players[playerCount].evoloved < players[playerCount].totalEvolutions)
            {
                // if he has, we disable it (if not it's still showing in the menu) and add 1 to the evolved count of that pokemon, the list it's going to update alone
                // in the update method

                selectablePlayers[playerCount].SetActive(false);
                players[playerCount].evoloved++;

                // use the stone and update the UI and the values
                
                int savedStones = PlayerPrefs.GetInt(DataController.evoStoneSave);

                --savedStones;

                PlayerPrefs.SetInt(DataController.evoStoneSave, savedStones);

                evoStones.text = savedStones.ToString();

                // save the evolved values

                switch (players[playerCount].mainPlayer.name)
                {
                    case "Pikachu": PlayerPrefs.SetInt(DataController.pikachuStones, players[playerCount].evoloved); break;
                    case "Machop": PlayerPrefs.SetInt(DataController.machopStones, players[playerCount].evoloved); break;
                    case "Eevee": PlayerPrefs.SetInt(DataController.eeveeStones, players[playerCount].evoloved); break;
                }

                // we have to save this evolved count in our PlayerPrefs, there has to be as much player prefs as base pokemons are, 3 in our case at the moment.
            }
        }

        update = true;
    }

    public void ResetSelectableList()
    {
        // we have to update the current list of selectable players, and have to convert it in an array (or a list if its an array for default)
        // to avoid a collection error.

        // we simply remove every single element of the list (remember, the update method it's updating the list)

        foreach(GameObject selectedPlayer in selectablePlayers)
            selectedPlayer.SetActive(false);

        foreach(GameObject player in selectablePlayers.ToArray())
            selectablePlayers.Remove(player);
    }

    public void SelectOurPlayer()
    {
        // we save the player name to find it in the next game scene.
        MenuController.playerName = selectablePlayers[playerCount].name;
    }

    public void NextPlayer()
    {
        // we want to go from the last element of the list to the first one
        // so if we are NOT in the last position of the list we add 1 to the count
        if (playerCount != selectablePlayers.Count - 1)
            playerCount++;
        else // if we reach the last one we put the count at 0 (and show the first element) so we are at the first list element.
            playerCount = 0;

        IsBlocked();

        LoopPlayers();
    }

    public void PreviousPlayer()
    {
        // the opposite here if we are NOT at the first position of the list
        // we add 1 to the count
        if (playerCount != 0)
            playerCount--;
        else // if we reach the 0 value, we show the last list element.
            playerCount = selectablePlayers.Count - 1;
        
        IsBlocked();

        LoopPlayers();
    }

    private void IsBlocked()
    {
        // save the current player name selected at menu

        string playerLockedName = players[playerCount].mainPlayer.name;

        // take with the player from our locked list

        LockedPlayer player = lockedPlayers.Find(player => player.name == playerLockedName);
        
        // if we found it 

        if(player != null)
        {
            // and it's locked

            if(player.unlock.locked) // show the text
                ShowLockedMessage(player);
            else // it's not locked, we show the select player button
                ShowSelectPlayerButton();
        }
        else // if we don't find it, we show the select player button
            ShowSelectPlayerButton();
        
    }

    private void ShowSelectPlayerButton()
    {
        locked = false;

        blocked.gameObject.SetActive(false);

        selectPlayer.gameObject.SetActive(true);
    }

    private void ShowLockedMessage(LockedPlayer player)
    {
        locked = true;

        unlockText.text = player.unlock.unlockMessage; // put the unlocked message at the UI text

        blocked.gameObject.SetActive(true);

        selectPlayer.gameObject.SetActive(false);
    }

    public void ResetPlayerCount()
    {
        // we reset the count when we go back in the menu
        playerCount = 0;
        reseted = true;
    }

    void LoopPlayers()
    {
        for (int player = 0; player < selectablePlayers.Count; player++)
        {
            // if the current element found its not the next player we disable it
            if (player != playerCount)
                selectablePlayers[player].SetActive(false);
            else // if we found it, enable it
                selectablePlayers[player].SetActive(true);
        }
    }
}

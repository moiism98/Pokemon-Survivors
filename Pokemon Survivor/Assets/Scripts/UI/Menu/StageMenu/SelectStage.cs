using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectStage : MonoBehaviour
{
    public Button selectStage;
    public GameObject lockedBox;
    public Text lockedText;
    public List<GameObject> stages = new List<GameObject>();
    public List<LockedStage> lockedStages = new List<LockedStage>();


    public int stageCount = 0;

    bool reseted = false;

    void Start()
    {
        // if the tutorial has not been complete
        // the game will offer you the tutorial stage as default
        
        if(PlayerPrefs.GetInt("TutorialComplete") == 0)
            TutorialFirst();
    }

    void Update()
    {
        CheckLockedState();

        CheckUnlocks();

        // if we go back to the select player menu
        // we loop the stages list again to put back his order
        if (reseted)
        {
            LoopStages();
            reseted = false;
        }
    }

    private void CheckUnlocks()
    {
        foreach(LockedStage lockedPlayer in lockedStages)
        {
            lockedPlayer.unlock.CheckUnlock(stages, lockedStages);
        }
    }

    private void CheckLockedState()
    {
        foreach(LockedStage lockedStage in lockedStages)
        {
            switch(lockedStage.name)
            {
                case "Dark Crater":

                    int isLocked = PlayerPrefs.GetInt(lockedStage.unlock.unlockSave);

                    if(isLocked != 0)
                        lockedStage.unlock.locked = !lockedStage.unlock.locked;

                break;
            }
        }
    }


    // we activate the tutorial stage as default
    void TutorialFirst()
    {
        stages[0].SetActive(false);
        stages[stages.Count - 1].SetActive(true);
        stageCount = stages.Count - 1;
    }

    // save the stage's name to load it at the next scene
    public void SelectStageToPlay()
    {
        MenuController.stageName = stages[stageCount].name;

        // stage's name used at transition UI

        switch(MenuController.stageName)
        {
            case "Lush Jungle": MenuController.stageUIName = "Jungla Sombria"; break;
            case "Cerulean Cave": MenuController.stageUIName = "Cueva Celeste"; break;
            case "Dark Crater": MenuController.stageUIName = "Crater Oscuro"; break;
            default: MenuController.stageUIName = MenuController.stageName; break;
        }
    }

    public void NextStage()
    {
        // if we are not at the last position of the array we continue adding 1 to the count
        if(stageCount != stages.Count - 1)
            stageCount++;
        else // if we reach the last position, we go back to the first one
            stageCount = 0;

        IsBlocked();

        // we loop the stages again to show next one
        LoopStages();
    }

    public void PrevStage() 
    { 
        // the opposite for the previous stage
        if(stageCount != 0)
            stageCount--;
        else
            stageCount = stages.Count - 1;

        IsBlocked();

        // we loop the stages again to show previous one
        LoopStages();
    }

    private void IsBlocked()
    {
        // save the current player name selected at menu

        string lockedStageName = stages[stageCount].name;

        // take with the player from our locked list

        LockedStage stage = lockedStages.Find(stage => stage.name == lockedStageName);
        
        // if we found it 

        if(stage != null)
        {
            // and it's locked

            if(stage.unlock.locked) // show the text
                ShowLockedMessage(stage);
            else // it's not locked, we show the select player button
                ShowSelectPlayerButton();
        }
        else // if we don't find it, we show the select player button
            ShowSelectPlayerButton();
        
    }

    private void ShowSelectPlayerButton()
    {
        lockedBox.SetActive(false); // button or locked text

        selectStage.gameObject.SetActive(true);
    }

    private void ShowLockedMessage(LockedStage stage)
    {
        lockedText.text = stage.unlock.unlockMessage; // put the unlocked message at the UI text

        lockedBox.SetActive(true);

        selectStage.gameObject.SetActive(false);
    }

    void LoopStages()
    {
        for(int stage = 0; stage < stages.Count; stage++)
        {
            // if the element looped its not the stage selected we disable it
            if(stage != stageCount)
                stages[stage].SetActive(false);
            else // if it is, we enable it
                stages[stage].SetActive(true);
        }
    }
}

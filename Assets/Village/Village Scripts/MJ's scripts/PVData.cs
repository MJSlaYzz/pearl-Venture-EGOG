using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PVData
{
    #region Variables

    [Header("Player Data")]
    int playerlevel;
    float PlayerExperiance;
    int gold;

    [Header("Levels Data")]
    int lastLevelUnlocked;

    [Header("Underwater Collectables Data")]
    int normalPearlsCollected;
    int specialPearlsCollected;
    int treasurePiecesCollected;
    int trashPiecesCollected;
    int plantsCollected;

    [Header("Village Collectables Data")]
    int redMushroomsCollected;
    int PurpleMushroomsCollected;
    int datesCollected;
    int spicesCollected;
    int fish;
    int villageTrashPiecesCollected;


    [Header("Village Quests Data")]
    List<bool> lockedQuestsList;
    List<bool> unlockedQuestsList;
    List<bool> completedQuestsList;
    int mainQuestProgressLevel;

    #endregion

    //constructor
    public PVData(CollectablesCounter collectablesCounter)
    {
        normalPearlsCollected = collectablesCounter.totalPearlsForShare;
    }



}

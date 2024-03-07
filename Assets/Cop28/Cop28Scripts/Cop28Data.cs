using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cop28Data
{
    #region Variables

    [Header("Level Data")]
    public int progressLevel;
    public float progressPoints;
    public int totalPearlsCollected;
    public bool maxedPointsReached;


    #endregion

    //constructor
    public Cop28Data(Cop28DataManager cop28DataManager)
    {
        progressLevel = cop28DataManager.currentLevelProgress;
        progressPoints = cop28DataManager.progressPoints;
        totalPearlsCollected = cop28DataManager.pearlsCollected;
        maxedPointsReached = cop28DataManager.maxedPointsReached;
    }



}

using System;
using UnityEngine;

// A system of using actions and events to send triggers within scripts as fast as possible
// Fastest and easiest way for communication between scripts and triggering
public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    void Awake(){ current = this; }
    
    #region Actions!
    
    public event Action jellyFishActionOn;
    public event Action jellyFishActionOff;
    
    #endregion

    #region Events!
    public void jellyFishEventOn()
    {
        if (jellyFishActionOn != null)
        {
            jellyFishActionOn();
        }
    }
    
    public void jellyFishEventOff()
    {
        if (jellyFishActionOff != null)
        {
            jellyFishActionOff();
        }
    }

    #endregion
    
}

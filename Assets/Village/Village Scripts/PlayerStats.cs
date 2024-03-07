using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int totalPearls;
    public string[,] fish = new string[3, 2]
        { {"Type 1", "0"},
          {"Type 2", "0"},
          {"Type 3", "0"} }; 

    public string[,] spices = new string[3,2]
        { {"Type 1", "0"},
          {"Type 2", "0"},
          {"Type 3", "0"} };
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWaterColor : MonoBehaviour
{
    public Material material;

    public Color colorHealthy0;
    public Color colorHealthy1;
    public Color colorHealthy2;
    public Color colorHealthy3;

    public bool Cop28 = false;
    public List<bool> healthStages = new List<bool>(4);

    private void Update()
    {
        ColorCheck();
    }
    private void ColorCheck()
    {
        if (!Cop28)
        {
            material.SetColor("Color_d05ea060fad6413199121899d2b4436b", colorHealthy3);
        }
        else
        {
            if (healthStages[0] == true)
            {
                material.SetColor("Color_d05ea060fad6413199121899d2b4436b", colorHealthy0);
                healthStages[1] = false;
                healthStages[2] = false;
                healthStages[3] = false;

            }
            else if (healthStages[1] == true)
            {
                material.SetColor("Color_d05ea060fad6413199121899d2b4436b", colorHealthy1);
                healthStages[0] = false;
                healthStages[2] = false;
                healthStages[3] = false;
            }
            else if (healthStages[2] == true)
            {
                material.SetColor("Color_d05ea060fad6413199121899d2b4436b", colorHealthy2);
                healthStages[0] = false;
                healthStages[1] = false;
                healthStages[3] = false;
            }
            else if (healthStages[3] == true)
            {
                material.SetColor("Color_d05ea060fad6413199121899d2b4436b", colorHealthy3);
                healthStages[0] = false;
                healthStages[1] = false;
                healthStages[2] = false;
                healthStages[3] = true;
            }
            else
            {
                material.SetColor("Color_d05ea060fad6413199121899d2b4436b", colorHealthy3);
            }
        }
    }
}





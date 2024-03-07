using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    [SerializeField] public int health;

    [SerializeField] public PlayerHealth playerhealth;

    [SerializeField] public Sprite emptyHeart;
    [SerializeField] public Sprite halfHeart;
    [SerializeField] public Sprite fullHeart;
    [SerializeField] public Image[] heartsArray;

    private void Update()
    {
        UpdateHealthUI();
    }
    public void UpdateHealthUI()
    {
        maxHealth = playerhealth.maxHealth;
        health = playerhealth.currentHealth;

        for (int i = 0; i < heartsArray.Length; i++)
        {
            //Debug.Log("I = " + i);
            if (health == 6)
            {
                heartsArray[0].sprite = fullHeart;
                heartsArray[1].sprite = fullHeart;
                heartsArray[2].sprite = fullHeart;
            }
            else if (health == 5)
            {
                heartsArray[0].sprite = fullHeart;
                heartsArray[1].sprite = fullHeart;
                heartsArray[2].sprite = halfHeart;
            }
            else if (health == 4)
            {
                heartsArray[0].sprite = fullHeart;
                heartsArray[1].sprite = fullHeart;
                heartsArray[2].sprite = emptyHeart;
            }
            else if (health == 3)
            {
                heartsArray[0].sprite = fullHeart;
                heartsArray[1].sprite = halfHeart;
                heartsArray[2].sprite = emptyHeart;
            }
            else if (health == 2)
            {
                heartsArray[0].sprite = fullHeart;
                heartsArray[1].sprite = emptyHeart;
                heartsArray[2].sprite = emptyHeart;
            }
            else if (health == 1)
            {
                heartsArray[0].sprite = halfHeart;
                heartsArray[1].sprite = emptyHeart;
                heartsArray[2].sprite = emptyHeart;
            }
            else if (health == 0)
            {
                heartsArray[0].sprite = emptyHeart;
                heartsArray[1].sprite = emptyHeart;
                heartsArray[2].sprite = emptyHeart;
            }
        }
    }
}

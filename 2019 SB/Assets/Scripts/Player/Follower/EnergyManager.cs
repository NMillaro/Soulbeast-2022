using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public Slider playerEnergyFill;
    public GameObject energySlider;
    public FloatValue monsterCurrentEnergy;
    public FloatValue energyBar;
    public float maxEnergy;


    void Start()
    {
        maxEnergy = monsterCurrentEnergy.initialValue;

        InitEnergy();
    }

    public void InitEnergy()
    {
        playerEnergyFill.value = 1;
    }

    public void UpdateEnergy()
    {
        maxEnergy = monsterCurrentEnergy.initialValue;
        energyBar.RuntimeValue = maxEnergy;
        float tempEnergy = monsterCurrentEnergy.RuntimeValue;
        playerEnergyFill.value = tempEnergy / maxEnergy;

        //if (playerEnergyFill.value <= 0)
        //{
        //    energySlider.SetActive(false);
        //}

    }
}

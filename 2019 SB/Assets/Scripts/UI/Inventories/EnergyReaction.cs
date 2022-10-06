using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyReaction : MonoBehaviour
{
    public FloatValue followerEnergy;
    public Message energyMessage;

   public void Use(int amountToIncrease)
    {
        //TODO energy system separate from main class
        if ((followerEnergy.RuntimeValue + amountToIncrease) < followerEnergy.initialValue)
        {
            followerEnergy.RuntimeValue += amountToIncrease;
        }
        else
        {
            followerEnergy.RuntimeValue = followerEnergy.initialValue;
        }
        energyMessage.Raise();
    }

}

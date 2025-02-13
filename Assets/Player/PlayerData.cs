using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public float interest { get; set; }
    public float stress { get; set; }


    public PlayerData()
    {
        interest = 1;
        stress = 0;
    }

    public PlayerData(float interest, float stress)
    {
        this.interest = interest;
        this.stress = stress;
    }
}

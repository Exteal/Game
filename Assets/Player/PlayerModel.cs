using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    // Start is called before the first frame update

    Dictionary<string, PlayerData> model;
    void Start()

    {
        foreach (var tag in Enum.GetNames(typeof(PageTags)))
        {

            model.Add(tag, new PlayerData());
            model.Add(tag, new PlayerData());

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public Dictionary<string, PlayerData> model;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        model = new Dictionary<string, PlayerData>();

        foreach (var tag in Enum.GetNames(typeof(PageTags)))
        {
            model.Add(tag, new PlayerData(0.2f, 0));
        }
    }
}

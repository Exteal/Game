using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public Dictionary<PageTags, PlayerData> model;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        model = new Dictionary<PageTags, PlayerData>();

        foreach (var tag in Enum.GetValues(typeof(PageTags)))
        {
            model.Add((PageTags)tag, new PlayerData(0.2f, 0));
        }
    }
}

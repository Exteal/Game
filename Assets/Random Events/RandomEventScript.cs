using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Random = System.Random;

public class RandomEventScript : MonoBehaviour
{
    // Start is called before the first frame update

    public static RandomEventScript eventsInstance;

    public static MainManager instance;

    private Dictionary<RandomEventType, PlayerData> randomEventsModel;
    void Start()
    {
        eventsInstance = FindAnyObjectByType<RandomEventScript>();
        instance = FindFirstObjectByType<MainManager>();

        
        randomEventsModel = new Dictionary<RandomEventType, PlayerData>();

        foreach (var implemented in GetComponents<RandomEvent>())
        {
            randomEventsModel.Add(implemented.getType(), new PlayerData(0f, -1f));
        }

        InvokeRepeating("PlayRandomEvent", 4, 2);

    }

    private void Awake()
    {
        if (eventsInstance != null)
        {
            Destroy(gameObject);
            return;
        }

        eventsInstance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void PlayRandomEvent()
    {

        var eventType = RandomEventChoice();

        Debug.Log("Choice : " + eventType);
        PlayEvent(eventType);
        UpdateModel(eventType);
    }

    private void PlayEvent(RandomEventType eventType)
    {
        foreach (var reve in GetComponents<RandomEvent>())
        {
            if (reve.getType() == eventType)
            {
                reve.playEvent();
                break;
            }
        }
    }

    private void UpdateModel(RandomEventType eventType)
    {
        var old = randomEventsModel[eventType];
        randomEventsModel[eventType] = new PlayerData(old.interest, old.stress + 2);
    }

    private RandomEventType RandomEventChoice()
    {
       /* Random random = new Random();
        Array values = Enum.GetValues(typeof(RandomEventType));
        RandomEventType randomType = (RandomEventType)values.GetValue(random.Next(values.Length));

        return randomType;
       */
        

        RandomEventType max = RandomEventType.KNOCK;
        
        foreach(var (key, val) in randomEventsModel)
        {
            if (val.stress == -1)
            {
                return key;
            }

            if(val.stress > randomEventsModel[max].stress)
            {
                max = key;
            }
        }

        return max;
    }
}

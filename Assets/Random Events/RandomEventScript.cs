using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class RandomEventScript : MonoBehaviour
{
    // Start is called before the first frame update

    public static RandomEventScript eventsInstance;
    public static BitalinoManager manager;
    private Dictionary<RandomEventType, PlayerData> randomEventsModel;
    
    void Start()
    {
        eventsInstance = FindAnyObjectByType<RandomEventScript>();
        randomEventsModel = new Dictionary<RandomEventType, PlayerData>();
        manager = FindAnyObjectByType<BitalinoManager>();

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
        if (manager.connected)
        {
            var eventType = RandomEventChoice();

            Debug.Log("Choice : " + eventType);
            PlayEvent(eventType);

            StartCoroutine(UpdateModel(eventType));
        }
        
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

    private IEnumerator UpdateModel(RandomEventType eventType)
    {
        yield return new WaitForSeconds(5);

        var old = randomEventsModel[eventType];

        var might = manager.stressLogs.TakeLast(5).ToList().Average();

        var newStress = Mathf.CeilToInt( (float)(old.stress + might) / 2);
        Debug.Log("Event : " + eventType + " old : " + old.stress + " might : " + might + "new : " + newStress);

        randomEventsModel[eventType] = new PlayerData(old.interest, newStress);

    }

    private RandomEventType RandomEventChoice()
    {
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

        var maxOrRd = new System.Random();
        if (maxOrRd.Next(1, 101) > 30)
            return max;

        return  randomEventsModel.ElementAt(maxOrRd.Next(0, randomEventsModel.Count)).Key;
    }
}

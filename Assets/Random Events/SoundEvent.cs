using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
public abstract class SoundEvent : RandomEvent
{

    private AudioClip clip;
    abstract public  string clipName {get; set;}
    public override bool usesSound()
    {
       return true;
    }


    public override void playEvent()
    {
        if (clip)
        {
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().clip = clip;
            GetComponent<AudioSource>().Play();
        }
    }



    private IEnumerator fetchClip()
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(Application.streamingAssetsPath + "/Sounds/" + clipName, AudioType.MPEG))
        {
            yield return uwr.SendWebRequest();

            try
            {
                if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError) Debug.Log($"{uwr.error}");
                else
                {
                    clip = DownloadHandlerAudioClip.GetContent(uwr);
                }
            }
            catch (Exception err)
            {
                Debug.Log($"{err.Message}, {err.StackTrace}");
            }
        }
    }
    void Start()
    {
        StartCoroutine(fetchClip());
    }

}

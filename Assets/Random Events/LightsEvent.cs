using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LightsEvent : SoundEvent
{
    public override string clipName { get => "lights.mp3"; set { } }

    public override RandomEventType getType()
    {
        return RandomEventType.LIGHTS;
    }

    public override void playEvent()
    {
        if (SceneManager.GetActiveScene().name == "ComputerScreen")
        {
            base.playEvent();
            StartCoroutine(LoadLevelAfterDelay(2));
        }
    }

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("CharacterScene");
    }
}

using UnityEngine;

public abstract class RandomEvent : MonoBehaviour
{
    public abstract RandomEventType getType();
    public abstract bool usesSound();

    public abstract void playEvent();
}

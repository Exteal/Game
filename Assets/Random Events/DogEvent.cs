using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DogEvent : SoundEvent
{
    public override string clipName { get => "bark.mp3"; set { }  }
    public override RandomEventType getType()
    {
        return RandomEventType.DOG;
    }

}

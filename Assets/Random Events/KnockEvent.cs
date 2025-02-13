using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KnockEvent : SoundEvent
{
    public override string clipName { get => "knock.mp3"; set { } }
    public override RandomEventType getType()
    {
        return RandomEventType.KNOCK;
    }
}

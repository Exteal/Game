
public class SirensEvent : SoundEvent
{
    public override string clipName { get => "sirens.mp3"; set { } }

    public override RandomEventType getType()
    {
        return RandomEventType.SIRENS;
    }
}

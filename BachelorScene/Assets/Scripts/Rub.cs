using System.Collections;
using System.Collections.Generic;

public class Rub
{
    // Class to keep track of rubs in terms of distance, duration, time and location

    public float Distance { get; protected set; }
    public float Duration { get; protected set; }
    public float OccuredAt { get; protected set; }
    public string Location { get; protected set; }

    public Rub(float distance, float duration, float occuredAt, string location)
    {
        Distance = distance;
        Duration = duration;
        OccuredAt = occuredAt;
        Location = location;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StateChangeRecord<T>
{
    // Class to keep track of state transitions and when they happened

    public T From { get; protected set; }
    public T To { get; protected set; }
    public long Time { get; protected set; }

    public StateChangeRecord(T from, T to, long time)
    {
        From = from;
        To = to;
        Time = time;
    }
}

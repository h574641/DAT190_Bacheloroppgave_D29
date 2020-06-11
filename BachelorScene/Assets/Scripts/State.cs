using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class State<T>
{
    public Action OnEnter { get; protected set; }
    public Func<T> OnStay { get; protected set; }
    public Action OnLeave { get; protected set; }

    public T Name { get; protected set; }

    public State(T name, Action onEnter, Func<T> onStay, Action onLeave)
    {
        Name = name;

        OnEnter = onEnter;
        OnStay = onStay;
        OnLeave = onLeave;
    }
}

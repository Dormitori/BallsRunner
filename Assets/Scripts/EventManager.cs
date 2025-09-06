using System;

public class EventManager
{
    public static event Action Scored;

    public static void OnScored()
    {
        Scored?.Invoke();
    }
}
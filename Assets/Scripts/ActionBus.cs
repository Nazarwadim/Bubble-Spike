using System;
using UnityEngine;

public static class ActionBus 
{
    public static Action MainBubbleKilled;
    public static Action<int> WoodDestroyed;
    public static Action<int> BadBubbleDestroyed;
}
using System;
using UnityEngine;

public class ScoreProgress : MonoBehaviour
{
    public int Count{get; private set;}

    public event Action<int> ProgressChanged;
    
    public void Change(int amount) {
        Count = amount;
        ProgressChanged?.Invoke(Count);
    }

    public void ResetScore() {
        Count = 0;
        ProgressChanged?.Invoke(Count);
    }
}

using UnityEngine;

public class BubbleSpawner : BaseSpawner
{
    public const int BubblesTypesCount = 3;
    public GameObject bubbleSimplePrefab;
    public GameObject bubbleArmoredPrefab;
    public GameObject bubbleHardPrefab;
    public Transform ToObject;

    public Transform BubbleParrent;

    public float MinSpeed = 0.1f;
    public float MaxSpeed = 1.5f;

    public float SimpleProbability = 75;

    public float ArmoredProbability = 15;

    public float HardProbabiliky = 10;

    protected override void SpawnObject(Vector3 spawnPosition)
    {
        int value = Random.Range(0, 100);
        GameObject bubblePref;
        if (value < SimpleProbability)
        {
            bubblePref = bubbleSimplePrefab;
        }
        else if (value < SimpleProbability + ArmoredProbability)
        {
            bubblePref = bubbleArmoredPrefab;
        }
        else
        {
            bubblePref = bubbleHardPrefab;
        }
        GameObject bubble = Instantiate(bubblePref, spawnPosition, Quaternion.identity);
        bubble.transform.SetParent(BubbleParrent);
        PositionMover positionMover = bubble.GetComponent<PositionMover>();
        positionMover.MoveToPosition(ToObject.position);
        positionMover.Speed = Random.Range(MinSpeed, MaxSpeed);
    }
}
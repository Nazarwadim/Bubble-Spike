using UnityEngine;

public class WoodSpawner : BaseSpawner
{
        public GameObject woodPrefab;
        public float woodSpeed = 5f;

        protected override void SpawnObject()
        {
            Vector3 spawnPosition = GetRandomPositionOnAllowedSides();

            GameObject wood = Instantiate(woodPrefab, spawnPosition, Quaternion.identity);

            Vector2 direction = (Vector2.zero - (Vector2)spawnPosition).normalized;

            MoveObject moveScript = wood.AddComponent<MoveObject>();
            moveScript.SetDirectionAndSpeed(direction, woodSpeed);
        }
}
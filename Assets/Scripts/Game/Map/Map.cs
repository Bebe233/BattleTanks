using UnityEngine;
namespace BEBE.Game.Map
{
    public class Map : MonoBehaviour
    {
        public Transform[] SpawnSpots;
        private const int capicity = 10;
        private void Awake()
        {
            SpawnSpots = new Transform[capicity];
            Transform spawn_1 = transform.Find("spawn_1");
            if (spawn_1 != null)
            {
                int count = spawn_1.childCount;
                for (int i = 0; i < count; i++)
                {
                    SpawnSpots[i] = spawn_1.GetChild(i);
                }
            }
            Transform spawn_2 = transform.Find("spawn_2");
            if (spawn_2 != null)
            {
                int count = spawn_2.childCount;
                for (int i = 0; i < count; i++)
                {
                    SpawnSpots[i + 5] = spawn_2.GetChild(i);
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using Interfaces.Runtime;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MisteryBox.Runtime
{
    [Serializable]
    public class MysteryBoxData
    {
        public GameObject m_prefab;
        public int m_quantity;
    }
    
    public class MisteryBoxSystem : NetworkBehaviour
    {
        public static MisteryBoxSystem Instance;

        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private GameObject _mysteryBoxPrefab;
        private List<GameObject> _spawnedMysteryBoxes = new List<GameObject>();
        private List<GameObject> _spawnedWeapons = new List<GameObject>();

        [SerializeField] private List<MysteryBoxData> _entries;
        private Queue<GameObject> _lootQueue = new Queue<GameObject>();
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }

        [Server]
        public void Reset()
        {
            _lootQueue.Clear();
            if(_spawnedWeapons.Count > 0) DespawnList(_spawnedWeapons);
            if(_spawnedMysteryBoxes.Count > 0) DespawnList(_spawnedMysteryBoxes);
            
            foreach (var entry in _entries)
            {
                for (int i = 0; i < entry.m_quantity; i++)
                {
                    _lootQueue.Enqueue(entry.m_prefab);
                }
            }

            _lootQueue = new Queue<GameObject>(Shuffle(_lootQueue));
        }

        [Server]
        private void DespawnList(List<GameObject> toDespawnList)
        {
            foreach (var toDespawn in toDespawnList)
            {
                NetworkServer.Destroy(toDespawn);
            }
            toDespawnList.Clear();
        }
        
        [Server]
        public void SpawnBox()
        {
            Debug.Log("Spawn ?");
            
            int count = Mathf.Min(_lootQueue.Count, _spawnPoints.Count);
            for (int i = 0; i < count; i++)
            {
                var prefab = _lootQueue.Dequeue();
                Transform pos = _spawnPoints[i];

                GameObject boxObj = Instantiate(_mysteryBoxPrefab, pos.position, pos.rotation);
                var box = boxObj.GetComponent<MysteryBox>();
                box.SetData(prefab);
                _spawnedMysteryBoxes.Add(boxObj);
                NetworkServer.Spawn(boxObj);
                Debug.Log("Spawned " + prefab.name);
            }
        }
        public void AddToSpawnedWeapons(GameObject obj)
        {
            _spawnedWeapons.Add(obj);
        }

        public void RemoveFromSpawnedWeapons(GameObject obj)
        {
            if(_spawnedWeapons.Contains(obj)) _spawnedWeapons.Remove(obj);
        }
        private List<T> Shuffle<T>(IEnumerable<T> list)
        {
            var array = new List<T>(list);
            for (int i = array.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
            return array;
        }
    }

    
}

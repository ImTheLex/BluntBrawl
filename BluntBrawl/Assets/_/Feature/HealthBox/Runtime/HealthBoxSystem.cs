using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HealthBox.Runtime
{
    public class HealthBoxSystem : NetworkBehaviour
    {
        [SerializeField] GameObject _healthBoxPrefab;
        [SerializeField] List<Transform> m_healthBoxesPositions = new List<Transform>();        
        private List<Transform> m_healthBoxAvailableSpots = new List<Transform>();        
        [SerializeField] private int _healthBoxMaxCount = 2;
        [SerializeField] private float _respawnTime = 15f;
        public int m_currentHealthBoxes;
        private List<GameObject> m_healthBoxes = new List<GameObject>();
        private bool _isReady = false;


        [Server]
        public void Reset()
        {
            CancelInvoke();
            m_currentHealthBoxes = 0;
            m_healthBoxAvailableSpots.Clear();
            foreach (var box in m_healthBoxes)
            {
                NetworkServer.Destroy(box);
            }
            m_healthBoxes.Clear();
            m_healthBoxAvailableSpots = new List<Transform>(m_healthBoxesPositions);
            
        }

        [Server]
        public void RestartCycle()
        {
            Invoke(nameof(FillHealthBoxSpawns), _respawnTime);
        }
        
        [Server]
        public void FillHealthBoxSpawns()
        {
            if(m_currentHealthBoxes >= _healthBoxMaxCount) return;
            m_currentHealthBoxes++;
            int random = Random.Range(0, m_healthBoxAvailableSpots.Count);
            Transform pos = m_healthBoxAvailableSpots[random];

            GameObject healthBox = Instantiate(_healthBoxPrefab, pos.position, pos.rotation);
            m_healthBoxes.Add(healthBox);
            NetworkServer.Spawn(healthBox);
            RpcSetParent(healthBox, pos);
            m_healthBoxAvailableSpots.RemoveAt(random);
            Invoke(nameof(FillHealthBoxSpawns),_respawnTime);
            
        }
        
        [Server]
        public void DecreaseCurrentHealthBoxes(GameObject objToDestroy)
        {
            CancelInvoke();
            m_currentHealthBoxes--;
            if (m_healthBoxes.Contains(objToDestroy))
            {
                Debug.Log("It Contains" + objToDestroy.name + "Does it contain : " + objToDestroy.transform.parent);
                m_healthBoxAvailableSpots.Add(objToDestroy.transform.parent);
                m_healthBoxes.Remove(objToDestroy);
            }
            NetworkServer.Destroy(objToDestroy);
            Invoke(nameof(FillHealthBoxSpawns),_respawnTime);
        }
        
        [ClientRpc]
        public void RpcSetParent(GameObject obj, Transform pos)
        {
            Debug.Log("RpcSetParent " + obj + "Pos " + pos );
            obj.transform.parent = pos;
        }
    }
}

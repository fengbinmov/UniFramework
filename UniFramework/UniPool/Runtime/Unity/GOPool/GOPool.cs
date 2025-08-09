using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Uni.GOPool
{
    public class GOPool : MonoBehaviour
    {
        public static GOPool Inst
        {
            get
            {
                if (_instance == null)
                {
                    var poolRoot = new GameObject("GOPool");
                    var cachedRoot = new GameObject("CachedRoot");
                    cachedRoot.transform.SetParent(poolRoot.transform, false);
                    cachedRoot.gameObject.SetActive(false);
                    _instance = poolRoot.AddComponent<GOPool>();
                    _instance._cachedRoot = cachedRoot.transform;
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        private static GOPool _instance;

        private Transform _cachedRoot;
        
        private Dictionary<int, GameObject> _prefabTemplates = new Dictionary<int, GameObject>();
        
        private Dictionary<int, GameObjectPool> _gameObjPools = new Dictionary<int, GameObjectPool>();

        private Dictionary<int, int> _gameObjRelations = new Dictionary<int, int>();

        private List<RecyclablePoolInfo> _poolInfoList = new List<RecyclablePoolInfo>();

        private void Awake()
        {
            if (_instance != null)
            {
                Debug.LogError("Uni.GOPool == Don't attach SimpleGOPoolManager on any object, use SimpleGOPoolManager.Instance instead!");
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            foreach (var pool in _gameObjPools.Values)
            {
                var deltaTime = pool.IfIgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
                pool.OnPoolUpdate(deltaTime);
            }
        }
        
        private void OnDestroy()
        {
            ClearAllPools(true);
        }

        public bool IfPoolValid(GameObject prefabTemplate)
        {
            return IfPoolValid(prefabTemplate.GetInstanceID());
        }

        public bool IfPoolValid(int prefabHash)
        {
            return _prefabTemplates.ContainsKey(prefabHash) && _gameObjPools.ContainsKey(prefabHash);
        }

        public GameObjectPool RegisterPrefab(GameObject prefabAsset, RecyclablePoolConfig config = null)
        {
            Assert.IsNotNull(prefabAsset);

            var prefabHash = prefabAsset.GetInstanceID();

#if EASY_POOL_DEBUG
            if (_gameObjPools.ContainsKey(prefabHash) || _prefabTemplates.ContainsKey(prefabHash))
            {
                Debug.LogError($"Uni.GOPool == RegisterPrefab {prefabAsset.name} but already registered");
                return null;
            }
#endif
            
            if (config == null)
            {
                config = GetSimpleGOPoolConfig(prefabAsset, null);
            }
            
            if (config.SpawnFunc == null)
            {
                config.SpawnFunc = () => DefaultCreateObjectFunc(prefabHash);
            }

            if (config.ExtraArgs == null || config.ExtraArgs.Length == 0)
            {
                config.ExtraArgs = new object[] { _cachedRoot };
            }

            _prefabTemplates[prefabHash] = prefabAsset;
            var newPool = new GameObjectPool(config);
            _gameObjPools[prefabHash] = newPool;
            _poolInfoList.Add(newPool.GetPoolInfoReadOnly());

            return newPool;
        }

        public bool UnRegisterPrefab(GameObject prefabTemplate)
        {
            return UnRegisterPrefab(prefabTemplate.GetInstanceID());
        }

        public bool UnRegisterPrefab(int prefabHash)
        {
            _prefabTemplates.Remove(prefabHash);
            RemoveObjectsRelationByAssetHash(prefabHash);
            if (_gameObjPools.TryGetValue(prefabHash, out var pool))
            {
                pool.ClearAll();
                _poolInfoList.Remove(pool.GetPoolInfoReadOnly());
                _gameObjPools.Remove(prefabHash);

                return true;
            }

            return false;
        }

        public GameObject Spawn(GameObject prefabTemplate)
        {
            if (prefabTemplate == null)
            {
                return null;
            }

            var prefabHash = prefabTemplate.GetInstanceID();

            if (!_gameObjPools.TryGetValue(prefabHash, out var pool))
            {
                pool = RegisterPrefab(prefabTemplate);
            }

            var newObj = pool.SpawnObject();

            _gameObjRelations[newObj.GetInstanceID()] = prefabHash;
            
            return newObj;
        }
        
        public bool TrySpawn(int prefabHash, out GameObject newObj)
        {
            newObj = null;
            if (_gameObjPools.TryGetValue(prefabHash, out var pool))
            {
                newObj = pool.SpawnObject();
                _gameObjRelations[newObj.GetInstanceID()] = prefabHash;
            }

            return newObj != null;
        }

        public bool Despawn(GameObject usedObj)
        {
            Assert.IsNotNull(usedObj);

            var objHash = usedObj.GetInstanceID();

            if (!_gameObjRelations.TryGetValue(objHash, out var assetHash))
            {
                return false;
            }

            if (!_gameObjPools.TryGetValue(assetHash, out var pool))
            {
                return false;
            }

            _gameObjRelations.Remove(objHash);
            
            return pool.DespawnObject(usedObj);
        }

        public bool ClearPoolByAssetHash(int prefabHash, bool onlyClearUnused = false)
        {
            if (_gameObjPools.TryGetValue(prefabHash, out var pool))
            {
                if (onlyClearUnused)
                {
                    pool.ClearUnusedObjects();   
                    return true;
                }
                else
                {
                    pool.ClearAll();
                    return true;
                }
            }
            
            return false;
        }

        public bool HavRelations(GameObject usedObj)
        {
            Assert.IsNotNull(usedObj);

            var objHash = usedObj.GetInstanceID();

            return _gameObjRelations.ContainsKey(objHash);
        }

        public void ClearAllUnusedObjects()
        {
            foreach (var poolPair in _gameObjPools)
            {
                poolPair.Value.ClearUnusedObjects();
            }
        }

        public static void ClearAllPools(bool ifDestroy)
        {
            if (_instance == null) return;

            _instance.ClearAll(ifDestroy);
        }

        public void ClearAll(bool ifDestroy)
        {
            foreach (var pool in _gameObjPools)
            {
                pool.Value.ClearAll();
            }

            _gameObjRelations.Clear();

            if (ifDestroy)
            {
                if (_cachedRoot)
                {
                    for (int i = _cachedRoot.childCount - 1; i >= 0; i--)
                    {
                        var child = _cachedRoot.GetChild(i).gameObject;
                        Destroy(child);
                    }
                }

                _prefabTemplates.Clear();
                _poolInfoList.Clear();
                _gameObjPools.Clear();
            }
        }

        private List<int> _tempRemovedObjectList = new List<int>();
        
        private void RemoveObjectsRelationByAssetHash(int assetHash)
        {
            foreach (var relation in _gameObjRelations)
            {
                if (relation.Value == assetHash)
                {
                    _tempRemovedObjectList.Add(relation.Key);
                }
            }

            foreach (var removeItem in _tempRemovedObjectList)
            {
                _gameObjRelations.Remove(removeItem);
            }
            
            _tempRemovedObjectList.Clear();
        }

        public List<RecyclablePoolInfo> GetPoolsInfo()
        {
            return _poolInfoList;
        }
        
        private GameObject DefaultCreateObjectFunc(int prefabHash)
        {
            if (_prefabTemplates.TryGetValue(prefabHash, out var prefabAsset))
            {
                if (prefabAsset != null)
                {
                    var gameObj = Instantiate(prefabAsset);
                    return gameObj;
                }
            }
            
            Debug.LogError($"Uni.GOPool == Cannot create object: {prefabHash}");
            return null;
        }

        public RecyclablePoolConfig GetSimpleGOPoolConfig(GameObject prefabAsset, Func<GameObject> spawnFunc)
        {
            Assert.IsNotNull(prefabAsset);

            var poolConfig = new RecyclablePoolConfig()
            {
                ObjectType = RecycleObjectType.GameObject,
                ReferenceType = typeof(GameObject),
                PoolId = $"SimplePool_{prefabAsset.name}_{prefabAsset.GetInstanceID()}",
                SpawnFunc = spawnFunc,
            };

            return poolConfig;
        }
    }
}

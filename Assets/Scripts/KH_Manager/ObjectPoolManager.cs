using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance {  get; set; }

    private Dictionary<string, Queue<GameObject>> _poolDic = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        instance = this;
    }
    public GameObject GetObject(string prefabPath)
    {
        if(_poolDic.ContainsKey(prefabPath) && _poolDic[prefabPath].Count > 0)
        {
            GameObject obj = _poolDic[prefabPath].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"프리팹을 찾을 수 없습니다: {prefabPath}");
            return null;
        }
        return Instantiate(prefab);
    }

    public void ReturnObject(string prefabPath, GameObject obj) 
    {
        obj.SetActive(false);

        if (_poolDic.ContainsKey(prefabPath) == false)
        {
            _poolDic.Add(prefabPath, new Queue<GameObject>());
        }
        _poolDic[prefabPath].Enqueue(obj);
    }
}

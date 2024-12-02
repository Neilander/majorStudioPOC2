using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "animalInfo", menuName = "ScriptableObjects/animalInfos", order = 1)]
public class animalPrefabsInfo : ScriptableObject
{
    public List<GameObject> animalPrefabs;
    public GameObject GetRandomAnimal()
    {
        // 假设有一个Prefab数组
        if (animalPrefabs.Count == 0)
            return null;
        return animalPrefabs[Random.Range(0, animalPrefabs.Count)];
    }
}

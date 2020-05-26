using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public class ECSManager : MonoBehaviour
{
    public static EntityManager manager;
    public GameObject virusPrefab;
    public int numVirus = 500;

    BlobAssetStore store;


    void Start()
    {
        store = new BlobAssetStore();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);
        Entity virus = GameObjectConversionUtility.ConvertGameObjectHierarchy(virusPrefab, settings);

        for (int i = 0; i < numVirus; i++)
        {
            var instance = manager.Instantiate(virus);
            float3 position = GetRandomFloat3(-50, 50);
            manager.SetComponentData(instance, new Translation() { Value = position });
            manager.SetComponentData(instance, new FloatData()
            {
                speed = UnityEngine.Random.Range(1, 10) / 10f
            });
        }
    }

    float3 GetRandomFloat3(float min, float max)
    {
        return new float3(UnityEngine.Random.Range(min, max),
                          UnityEngine.Random.Range(min, max),
                          UnityEngine.Random.Range(min, max));
    }

    private void OnDestroy()
    {
        store.Dispose();
    }
}

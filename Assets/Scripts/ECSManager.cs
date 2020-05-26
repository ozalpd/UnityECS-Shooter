using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


public class ECSManager : MonoBehaviour
{
    public static EntityManager manager;
    public GameObject player;

    public GameObject virusPrefab;
    public int numVirus = 500;

    public GameObject redBloodCellPrefab;
    public int numBloodCell = 500;

    public GameObject bulletPrefab;
    public int numBullets = 10;
    private Entity bulletEntity;

    BlobAssetStore store;


    private void Start()
    {
        store = new BlobAssetStore();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);

        Entity virus = GameObjectConversionUtility.ConvertGameObjectHierarchy(virusPrefab, settings);
        Instaniate(virus, numVirus);

        Entity redCell = GameObjectConversionUtility.ConvertGameObjectHierarchy(redBloodCellPrefab, settings);
        Instaniate(redCell, numBloodCell);

        bulletEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);
    }

    private void Instaniate(Entity entity, int numOfInstance)
    {
        for (int i = 0; i < numOfInstance; i++)
        {
            var instance = manager.Instantiate(entity);
            float3 position = GetRandomFloat3(-50, 50);
            manager.SetComponentData(instance, new Translation() { Value = position });
            manager.SetComponentData(instance, new FloatData()
            {
                speed = UnityEngine.Random.Range(1, 10) / 10f
            });
        }
    }

    private float3 GetRandomFloat3(float min, float max)
    {
        return new float3(UnityEngine.Random.Range(min, max),
                          UnityEngine.Random.Range(min, max),
                          UnityEngine.Random.Range(min, max));
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        for (int i = 0; i < numBullets; i++)
        {
            var instance = manager.Instantiate(bulletEntity);
            var startPos = player.transform.position + UnityEngine.Random.insideUnitSphere * 2;
            manager.SetComponentData(instance, new Translation() { Value = startPos });
            manager.SetComponentData(instance, new Rotation() { Value = player.transform.rotation });
        }
    }

    private void OnDestroy()
    {
        store.Dispose();
    }
}

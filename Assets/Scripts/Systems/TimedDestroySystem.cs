using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class TimedDestroySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dt = Time.DeltaTime;
        Entities.WithoutBurst()
                .WithStructuralChanges()
                .ForEach((Entity entity, ref LifeTimeData lifeTime) =>
                {
                    lifeTime.timeLeft -= dt;
                    if (lifeTime.timeIsUp)
                    {
                        EntityManager.DestroyEntity(entity);
                    }
                })
                .Run();
        
        
        var manager = ECSManager.manager;
        Entities.WithoutBurst()
                .WithStructuralChanges()
                .ForEach((Entity entity, ref Translation pos, ref LifeData lifeData) =>
                {
                    if (lifeData.isAlive == false)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            var splat = manager.Instantiate(ECSManager.splatEntity);
                            float3 offset = (float3)UnityEngine.Random.insideUnitSphere * 2.0f;
                            float3 direct = new float3(UnityEngine.Random.Range(-1, 1),
                                                       UnityEngine.Random.Range(-1, 1),
                                                       UnityEngine.Random.Range(-1, 1));

                            manager.SetComponentData(splat, new Translation { Value = pos.Value + offset });
                            manager.SetComponentData(splat, new PhysicsVelocity { Linear = direct * 5f });
                            manager.SetComponentData(splat, new LifeTimeData { timeLeft = UnityEngine.Random.Range(75, 500) / 100f });
                        }
                        EntityManager.DestroyEntity(entity);
                    }
                })
                .Run();
        return inputDeps;
    }
}

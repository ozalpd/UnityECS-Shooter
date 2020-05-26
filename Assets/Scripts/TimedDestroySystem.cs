using Unity.Entities;
using Unity.Jobs;

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
        return inputDeps;
    }
}

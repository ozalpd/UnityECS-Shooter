using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class BulletMoveSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dt = Time.DeltaTime;
        var jobHandle = Entities.WithName("BulletMoveSystem")
                                        //this is a parallel processing iteration
                                        .ForEach((ref PhysicsVelocity physics,
                                                  ref Translation position,
                                                  ref Rotation rotation,
                                                  ref BulletData bulletData) =>
                                        {
                                            physics.Angular = float3.zero;
                                            physics.Linear += dt * bulletData.speed * math.forward(rotation.Value);
                                        })
                                        .Schedule(inputDeps);
        jobHandle.Complete();
        return jobHandle;
    }
}

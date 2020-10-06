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
        var jobHandle = Entities.ForEach((ref PhysicsVelocity velocity,
                                          in Rotation rotation,
                                          in BulletData bulletData) =>
                                {
                                    velocity.Angular = float3.zero;
                                    velocity.Linear += dt * bulletData.speed * math.forward(rotation.Value);
                                })
                                .Schedule(inputDeps);
        jobHandle.Complete();
        return jobHandle;
    }
}

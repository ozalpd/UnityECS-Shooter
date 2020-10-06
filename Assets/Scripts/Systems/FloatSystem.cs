using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public class FloatSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dt = Time.DeltaTime;
        var jobHandle = Entities.ForEach((ref PhysicsVelocity velocity,
                                          in Translation position,
                                          in FloatData floatData) =>
                                {
                                    float s = math.sin((dt + position.Value.x) * 0.5f) * floatData.speed;
                                    float c = math.cos((dt + position.Value.y) * 0.5f) * floatData.speed;
                                    float3 direction = new float3(s, c, s);

                                    velocity.Linear += direction;
                                })
                                .Schedule(inputDeps);
        jobHandle.Complete();
        return jobHandle;
    }
}

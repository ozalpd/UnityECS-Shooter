using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class BulletCollisionSystem : JobComponentSystem
{
    BuildPhysicsWorld buildPhysicsWorld;
    StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var collision = new CollisionEventImpulseJob(bulletGroup: GetComponentDataFromEntity<BulletData>(),
                                                     targetGroup: GetComponentDataFromEntity<LifeData>());

        JobHandle jobHandle = collision.Schedule(simulation: stepPhysicsWorld.Simulation,
                                                 world: ref buildPhysicsWorld.PhysicsWorld,
                                                 inputDeps: inputDeps);

        jobHandle.Complete();
        return jobHandle;
    }

    struct CollisionEventImpulseJob : ICollisionEventsJob
    {
        public CollisionEventImpulseJob(ComponentDataFromEntity<BulletData> bulletGroup, ComponentDataFromEntity<LifeData> targetGroup)
        {
            _bulletGroup = bulletGroup;
            _aliveGroup = targetGroup;
        }

        private ComponentDataFromEntity<BulletData> _bulletGroup;
        private ComponentDataFromEntity<LifeData> _aliveGroup;

        public void Execute(CollisionEvent collisionEvent)
        {
            Entity a = collisionEvent.Entities.EntityA;
            Entity b = collisionEvent.Entities.EntityB;
            Entity bullet = _bulletGroup.Exists(a) ? a : _bulletGroup.Exists(b) ? b : Entity.Null;
            if (bullet.Equals(Entity.Null)) //none of the colliders is a bullet
                return;

            Entity aliveTarget = _aliveGroup.Exists(a) ? a : _aliveGroup.Exists(b) ? b : Entity.Null;
            if (aliveTarget.Equals(Entity.Null)) //must hit something other than an alive target
                return;

            _aliveGroup[aliveTarget] = new LifeData { isAlive = false };
        }
    }
}

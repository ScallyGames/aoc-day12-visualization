using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    
    [UpdateAfter(typeof(ApplyVelocitySystem))]
    public class LookTowardsVelocitySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .ForEach((ref Rotation rotation, in Velocity velocity) =>
                {
                    rotation.Value = quaternion.AxisAngle(
                        new float3(0, 1, 0), 
                        math.atan2(velocity.Value.z, velocity.Value.x)
                    );
                })
                .ScheduleParallel();
        }
    }
}
using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.UIElements;

namespace Systems
{
    [UpdateBefore(typeof(ApplyVelocitySystem))]
    public class LimitVelocitySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .ForEach((ref Velocity velocity, in MaxVelocity maxVelocity) =>
                {
                    velocity.Value = math.normalizesafe(velocity.Value) * math.min(maxVelocity.Value, math.length(velocity.Value));
                })
                .ScheduleParallel();
        }
    }
}
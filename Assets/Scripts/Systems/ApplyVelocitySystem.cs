using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public class ApplyVelocitySystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities
                .ForEach((ref Translation translation, in Velocity velocity) =>
                {
                    translation.Value += velocity.Value;
                })
                .ScheduleParallel();
        }
    }
}
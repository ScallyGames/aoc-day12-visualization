using System;
using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    [UpdateBefore(typeof(ApplyVelocitySystem))]
    public class MoveAlongPathSystem : SystemBase
    {
        private EntityCommandBufferSystem ecbSystem;

        protected override void OnCreate()
        {
            this.ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var ecb = ecbSystem.CreateCommandBuffer().AsParallelWriter();
            
            Entities
                .ForEach((Entity entity, int entityInQueryIndex, ref Velocity velocity, ref NextPathIndex nextPathIndex, in LocalToWorld ltw, in Speed speed, in DynamicBuffer<PathNode> path) =>
                {
                    return;
                    if (nextPathIndex.Value >= path.Length)
                    {
                        ecb.DestroyEntity(entityInQueryIndex, entity);
                        var ent = ecb.CreateEntity(entityInQueryIndex);
                        ecb.AddComponent<SpawnInstruction>(entityInQueryIndex, ent);
                        return;
                    }

                    var targetPosition = path[nextPathIndex.Value].Value;
                    var position = ltw.Position;
                    var vectorToTarget = targetPosition - position;
                    
                    if (math.lengthsq(vectorToTarget) < 0.005f)
                    {
                        nextPathIndex.Value++;
                        velocity.Value = float3.zero;
                        return;
                    }

                    
                    var targetVelocity= math.normalize(vectorToTarget) * math.min(math.length(vectorToTarget), speed.Value);
                    
                    velocity.Value += (targetVelocity - velocity.Value) * deltaTime * 6;
                })
                .ScheduleParallel();
            
            this.ecbSystem.AddJobHandleForProducer(this.Dependency);
        }
    }
}
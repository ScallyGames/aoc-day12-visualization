using System;
using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Systems
{
    [UpdateBefore(typeof(ApplyVelocitySystem))]
    [UpdateAfter(typeof(MoveAlongPathSystem))]
    public class WanderSystem : SystemBase
    {
        private Random rng;

        protected override void OnCreate()
        {
            this.rng = Random.CreateFromIndex(27);

            // for (int i = 0; i < 8; i++)
            // {
            //     var rotation = quaternion.AxisAngle(new float3(0, 1, 0), i * 45 * math.PI / 180f);
            //     var vector = math.mul(rotation, new float3(0, 0, 1));
            //
            //     var angle = math.atan2(vector.z, vector.x) - math.PI / 2;
            //     if (angle < 0) angle += 2 * math.PI;
            //     Debug.Log(Math.Round(angle / math.PI * 180f));
            // }
        }

        protected override void OnUpdate()
        {   
            var deltaTime = Time.DeltaTime;
            var rng = Random.CreateFromIndex(this.rng.NextUInt());
            
            Entities
                .WithoutBurst()
                .ForEach((ref Velocity velocity, ref WanderAngle wanderAngle, in LocalToWorld ltw, in WanderParams wanderParams) =>
                {
                    // if (math.lengthsq(velocity.Value) < 0.001f) return;

                    var wanderCenter = /*math.normalize(velocity.Value)*/ /*ltw.Forward*/ new float3(0, 0, 1) * wanderParams.Distance;
                    var wanderRotation = quaternion.AxisAngle(new float3(0, 1, 0), wanderAngle.Value);
                    Debug.DrawRay(ltw.Position, wanderCenter, Color.magenta);
                    var wanderOffset = math.mul(
                        wanderRotation,
                        new float3(0, 0, wanderParams.Radius)
                    );
                    var wanderVelocity = wanderCenter +
                                         // math.mul(
                                         //     quaternion.AxisAngle(new float3(0, 1, 0), math.atan2(velocity.Value.y, velocity.Value.x)), 
                                         wanderOffset;
                                         // );
                    
                    // for (int i = 0; i < 8; i++)
                    // {
                    //     Debug.DrawRay(ltw.Position + wanderCenter, 
                    //         math.mul(quaternion.AxisAngle(new float3(0, 1, 0), i * math.PI / 4f), new float3(0, 0, wanderParams.AdjustmentRadius)), Color.magenta);
                    // }

                    wanderVelocity.y *= 0.1f;
                    wanderVelocity = math.normalize(wanderVelocity) * wanderParams.Strength;


                    Debug.DrawRay(ltw.Position, wanderVelocity, Color.red);
                    
                    
                    var offsetRotation = quaternion.AxisAngle(new float3(0, 1, 0), rng.NextFloat(0, 2 * math.PI));
                    var newPosition =
                        wanderOffset +
                        math.mul(offsetRotation, new float3(0, 0, wanderParams.AdjustmentRadius));

                    var angleNew = math.atan2(newPosition.z, newPosition.x) - math.PI / 2;
                    if (angleNew < 0) angleNew += 2 * math.PI;
                    angleNew = 2 * math.PI - angleNew;
                    
                    wanderAngle.Value = angleNew;

                    Debug.DrawRay(float3.zero, wanderOffset, Color.cyan);
                    
                    for (int i = 0; i < 8; i++)
                    {
                        Debug.DrawRay(float3.zero + wanderOffset, 
                            math.mul(quaternion.AxisAngle(new float3(0, 1, 0), i * math.PI / 4f), new float3(0, 0, wanderParams.AdjustmentRadius)), Color.magenta);
                    }
                    Debug.DrawRay(float3.zero, newPosition, Color.red);


                    Debug.DrawRay(ltw.Position, wanderVelocity * 10, Color.green);
                    Debug.DrawRay(ltw.Position, velocity.Value * 10, Color.blue);
                    float3 steeringForce = (wanderVelocity - velocity.Value) * wanderParams.SteeringStrength * Time.DeltaTime;
                    Debug.DrawRay(ltw.Position + velocity.Value, steeringForce * 10, Color.red);
                    velocity.Value += steeringForce; // math.mul(ltw.Rotation, wanderVelocity) * deltaTime;

                    // Debug.DrawRay(ltw.Position, math.mul(ltw.Rotation, wanderVelocity), Color.yellow);
                })
                .Run();
                // .ScheduleParallel();
        }
    }
}
using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct WanderParams : IComponentData
    {
        public float Strength;
        public float Distance;
        public float Radius;
        public float AdjustmentRadius;
        public float SteeringStrength;
    }
}
using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct Velocity : IComponentData
    {
        public float3 Value;
    }
}
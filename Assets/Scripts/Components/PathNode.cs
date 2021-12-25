using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Components
{
    [InternalBufferCapacity(16)]
    public struct PathNode : IBufferElementData
    {
        public float3 Value;
    }
}
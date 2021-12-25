using System;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Components
{
    [Serializable]
    [MaterialProperty("ColorMine", MaterialPropertyFormat.Float4)]
    public struct MaterialColorMine : IComponentData
    {
        public float4 Value;
    }
}
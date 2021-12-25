using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct MaxVelocity : IComponentData
    {
        public float Value;
    }
}
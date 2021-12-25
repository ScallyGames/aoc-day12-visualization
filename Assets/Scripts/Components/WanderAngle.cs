using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct WanderAngle : IComponentData
    {
        public float Value;
    }
}
using Unity.Entities;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct EntityPrefabs : IComponentData
    {
        public Entity SubmarinePrefab;
    }
}
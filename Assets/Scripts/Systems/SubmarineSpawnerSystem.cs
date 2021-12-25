using System.Collections.Generic;
using System.Linq;
using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Systems
{
    public class SubmarineSpawnerSystem : SystemBase
    {
        private EntityCommandBufferSystem ecbSystem;
        
        private int burstSize = 1;

        private CaveSystemGenerator generator;

        private Random rng;

        private int nextPathIndex = 0;
        private List<List<Cave>> paths;

        protected override void OnCreate()
        {
            this.rng = Random.CreateFromIndex(27);
            this.ecbSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {   
            if (generator == null)
            {
                generator = GameObject.FindObjectOfType<CaveSystemGenerator>();
                return;
            }

            if (this.paths == null || this.paths.Count == 0)
            {
                this.paths = generator.caves["start"].GetAllPathsTo("end", new List<Cave>()).OrderBy(x => rng.NextFloat(0f, 1f)).ToList();
            }
        
            if (Input.GetButtonDown("Jump"))
            {
                for (int i = 0; i < burstSize; i++)
                {
                    var ent = EntityManager.CreateEntity();
                    EntityManager.AddComponent<SpawnInstruction>(ent);
                }
            }

            var ecb = this.ecbSystem.CreateCommandBuffer();
            var prefab = GetSingleton<EntityPrefabs>().SubmarinePrefab;

            int numberOfSpawns = 0;
            
            Entities
                .WithoutBurst()
                .WithAll<SpawnInstruction>()
                .ForEach((Entity entity) =>
                {
                    numberOfSpawns++;
                    ecb.DestroyEntity(entity);
                })
                .Run();

            for (int i = 0; i < numberOfSpawns; i++)
            {
                if (nextPathIndex >= this.paths.Count) return;
                    
                var color = Color.HSVToRGB(((nextPathIndex * 19) % 360) / 360f, 1.0f, 1.0f);
                SpawnNext(EntityManager, prefab, this.paths[nextPathIndex], color, ref rng);
                nextPathIndex++;
            }
            
            this.ecbSystem.AddJobHandleForProducer(this.Dependency);
        }

        private static void SpawnNext(
            EntityManager entityManager, 
            Entity prefab, 
            List<Cave> path,
            Color color, 
            ref Random rng
        )
        {
        
            Entity entity = entityManager.Instantiate(prefab);

            entityManager.AddComponent<NextPathIndex>(entity);
            var buffer = entityManager.AddBuffer<PathNode>(entity);
            
            foreach (var node in path)
            {
                buffer.Add(new PathNode() {Value = node.GameObject.transform.position });
            }

            
            entityManager.SetComponentData(entity, new Translation()
            {
                Value = path[0].GameObject.transform.position,
            });
            
            entityManager.SetComponentData(entity, new Speed()
            {
                Value = entityManager.GetComponentData<Speed>(entity).Value * rng.NextFloat(0.9f, 1.1f),
            });
            
            entityManager.AddComponentData(entityManager.GetBuffer<LinkedEntityGroup>(entity)[1].Value, new MaterialColorMine()
            {
                Value = new float4(color.r, color.g, color.b, 1f),
            });

        }
    }
}

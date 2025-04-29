using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class ZombieTagAuthoring : MonoBehaviour
{
    class Baker : Baker<ZombieTagAuthoring>
    {
        public override void Bake(ZombieTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<ZombieTag>(entity);
        }
    }
}
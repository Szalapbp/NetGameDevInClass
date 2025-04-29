using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerTagAuthoring : MonoBehaviour
{
    class Baker : Baker<PlayerTagAuthoring>
    {
        public override void Bake(PlayerTagAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PlayerTag>(entity);
        }
    }
}

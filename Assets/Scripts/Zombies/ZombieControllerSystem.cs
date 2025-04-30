using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using UnityEngine;

public partial struct ZombieControllerSystem : ISystem
{
    public void OnCreate(ref SystemState state) { }

    public void OnDestroy(ref SystemState state) { }

    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime; // Get time step

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        // Query zombies
        foreach (var (zombieTransform, zombieEntity) in SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithEntityAccess()
                     .WithAll<ZombieTag>())
        {
            // Initialize closest player variables
            float3 closestPlayerPosition = float3.zero;
            float closestDistance = float.MaxValue;

            // Iterate over players to find the closest one
            foreach (var playerTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>())
            {
                float distance = math.distance(zombieTransform.ValueRO.Position, playerTransform.ValueRO.Position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayerPosition = playerTransform.ValueRO.Position;
                }
            }

            // Move the zombie toward the closest player
            if (closestDistance < float.MaxValue)
            {
                float3 direction = math.normalize(closestPlayerPosition - zombieTransform.ValueRO.Position);
                float speed = 3f; // Adjust zombie movement speed
                zombieTransform.ValueRW.Position += direction * speed * deltaTime;

                // If the zombie is close enough, deal damage and despawn
                if (closestDistance <= 1.5f) // Attack range
                {
                    DealDamageToPlayer(ref state, closestPlayerPosition);
                    ecb.DestroyEntity(zombieEntity); // Despawn the zombie
                }
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    private void DealDamageToPlayer(ref SystemState state, float3 playerPosition)
    {
        foreach (var (playerHealth, playerTransform) in SystemAPI.Query<RefRW<HealthComponent>, RefRO<LocalTransform>>())
        {
            if (math.distance(playerPosition, playerTransform.ValueRO.Position) < 1.5f)
            {
                playerHealth.ValueRW.CurrentHealth -= 10f; // Deal damage
                UnityEngine.Debug.Log($"Player at {playerPosition} took 10 damage!");

                // Check if player's health has reached 0
                if (playerHealth.ValueRW.CurrentHealth <= 0)
                {
                    // Increment death counter
                    playerHealth.ValueRW.deaths += 1;
                    UnityEngine.Debug.Log($"Player at {playerPosition} died! Total deaths: {playerHealth.ValueRW.deaths}");

                    // Optional: Handle additional logic for player death (e.g., triggering respawn)
                }
            }
        }
    }

}
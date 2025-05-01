using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

[UpdateInGroup(typeof(SimulationSystemGroup))] // Ensures the system runs during the simulation phase
public partial struct GameOverSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        string winnerName = null;
        string loserName = null;

        foreach (var (health, entity) in SystemAPI.Query<RefRO<HealthComponent>>().WithEntityAccess())
        {
            // Check if the player has died 3 times
            if (health.ValueRO.deaths >= 3)
            {
                // Assign the loser
                loserName = $"Player {health.ValueRO.ownerNetworkID}";

                // Assign the winner (in multiplayer or find AI placeholder in single-player)
                foreach (var (otherHealth, otherEntity) in SystemAPI.Query<RefRO<HealthComponent>>().WithEntityAccess())
                {
                    if (otherEntity != entity)
                    {
                        winnerName = $"Player {otherHealth.ValueRO.ownerNetworkID}";
                    }
                }

                // Handle single-player placeholder
                if (string.IsNullOrEmpty(winnerName)) winnerName = "CPU";

                // Store names for the Game Over screen
                PlayerPrefs.SetString("WinnerName", winnerName);
                PlayerPrefs.SetString("LoserName", loserName);

                // Transition to the Game Over scene
                SceneManager.LoadScene("GameOver");

                Debug.Log($"Game Over triggered! Winner: {winnerName}, Loser: {loserName}");
                break; // Stop after triggering game over
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}

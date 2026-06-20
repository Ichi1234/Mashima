using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float appoximateNoise = 8f;

    private Player player;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void InitializePlayer(Player player) => this.player = player;

    public Vector3 PlayerAppoximatedLocation()
    {
        if (player == null) return Vector3.zero;

        float noiseX = Random.Range(-appoximateNoise, appoximateNoise);
        float noiseZ = Random.Range(-appoximateNoise, appoximateNoise);

        Vector3 rawPoint = new Vector3(
            player.transform.position.x + noiseX,
            player.transform.position.y,
            player.transform.position.z + noiseZ
        );

        if (NavMesh.SamplePosition(rawPoint, out NavMeshHit hit, appoximateNoise / 2, NavMesh.AllAreas))
            return hit.position;

        return player.transform.position;
    }

    public CapsuleCollider GetPlayerDetectionCollider() => player.DetectionCollider;
}

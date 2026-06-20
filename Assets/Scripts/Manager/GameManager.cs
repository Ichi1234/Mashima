using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float defaultAppoximateNoise = 8f;
    [SerializeField] private EyeBlinkingVfx deathVfx;

    private float appoximateNoise;

    private Player player;

    private Vector3 playerSpawnPos;

    public static GameManager Instance;

    public System.Action OnPlayerDeath;

    public float DefaultNoise => defaultAppoximateNoise;

    private void Awake()
    {
        Instance = this;

        appoximateNoise = defaultAppoximateNoise;
    }

    private void OnEnable() => OnPlayerDeath += PlayerDeath;

    private void OnDisable() => OnPlayerDeath -= PlayerDeath;

    public void InitializePlayer(Player player)
    {
        this.player = player;

        playerSpawnPos = player.transform.position;
    }

    public void PlayerDeath()
    {
        deathVfx.Play();
        player.transform.position = playerSpawnPos;
    }

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

        if (NavMesh.SamplePosition(rawPoint, out NavMeshHit hit, defaultAppoximateNoise / 2, NavMesh.AllAreas))
            return hit.position;

        return player.transform.position;
    }

    public CapsuleCollider GetPlayerDetectionCollider() => player.DetectionCollider;

    public void SetAppoximateNoise(float noise) => appoximateNoise = noise;

    public void ResetAppoximateNoise() => appoximateNoise = defaultAppoximateNoise;
} 

using UnityEngine;
using UnityEngine.AI;


[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    [SerializeField] private float defaultAppoximateNoise = 8f;
    [SerializeField] private WakeUpVfx deathVfx;

    private float appoximateNoise;

    private Player player;

    private Vector3 playerSpawnPos;

    public static GameManager Instance;

    public System.Action OnPlayerDeath;

    public System.Action OnElectricRepaired;

    public float DefaultNoise => defaultAppoximateNoise;
    public PlayerMode CurPlayerMode => player.CurPlayerMode;

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

        if (NavMesh.SamplePosition(rawPoint, out NavMeshHit hit, appoximateNoise / 2, NavMesh.AllAreas))
            return hit.position;

        NavMesh.SamplePosition(player.transform.position, out NavMeshHit fallbackHit, 12f, NavMesh.AllAreas);
        return fallbackHit.position;
    }

    public CapsuleCollider GetPlayerDetectionCollider()
    {
        if (player == null)
        {
            Debug.Log("Player is null");
        }

        else if (player.DetectionCollider == null)
        {
            Debug.Log("HOW TF YOU ARE NULL PLAYER COLLIDER");
        }

        return player.DetectionCollider;
    }

    public void SetAppoximateNoise(float noise) => appoximateNoise = noise;

    public void ResetAppoximateNoise() => appoximateNoise = defaultAppoximateNoise;
} 

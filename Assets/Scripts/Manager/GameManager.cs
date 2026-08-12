using UnityEngine;
using UnityEngine.AI;


[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    [SerializeField] private float defaultAppoximateNoise = 8f;
    [SerializeField] private float doorSlamForce = 300;

    private float appoximateNoise;

    private Player player;

    private Vector3 playerSpawnPos;

    public static GameManager Instance;

    public System.Action OnPlayerDeath;

    public System.Action OnElectricRepaired;
    public bool IsGameStarted { get; private set; } = false;
    public bool IsGameEnded { get; private set; } = false;

    public bool PlayerIsRunning => player.IsRunning();
    public float DoorSlamForce => doorSlamForce;

    public float DefaultNoise => defaultAppoximateNoise;
    public PlayerMode CurPlayerMode => player.CurPlayerMode;
    public bool IsInVR => player.CurPlayerMode == PlayerMode.VR;

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

        player.PlayerCanvas.PlayWakeUpEffect();
    }

    public void PlayerDeath()
    {
        player.PlayerCanvas.PlayWakeUpEffect();

        ResearchLogger.Log("Player death");
        player.ResetPlayer(playerSpawnPos);
        ResetAppoximateNoise();
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

    public void SetGameEnd()
    {
        player.PlayerCanvas.PlayEndingScene();

        IsGameEnded = true;
    }

    public void BeginTheGame()
    {
        ResearchLogger.Log("Game starto!");
        IsGameStarted = true;
    }
} 

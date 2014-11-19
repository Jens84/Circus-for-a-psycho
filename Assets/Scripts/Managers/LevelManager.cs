using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{

    public static LevelManager Instance { get; private set; }   // singleton - (design pattern that restricts the instantiation of a class to one object)

    // So that we can access the player and camera anywhere easily through the level manager
    public Player Player { get; private set; }
    public CameraController Camera { get; private set; }
    public TimeSpan RunningTime { get { return DateTime.UtcNow - _started; } }

    public int CurrentTimeBonus
    {
        get
        {
            var secondDifference = (int)(BonusCutoffSeconds - RunningTime.TotalSeconds);
            return Mathf.Max(0, secondDifference) * BonusSecondMultiplier;
        }
    }

    private List<Checkpoint2D> _checkpoints;     // all the checkpoints in a list
    private int _currentCheckpointIndex;
    private DateTime _started;
    private int
        _savedPoints,
        _savedBacon;

    public Checkpoint2D DebugSpawn;  // to spawn the player in different points while testing
    public int BonusCutoffSeconds;  // the max amount of seconds available the player has to get a bonus, till a checkpoint is reached, set for each level, any time after that the player won't receive a bonus
    public int BonusSecondMultiplier;   // how much bonus the player should receive (based on the time)
    private int LvL = 1;

    public void Awake()                         // initialization
    {
        _savedPoints = GameManager.Instance.Points; // Points through levels
        _savedBacon = 0;
        Instance = this;
    }

    public void Start()
    {
        // Our checkpoints collection will be order by the position of the checkpoints in the x axes
        _checkpoints = FindObjectsOfType<Checkpoint2D>().OrderBy(t => t.transform.position.x).ToList();
        _currentCheckpointIndex = _checkpoints.Count > 0 ? 0 : -1;      // if you find checkpoints in the scene start from 0 else -1

        // Cashing in level manager
        Player = FindObjectOfType<Player>();
        Camera = FindObjectOfType<CameraController>();

        _started = DateTime.UtcNow;

        var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();    // Find all objects that implement the Interface
        foreach (var listener in listeners)
        {
            for (var i = _checkpoints.Count - 1; i >= 0; i--)   // loop through all the checkpoints backwards
            {
                var distance = ((MonoBehaviour)listener).transform.position.x - _checkpoints[i].transform.position.x;   // value between checkpoint and the object we are checking
                if (distance < 0)
                    continue;

                _checkpoints[i].AssignObjectToCheckpoint(listener);
                break;  // break after finding and assigning the first object
            }
        }

        // Conditional preprocessor directives
        // Does not exist after we build/stand alone - only used while in editor/debugging
#if UNITY_EDITOR
        if (DebugSpawn != null)
            DebugSpawn.SpawnPlayer(Player);
        else if (_currentCheckpointIndex != -1)
            _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);
#else
        if(_currentCheckpointIndex != -1)
            _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);
#endif
    }

    public void Update()        // for checkpoints
    {
        var isAtLastCheckpoint = _currentCheckpointIndex + 1 >= _checkpoints.Count;     // boolean to check for last checkpoint
        if (isAtLastCheckpoint)
            return;

        var distanceToNextCheckpoint = _checkpoints[_currentCheckpointIndex + 1].transform.position.x - Player.transform.position.x;
        if (distanceToNextCheckpoint >= 0)
            return;

        // After this line, we have hit a new checkpoint
        _checkpoints[_currentCheckpointIndex].PlayerLeftCheckpoint();
        _currentCheckpointIndex++;
        _checkpoints[_currentCheckpointIndex].PlayerHitCheckpoint();

        // Get points based on checkpoints
        GameManager.Instance.AddPoints(CurrentTimeBonus);
        _savedPoints = GameManager.Instance.Points;
        _savedBacon = GameManager.Instance.Bacon;
        _started = DateTime.UtcNow;
    }

    public void GotoNextLevel(string levelName)
    {
        StartCoroutine(GotoNextLevelCo(levelName));
    }

    private IEnumerator GotoNextLevelCo(string levelName)
    {
        Player.FinishLevel();

        FloatingText.Show("Level Complete!", "CheckpointText", new CenteredTextPositioner(.2f));
        yield return new WaitForSeconds(1f);
        GameManager.Instance.AddPoints(CurrentTimeBonus);
        // Show all points in end of level
        FloatingText.Show(string.Format("+{0} points!", GameManager.Instance.Points), "CheckpointText", new CenteredTextPositioner(.1f));
        yield return new WaitForSeconds(5f);

        GameManager.Instance.ResetBacon(0);
        GameManager.Instance.ResetBalloons(0);

        int tmp = GameManager.Instance.Points;
        if (LvL != 1)
            tmp = tmp / 2;

        if (tmp > 170)
        {
            FloatingText.Show("Well done! Great score!!!", "CheckpointText", new CenteredTextPositioner(.1f));
            yield return new WaitForSeconds(5f);
        }

        LvL += 1;

        if (string.IsNullOrEmpty(levelName))
            Application.LoadLevel(0);
        else
            Application.LoadLevel(levelName);
    }

    public void DisplayInfoText(string[] Text)
    {
        StartCoroutine(DisplayInfoTextCo(Text));
    }

    private IEnumerator DisplayInfoTextCo(string[] text)
    {

        for (int i = 0; i < text.Length; i++)
        {
            FloatingText.Show(text[i],
                "InformationText",
                new CenteredTextPositioner(.1f));

            yield return new WaitForSeconds(1f);
        }
    }

    public void KillPlayer()
    {
        StartCoroutine(KillPlayerCo());
    }

    private IEnumerator KillPlayerCo()  // Coroutine to murder the player
    {
        Player.Kill();
        Camera.IsFollowing = false;
        yield return new WaitForSeconds(2f);

        Camera.IsFollowing = true;

        if (_currentCheckpointIndex != -1)
            _checkpoints[_currentCheckpointIndex].SpawnPlayer(Player);

        _started = DateTime.UtcNow;
        GameManager.Instance.ResetBacon(_savedBacon);
        GameManager.Instance.ResetPoints(_savedPoints); // Reset points after respawn to the ones the player had before dying
    }
}

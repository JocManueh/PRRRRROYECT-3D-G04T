using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private float globalTime = 0;

    private int score = 0;
    private int ItemsCount = 0;

    public float GlobalTime { get => globalTime; set => globalTime = value; }
    public int Score { get => score; set => score = value; }
    public int ItemsCount1 { get => ItemsCount; set => ItemsCount = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

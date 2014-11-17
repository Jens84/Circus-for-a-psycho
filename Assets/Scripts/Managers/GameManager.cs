using UnityEngine;

public class GameManager
{
    private static GameManager _instance;   // singleton - (design pattern that restricts the instantiation of a class to one object)
    public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } } // return the _instance of the Game manager if not null, else instantiate a new one, through the private constructor

    public int Points { get; private set; }

    // Empty Constructor  + private, so that only the GameManager can instance it
    private GameManager()
    {
    }

    public void Reset()
    {
        Points = 0;
    }

    public void ResetPoints(int points)
    {
        Points = points;
    }

    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }
}

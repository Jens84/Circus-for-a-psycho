using UnityEngine;

public class GameManager
{
    private static GameManager _instance;   // singleton - (design pattern that restricts the instantiation of a class to one object)
    public static GameManager Instance { get { return _instance ?? (_instance = new GameManager()); } } // return the _instance of the Game manager if not null, else instantiate a new one, through the private constructor

    public int Points { get; private set; }
    public int Bacon { get; private set; }
    public int Balloon { get; private set; }

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

    public void ResetBacon(int bacon)
    {
        Bacon = bacon;
    }

    public void ResetBalloons(int balloon)
    {
        Balloon = balloon;
    }

    public void AddPoints(int pointsToAdd)
    {
        Points += pointsToAdd;
    }

    public void AddBacon(int baconToAdd)
    {
        Bacon += baconToAdd;
    }

    public void AddBalloon(int balloonToAdd)
    {
        Balloon += balloonToAdd;
    }
}

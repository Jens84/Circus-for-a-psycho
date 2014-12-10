using UnityEngine;
using System.Collections;

public class EndingGame : MonoBehaviour
{

    private SimpleEnemyAI _director;

    public void Awake()
    {
        _director = GameObject.FindObjectOfType<SimpleEnemyAI>();
    }

    void Update()
    {
        if (!_director.gameObject.activeSelf)
        {
            // Debug.Log("Director died, starting Coroutine");
            StartCoroutine(LowerPlat());
        }
    }

    private IEnumerator LowerPlat()  // Coroutine to murder the player
    {
        yield return new WaitForSeconds(2f);
        if (transform.position.y > 210)
            transform.Translate(0, -10 * Time.deltaTime, 0);
    }
}

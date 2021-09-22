using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ToArcade()
    {
        SceneManager.LoadSceneAsync(2);
        StartCoroutine(loadTargetScene(1));

    }

    public void ToSurvival()
    {
        SceneManager.LoadSceneAsync(2);
        StartCoroutine(loadTargetScene(0));
    }

    private IEnumerator loadTargetScene(int index)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync(index);
    }
}

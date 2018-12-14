using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoad : MonoBehaviour
{
    [SerializeField] private Image _loading;
    private CinematicHandling _cinematic;

    public delegate void ExitDelegate(float length);

    public static event ExitDelegate OnExitScene;
    
    private void Awake()
    {
        _cinematic = GetComponentInChildren<CinematicHandling>();
    }

    private bool loading = false;
    
    public void ChangeScene(string sceneName)
    {
        if (!loading)
            StartCoroutine(LoadYourAsyncScene(sceneName));
    }

    private IEnumerator LoadYourAsyncScene(string sceneName)
    {
        _loading.fillAmount = 0;

        if (OnExitScene != null)
            OnExitScene(0.5f);
        
        _cinematic.FadeToBlack(0.5f);
        yield return new WaitForSeconds(0.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            _loading.fillAmount = asyncLoad.progress + 0.1f;
            yield return null;
        }
    }
}

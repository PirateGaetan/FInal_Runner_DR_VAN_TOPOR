using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    public void changeScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}


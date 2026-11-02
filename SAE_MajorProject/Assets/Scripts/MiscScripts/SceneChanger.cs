using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    private string WebpageURL = "";
    public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void ChangeScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public void LoadWebpage()
    {
        Application.OpenURL(WebpageURL);
    }

    public void QuitGame()
    {
        // run QUit game analytics
        Application.Quit();
    }
}

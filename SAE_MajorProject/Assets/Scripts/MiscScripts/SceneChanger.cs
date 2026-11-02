using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    private string WebpageURL = "";
    async public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    async public void ChangeScene(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    async public void LoadWebpage()
    {
        Application.OpenURL(WebpageURL);
    }

    async public void QuitGame()
    {
        // run QUit game analytics
        Application.Quit();
    }
}

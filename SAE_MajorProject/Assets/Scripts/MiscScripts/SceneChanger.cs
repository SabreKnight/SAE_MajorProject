using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    private string WebpageURL = "https://forms.gle/wDJByGH88dzX3REg9";
    public void ChangeScene(string SceneName)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("current scene is: " + sceneName);
        if(sceneName == "Scene2_InputTest")
        {
            Debug.Log("calling final analytic upload in: " + sceneName);
            AnalyticsManager.Instance.FinalStats(false);
        }
        SceneManager.LoadScene(SceneName);
    }

    public void ChangeScene(int SceneIndex)
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log("current scene is: " + sceneName);
        if(sceneName == "Scene2_InputTest")
        {
            Debug.Log("calling final analytic upload in: " + sceneName);
            AnalyticsManager.Instance.FinalStats(false);
        }
        SceneManager.LoadScene(SceneIndex);
    }

    public void LoadWebpage()
    {
        Application.OpenURL(WebpageURL);
    }

    public void QuitGame()
    {
        AnalyticsManager.Instance.FinalStats(true);
    }
}

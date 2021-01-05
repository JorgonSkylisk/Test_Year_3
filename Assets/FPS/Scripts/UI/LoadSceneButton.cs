using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoadSceneButton : MonoBehaviour
{
    public string sceneName = "";

    private void Update()
    {
        string lastlevel = PlayerPrefs.GetString("lastlevel");

        if (EventSystem.current.currentSelectedGameObject == gameObject 
            && Input.GetButtonDown(GameConstants.k_ButtonNameSubmit))
        {
            LoadTargetScene();
            LoadLastScene();
        }
    }

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadLastScene()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("lastlevel"));
    }
}

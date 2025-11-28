using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingScene");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("메인메뉴로 돌아가기");
    }
}

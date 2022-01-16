/* 게임 씬의 전환, 종료
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void SetTitleScene()
    {
        SceneManager.LoadScene(0);
    }

    public void SetWaveScene()
    {
        SceneManager.LoadScene(1);
    }

    public void SetExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

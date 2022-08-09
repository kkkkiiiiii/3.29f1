using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void GotoGameScene()
    {
        SceneManager.LoadScene(0);
    }

    public void GotoShopScene()
    {
        SceneManager.LoadScene(1);
    }
}

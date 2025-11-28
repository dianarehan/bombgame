using UnityEngine;

public class QuickMenu : MonoBehaviour
{
    public void LoadScene(){
        SceneTransitionManager.Instance.LoadScene("Funeral");
    }
    public void ExitGame(){
        Application.Quit();
    }
}

using UnityEngine;

public class LoadScreenView : MonoBehaviour
{
    public void StartLoad()
    {
        gameObject.SetActive(true);
    }

    public void FinishLoad()
    {
        gameObject.SetActive(false);
    }
}

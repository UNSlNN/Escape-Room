using UnityEngine;

public class UIDisplay : MonoBehaviour
{
    public GameObject winUI;
    public GameObject loseUI;
    Player status;

    void Start()
    {
        // Set Active UI first start
        winUI.SetActive(false);
        loseUI.SetActive(false);
        status = FindObjectOfType<Player>();
    }
    void Update()
    {
        if(status.islose)
        {
            UpdateStatus(loseUI);
        }
        if(status.iswinner)
        {
            UpdateStatus(winUI);
        }
    }
    void UpdateStatus(GameObject _status)
    {
        _status.SetActive(true);
        Time.timeScale = 0; // Pause Game 
    }
}

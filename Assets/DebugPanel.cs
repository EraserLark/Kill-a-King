using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugPanel : MonoBehaviour
{
    GameObject debugPanel;
    TextMeshProUGUI speedText;
    bool panelOpen = false;

    private void Awake()
    {
        debugPanel = this.gameObject;
        speedText = transform.GetComponentInChildren<TextMeshProUGUI>();
        MoveMenu();
    }

    public void MoveMenu()
    {
        panelOpen = !panelOpen;

        if(panelOpen)
        {
            debugPanel.transform.position += new Vector3(200f, 0f, 0f);
        }
        else if(!panelOpen)
        {
            debugPanel.transform.position += new Vector3(-200f, 0f, 0f);
        }
    }

    public void UpdateSpeed(float speed)
    {
        speedText.text = "Speed: " + Mathf.RoundToInt(speed);
        speedText.text += "\nExact Speed: " + speed;
    }
}

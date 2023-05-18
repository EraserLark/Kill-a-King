using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugPanel : MonoBehaviour
{
    GameObject debugPanel;
    TextMeshProUGUI speedText;
    bool panelOpen = false;
    float width;

    [SerializeField]
    GameObject player;
    Rigidbody playerRb;

    [SerializeField]
    RectTransform phoneBorder;

    private void Awake()
    {
        debugPanel = this.gameObject;
        width = debugPanel.GetComponent<RectTransform>().sizeDelta.x;
        speedText = transform.GetComponentInChildren<TextMeshProUGUI>();
        MoveMenu();
    }

    private void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float currentSpeed = playerRb.velocity.magnitude;
        UpdateSpeed(currentSpeed);
    }

    public void MoveMenu()
    {
        panelOpen = !panelOpen;

        if(panelOpen)
        {
            debugPanel.transform.position += new Vector3(width, 0f, 0f);
        }
        else if(!panelOpen)
        {
            debugPanel.transform.position += new Vector3(-width, 0f, 0f);
        }
    }

    public void UpdateSpeed(float speed)
    {
        speedText.text = "Speed: " + Mathf.RoundToInt(speed);
        speedText.text += "\nExact Speed: " + speed;
    }

    public void RevealPhoneBorder(Toggle toggle)
    {
        phoneBorder.gameObject.SetActive(toggle.isOn);
    }
}

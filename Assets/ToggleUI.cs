using UnityEngine;
using System;

public class ToggleUI : MonoBehaviour
{
    public GameObject networkUIPanel;
    private Boolean toggle = true;

    public void ToggleUIButtonHandler()
    {
        toggle = !toggle;
        networkUIPanel.SetActive(toggle);
    }
}

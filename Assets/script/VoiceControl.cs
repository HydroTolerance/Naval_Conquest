using UnityEngine;
using TMPro;
using Photon.Voice.Unity;

public class VoiceControl : MonoBehaviour
{
    public TMP_Text muteStateText; // Reference to the TMP_Text component to display the mute state
    private Recorder recorder;

    private void Start()
    {
        recorder = FindObjectOfType<Recorder>();
        UpdateMuteStateText();
    }

    public void ToggleMute()
    {
        if (recorder != null)
        {
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
            UpdateMuteStateText();
        }
    }

    private void UpdateMuteStateText()
    {
        if (muteStateText != null)
        {
            muteStateText.text = recorder.TransmitEnabled ? "Unmuted" : "Muted";
        }
    }
}

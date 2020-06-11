using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class VRDesktopCameraEnabler : MonoBehaviour
{
    // Sets and unsets objects based on editor being in VR or desktop mode
    // Prevents issues with camera and audio listeners

    public List<GameObject> m_vrCameraEnables;
    public List<GameObject> m_vrCameraActivates;
    public List<GameObject> m_vrAudioListeners;


    public List<GameObject> m_editorCameraEnables;
    public List<GameObject> m_editorCameraActivates;
    public List<GameObject> m_editorAudioListeners;

    public bool m_state = true;
    private bool previousState;

    // Start is called before the first frame update
    void Start()
    {
        previousState = m_state;

        UpdateState(true);
    }

    private void setCameraEnabled(GameObject obj, bool state)
    {
        Camera camera = obj?.GetComponentInChildren<Camera>();

        if (camera != null)
        {
            camera.enabled = state;
        }
    }

    private void setAudioListenerEnabled(GameObject obj, bool state)
    {
        AudioListener audioListener = obj?.GetComponentInChildren<AudioListener>();

        if (audioListener != null)
        {
            audioListener.enabled = state;
        }
    }

    public void UpdateState(bool force=false)
    {
        m_state = VRKBMInputState.UsingVR;

        if (m_state != previousState || force)
        {
            Debug.Log($"Camera Switcher state is now {m_state}");

            m_vrCameraActivates.ForEach(o => o.SetActive(m_state));
            m_vrCameraEnables.ForEach(o => setCameraEnabled(o, m_state));
            m_vrAudioListeners.ForEach(o => setAudioListenerEnabled(o, m_state));

            m_editorCameraActivates.ForEach(o => o.SetActive(!m_state));
            m_editorCameraEnables.ForEach(o => setCameraEnabled(o, !m_state));
            m_editorAudioListeners.ForEach(o => setAudioListenerEnabled(o, !m_state));

            previousState = m_state;
        }
    }

    void Update()
    {
        UpdateState();
    }
}

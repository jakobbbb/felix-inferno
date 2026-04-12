using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    [SerializeField]
    private AudioMixer m_Mixer;

    private float m_Speed = 10.0f;

    public void Update() {
        var target_db = 0.0f;
        if (DialogueManager.Instance.Runner.IsDialogueRunning) {
            target_db = -8.0f;
        }

        m_Mixer.GetFloat("MusicVol", out float current);

        var target_db_smoothed = Mathf.Lerp(current, target_db, Time.deltaTime * m_Speed);
        m_Mixer.SetFloat("MusicVol", target_db_smoothed);
    }
}

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    [Header("Music Settings")]
    [SerializeField] private AudioClip musicClip;
    [SerializeField] [Range(0f, 1f)] private float volume = 0.5f;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool playOnAwake = true;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (musicClip != null)
        {
            audioSource.clip = musicClip;
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.playOnAwake = playOnAwake;

            if (playOnAwake)
            {
                audioSource.Play();
            }
        }
        else
        {
            Debug.LogWarning("BackgroundMusic: No audio clip assigned!", this);
        }
    }

    /// <summary>
    /// Call this method to change background tracks dynamically.
    /// </summary>
    public void PlayNewTrack(AudioClip newClip)
    {
        if (newClip == null) return;
        
        musicClip = newClip;
        audioSource.clip = musicClip;
        audioSource.Play();
    }

    /// <summary>
    /// Call this method to stop or pause music.
    /// </summary>
    public void StopMusic()
    {
        audioSource.Stop();
    }
}
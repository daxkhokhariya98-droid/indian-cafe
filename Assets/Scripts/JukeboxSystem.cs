using UnityEngine;
using System.Collections.Generic;

public enum TrackType
{
    DefaultAudio,   
    ChillBeats,     
    MidnightJazz,   
    NatureAmbience  
}

[System.Serializable]
public class JukeboxTrack
{
    public TrackType trackType;
    public AudioClip audioClip;
    [Tooltip("Is track ka passive bonus kitna strong hoga")]
    public float bonusValue = 1.1f; 
}

public class JukeboxSystem : MonoBehaviour
{
    public static JukeboxSystem Instance;

    [Header("Audio Source")]
    [SerializeField] private AudioSource musicAudioSource;

    [Header("Tracks Database")]
    [SerializeField] private List<JukeboxTrack> availableTracks = new List<JukeboxTrack>();

    [Header("Current Status")]
    [SerializeField] private TrackType currentTrackType;
    private bool isMusicPlaying = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        PlayTrack(TrackType.DefaultAudio);
    }

    public void ChangeTrack(TrackType newTrackType)
    {
        JukeboxTrack targetTrack = availableTracks.Find(t => t.trackType == newTrackType);

        if (targetTrack != null && targetTrack.audioClip != null)
        {
            currentTrackType = newTrackType;
            musicAudioSource.clip = targetTrack.audioClip;
            musicAudioSource.loop = true;
            
            if (isMusicPlaying)
            {
                musicAudioSource.Play();
            }
            
            Debug.Log($"Now Playing: {newTrackType}");
        }
        else
        {
            Debug.LogWarning($"Track {newTrackType} ka AudioClip assigned nahi hai!");
        }
    }

    private void PlayTrack(TrackType trackType)
    {
        ChangeTrack(trackType);
    }

    // --- UI BUTTONS KE LIYE DIRECT FUNCTIONS ---
    public void PlayDefaultAudio() { ChangeTrack(TrackType.DefaultAudio); }
    public void PlayChillBeats() { ChangeTrack(TrackType.ChillBeats); }
    public void PlayMidnightJazz() { ChangeTrack(TrackType.MidnightJazz); }
    public void PlayNatureAmbience() { ChangeTrack(TrackType.NatureAmbience); }

    // --- UI PANEL ON / OFF FUNCTIONS ---
    public void TurnMusicOff()
    {
        isMusicPlaying = false;
        if (musicAudioSource.isPlaying) musicAudioSource.Pause();
    }

    public void TurnMusicOn()
    {
        isMusicPlaying = true;
        if (!musicAudioSource.isPlaying && musicAudioSource.clip != null) musicAudioSource.Play();
    }

    public void ToggleMusic()
    {
        if (isMusicPlaying) TurnMusicOff();
        else TurnMusicOn();
    }

    // --- PASSIVE BONUSES CHECKERS ---
    public float GetPatienceMultiplier()
    {
        if (isMusicPlaying && currentTrackType == TrackType.ChillBeats)
        {
            JukeboxTrack track = availableTracks.Find(t => t.trackType == TrackType.ChillBeats);
            return track != null ? track.bonusValue : 1f;
        }
        return 1f;
    }

    public float GetSalarymanTipMultiplier()
    {
        if (isMusicPlaying && currentTrackType == TrackType.MidnightJazz)
        {
            JukeboxTrack track = availableTracks.Find(t => t.trackType == TrackType.MidnightJazz);
            return track != null ? track.bonusValue : 1f;
        }
        return 1f;
    }

    public float GetCleaningTimeMultiplier()
    {
        if (isMusicPlaying && currentTrackType == TrackType.NatureAmbience)
        {
            JukeboxTrack track = availableTracks.Find(t => t.trackType == TrackType.NatureAmbience);
            return track != null ? (2f - track.bonusValue) : 1f;
        }
        return 1f;
    }
}
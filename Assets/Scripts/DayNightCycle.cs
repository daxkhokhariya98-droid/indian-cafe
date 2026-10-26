using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [Tooltip("Full day duration in real-world seconds.")]
    [SerializeField] private float dayDurationInSeconds = 120f;
    [Range(0f, 1f)]
    [SerializeField] private float timeOfDay = 0.25f; // Starts at morning (0.25)

    [Header("Sun & Light Settings")]
    [SerializeField] private Light sunLight;
    [SerializeField] private Gradient sunColor;
    [SerializeField] private AnimationCurve sunIntensity;

    [Header("Skybox Settings")]
    [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;

    private float timeMultiplier;

    private void Start()
    {
        timeMultiplier = 1f / dayDurationInSeconds;
    }

    private void Update()
    {
        // Advance time
        timeOfDay += timeMultiplier * Time.deltaTime;
        if (timeOfDay >= 1f) timeOfDay = 0f;

        UpdateSunPosition();
        UpdateSunLighting();
        UpdateSkybox();
    }

    private void UpdateSunPosition()
    {
        // Rotate the sun 360 degrees over the full 0..1 day cycle
        float sunAngle = timeOfDay * 360f - 90f; // -90 offset places 0.25 at sunrise
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
    }

    private void UpdateSunLighting()
    {
        if (sunLight == null) return;

        // Evaluate color and intensity from curves/gradients based on normalized time (0..1)
        if (sunColor != null) sunLight.color = sunColor.Evaluate(timeOfDay);
        if (sunIntensity != null) sunLight.intensity = sunIntensity.Evaluate(timeOfDay);
    }

    private void UpdateSkybox()
    {
        if (daySkybox == null || nightSkybox == null) return;

        // Daytime is roughly between 0.2 (sunrise) and 0.75 (sunset)
        bool isDaytime = timeOfDay > 0.2f && timeOfDay < 0.75f;
        Material targetSkybox = isDaytime ? daySkybox : nightSkybox;

        if (RenderSettings.skybox != targetSkybox)
        {
            RenderSettings.skybox = targetSkybox;
            DynamicGI.UpdateEnvironment(); // Refreshes ambient lighting reflections
        }
    }
}
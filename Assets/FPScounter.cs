using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AdvancedFPSCounter : MonoBehaviour
{
    public float updateInterval = 1.0f; // Update interval in seconds

    private float deltaTime = 0.0f;
    private int frameCount = 0;
    private float totalTime = 0.0f;

    private float highestFPS = 0.0f;
    private float lowestFPS = float.MaxValue;
    private List<float> frameTimes = new List<float>();

    void Start()
    {
        // Start coroutine to calculate FPS
        StartCoroutine(UpdateFPS());
    }

    void Update()
    {
        deltaTime += Time.unscaledDeltaTime;
        frameCount++;
        totalTime += Time.unscaledDeltaTime;
        frameTimes.Add(Time.unscaledDeltaTime);

        float currentFPS = 1.0f / Time.unscaledDeltaTime;

        if (currentFPS > highestFPS)
        {
            highestFPS = currentFPS;
        }

        if (currentFPS < lowestFPS)
        {
            lowestFPS = currentFPS;
        }
    }

    private System.Collections.IEnumerator UpdateFPS()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);

            float avgFPS = frameCount / totalTime;

            // Calculate 1% low FPS
            frameTimes.Sort();
            int onePercentIndex = Mathf.CeilToInt(frameTimes.Count * 0.01f);
            float onePercentLowFPS = 1.0f / frameTimes.GetRange(0, onePercentIndex).Average();

            Debug.Log($"Highest FPS: {highestFPS}");
            Debug.Log($"Lowest FPS: {lowestFPS}");
            Debug.Log($"Average FPS: {avgFPS}");
            Debug.Log($"1% Low FPS: {onePercentLowFPS}");

            // Reset for next interval
            frameCount = 0;
            totalTime = 0.0f;
            frameTimes.Clear();
        }
    }
}
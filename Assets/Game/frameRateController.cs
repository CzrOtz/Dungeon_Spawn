using UnityEngine;

public class FrameRateController : MonoBehaviour
{
    public int targetFrameRate = 60; // Set your desired FPS here

    void Start()
    {
        // Set the desired target frame rate
        Application.targetFrameRate = targetFrameRate;
    }
}

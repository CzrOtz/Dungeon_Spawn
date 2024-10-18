using System.Collections;
using Cinemachine;
using UnityEngine;

public class vcScript : MonoBehaviour
{
    public CinemachineVirtualCamera vc;
    private testAgentScript boss;
    private bool cameraAdjusted = false;
    private float originalOrthoSize;
    private float targetOrthoSize = 9.5f;
    private float zoomDuration = 1.0f; // Duration of the zoom effect in seconds
    private bool isZooming = false;
    private bool bossDefeated = false;

    void Start()
    {
        if (vc == null)
        {
            vc = GetComponent<CinemachineVirtualCamera>();
        }
        originalOrthoSize = vc.m_Lens.OrthographicSize;
    }

    void Update()
    {
        if (bossDefeated)
        {
            // Boss has been defeated; no further action needed
            return;
        }

        if (!cameraAdjusted)
        {
            // Try to find the boss if we haven't already
            if (boss == null)
            {
                boss = FindObjectOfType<testAgentScript>();
            }
            else if (!isZooming)
            {
                // Boss has spawned, start zooming out
                StartCoroutine(ZoomCamera(vc.m_Lens.OrthographicSize, targetOrthoSize, zoomDuration));
                cameraAdjusted = true;
                isZooming = true;
            }
        }
        else
        {
            // Boss is dead or null, zoom back to original size
            if ((boss != null && boss.dead) || boss == null)
            {
                if (!isZooming)
                {
                    StartCoroutine(ZoomCamera(vc.m_Lens.OrthographicSize, originalOrthoSize, zoomDuration));
                    cameraAdjusted = false;
                    isZooming = true;
                    bossDefeated = true; // Boss defeated, no need to search again
                }
            }
        }
    }

    IEnumerator ZoomCamera(float fromSize, float toSize, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            vc.m_Lens.OrthographicSize = Mathf.Lerp(fromSize, toSize, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        vc.m_Lens.OrthographicSize = toSize;
        isZooming = false;
    }
}


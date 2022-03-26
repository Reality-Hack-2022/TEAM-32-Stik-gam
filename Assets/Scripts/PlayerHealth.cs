using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int PlayerID = -1;
    public bool Alive = false;
    public OVRPassthroughLayer passthroughLayer;
    public Color savedColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator killme() {
        yield return new WaitForSeconds(1.5f);
        Death();
    }

    public void Death() {
        StartCoroutine(RedFade());
        EventBus.Publish(new PlayerEvents.PlayerDeathEvent(PlayerID));

    }

    IEnumerator RedFade() {
        StartCoroutine(FadeToCurrentStyle(0.2f));
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FadeToDefaultPassthrough(0.8f));
    }

    IEnumerator FadeToCurrentStyle(float fadeTime)
    {
        float timer = 0.0f;
        float brightness = passthroughLayer.colorMapEditorBrightness;
        float contrast = passthroughLayer.colorMapEditorContrast;
        float posterize = passthroughLayer.colorMapEditorPosterize;
        Color edgeCol = new Color(savedColor.r, savedColor.g, savedColor.b, 0.0f);
        passthroughLayer.edgeRenderingEnabled = true;
        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;
            float normTimer = Mathf.Clamp01(timer / fadeTime);
            passthroughLayer.edgeColor = Color.Lerp(edgeCol, savedColor, normTimer);
            yield return null;
        }
    }

    IEnumerator FadeToDefaultPassthrough(float fadeTime)
    {
        float timer = 0.0f;
        float brightness = passthroughLayer.colorMapEditorBrightness;
        float contrast = passthroughLayer.colorMapEditorContrast;
        float posterize = passthroughLayer.colorMapEditorPosterize;
        Color edgeCol = passthroughLayer.edgeColor;
        while (timer <= fadeTime)
        {
            timer += Time.deltaTime;
            float normTimer = Mathf.Clamp01(timer / fadeTime);
            passthroughLayer.colorMapEditorBrightness = Mathf.Lerp(brightness, 0.0f, normTimer);
            passthroughLayer.colorMapEditorContrast = Mathf.Lerp(contrast, 0.0f, normTimer);
            passthroughLayer.colorMapEditorPosterize = Mathf.Lerp(posterize, 0.0f, normTimer);
            passthroughLayer.edgeColor = Color.Lerp(edgeCol, new Color(edgeCol.r, edgeCol.g, edgeCol.b, 0.0f), normTimer);
            if (timer > fadeTime)
            {
                passthroughLayer.edgeRenderingEnabled = false;
            }
            yield return null;
        }
    }
}

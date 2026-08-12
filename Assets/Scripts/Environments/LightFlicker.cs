using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private float intensity = 5;

    private Material mat;

    private void Awake()
    {
        if (meshRenderer != null)
        {
            mat = meshRenderer.materials[1];
            mat.EnableKeyword("_EMISSION");
        }


    }

    private void Start()
    {
        StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            FlickerLight(0);
            yield return new WaitForSeconds(0.05f);

            FlickerLight(0.7f);

            yield return new WaitForSeconds(0.03f);

            FlickerLight(0);

            yield return new WaitForSeconds(0.04f);

            FlickerLight(1.3f);

            yield return new WaitForSeconds(0.02f);

            FlickerLight(0);

            yield return new WaitForSeconds(Random.Range(2f, 8f));
        }
    }

    private void FlickerLight(float flickerVal)
    {
        if (meshRenderer != null)
        {
            mat.SetColor("_EmissionColor", Color.white * Mathf.Clamp(flickerVal, 1, 100));
        }

        lightSource.intensity = intensity - flickerVal;

    }
}
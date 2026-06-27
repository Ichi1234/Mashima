using System.Collections;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    [SerializeField] private Renderer meshRenderer;

    private Material mat;

    private void Awake()
    {
        mat = meshRenderer.materials[1];
        mat.EnableKeyword("_EMISSION");

    }

    private void Start()
    {
        StartCoroutine(FlickerRoutine());
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            lightSource.intensity = 1.0f;
            mat.SetColor("_EmissionColor", Color.white * 1f);
            yield return new WaitForSeconds(0.05f);

            lightSource.intensity = 0.7f;
            mat.SetColor("_EmissionColor", Color.white * 0.7f);
            yield return new WaitForSeconds(0.03f);

            lightSource.intensity = 1.0f;
            mat.SetColor("_EmissionColor", Color.white * 1f);
            yield return new WaitForSeconds(0.04f);

            lightSource.intensity = 0.3f;
            mat.SetColor("_EmissionColor", Color.white * 0.3f);
            yield return new WaitForSeconds(0.02f);

            lightSource.intensity = 1.0f;
            mat.SetColor("_EmissionColor", Color.white * 1f);

            yield return new WaitForSeconds(Random.Range(2f, 8f));
        }
    }
}
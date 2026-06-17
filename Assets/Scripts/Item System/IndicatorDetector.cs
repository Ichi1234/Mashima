using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private readonly List<Indicator> nearbyIndicators = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Indicator indicator))
        {

            nearbyIndicators.Add(indicator);

            indicator.RecivedPlayerData(transform.root);
            indicator.Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Indicator indicator))
        {
            nearbyIndicators.Remove(indicator);

            indicator.Hide();
        }
    }
}
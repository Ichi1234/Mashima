using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private readonly List<Indicator> nearbyIndicators = new();

    private void OnTriggerStay(Collider other)
    {
        if (GameManager.Instance.CurPlayerMode == PlayerMode.VR)
        {
            return;
        }

        Indicator indicator = other.GetComponentInParent<Indicator>();
        if (indicator != null)
        {

            nearbyIndicators.Add(indicator);

            indicator.RecivedPlayerData(transform.root);
            indicator.Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Indicator indicator = other.GetComponentInParent<Indicator>();

        if (indicator != null)
        {
            nearbyIndicators.Remove(indicator);

            indicator.Hide();
        }
    }
}
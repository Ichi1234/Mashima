using UnityEngine;

public class ERAlarm : MonoBehaviour
{
    [SerializeField] private Light alarmLightA;
    [SerializeField] private Light alarmLightB;
    [SerializeField] private float rotateSpeed = 2f;

    private void Update()
    {
        alarmLightA.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
        alarmLightB.transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}

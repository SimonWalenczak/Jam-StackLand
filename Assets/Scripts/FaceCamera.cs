using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera Camera;

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.transform.position);
    }
}

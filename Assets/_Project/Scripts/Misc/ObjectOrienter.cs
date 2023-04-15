using UnityEngine;

public class ObjectOrienter : MonoBehaviour
{
    Transform objectoToFollow;

    public void Init(Transform _objectToFollow)
    {
        objectoToFollow = _objectToFollow;
    }

    private void LateUpdate()
    {
        if(objectoToFollow != null)
        {
            transform.LookAt(transform.position + objectoToFollow.rotation * Vector3.forward, objectoToFollow.rotation * Vector3.up);
        }
    }
}

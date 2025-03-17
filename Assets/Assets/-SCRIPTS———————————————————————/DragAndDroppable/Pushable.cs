using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour, iPushable
{
    [SerializeField]GameObject pushedFX;
    [SerializeField][Range(0,1f)] float pushTimeOut = 0.1f;
    Rigidbody rb => GetComponent<Rigidbody>();
    bool disabled = false;

    public void Push(Vector3 origin, Vector3 force)
    {
        if (disabled) return;
        rb.AddForceAtPosition(force,origin,ForceMode.Impulse);
        if (pushedFX) Instantiate(pushedFX,transform.position,transform.rotation);
        
        if (pushTimeOut>0)
        {
            disabled = true;
            StartCoroutine(WaitTimeOut());
        }
    }

    IEnumerator WaitTimeOut()
    {
        float timer = pushTimeOut;
        while (timer>0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        disabled = false;
    }
    
} // SCRIPT END


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

public class PathFollower : MonoBehaviour {

    public Transform[] path ;
    public float speed = 1.0f;
    public float waitTimeInSeconds = 10;

    private float reachDist = 1.0F;
    private int currentPoint = 0;

    // Use this for initialization
    void Start () {
	}

    private bool wait = false;
    IEnumerator Coro()
    {
        Debug.Log("end");
        wait = true;
        yield return new WaitForSeconds(waitTimeInSeconds);
        transform.position = path[0].position;
        currentPoint = 0;
        wait = false;
        Debug.Log("start");
    }

	// Update is called once per frame
	void Update () {
        if(currentPoint<path.Length)
        {
            Vector3 vec = path[currentPoint].position - transform.position;
            Vector3 dir = vec.normalized;
            float dist = vec.magnitude;

            if (dist <= reachDist)
            {
                Debug.Log("меньше");
                currentPoint++;
            }

            transform.position += dir * Time.deltaTime * speed;
            transform.eulerAngles = new Vector3(0,Mathf.Acos(-dir.z)*Mathf.Sign(-dir.x)/Mathf.PI*180,0);
        }
        else if(!wait)
        {
            StartCoroutine(Coro());
        }
        
	}

    void OnDrawGizmos()
    {
        for (int i = 0; i < path.Length-1; i++)
        {
            if (path[i+1] != null)
            {
                Gizmos.DrawLine(path[i].position,path[i+1].position);
            }
        }
    }
}

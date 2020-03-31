using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NonInfected : MonoBehaviour
{
    public float speed;
    public float directionChangeInterval;
    public float maxHeadingChange;
    public Material red;
    public bool lockedOn;
    public bool canLock = true;
    public bool infected = false;

    NavMeshAgent ai;
    float heading;
    Vector3 targetRotation;

    private void Awake()
    {
        ai = GetComponent<NavMeshAgent>();
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        StartCoroutine(NewHeading());
    }

    private void Update()
    {
        if (infected)
        {
            Renderer rend = gameObject.GetComponent<Renderer>();
            rend.material = red;
            Detection();
          
            

        }
        else if (!infected)
        {
            Wander();
        }
    }
    public void Wander()
    {
        transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, targetRotation, Time.deltaTime * directionChangeInterval);
        var forward = transform.TransformDirection(Vector3.forward);
        ai.Move(forward * speed * Time.deltaTime);
    }
    

    public void Detection()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.yellow);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            if (canLock)
            {
                if (hit.transform.CompareTag("Enemy"))
                {

                    lockedOn = true;

                }
            }
            

            if (lockedOn)
            {
                ai.SetDestination(hit.transform.position);
                transform.LookAt(hit.transform.position);
            }
        }

        if (!lockedOn &&infected)
        {
           Wander();
        }
    }

    IEnumerator NewHeading()
    {
        while (true)
        {
            NewHeadingRoutine();
            yield return new WaitForSeconds(directionChangeInterval);
        }
    }

    void NewHeadingRoutine()
    {
        var floor = Mathf.Clamp(heading - maxHeadingChange, 0, 360);
        var ceil = Mathf.Clamp(heading + maxHeadingChange, 0, 360);
        heading = Random.Range(floor, ceil);
        targetRotation = new Vector3(0, heading, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (infected)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {

                other.gameObject.GetComponent<NonInfected>().infected = true;
                //Renderer rend = other.gameObject.GetComponent<Renderer>();
                //rend.material = red;
                lockedOn = false;
                //canLock = false;

            }
        }
        
    }
}

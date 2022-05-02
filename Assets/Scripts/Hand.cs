using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{

    public Vector3 holdPosition = new Vector3(0, -0.3f, 0.03f);
    public Vector3 holdRotation = new Vector3(0, 0, 0);

    private bool holdingSword = false;
    private GameObject sword = null;

    public OVRInput.Controller controller;

    private float indexTriggerState = 0;
    private float handTriggerState = 0;
    private float oldIndexTriggerState = 0;

    private bool shaking = false;

    // Update is called once per frame
    void Update()
    {
        oldIndexTriggerState = indexTriggerState;
        indexTriggerState = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller);
        handTriggerState = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller);
        if (holdingSword)
        {
 
            if (handTriggerState < 0.9f)
                Release();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            if (handTriggerState > 0.9f && !holdingSword)
            {
                Grab(other.gameObject);

            }
        }
    }

    void Grab(GameObject obj)
    {

        holdingSword = true;
        sword = obj;

        sword.transform.parent = transform;

        sword.transform.localPosition = holdPosition;
        sword.transform.localEulerAngles = holdRotation;
        sword.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        sword.GetComponent<Rigidbody>().useGravity = false;
        sword.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Release()
    {
        sword.transform.parent = null;

        Rigidbody rigidbody = sword.GetComponent<Rigidbody>();

        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;

        rigidbody.velocity = OVRInput.GetLocalControllerVelocity(controller);

        holdingSword = false;
        sword = null;
    }

}


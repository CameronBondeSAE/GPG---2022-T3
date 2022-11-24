using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using Lloyd;
using NodeCanvas.Tasks.Actions;
using Unity.VisualScripting;
using UnityEditor;

public enum LaserDoorType
{
    DetectPlayer,
    Interactable
};

public class DoorSingleModel : MonoBehaviour, IHeatSource
{
    public LaserDoorType myType;

    [SerializeField] private float fireDamage;

    private Flammable flammableComp;
    
    [SerializeField] private float timeMoving;
    
    [Header ("Set Line Stats")]
    [SerializeField] private float numLines;
    
    [SerializeField] private float lineLength;
    
    [SerializeField]  private float lineThickness;
    [SerializeField] private float intensity;
    
    [SerializeField] private float distanceBetween;

    [SerializeField] private float zDepth;

    private Vector3 origPos;
    private Vector3 currentPos;

    private Vector3 targetPos;

    private Vector3 downPos;
    private float downLength;

    private bool isOpen;

    private DoorSingleView doorView;

    [Header("Enemy Detection Box. Player Detection is *1.2")] [SerializeField]
    private Vector3 halfExtents;

    [SerializeField] private LayerMask playerLayer;

    private void OnEnable()
    {
        
        doorView = GetComponentInChildren<DoorSingleView>();
        doorView.SetColor(Color.red);

        origPos = transform.position;

        //previousPos;
        currentPos = origPos;

        targetPos = downPos;

        downLength = lineLength;

        downPos = new Vector3(transform.position.x, transform.position.y - downLength, transform.position.z);
        
        isOpen = false;
    }

    private void FixedUpdate()
    {
        TrackPos();

        if (myType == LaserDoorType.DetectPlayer)
        {
            ChangePos();

           DetectPlayer();
            if(!isOpen)
                DetectBurnVictim();
        }
        
        doorView.SetLineStats(numLines, lineLength, lineThickness, intensity, distanceBetween);
    }

    private void ChangePos()
    {
        StartCoroutine(LerpPosition());
    }

    IEnumerator LerpPosition()
    {
        if (isOpen)
        {
            targetPos = downPos;
            doorView.SetColor(Color.green);
        }

        else
        {
            targetPos = origPos;
            doorView.SetColor(Color.red);
        }

        float time = 0;
        while (time < timeMoving)
        {
            transform.position = Vector2.Lerp(currentPos, targetPos, time / (timeMoving));
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
    }
    
    public void Interact()
    {
        if (myType == LaserDoorType.Interactable)
        {
            isOpen = !isOpen;
            StartCoroutine(LerpPosition());
        }
    }

    private void DetectBurnVictim()
    {
        halfExtents = new Vector3(distanceBetween + numLines, lineLength, zDepth);
        Collider[] hitColl = Physics.OverlapBox(origPos, halfExtents, Quaternion.identity);
        foreach (var hitCollider in hitColl)
        {
            if (hitCollider.GetComponent<Flammable>() != null)
            {
                flammableComp = hitCollider.GetComponent<Flammable>();
                flammableComp.ChangeHeat(this, fireDamage);
            }
        }
    }

    private void DetectPlayer()
    {
        origPos = new Vector3(origPos.x*(numLines+distanceBetween), origPos.y, origPos.z);
        halfExtents = new Vector3(distanceBetween* + numLines*1.2f, lineLength*1.2f, zDepth*1.2f);
        Collider[] hitColl = Physics.OverlapBox(origPos, halfExtents, Quaternion.identity);
        foreach (var hitCollider in hitColl)
        {
            if (hitCollider.GetComponent<IControllable>() != null)
            isOpen = true;
            return;
        }

        isOpen = false;
    }

    private void TrackPos()
    {
        currentPos = transform.position;
        doorView.SetPosition(currentPos);
    }
}
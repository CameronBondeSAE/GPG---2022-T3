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

    private IHeatSource myself;

    private Flammable flammableComp;
    
    [SerializeField] private float timeMoving;
    
    [Header ("Set Line Stats")]
    [SerializeField] private float numLines;
    
    [SerializeField] private float lineLength;
    
    [SerializeField]  private float lineThickness;
    [SerializeField] private float intensity;
    
    [SerializeField] private float distanceBetween;

    private Vector3 origPos;
    private Vector3 currentPos;

    private Vector3 targetPos;

    private Vector3 downPos;
    private float downLength;

    private bool isOpen;

    private DoorSingleView doorView;

    [Header("Player Detection Sphere")]
    //detecting Player to Open/Close automatically
    [SerializeField]
    private float radius;

    [Header("Enemy Detection Box")] [SerializeField]
    private Vector3 halfExtents;

    [SerializeField] private LayerMask playerLayer;

    private void OnEnable()
    {
        myself = GetComponent<IHeatSource>();
        
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

           DetectPlayer(origPos, radius);
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
        halfExtents = new Vector3(distanceBetween + numLines, lineLength, 1);
        Collider[] hitColl = Physics.OverlapBox(origPos, halfExtents, Quaternion.identity);
        foreach (var hitCollider in hitColl)
        {
            if (hitCollider.GetComponent<Flammable>() != null)
            {
                flammableComp = hitCollider.GetComponent<Flammable>();
                flammableComp.ChangeHeat(fireDamage);
            }
        }
    }

    private void DetectPlayer(Vector3 center, float rad)
    {
        center = new Vector3(origPos.x*(numLines+distanceBetween), origPos.y, origPos.z);
        rad = radius;

        Collider[] hitColl = Physics.OverlapSphere(center, rad, playerLayer);
        foreach (var hitCollider in hitColl)
        {
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
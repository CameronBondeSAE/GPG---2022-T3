using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ManEaterView : MonoBehaviour
{
    public ManEater manEater;
    public GameObject bulb;
    public GameObject stalk;

    private Material[] body;
    private float curDirection = -1;

    // Start is called before the first frame update
    void Start()
    {
        body = new Material[2];
        body[0] = bulb.GetComponent<Renderer>().material;
        body[1] = stalk.GetComponent<Renderer>().material;
        
        Idle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Idle()
    {
        bulb.transform.DOLocalJump((Vector3.left * curDirection)/2f, -0.1f, 1, 1f, false);
        StartCoroutine(NextBounce());
    }

    IEnumerator NextBounce()
    {
        curDirection *= -1f;
        yield return new WaitForSeconds(1f);
        
        Idle();
    }
}

using Shapes;
using UnityEngine;

public class RaycastScannerTest : ImmediateModeShapeDrawer
{
    public float radarSpeed = 100f;
    private float timer;
    private Vector3 dir;
    
    RaycastHit hitInfo;

    public LayerMask pingLayer;
    
    private void Update()
    {
        timer += Time.deltaTime * radarSpeed;
        if (timer >= 360f)
        {
            timer = 0f;
        }
        
        dir = Quaternion.Euler(0, timer, 0) * transform.forward;
    
        Ray ray = new Ray(transform.position, dir);

        if (Physics.Raycast(ray,out hitInfo))
        {
            if (hitInfo.collider.gameObject.layer == pingLayer)
            {
                //StartCoroutine(hitInfo.collider.GetComponent<PingObject>().pinged());
                print("Pinged");
            }
        }
    }
}

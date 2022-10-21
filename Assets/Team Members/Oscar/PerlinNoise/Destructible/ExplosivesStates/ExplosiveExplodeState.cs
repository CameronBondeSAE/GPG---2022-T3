using Unity.Mathematics;
using UnityEngine;

namespace Oscar
{
    public class ExplosiveExplodeState : MonoBehaviour
    {
        public int amountOfFragments = 4;
        public float distance = 1f;
        public GameObject rotator;
        public Vector3 rotatorAxis;
        public GameObject explosivefragments;

        private float transformRotation;
        // Start is called before the first frame update
        void Start()
        {
            for (int fragment = 0; fragment < amountOfFragments; fragment++)
            {
                GameObject brokenFragments = Instantiate(explosivefragments,
                    this.transform.position + new Vector3(1,0,0), 
                    quaternion.RotateY(transformRotation)) as GameObject;
                
                brokenFragments.transform.parent = rotator.transform;
                
                transformRotation += 90f;
            }
            DestroyExplosiveModel();
            print("Exploded! Im Broken and Dead!");
        }
        void DestroyExplosiveModel()
        {
            Destroy(this.gameObject);
        }
    }
}

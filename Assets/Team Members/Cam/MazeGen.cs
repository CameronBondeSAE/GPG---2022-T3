using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
    public GameObject caveBrickPrefab;
    public GameObject itemPrefab;
    public int cubeSize = 1;
    public Vector2 size;
    public float scale = 0.01f;
    public float perlinThreshold = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < size.x; x=x+cubeSize)
        {
            Vector3 brickPosition = new Vector3();
            float perlinNoise = Mathf.PerlinNoise(x*scale,0);

            brickPosition.x = x;
            brickPosition.y = perlinNoise;
            brickPosition.z = 1f;

            if (perlinNoise<perlinThreshold)
            {
	            Instantiate(caveBrickPrefab, brickPosition, Quaternion.identity);
            }
            else
            {
	            // Only if in a gap
	            // Use Random.Range or Perlin again to determine density of items
	            Instantiate(itemPrefab, brickPosition, Quaternion.identity);
            }
	        // Scale my brick
	        // TODO
        }

        // GameObject wall = Instantiate(caveBrickPrefab, new Vector3(size.x,0,0), Quaternion.identity);
        // wall.transform.localScale = new Vector3(size.x, cubeSize, cubeSize);
    }

    public void PlaceItem()
    {
	    
    }

    public void PlaceWall()
    {
	    
    }
}

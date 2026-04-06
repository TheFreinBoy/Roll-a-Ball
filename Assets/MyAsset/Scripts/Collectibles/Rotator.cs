using UnityEngine;

public class Rotator : MonoBehaviour
{

public float XVector = 0f; 
public float YVector = 45f;
public float ZVector = 0;
 // Update is called once per frame
 void Update()
    {
 // Rotate the object on X and  Y axes by specified amounts, adjusted for frame rate.
        transform.Rotate (new Vector3 (XVector, YVector, ZVector) * Time.deltaTime);
    }
 
}
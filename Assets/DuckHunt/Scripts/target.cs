using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class target : MonoBehaviour {

    public Camera targetCamera;                         // Camera used to calculate the angular size

    [SerializeField]
    private float angularSize = 1.0f;                    // Constant angular size.
    
    // Note that the scale of the box collider compared to the circular target is exactly 0.785.
    // This is by design, as it is the ratio of the area of a box compared to a circle with the same
    // diameter as the length of the box. Keeping the circle at some unit 1, means that a box with
    // the area of that circle is 0.785 the size. This allows me to use a more performant box 
    // collider with the same effective area as the visually presented circle, and have it not
    // interfer with the results of the study. More information can be found at the link below:
    // http://teachingmath.info/pie.htm

    // Use this for initialization
    void Start () {
        // Initialize
        targetCamera = Camera.main;

        // Adjust scale for distance
        float distance = Vector3.Distance( this.transform.position, targetCamera.transform.position );
        float scale = adjustScale( distance );
        this.transform.localScale = Vector3.one * scale;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.LookAt( targetCamera.transform );
	}

    void setAngularSize(float value) {
        this.angularSize = value;
    }

    // Parameter distance from the player to achieve the correct angular size.
    float adjustScale(float distance) {
        float s = 0f;                               // Scale
        float a = Mathf.Deg2Rad * angularSize;  // Angular size (in radians)
        float d = distance;                         // Distance
        s = Mathf.Tan( a / 2 ) * 2 * d;             // Calculate
        return s;
    }

    // Parameter scale to achieve the correct angular size.
    float adjustDistance(float scale) {
        float s = scale;                            // Scale
        float a = Mathf.Deg2Rad * angularSize;  // Angular size (in radians)
        float d = 0f;                               // Distance
        float den = Mathf.Tan( a / 2 ) * 2;         // Calculate denominator of the equation
        d = s / den;                                // Calculate the remainder of the equation
        return s;
    }
    
}

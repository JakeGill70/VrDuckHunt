using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrDuckHunt.FileManagement;


public class gameManager : MonoBehaviour {

    // Manage the camera modes
    public enum CameraMode { PC, VR };
    public CameraMode cameraMode = CameraMode.PC;
    [SerializeField]
    private GameObject[] cameraRigs;
    private Camera camera;

    // Manage the files
    private MyFileManager fileManager;
    private bool allFilesWrittenTo = false;

    [SerializeField]
    private Transform currentTarget;
    int carrot;

	// Use this for initialization
	void Start () {

        activateRig();
        
        TargetData[] targetData = generateRandomTargets( 10 );

        fileManager = new MyFileManager( targetData, true );

        Debug.Log( Application.dataPath );
	}

    void activateRig() {
        // The Unity object tag that identifies the rigs.
        string rigTag = "";

        // Set the rigTag to the match the appropriate setting
        if (cameraMode == CameraMode.PC)
        {
            rigTag = "PcRig";
        }
        else if (cameraMode == CameraMode.VR)
        {
            rigTag = "VrRig";
        }

        // Search through the various rig objects until the appropriate one is activated
        foreach (GameObject go in cameraRigs)
        {
            if (go.tag == rigTag)
            {
                go.SetActive( true );
            }
        }

        // Set the local reference to the camera to the first
        // camera instance that is active and tagged as "MainCamera"
        camera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
        if (!allFilesWrittenTo)
        {
            DataLog data = new DataLog( getDistanceFromGaze(), camera.transform.position, camera.transform.rotation );
            fileManager.recordData( data );
        }
        
        if (gazeCheck() && !allFilesWrittenTo)
        {
            transform.position = transform.position + Vector3.up * 5;
            allFilesWrittenTo = !fileManager.nextRecordingData();
            Debug.Log( "HIT!!!" + Time.realtimeSinceStartup.ToString() );
        }
	}

    private bool gazeCheck() {
        
        // Extend the ray to be twice the distance from the partcipant's camera from the current target
        float distanceToTravel = Vector3.Distance( camera.transform.position, currentTarget.position ) * 2;

        // Create the ray
        Ray ray = getCenteredCameraRay();   // Create the "real" ray
        RaycastHit hit;         // Place to save the hit information

        // Cast the ray
        bool hitSomething = Physics.Raycast( ray, out hit, distanceToTravel );

        // True if something with the tag "Target" was hit
        return (hitSomething && hit.collider.transform.tag == "Target") ;
    }

    private Ray getCenteredCameraRay() {
        return camera.ViewportPointToRay( new Vector2( 0.5f, 0.5f ) );
    }

    private float getDistanceFromGaze () {
        Ray ray = getCenteredCameraRay();
        Debug.DrawRay( ray.origin, ray.direction );
        float distanceToTarget = Vector3.Distance( camera.transform.position, currentTarget.position );
        Vector3 gazePoint = ray.GetPoint( distanceToTarget );
        float distanceToGaze = Vector3.Distance( gazePoint, currentTarget.position );
        return distanceToGaze;
    }

    private void OnDestroy()
    {
        close();
    }

    private void close() {
        fileManager.stopRecordingData();
    }

    private TargetData[] generateRandomTargets(int size) {
        // Because the points are generated in pairs, if the size is odd, 
        // increase it by one so that the size will be even.
        if (size % 2 != 0)
        {
            size++;
        }

        // Create the targets
        TargetData[] data = new TargetData[size];
        for (int i = 0; i < size; i+=2)
        {
            // Create the taget data
            float angularSize = Random.value * 1.5f + 0.5f;
            angularSize = angularSize * 5;
            float distance1 = Random.Range( 3, 50 );
            float distance2 = Random.Range( 3, 50 );
            string fileName1 = "Target_" + i.ToString() + ".dat";
            string fileName2 = "Target_" + (i+1).ToString() + ".dat";

            // If the targets are within 3 units of one another, 
            // Recalculate one of the distances until they are
            // Farther apart
            while (Mathf.Abs( distance1 - distance2 ) < 3f)
            {
                distance2 = Random.Range( 3, 50 );
            }

            // Move the data into the target structs
            TargetData targetData1 = new TargetData( angularSize, distance1, fileName1 );
            TargetData targetData2 = new TargetData( angularSize, distance2, fileName2 );

            // Save the structs to the output
            data[i] = targetData1;
            data[i+1] = targetData2;
        }

        return data;
    }
    
}

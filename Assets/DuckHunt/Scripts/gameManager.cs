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
    private Camera targetCamera;

    // Manage the files
    private MyFileManager fileManager;
    private bool allFilesWrittenTo = false;

    // Manage Targets
    private GameObject currentTarget;
    [SerializeField]
    private GameObject targetPrefab;
    private TargetData[] targetData;
    private int currentTargetIndex = 0;

    public static gameManager Instance;

    

    private void createSingleton() {
        if (gameManager.Instance == null)
        {
            gameManager.Instance = this;
        }
        else
        {
            if (gameManager.Instance != this)
            {
                Destroy( this );
            }
        }
    }

    public Camera getCamera() {
        return this.targetCamera;
    }

	// Use this for initialization
	void Start () {

        createSingleton();  // Create a static reference to this object

        activateRig();  // Adjust any settings at runtime

        targetData = generateRandomTargets(10);     // Generates randomized target data

        fileManager = new MyFileManager( targetData, true );    // Create a file manager using this target data

        createTarget();

        // Debug info
        Debug.Log( Application.dataPath );
        Debug.Log( "Start Finished" );
	}

    /// <summary>
    /// Adjusts settings for unity gameobjects at runtime. 
    /// Activates objects and cameras.
    /// </summary>
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
                targetCamera = go.GetComponentInChildren<Camera>();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        /*
        if (!allFilesWrittenTo)
        {
            DataLog data = new DataLog( getDistanceFromGaze(), targetCamera.transform.position, targetCamera.transform.rotation );
            fileManager.recordData( data );
        }
        
        if ((gazeCheck() && !allFilesWrittenTo))
        {

            allFilesWrittenTo = !fileManager.nextRecordingData();
            Debug.Log( "HIT!!!" + Time.realtimeSinceStartup.ToString() );
            if (!allFilesWrittenTo)
            {
                createTarget();
            }
        }
        */

        //if (currentTargetIndex < targetData.Length)
        //{
        //    DataLog data = new DataLog( getDistanceFromGaze(), targetCamera.transform.position, targetCamera.transform.rotation );
        //    fileManager.recordData( data );
        //    if (gazeCheck())
        //    {
        //        fileManager.stopRecordingData();
        //        fileManager.increaseTargetIndex();
        //    }
        //}

        

        if (currentTarget != null)
        {
            DataLog data = new DataLog( getDistanceFromGaze(), targetCamera.transform.position, targetCamera.transform.rotation );
            fileManager.recordData( data );

            if (gazeCheck())
            {
                destroyTarget();
                fileManager.stopRecordingData();
            }
        }

        if (currentTarget == null && currentTargetIndex < targetData.Length - 1)
        {
            fileManager.increaseTargetIndex();
            fileManager.startRecordingData();
            currentTargetIndex++;
            createTarget();
        }
    }

    private void createTarget()
    {
        try
        {
            // Get the information from the targetData
            float distance = targetData[currentTargetIndex].distance;
            float angularSize = targetData[currentTargetIndex].angularSize;

            // Psuedo-randomize spawn position
            Quaternion randAng = Quaternion.Euler( Random.Range( 0, 10 ), Random.Range( -20, 20 ), 0 ); // Randomize angle
            Vector3 spawnPosition = (Vector3.up * 5) + (randAng * Vector3.forward * distance);

            // Destroy the old target if it exists
            destroyTarget();

            // Create the new target, update it's angular size, and set it to be the currentTarget
            currentTarget = GameObject.Instantiate( targetPrefab, spawnPosition, Quaternion.identity ) as GameObject;
            target currentTargetComponent = currentTarget.GetComponent<target>();
            currentTargetComponent.Initialize( angularSize );

            // currentTargetIndex++; // Increase the target index for the next cycle.
            Debug.Log( "Target Created." );
        }
        catch (System.IndexOutOfRangeException)
        {
            // Do Nothing
            Debug.LogWarning( "Index Out of Range" );
        }
    }

    private void destroyTarget()
    {
        if (currentTarget != null)
        {
            GameObject.Destroy( currentTarget );
        }
    }

    /// <summary>
    /// Gets the distance from the camera to the current target
    /// </summary>
    /// <returns></returns>
    private float getDistanceToTarget() {
        return Vector3.Distance( targetCamera.transform.position, currentTarget.transform.position );
    }

    /// <summary>
    /// Creates a ray cast from the camera's (read as: user's) perspective.
    /// </summary>
    /// <returns></returns>
    private Ray getCenteredCameraRay()
    {
        return targetCamera.ViewportPointToRay( new Vector2( 0.5f, 0.5f ) );
    }

    /// <summary>
    /// Performs a ray cast from the camera's (read as: user's) perspective.
    /// </summary>
    /// <returns>True if the raycast hits an object tagged "Target"</returns>
    private bool gazeCheck() {
        
        // Extend the ray to be twice the distance from the partcipant's camera from the current target
        float distanceToTravel = getDistanceToTarget() * 2;

        // Create the ray
        Ray ray = getCenteredCameraRay();   // Create the "real" ray
        RaycastHit hit;         // Place to save the hit information

        // Cast the ray
        bool hitSomething = Physics.Raycast( ray, out hit, distanceToTravel );

        // True if something with the tag "Target" was hit
        return (hitSomething && hit.collider.transform.tag == "Target") ;
    }

    /// <summary>
    /// Calculate the distance from the camera to where the player is looking at.
    /// The point that the player is looking at is assumed to be at the same distance
    /// away as the target.
    /// </summary>
    /// <returns>The distance from where the player is looking at to where the player is looking.</returns>
    private float getDistanceFromGaze () {
        // Create the ray cast
        Ray ray = getCenteredCameraRay();
        Debug.DrawRay( ray.origin, ray.direction, Color.red );

        // Get the point representing where the player is looking at.
        float distanceToTarget = getDistanceToTarget();
        Vector3 gazePoint = ray.GetPoint( distanceToTarget );

        // Calculate the distance from the camera to where the player is looking at
        float distanceToGaze = Vector3.Distance( gazePoint, currentTarget.transform.position );
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

        float minDistanceOfSeparation = 7.5f;         // Minimum Distance of Seperation between the two pairs of targets.
        float minDistance = 3.0f;   // Minimum distance to the target from the camera.
        float maxDistance = 50.0f;  // Maximum distance to the target from the camera.
        float minAngularSize = 2.5f;    // Minimum angular size
        float maxAngularSize = 10.0f;   // Maximum angular size


        size -= 1;          // Becausing counting in arrays begins at 0, adjust the count.
        // Because the points are generated in pairs, if the size is now odd, 
        // increase it by one so that the size will be even.
        // Size + 1 is used because counting begins at 0 inside of the array
        if (size % 2 != 0)
        {
            size++;
        }
        

        // Create the targets
        TargetData[] data = new TargetData[size];
        for (int i = 0; i < size; i+=2)
        {
            Debug.Log( string.Format("Target {0} and {1} created.", i, i + 1) );
            // Create the taget data
            float angularSize = Random.Range( minAngularSize, maxAngularSize );
            float distance1 = Random.Range( minDistance, maxDistance );
            float distance2 = Random.Range( minDistance, maxDistance );
            string fileName1 = "Target_" + i.ToString() + ".dat";
            string fileName2 = "Target_" + (i+1).ToString() + ".dat";

            // If the targets are within 3 units of one another, 
            // Recalculate one of the distances until they are
            // Farther apart
            while (Mathf.Abs( distance1 - distance2 ) < minDistanceOfSeparation)
            {
                distance2 = Random.Range( minDistance, maxDistance );
            }

            // Move the data into the target structs
            TargetData targetData1 = new TargetData( angularSize, distance1, fileName1 );
            TargetData targetData2 = new TargetData( angularSize, distance2, fileName2 );

            // Save the structs to the output
            data[i] = targetData1;
            data[i+1] = targetData2;
        }

        // Randomize the order so the pairs are not placed directly next to one another
        ArrayRandomizer.Randomize( data );
        return data;
    }
    
}

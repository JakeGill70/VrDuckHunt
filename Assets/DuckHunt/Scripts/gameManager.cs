using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrDuckHunt.FileManagement;

public class gameManager : MonoBehaviour {

    public int records = 0;
    MyFileManager fileManager;

	// Use this for initialization
	void Start () {

        
        TargetData[] targetData = generateRandomTargets( 10 );

        fileManager = new MyFileManager( targetData, true );

        Debug.Log( Application.dataPath );
	}
	
	// Update is called once per frame
	void Update () {
        DataLog data = new DataLog( 50, transform.position, transform.rotation );
        fileManager.recordData( data );
        if (Input.GetMouseButtonUp( 0 ))
        {
            transform.position = transform.position + Vector3.up * 5;
            fileManager.nextRecordingData();
        }
	}

    private void OnDestroy()
    {
        close();
    }

    private void close() {
        fileManager.stopRecordingData();
    }

    private TargetData[] generateRandomTargets(int size) {
        TargetData[] data = new TargetData[size];
        for (int i = 0; i < size; i++)
        {
            TargetData td = new TargetData( Random.value * 1.5f + 0.5f, Random.Range( 10, 150 ), "Target_" + i.ToString() + ".dat" );
            data[i] = td;
        }

        return data;
    }
    
}

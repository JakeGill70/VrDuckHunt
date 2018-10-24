using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VrDuckHunt.FileManagement;

public class gameManager : MonoBehaviour {

    public int records = 0;

	// Use this for initialization
	void Start () {
        
        string fileName = "Session case #" + Utils.generateTag(5);
        TargetData[] targetData = generateRandomTargets( 10 );

        Debug.Log( "XML Write Started" );

        WriteToXmlFile.createXmlFile( fileName );
        WriteToXmlFile.addTargetDataToXML( targetData );
        WriteToXmlFile.closeFile();

        Debug.Log( "XML Write Complete" );

        Debug.Log( Application.dataPath );
        
        WriteToBinaryFile.beginWriteToBinaryFile( targetData[0].fileTag );
	}
	
	// Update is called once per frame
	void Update () {
        if (records < 50)
        {
            records++;
            DataLog data = new DataLog( 50, transform.position, transform.rotation );
            WriteToBinaryFile.writeToBinaryFile( data );
        }
	}

    private void OnDestroy()
    {
        close();
    }

    private void close() {
        WriteToBinaryFile.endWriteToBinaryFile();

    }

    private TargetData[] generateRandomTargets(int size) {
        TargetData[] data = new TargetData[size];
        for (int i = 0; i < size; i++)
        {
            TargetData td = new TargetData( Random.value * 1.5f + 0.5f, Random.Range( 10, 150 ), "Target_" + i.ToString() + ".dat" );
            WriteToBinaryFile.createBinaryFile( td.fileTag );
            data[i] = td;
        }

        return data;
    }
    
}

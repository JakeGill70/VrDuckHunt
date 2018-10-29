using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace VrDuckHunt.FileManagement.Binary
{
    static class WriteToBinaryFile
    {
        static BinaryWriter bwStream;
        static bool isCreatingFile = false;

        static public void createBinaryFile(string fileName) {
            UnityEngine.Debug.Log( fileName + " created." );
            float sizeMb = 1.00f;
            float MegaByteToByteConversion = 1000000;
            int bufferSize = (int)(sizeMb * MegaByteToByteConversion);

            File.Create( Utils.getDataPath() + fileName, bufferSize );
        }

        static public void beginWriteToBinaryFile( string fileName ) {
            isCreatingFile = true;
            string fileNameWithPath = Utils.getDataPath() + fileName;
            Debug.Log( "About to open file: " + fileNameWithPath );
            bwStream = new BinaryWriter( new FileStream( fileNameWithPath, FileMode.Open ) );
        }

        static public void endWriteToBinaryFile() {
            bwStream.Close();
            isCreatingFile = false;
        }

        static public void writeToBinaryFile(DataLog data) {
            if (bwStream == null)
            {
                Debug.LogError( "No binary Writer Stream!!" );
            }
            bwStream.Write( data.distance );
            writeVector3( data.position );
            writeQuaternion( data.rotation );
        }

        static private void writeVector3( UnityEngine.Vector3 v ) {
            bwStream.Write( v.x );
            bwStream.Write( v.y );
            bwStream.Write( v.z );
        }

        static private void writeQuaternion( UnityEngine.Quaternion q )
        {
            bwStream.Write( q.x );
            bwStream.Write( q.y );
            bwStream.Write( q.z );
            bwStream.Write( q.w );
        }
    }
}

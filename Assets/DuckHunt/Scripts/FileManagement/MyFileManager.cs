using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using VrDuckHunt.FileManagement.Binary;
using VrDuckHunt.FileManagement.Xml;

namespace VrDuckHunt.FileManagement
{
    class MyFileManager
    {
        private string sessionName;
        private string sessionDirectory;
        private TargetData[] targetData;
        private int targetIndex = 0;
        private bool isRecording = false;

        private string sessionLocalDirectory { get { return sessionName + "/"; } }

        public MyFileManager(TargetData[] td, bool useColdStart) {
            startSession( td, useColdStart );
        }

        /// <summary>
        /// Creates a directory for the session
        /// </summary>
        private void prepDirectory()
        {
            sessionDirectory = Utils.getDataPath() + sessionName;
            System.IO.Directory.CreateDirectory( sessionDirectory );
        }

        /// <summary>
        /// Creates all necessary binary files.
        /// </summary>
        private void prepBinary()
        {
            for (int i = 0; i < targetData.Length; i++)
            {
                WriteToBinaryFile.createBinaryFile( sessionLocalDirectory + targetData[i].fileTag );
            }
        }

        /// <summary>
        /// Creates and populates the XML file
        /// </summary>
        private void prepXML()
        {
            WriteToXmlFile.createXmlFile( sessionLocalDirectory + sessionName );
            WriteToXmlFile.addTargetDataToXML( targetData );
            WriteToXmlFile.closeFile();
        }

        /// <summary>
        /// Creates the necessary files for data collection.
        /// </summary>
        /// <param name="td">The target data used to generate the files.</param>
        /// <param name="useColdStart">Yes/No, immediately begin recording data?</param>
        public void startSession( TargetData[] td, bool useColdStart )
        {
            sessionName = "Session case #" + Utils.generateTag( 5 );
            targetData = td;
            targetIndex = 0;

            prepDirectory();    // Make a directory to hold session data
            prepXML();          // Create and populate XML with the target data
            prepBinary();       // Creates a binary file foreach target data

            if (useColdStart)
            {
                startRecordingData();
            }
        }

        public void startRecordingData() {
            WriteToBinaryFile.beginWriteToBinaryFile( sessionLocalDirectory + targetData[targetIndex].fileTag );
            isRecording = true;
        }

        public void recordData(DataLog d) {
            WriteToBinaryFile.writeToBinaryFile( d );
        }

        public void stopRecordingData() {
            WriteToBinaryFile.endWriteToBinaryFile();
            isRecording = false;
        }

        /// <summary>
        /// Automatically stops recording data about the current target, 
        /// and begins recording data about the next target.
        /// </summary>
        /// <returns>Returns false when there are no more records</returns>
        public bool nextRecordingData() {
            stopRecordingData();
            targetIndex++;
            if (targetIndex <= targetData.Length)
            {
                startRecordingData();
                return true;
            }
            return false;
        }

        public int increaseTargetIndex() {
            targetIndex++;
            return targetIndex;
        }

        public TargetData? getCurrentTarget() {
            try
            {
                return targetData[targetIndex];
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        public TargetData? getNextTarget() {
            try
            {
                return targetData[targetIndex + 1];
            }
            catch (IndexOutOfRangeException) {
                return null;
            }
        }

        public string getSessionName() {
            return sessionName;
        }

        public void compressSessionFiles() {
            GZipStream gzOut = new GZipStream( File.Open( this.sessionDirectory + ".zip", FileMode.OpenOrCreate ), CompressionMode.Compress );
            DirectoryInfo selectedDirectory = new DirectoryInfo( this.sessionDirectory );
            StreamWriter sw = new StreamWriter( gzOut );

            foreach (FileInfo file in selectedDirectory.GetFiles())
            {
                FileStream sr = file.OpenRead();
                float val = (float)sr.ReadByte();
                while (val != -1f)
                {
                    sw.Write( val );
                    val = (float)sr.ReadByte();
                }
                sr.Close();
            }
            gzOut.Close();
        }
    }
}

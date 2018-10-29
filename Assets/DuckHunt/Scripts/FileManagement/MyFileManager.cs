using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrDuckHunt.FileManagement.Binary;
using VrDuckHunt.FileManagement.Xml;

namespace VrDuckHunt.FileManagement
{
    class MyFileManager
    {
        private string sessionName;
        private TargetData[] targetData;
        private int targetIndex = 0;
        private bool isRecording = false;

        public MyFileManager(TargetData[] td, bool useColdStart) {
            startSession( td, useColdStart );
        }

        public MyFileManager() {
            sessionName = "Session case #" + Utils.generateTag( 5 );
            isRecording = false;
            targetIndex = 0;
            targetData = null;
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

            prepXML();
            prepBinary();

            if (useColdStart)
            {
                startRecordingData();
            }
        }

        /// <summary>
        /// Creates all necessary binary files.
        /// </summary>
        private void prepBinary()
        {
            for (int i = 0; i < targetData.Length; i++)
            {
                WriteToBinaryFile.createBinaryFile( targetData[i].fileTag );
            }
        }

        /// <summary>
        /// Creates and populates the XML file
        /// </summary>
        private void prepXML() {
            WriteToXmlFile.createXmlFile( sessionName );
            WriteToXmlFile.addTargetDataToXML( targetData );
            WriteToXmlFile.closeFile();
        }

        public void startRecordingData() {
            WriteToBinaryFile.beginWriteToBinaryFile( targetData[targetIndex].fileTag );
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
    }
}

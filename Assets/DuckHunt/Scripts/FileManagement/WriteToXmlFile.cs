using System;
using System.Xml;
using UnityEngine;


namespace VrDuckHunt.FileManagement.Xml
{
    public class WriteToXmlFile
    {
        static private bool isCreatingFile = false;
        static private XmlWriter xmlWriter;

        public static void createXmlFile( string fileName )
        {
            if (isCreatingFile)
            {
                throw new Exception( "XML File is currently open" );
            }
            string fileNameWithExtension = Utils.getDataPath() + fileName + ".xml";
            isCreatingFile = true;
            xmlWriter = XmlWriter.Create( fileNameWithExtension );
            xmlWriter.WriteStartDocument();
            writeSessionDataToXML();
        }

        public static void closeFile()
        {
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            isCreatingFile = false;
        }

        public static void addTargetDataToXML( TargetData[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                addTargetDataToXML( data[i] );
            }
        }

        public static void addTargetDataToXML( float angularSize, float distance, string fileTag )
        {
            TargetData data = new TargetData( angularSize, distance, fileTag );
            addTargetDataToXML( data );
        }

        public static void addTargetDataToXML( TargetData data )
        {
            if (!isCreatingFile)
            {
                throw new Exception( "Currently no XML File is open" );
            }
            xmlWriter.WriteStartElement( "target" );
            writeElementToXML( "angularSize", data.angularSize.ToString() );
            writeElementToXML( "distance", data.distance.ToString() );
            writeElementToXML( "fileTag", data.fileTag );
            xmlWriter.WriteEndElement();
        }

        private static void writeSessionDataToXML()
        {
            xmlWriter.WriteStartElement( "session" );
            xmlWriter.WriteAttributeString( "date", DateTime.Now.ToString( "g" ) );
        }

        private static void writeElementToXML( string elementName, string value )
        {
            xmlWriter.WriteStartElement( elementName );
            xmlWriter.WriteString( value );
            xmlWriter.WriteEndElement();
        }
    }
}

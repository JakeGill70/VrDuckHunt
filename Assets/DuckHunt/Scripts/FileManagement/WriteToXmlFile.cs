using System;
using System.Xml;
using UnityEngine;


namespace VrDuckHunt.FileManagement
{
    public class WriteToXmlFile
    {

        [Serializable]
        class FileName
        {
            private DateTime date;
            [SerializeField]
            private string name;
            private static System.Random rand;
            private System.Random random
            {
                get
                {
                    if (rand == null)
                    {
                        rand = new System.Random();
                    }
                    return rand;
                }
            }

            public FileName()
            {
                int tagSize = 10;
                date = DateTime.Now;

                //name = date.ToString( "g" );
                name = " - Run:";
                name += generateTag( tagSize );
            }

            public FileName( int tagSize )
            {
                date = DateTime.Now;

                //name = date.ToString( "g" );
                name = " - Run:";
                name += generateTag( tagSize );
            }

            private string generateTag( int size )
            {
                string tag = "";
                for (var i = 0; i < size; i++)
                {
                    tag += random.Next( 0x0, 0xF ).ToString( "x" );
                }
                return tag;
            }

            public string Name { get { return name; } }
        }

        static private bool isCreatingFile = false;
        static private XmlWriter xmlWriter;

        public static void createXmlFile( string fileName )
        {
            string fileNameWithExtension = fileName + ".xml";
            isCreatingFile = true;
            xmlWriter = XmlWriter.Create( fileNameWithExtension );
            xmlWriter.WriteStartDocument();
            writeSessionDataToXML();
        }

        public static void closeFile()
        {
            isCreatingFile = false;
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
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

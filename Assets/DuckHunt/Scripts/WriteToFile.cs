using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteToFile : MonoBehaviour {

    [Serializable]
    class FileName {
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

        public FileName() {
            int tagSize = 10;
            date = DateTime.Now;

            //name = date.ToString( "g" );
            name = " - Run:";
            name += generateTag( tagSize );
        }

        public FileName(int tagSize)
        {
            date = DateTime.Now;

            //name = date.ToString( "g" );
            name = " - Run:";
            name += generateTag( tagSize );
        }

        private string generateTag(int size) {
            string tag = "";
            for (var i = 0; i < size; i++)
            {
                tag += random.Next( 0x0, 0xF ).ToString( "x" );
            }
            return tag;
        }

        public string Name { get{ return name; } }
    }

    [SerializeField]
    private FileName fileName;

    private DateTime date = DateTime.Now;

	// Use this for initialization
	void Start () {
        fileName = new FileName();
	}
	
	// Update is called once per frame
	void Update () {

	}
}

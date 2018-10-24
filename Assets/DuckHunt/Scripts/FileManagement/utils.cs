using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VrDuckHunt.FileManagement
{
    static class Utils
    {
        static private Random random;
        static private Random Random {
            get {
                if (random == null) {
                    random = new Random();
                }
                return random;
            }
        }

        public static string generateTag( int size )
        {
            string tag = "";
            for (var i = 0; i < size; i++)
            {
                tag += Random.Next( 0x0, 0xF ).ToString( "x" );
            }
            return tag;
        }

        public static string getDataPath() {
            return UnityEngine.Application.dataPath + "/Sessions/";
        }
    }
}

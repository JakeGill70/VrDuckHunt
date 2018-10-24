using UnityEngine;

namespace VrDuckHunt.FileManagement
{
    struct DataLog
    {
        public float distance;
        public Vector3 position;
        public Quaternion rotation;

        public DataLog( float d, Vector3 pos, Quaternion rot ) {
            distance = d;
            position = pos;
            rotation = rot;
        }
    }
}

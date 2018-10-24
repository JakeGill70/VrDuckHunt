namespace VrDuckHunt.FileManagement
{
    public struct TargetData
    {
        public float angularSize;
        public float distance;
        public string fileTag;

        public TargetData( float a, float d, string t )
        {
            angularSize = a;
            distance = d;
            fileTag = t;
        }
    }
}

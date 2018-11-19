namespace VrDuckHunt.FileManagement
{
    /// <summary>
    /// Meta information about targets, generated at the start of a simulation.
    /// </summary>
    [System.Serializable]
    public struct TargetData
    {
        public float angularSize;
        public float distance;
        public string fileTag;

        /// <summary>
        /// Create meta data for a target.
        /// </summary>
        /// <param name="a">Angular Size</param>
        /// <param name="d">Distance</param>
        /// <param name="t">Associaited File Tag</param>
        public TargetData( float a, float d, string t )
        {
            angularSize = a;
            distance = d;
            fileTag = t;
        }

        /// <summary>
        /// Create meta data for a target.
        /// </summary>
        /// <param name="a">Angular Size</param>
        /// <param name="d">Distance</param>
        /// <param name="t">Associaited File Tag</param>
        public TargetData( float a, float d )
        {
            angularSize = a;
            distance = d;
            fileTag = "target_" + Utils.generateTag(5);
        }
    }
}

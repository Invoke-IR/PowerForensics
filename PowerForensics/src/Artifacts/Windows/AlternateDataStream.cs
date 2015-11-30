namespace PowerForensics.Artifacts
{
    #region AlternateDataStreamClass

    public class AlternateDataStream
    {
        #region Properties

        public readonly string FullName;
        public readonly string Name;
        public readonly string StreamName;

        #endregion Properties

        #region Constructors

        internal AlternateDataStream(string fullName, string name, string streamName)
        {
            FullName = fullName;
            Name = name;
            StreamName = streamName;
        }

        #endregion Constructors
    }

    #endregion AlternateDataStreamClass
}

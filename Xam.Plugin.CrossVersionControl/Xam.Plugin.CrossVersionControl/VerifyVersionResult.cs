namespace Xam.Plugin.CrossVersionControl
{
    /// <summary>
    /// Result of Version Verification on stores
    /// </summary>
    public class VerifyVersionResult
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public VerifyVersionResult()
        {
            AppName = "Not Found on Store";
            StoreCurrentVersion = "0.0.0";
            VersioningType = VersioningType.NotIdentified;
        }

        /// <summary>
        /// Store application public name
        /// </summary>
        public string AppName {get; set;}

        /// <summary>
        /// Store published current version
        /// </summary>
        public string StoreCurrentVersion {get; set;}

        /// <summary>
        /// Which version naming is used for control
        /// </summary>
        public VersioningType VersioningType {get; set;}
    }

    /// <summary>
    /// Enumerator for versioning naming type
    /// </summary>
    public enum VersioningType
    {
        /// <summary>
        /// Used incremental number for updates
        /// </summary>
        ByIncrementalNumber,
        /// <summary>
        /// Used names for version updates as Lollipop
        /// </summary>
        ByCodeName,

        /// <summary>
        /// Occours when not found on store
        /// </summary>
        NotIdentified
    }
}

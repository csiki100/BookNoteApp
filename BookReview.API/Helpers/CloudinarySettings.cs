namespace DatingApp.API.Helpers
{
    ///<summary>
    ///Class that contains the data needed to configure Cloudinary
    ///</summary>
    public class CloudinarySettings
    {
        ///<summary>
        ///Cloudinary account's Cloud Name
        ///</summary>
        public string CloudName { get; set; }

        ///<summary>
        ///Cloudinary account's API Key
        ///</summary>
        public string ApiKey { get; set; }

        ///<summary>
        ///Cloudinary account's API Secret
        ///</summary>
        public string ApiSecret { get; set; }
    }
}
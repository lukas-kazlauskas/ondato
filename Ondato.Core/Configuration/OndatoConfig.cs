namespace Ondato.Core.Configuration
{
    public class OndatoConfig
    {
        public static string Section => "DictionaryConfig";
        public KeyValueStoreType KeyValueStoreType { get; set; }
        public int MaxExpireInSeconds { get; set; }
        public int CleanUpInSeconds{ get; set; }
    }
}
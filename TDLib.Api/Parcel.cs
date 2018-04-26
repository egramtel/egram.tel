using Newtonsoft.Json;

namespace TD
{
    public abstract class Parcel
    {
        [JsonProperty("@type")]
        public virtual string DataType { get; set; }
        
        [JsonProperty("@extra")]
        public virtual string Extra { get; set; }
    }
}
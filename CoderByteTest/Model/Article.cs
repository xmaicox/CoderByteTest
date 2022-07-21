using CoderByteTest.ModelHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CoderByteTest.Model
{
    [DataContract]
    public class Article
    {

        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(Title))
                    return Title;

                if (!string.IsNullOrEmpty(StoryTitle))
                    return StoryTitle;

                else return null;
            }
        }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Name = "title")]
        public string Title { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Name = "url")]
        public string Url { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Name = "author")]
        public string Author { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Name = "num_comments")]
        public int? NumComments { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Name = "story_id")]
        public string StoryId { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Name = "story_title")]
        public string StoryTitle { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Name = "story_url")]
        public string StoryUrl { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Name = "parent_id")]
        public string ParentId { get; set; }


        [JsonConverter(typeof(MicrosecondEpochConverter))]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
    }
}

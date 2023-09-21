using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace BasicAPI.Models
{
    public class PersonDto
    {
        //DTO for PullRequests
        //Returns all properties, a full Name, and a Description
        //Json requests only show Name and Description
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public string FirstName { get; set; }

        [JsonIgnore]
        public string LastName { get; set; }

        public string Name { get { return $"{FirstName} {LastName}"; } }

        [JsonIgnore]
        public string Profession { get; set; }

        [JsonIgnore]
        public int Age { get; set; }

        public string Description { get { return $"{Profession} at the age of {Age}"; } }

    }
}

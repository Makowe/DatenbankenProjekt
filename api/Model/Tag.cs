using System;

namespace api {
    /// <summary>
    /// Model class that represents a single component. 
    /// </summary>
    public class Tag {
        
        public Tag(int id, string name) {
            this.Id = id;
            this.Name = name;
        }
        public Tag() { }

        public int? Id { get; set; }
        public string Name { get; set; }
    }
}

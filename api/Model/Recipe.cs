using System;
using System.Collections.Generic;

namespace api {
    /// <summary>
    /// model class that represents a single recipe
    /// </summary>
    public class Recipe {
        public Recipe() { }
        public Recipe(int id, string name, int people) {
            this.Id = id;
            this.Name = name;
            this.People = people;
        }
        public string Name { get; set; }
        public int? Id { get; set; }
        public int People { get; set; }
        public List<Component> Components { get; set; }
        public List<Instruction> Instructions { get; set; }
        public List<Tag> Tags { get; set; }

    }
}

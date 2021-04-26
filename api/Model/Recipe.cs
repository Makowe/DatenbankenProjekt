using System;
using System.Collections.Generic;

namespace api {
    public class Recipe {
        public Recipe() { }
        public Recipe(int id, string name, int people) {
            this.Id = id;
            this.Name = name;
            this.People = people;
        }
        public object Name { get; set; }
        public object Id { get; set; }
        public object People { get; set; }
        public List<Component> Components { get; set; }
        public List<Instruction> Instructions { get; set; }

    }
}

using System;

namespace api {
    /// <summary>
    /// Model class that represents an instruction of a recipe
    /// </summary>
    public class Instruction {
        public Instruction(int step, string description) {
            this.Step = step;
            this.Description = description;
        }
        public int Step { get; set; }
        public string Description { get; set; }
    }
}

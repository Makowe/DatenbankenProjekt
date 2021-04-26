using System;

namespace api {
    public class Instruction {
        public Instruction(int step, string description) {
            this.Step = step;
            this.Description = description;
        }
        public int Step { get; set; }
        public string Description { get; set; }
    }
}

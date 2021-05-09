using System;

namespace api {
    public class Component {
        public Component(int id, string name, double amount, string unitName, string unitShortname) {
            this.Id = id;
            this.Name = name;
            this.Amount = amount;
            this.UnitShortname = unitShortname;
            this.UnitName = unitName;
        }

        public Component(int id, string name) {
            this.Id = id;
            this.Name = name;
        }
        public Component() { }

        public int? Id { get; set; }
        public string Name { get; set; }
        public double? Amount { get; set; }
        public string UnitShortname { get; set; }
        public string UnitName { get; set; }
    }
}

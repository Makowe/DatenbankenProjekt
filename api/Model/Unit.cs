using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model {
    public class Unit {
        public Unit() {
            this.Name = "";
            this.Shortname = "";
        }
        public Unit(int? id, string unitName, string unitShortname) {
            this.Name = unitName;
            this.Shortname = unitShortname;
            this.Id = id;
        }
        public string Name { get; set; }
        public string Shortname { get; set; }
        public int? Id { get; set; }
    }
}

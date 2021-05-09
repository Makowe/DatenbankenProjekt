using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model {
    public class Unit {
        public Unit() {
            this.UnitName = "";
            this.UnitShortname = "";
        }
        public Unit(int? id, string unitName, string unitShortname) {
            this.UnitName = unitName;
            this.UnitShortname = unitShortname;
            this.Id = id;
        }
        public string UnitName { get; set; }
        public string UnitShortname { get; set; }
        public int? Id { get; set; }
    }
}

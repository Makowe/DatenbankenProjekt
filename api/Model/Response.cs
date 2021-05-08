namespace api.Model {
    public class Response {
        public Response() {
            this.Value = 0;
            this.Message = "";
        }

        public Response(int value, string message) {
            this.Value = value;
            this.Message = message;
        } 

        public Response(int? value, string message) {
            this.Value = (int)value;
            this.Message = message;
        } 


        public int Value {get;set; }
        public string Message {get;set;}

    }
}

namespace api.Model {
    /// <summary>
    /// Class that specifies Response of a function. Class is used for better communication between methods
    /// </summary>
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

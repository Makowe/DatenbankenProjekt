namespace api.Model {
    /// <summary>
    /// Class that specifies Response of a function. Class is used for better communication between methods
    /// </summary>
    public class CustomResponse {
        public CustomResponse() {
            this.Value = 0;
            this.Message = "";
        }

        public CustomResponse(int value, string message) {
            this.Value = value;
            this.Message = message;
        } 

        public int Value { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// returns a reponse message containing a default error message.
        /// </summary>
        /// <returns></returns>
        public static CustomResponse ErrorMessage() {
            return new CustomResponse(0, "Die Anweisung konnte nicht ausgeführt werden. Möglicherweise liegt ein Problem bei der Datenbank vor.");
        }

        /// <summary>
        /// returns a response message containing an empty success message
        /// </summary>
        /// <returns></returns>
        public static CustomResponse SuccessMessage() {
            return new CustomResponse(1, "");
        }
    }
}

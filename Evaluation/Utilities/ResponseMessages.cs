namespace Evaluation.Utilities
{
    public static class ResponseMessages
    {
        public static string Successful { get; set; } = "Successfully Completed!";
        public static string Added { get; set; } = "Successfully Added!";
        public static string Modified { get; set; } = "Successfully Modified!";
        public static string Deleted { get; set; } = "Successfully Deleted!";
        public static string NotFound { get; set; } = "Record NotFound!";
        public static string ExceptionMessage { get; set; } = "Unknown Error!";
        public static string DbExceptionMessage { get; set; } = "ErrorUpdatingDb!";
        public static string InvalidDates { get; set; } = "Dates Are Invalid!";
        public static string InvalidData { get; set; } = "Data is Invalid";
    }
}

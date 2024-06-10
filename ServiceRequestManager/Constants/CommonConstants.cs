namespace ServiceRequestManager.Constants
{
    public enum CurrentStatus
    {
        NotApplicable,
        Created,
        InProgress,
        Complete,
        Canceled
    }
    public static class ResponseStatus
    {
        public const string Successful = "Successful";
        public const string UnSuccessful = "UnSuccessful";
    }
}

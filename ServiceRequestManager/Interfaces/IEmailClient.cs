namespace ServiceRequestManager.Interfaces
{
	public interface IEmailClient
	{
		public Task SendEmail(string Message);
    }
}

namespace XUnitIntroduction.Application
{
  public class EmailSender : IEmailSender
  {
    public void SendEmail(string message)
    {
      Console.WriteLine($"Email sent with message: {message}");
    }
  }
}

namespace library.common
{
    public interface IEmailService
    {
        void SendWelcomeEmail(string toEmail, string name, string town);
        void SendForgotPasswordReminder(string toEmail, string name, string guid);
        void SendForgotPasswordConfirmation(string toEmail, string name);
    }
}

namespace library.common
{
    public interface IEmail
    {
        void SendWelcomeEmail(string toEmail, string name, string town);
        void SendForgotPasswordReminder(string toEmail, string name, string link);
        void SendForgotPasswordConfirmation(string toEmail, string name);
    }
}

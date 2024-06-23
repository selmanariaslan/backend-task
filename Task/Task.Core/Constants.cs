namespace Task.Core
{
    public static class Constants
    {
        public static class ErrorMessageTypes
        {
            public const string Info = "Info";
            public const string Warning = "Warning";
            public const string Error = "Error";

            public const string BusinessError = "BusinessError";
            public const string ExceptionLogId = "ExceptionLogId";
            public const string UserMessage = "UserMessage";
        }

        public static class DefaultUserMessagesTR
        {
            public const string Success = "İşleminiz başarıyla gerçekleşti.";
            public const string Error = "İşleminiz gerçekleştirilmeye çalışılırken bir hata alındı.";
            public const string Warning = "İşleminiz gerçşekleştirilemedi, Lütfen birazdan tekrar deneyin.";
        }
        public static class DefaultUserMessagesDE
        {
            public const string Success = "Ihre Transaktion war erfolgreich.";
            public const string Error = "Beim Versuch, Ihre Transaktion zu bearbeiten, ist ein Fehler aufgetreten.";
            public const string Warning = "Ihre Transaktion konnte nicht abgeschlossen werden. Bitte versuchen Sie es in Kürze erneut.";
        }
        public static class DefaultUserMessagesEn
        {
            public const string Success = "Your transaction was successful.";
            public const string Error = "An error was received while trying to process your transaction.";
            public const string Warning = "Your transaction could not be processed, please try again in a moment.";
        }

        public static class DbStatus
        {
            public const int Deleted = -1;
            public const int Passive = 0;
            public const int Active = 1;
        }
    }
}
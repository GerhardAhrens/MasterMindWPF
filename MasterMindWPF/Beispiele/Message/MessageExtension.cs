//-----------------------------------------------------------------------
// <copyright file="MainWindow.cs" company="Lifeprojects.de">
//     Class: MainWindow
//     Copyright © Lifeprojects.de 2026
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>05.03.2026 18:21:36</date>
//
// <summary>
// WPF Template mit Minimalfunktionen
// </summary>
//-----------------------------------------------------------------------

namespace System.Windows
{

    public static class MessageExtension
    {
        public static MessageBoxResult Hinweis(this IMessageBase self, string titel, string message, bool withSound = false)
        {
            MessageBoxResult result = self.ShowMessage(titel, message, withSound);
            return result;
        }

        public static MessageBoxResult Question(this IMessageBase self, string titel, string message)
        {
            MessageBoxResult result = self.ShowMessage(titel, message, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            return result;
        }

        public static MessageBoxResult EndTheGameYN(this IMessageBase self)
        {
            string msgBoxTitle = LocalizationValue.Get("PlayerBreakTitel_DE");
            string msgBoxMessage = LocalizationValue.Get("PlayerBreak_DE");

            MessageBoxResult result = self.ShowMessage(msgBoxTitle, msgBoxMessage, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            return result;
        }

        public static MessageBoxResult PlayerWinGame(this IMessageBase self, int versuche)
        {
            string msgBoxTitle = LocalizationValue.Get("PlayerWinTitel_DE");
            string msgBoxMessage = LocalizationValue.Get("PlayerWin_DE", versuche);

            MessageBoxResult result = self.ShowMessage(msgBoxTitle, msgBoxMessage, MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            return result;
        }

        public static MessageBoxResult PlayerNotWinGame(this IMessageBase self, int versuche)
        {
            string msgBoxTitle = LocalizationValue.Get("PlayerNotWinTitel_DE");
            string msgBoxMessage = LocalizationValue.Get("PlayerNotWin_DE", versuche);

            MessageBoxResult result = self.ShowMessage(msgBoxTitle, msgBoxMessage,MessageBoxButton.OK,MessageBoxImage.Information,MessageBoxResult.OK);
            return result;
        }

        public static MessageBoxResult AppExitMessage(this IMessageBase self, string args = null)
        {
            MessageBoxResult result;

            string msgBoxTitle = LocalizationValue.Get("MessageExit_Titel_DE");
            if (args != null)
            {
                string msgBoxDescription = LocalizationValue.Get("MessageExit_Text_DE", args);
                result = self.ShowMessage(msgBoxTitle, msgBoxDescription, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            }
            else
            {
                string msgBoxDescription = LocalizationValue.Get("MessageExit_Text_DE");
                result = self.ShowMessage(msgBoxTitle, msgBoxDescription, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            }

            return result;
        }
    }
}
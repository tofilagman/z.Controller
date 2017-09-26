using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace z.Controller.Mail
{
    public class SendGrid
    {
        private string ApiKey;
        static bool IsEnabled = false;
        private SendGridProp Prop;

        public SendGrid(string apikey)
        {
            this.ApiKey = apikey;
        }

        public SendGridProp SetProp
        {
            set
            {
                this.Prop = value;
            }
        }

        public async void SendMessage(string Subject, string Email, string Body)
        {

            var msg = new SendGridMessage();
            msg.SetFrom(new EmailAddress(Prop.Sender, Prop.DisplayName));

            var rep = new List<EmailAddress>
            {
                new EmailAddress(Email),
            };

            msg.AddTos(rep);

            msg.SetSubject(Subject);

            msg.AddContent(MimeType.Html, Body);

            var client = new SendGridClient(ApiKey);

            await client.SendEmailAsync(msg);
        }

        public class SendGridProp
        {
            public string Sender;
            public string DisplayName;
            public string CarbonCopy;
        }
    }
}

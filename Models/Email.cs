namespace Locum_Backend.Models
{
    public class Email
    {
        public string firstName { get; set; }

        public string HtmlMail(string narration, string applink, string salutation,
                            string emailfromsystem, string narration1, string logourl)

        {
            string htmlBody = "<table><tr><td colspan='4' style='font-family: Raleway; font-size: 12pt;color: darkblue;'>" +
                               "<div style='max-width: 600px;margin: 0 auto;padding: 20px;'>" +
                               "<div style='text-align: center;margin-bottom: 20px;'>" +
                               "<img src='" + logourl + "' style='width: 150px;'>" +
                               "</div>" +
                               "<h1 style='color: darkblue;text-align: center;font-size: 24px;margin-top: 0;'>" + narration + "</h1>" +
                               "<div style='padding: 20px;background-color: #f8f9fa;border-radius: 5px;'>" +
                               "<p style='color: #333333;margin-bottom: 10px;'>" + salutation + "</p>" +
                               "<p style='color: #333333;margin-bottom: 10px;'>" + emailfromsystem + "</p>" +
                               "<p>" + narration1 + "</p>" +
                                "</div>" + "<div style='margin: 20px;'>" + "</div>" +
                               "<div>" + " <a href='" + applink + @"' style='display: inline-block; margin-top: 20px; padding: 10px 20px; background-color: #007bff; font-size: medium; color: #ffffff; text-decoration: none; border-radius: 10px; padding: 5px;'>Visit Evercare Locum Payment System</a>" + "</div>" +
                               "</div>" +
                               "</body>";
            return htmlBody;

        }


    }

}
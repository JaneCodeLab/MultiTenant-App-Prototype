
namespace Infrastructure;

public static class Template
{
    public static string GetEmailTemplate()
    {
        return @"<!DOCTYPE html>
                        <html>
                        <head>
                            <meta charset='utf-8'>
                            <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                            <meta name='description' content=''>
                            <meta name='viewport' content='width=device-width, initial-scale=1'>
                            <link rel='stylesheet' href=''>
                        </head>
                        <body style='margin:0;padding:0;background-color: #ffffff;'>
                            <table style='width:600px;' cellpadding='0' cellspacing='0' align='center'>
                                <tr>
                                    <td style='background-color: #ffffff;padding:20px 0'>
                                        <table style='width:600px;' cellpadding='0' cellspacing='0' border='0' align='center'>
                                            <tr>
                                                <td style='border-top:5px solid #404040; width:600px; margin:0 auto;  background-color: #F3F3F7; padding: 55px 65px 70px 65px'>
                                                    <table style='' cellpadding='0' cellspacing='0' border='0'>
                                                        <tr>
                                                            <td style='text-align: right;'>
                                                                <table style='width:99px' align='right' cellpadding='0' cellspacing='0' border='0'>
                                                                    <tr>
                                                                        <td style='width:33px;text-align: right;'>
                                                                            <a style='text-decoration: none;' href='https://www.facebook.com/' target='_blank'>
                                                                                <img src='/Images/20201202/facebook.png' alt=''>
                                                                            </a>
                                                                        </td>
                                                                        <td style='width:33px;text-align: right;'>
                                                                            <a style='text-decoration: none;' href='https://twitter.com/' target='_blank'>
                                                                                <img src='/Images/20201202/twitter.png' style='margin-left: 16px;' alt=''>
                                                                            </a>
                                                                        </td>
                                                                        <td style='width:33px;text-align: right;'>
                                                                            <a style='text-decoration: none;' href='https://www.instagram.com/' target='_blank'>
                                                                                <img src='/Images/20201202/instagram.png' style='margin-left: 16px;' alt=''>
                                                                            </a>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style='font-size:13px; font-weight:400;font-family:'Open Sans',sans-serif; color:#707070; line-height: 23px;'>
                                                                <br>
                                                                {0}<br><br>
                                                             </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan='2'
                                                                style='font-size:13px; font-weight:400;font-family:'Open Sans',sans-serif; color:#707070; line-height: 23px;'>
                                                                {1} <br><br><br>
                                                                {2} <br>
                                                                {3}
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </body>
                        </html>";
    }
}
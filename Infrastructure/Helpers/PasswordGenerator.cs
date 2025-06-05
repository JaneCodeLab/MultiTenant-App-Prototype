
namespace Infrastructure.Helpers
{
    public static class PasswordGenerator
    {
        public static string GetNewPassword()
        {
            string pass = string.Empty;
            Random r = new Random();

            for (int i = 0; i < 7; i++)
                pass += GetRandomChar(r);

            pass += GetRandomChar(r, CharType.number);
            pass += GetRandomChar(r, CharType.UpperCase);
            pass += GetRandomChar(r, CharType.LowerCase);
            pass += GetRandomChar(r, CharType.Symbol);

            for (int i = 0; i < 7; i++)
                pass += GetRandomChar(r);


            return pass;
        }

        public static char GetRandomChar(Random random, CharType charType = CharType.All)
        {
            char[] chars;

            switch (charType)
            {
                case CharType.All:
                default:
                    chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?ABCDEFGHIJKLMNOPQRSTUVWXYZ&".ToCharArray();
                    break;
                case CharType.UpperCase:
                    chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                    break;
                case CharType.LowerCase:
                    chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
                    break;
                case CharType.number:
                    chars = "1234567890".ToCharArray();
                    break;
                case CharType.Symbol:
                    chars = "$%#@!*&".ToCharArray();
                    break;
            }
            int i = random.Next(chars.Length);
            return chars[i];
        }

        public enum CharType
        {
            All,
            UpperCase,
            LowerCase,
            number,
            Symbol
        }
    }
}

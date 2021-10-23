using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WarcraftGuildTests.DataGenerator
{

    public static class RandomDataGenerator
    {
        private static RandomNumberGenerator Random { get; set; }

        static RandomDataGenerator()
        {
            Random = RandomNumberGenerator.Create();
        }

        private static ulong GenerateUlong
        {
            get
            {
                byte[] buffer = new byte[sizeof(ulong)];

                Random.GetBytes(buffer);
                return BitConverter.ToUInt64(buffer, 0);
            }
        }

        public static string RandomString(char[] chars, int minLength, int maxLength)
        {
            StringBuilder builder = new StringBuilder();
            int length = ((int)GenerateUlong % (maxLength - minLength)) + minLength;
            for (int i = 0; i < length; i++)
            {
                builder.Append(chars[(int)(GenerateUlong % (ulong)chars.Length)]);
            }
            return builder.ToString();
        }

        public static string RandomName(int length)
        {
            string name = RandomString(CharList.LowerLetters, 0, length);
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return $"{char.ToUpper(name[0])}{name[1..]}";
        }

        public static TEnum RandomEnum<TEnum>() where TEnum : Enum
        {
            Array values = Enum.GetValues(typeof(TEnum));
            return (TEnum)values.GetValue((int)(GenerateUlong % (ulong)values.Length));
        }

        public static bool RandomBool()
        {
            return GenerateUlong % 2 == 0;
        }

        public static ulong RandomUlong(ulong max = ulong.MaxValue)
        {
            return GenerateUlong % max;
        }

        public static uint RandomUint(uint max = uint.MaxValue)
        {
            return (uint) GenerateUlong % max;
        }
    }
}

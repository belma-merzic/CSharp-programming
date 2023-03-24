using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FIT_Api_Examples.Helper
{
    public static class Ekstenzije
    {
        public static string RemoveTags(this string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }

    public static byte[] ParseToBytes(this string base64string)
    {
      base64string = base64string.Split(',')[1];   //zato sto nam se prilikom funkcije Snimi() kao slika_korisnika_nova_base64 proslijeđuje --> "data:image/jpeg;base64, pa tek onda Base64 te slike, tkd trebamo izbaciti sve to to Base64 koda
      return System.Convert.FromBase64String(base64string);
    }
  }
}

using System.IO;

namespace FIT_Api_Examples.Helper
{
  public class Fajlovi
  {
    public static byte[]? Ucitaj(string path)
    {
      try
      {
        return System.IO.File.ReadAllBytes(path);
      }
      catch (System.Exception e)
      {
        return null;
      }
    }

    internal static void Snimi(byte[] slika_bajtovi, string path)
    {
      var directoryName = Path.GetDirectoryName(path);
      if (directoryName != null)
        Directory.CreateDirectory(directoryName);

      using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
      fs.Write(slika_bajtovi, 0, slika_bajtovi.Length);
      fs.Close();
    }
  }
}

namespace Karin;

public class DebugHelper
{
  public static void Print(Dictionary<string, int> dictionary)
  {
    Console.WriteLine("{");
    foreach(KeyValuePair<string, int> pair in dictionary)
    {
      Console.WriteLine("  {0}: {1}", pair.Key, pair.Value);
    }
    Console.WriteLine("}");
  }

  public static void Log(string message)
  {
    Console.WriteLine(message);
  }
}

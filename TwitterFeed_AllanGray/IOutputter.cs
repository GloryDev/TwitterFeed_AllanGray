namespace TwitterFeed_AllanGray {
  public interface IOutputter 
  {
    void WriteLine(string line);
  }

  public class ConsoleOutputter : IOutputter {
    public void WriteLine(string line) {
      System.Console.WriteLine(line);
    }
  }
}
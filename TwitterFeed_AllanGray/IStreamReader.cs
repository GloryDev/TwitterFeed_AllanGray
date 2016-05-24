namespace LillithClasses {
  public interface IStreamReader {
    bool EndOfStream { get; }
    void Close();
    string ReadLine();
  }
}
using System.IO;

namespace LillithClasses {
  public class StreamReaderWrapper : IStreamReader {
    private readonly StreamReader _streamReader;

    public StreamReaderWrapper(StreamReader streamReader) {
      this._streamReader = streamReader;
    }

    public bool EndOfStream => _streamReader.EndOfStream;

    public void Close() {
      _streamReader.Close();
    }

    public string ReadLine() {
      return _streamReader.ReadLine();
    }
  }
}
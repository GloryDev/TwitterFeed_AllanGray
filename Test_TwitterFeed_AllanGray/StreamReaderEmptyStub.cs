using System.Collections.Generic;
using LillithClasses;

namespace TestLilithClasses {
  public class StreamReaderEmptyStub : IStreamReader {
    public bool EndOfStream => true;

    public void Close() {
    }

    public string ReadLine() {
      return "";
    }
  }
  //Note: I could use a mocking library to do this and if the standard in the organisation was to 
  //  use a mocking library then I would. However for devs who are not used to the syntax of mocking libraries
  //  it can be hard to understand so I have rolled my own instead.
  public class StreamReaderFake : IStreamReader {
    private readonly IList<string> _itemsList;
    private int _linePointer = 0;

    public StreamReaderFake(IList<string> itemsList) {
      _itemsList = itemsList;
    }

    public bool EndOfStream => _linePointer == _itemsList.Count;

    public void Close() {
    }

    public string ReadLine() {
      return _itemsList[_linePointer++];
    }
  }
}
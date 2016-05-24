using System;
using System.Collections.Generic;
using System.IO;

namespace TwitterFeed_AllanGray {
  public interface IUserFileLoader
  {
    List<UserConfig> LoadUsersConfig();

    List<Exception> Exceptions { get; }
  }
  public class UserFileLoader: IUserFileLoader {
    private readonly string _userFileName;
    public List<Exception> Exceptions { get; } = new List<Exception>();

    public UserFileLoader(string userFileName) {
      _userFileName = userFileName;
    }

    public List<UserConfig> LoadUsersConfig() {
      var usersConfigList = new List<UserConfig>();
      var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
      try
      {
        var fileInfo = new FileInfo($@"{baseDirectory}\" + _userFileName);
        using (var filestream = new FileStream(fileInfo.FullName, FileMode.Open))
        {
          var fileStream = new StreamReader(filestream);
          //Note:- I could put this under test by creating a IStreamReader and a StreamReaderWrapper
          // and a StreamReaderFake but it seems like a lot of effort to test the ReadLine
          // If I was to worry about performance later where the performance of reading a long file putting it into
          //  a list and then processing the list was a problem then I would refactor this code
          //  to insert a test double for the streamReader where this method would then 
          // return an IStreamReader instead of a list. In the interests of making it work. then make it pretty and 
          // then make it perform I will leave it for now.
          
          //Even if this where not a test app I would put a similar note here so that a less experienced dev
          // or someone who does not know the code as well would understand this design decision and my 
          // thinking with it. 
          while (!fileStream.EndOfStream)
          {
            try {
              var userConfig = fileStream.ReadLine();
              usersConfigList.Add(new UserConfig(userConfig));
            }
            catch (Exception e) {
              Exceptions.Add(e);
            }
          }
        }
      }
      catch (Exception e)
      {
        Exceptions.Add(e);
      }

      return usersConfigList;
    }
  }
}
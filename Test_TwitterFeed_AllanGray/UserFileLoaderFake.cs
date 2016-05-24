using System;
using System.Collections.Generic;
using TwitterFeed_AllanGray;

namespace Test_TwitterFeed_AllanGray {


  public class UserFileLoaderFake : IUserFileLoader {
    private readonly List<UserConfig> _userConfigList = new List<UserConfig>();

    public UserFileLoaderFake(IList<string> userConfigList ) {
      foreach (var userConfig in userConfigList) {
        _userConfigList.Add(new UserConfig(userConfig));
      }
    }

    public List<UserConfig> LoadUsersConfig() {
      return _userConfigList;
    }

    public List<Exception> Exceptions { get; } = new List<Exception>();
  }
}
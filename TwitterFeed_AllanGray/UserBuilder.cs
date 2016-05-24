using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace TwitterFeed_AllanGray {
  public class UserBuilder {
    private readonly IUserFileLoader _userFileLoader;
    private readonly List<Exception> _exceptions = new List<Exception>();


    public UserBuilder(IUserFileLoader userFileLoader) {
      _userFileLoader = userFileLoader;
    }

    public List<User> BuildUsers() {
      var usersConfigList = _userFileLoader.LoadUsersConfig();
      _exceptions.AddRange(_userFileLoader.Exceptions);
      //input in format <<username>> folows <<username>>, <<username>>, <<username>> .......
      // one user can follow zero, one or many other users
      //Note: This code could be made more efficient by not traversing the User Config list twice.
      //  The algorithm would however become more complex and I would wait for performance testing
      //   before changing it.

      //Note: If more work was being done on this I would also consider creating a userConfig object that would be
      // be used to process the string into instead of using the generic list. that is generated from the split.
      // this userConfig would then allow this algorithm to be cleaned up.
      // var usersList = usersConfigList.Select(userConfig => new User(userConfig.UserName)).ToList();
      List<User> users = new List<User>();
      foreach (var userConfig in usersConfigList) {
        //var userConfig = usersConfigList.Find(config => config.UserName == user.UserName);
        var foundUser = users.Find(user => user.Name == userConfig.UserName);
        if (foundUser == null) {
          foundUser = new User(userConfig.UserName);
          users.Add(foundUser);
        }
        foreach (var follow in userConfig.Follows) {
          var followedUser = users.Find(user1 => user1.Name == follow);
          if (followedUser == null) {
            followedUser = new User(follow);
            users.Add(followedUser);
          }
          foundUser.AddFollows(followedUser);
        }
      }
      return users;
    }

    private static DataException CreateException(User user, string follow) {
      return new DataException("User : '" + user.Name + "' is configured to follow '" + follow +
                               "' who was not loaded in the config file");
    }

    public IList<Exception> GetErrors() {
      return _exceptions;
    }
  }
}
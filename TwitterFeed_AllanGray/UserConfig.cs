using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TwitterFeed_AllanGray {
  public class UserConfig
  {
    public UserConfig(string userConfigLine)
    {
      if (String.IsNullOrWhiteSpace(userConfigLine))
        throw new ArgumentException("Argument is null or whitespace", nameof(userConfigLine));
      this.Follows = new List<string>();

      var usersAndFollows = userConfigLine.Split(new[] { "follows" }, StringSplitOptions.RemoveEmptyEntries);
      this.UserName = GetUserName(usersAndFollows);

      if (HasFollowers(usersAndFollows)) {
        var followsSection = GetFollows(usersAndFollows);

        var follows = followsSection.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var follow in follows) {
          this.Follows.Add(follow.Trim());
        }
      }
    }

    public string UserName { get; private set; }
    public IList<string> Follows { get; }

    private static string GetUserName(IEnumerable<string> usersAndFollowers)
    {
      return usersAndFollowers.First().Trim();
    }

    private static string GetFollows(string[] usersAndFollowers)
    {
      return usersAndFollowers[1];
    }

    private static bool HasFollowers(string[] usersAndFollowers)
    {
      return usersAndFollowers.Length > 1;
    }
  }
}
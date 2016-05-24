using System;
using System.Collections.Generic;
using System.Data;

namespace TwitterFeed_AllanGray {
  public class TweetBuilder {
    private readonly TweetFileLoader _tweetFileLoader;
    private readonly List<User> _users;

    public TweetBuilder(TweetFileLoader tweetFileLoader, List<User> users) {
      _tweetFileLoader = tweetFileLoader;
      _users = users;
    }

    public IList<Tweet> BuildTweets() {
      var exceptions = new List<Exception>();
      var tweets = _tweetFileLoader.LoadTweets();
      var buildTweets = new List<Tweet>();
      if (tweets.Count <= 0) return buildTweets;

      foreach (var tweetLine in tweets) {
        var tweetContent = tweetLine.TweetContent;
        var tweetUserName = tweetLine.TweeterName;
        var foundUser = _users.Find(user => user.Name == tweetUserName);


        if (foundUser == null) {
          exceptions.Add(GetUserNotFoundException(tweetUserName, tweetLine));
          continue;
        }

        var tweet = new Tweet(foundUser, tweetContent);
        foreach (var user in foundUser.FollowedBy) {
          user.Tweets.Add(tweet);
        }
        foundUser.Tweets.Add(tweet);
        buildTweets.Add(tweet);
      }

      if(exceptions.Count > 0) throw new AggregateException(exceptions);
      return buildTweets;
    }

    private static DataException GetUserNotFoundException(string tweetUserName, TweetLine tweetLine) {
      return new DataException("The user '" + tweetUserName + "' with tweet '" 
                               + tweetLine + "' was not found in the user config file.");
    }
  }
}
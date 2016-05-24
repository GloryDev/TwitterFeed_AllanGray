using System.Collections.Generic;

namespace TwitterFeed_AllanGray {
  public class TweetOutputFormatter {
    private readonly IOutputter _outputter;

    public TweetOutputFormatter(IOutputter outputter) {
      _outputter = outputter;
    }

    public void Output(List<User> users) {
      foreach (var user in users) {
        _outputter.WriteLine(user.Name);
        OutputUsersTweets(user);
      }
    }

    private void OutputUsersTweets(User user) {
      foreach (var tweet in user.Tweets) {
        _outputter.WriteLine("    @" + tweet.User.Name + ": " + tweet.TweetContent);
      }
    }
  }
}
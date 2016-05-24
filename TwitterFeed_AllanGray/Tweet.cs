using System;

namespace TwitterFeed_AllanGray {
  public class Tweet
  {
    public User User { get; }
    public string TweetContent { get; }

    public Tweet(User user, string tweetContent)
    {
      if (user == null) throw new ArgumentNullException(nameof(user));
      this.User = user;
      this.TweetContent = tweetContent;
    }

    public  string UserName => this.User.Name;

    public override string ToString() {
      return this.User.Name + "> " + this.TweetContent;
    }
  }
}
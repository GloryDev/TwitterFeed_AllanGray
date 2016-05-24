namespace TwitterFeed_AllanGray {
  public class TweetLine {
    public string TweeterName { get; private set; }
    public string TweetContent { get; private set; }

    public TweetLine(string userName, string tweetContent) {
      TweeterName = userName;
      TweetContent = tweetContent;
    }
  }
}
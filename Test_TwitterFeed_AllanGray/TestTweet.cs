using System.Linq;
using NUnit.Framework;
using TwitterFeed_AllanGray;

namespace Test_TwitterFeed_AllanGray {
  [TestFixture]
  public class TestTweet {
    [Test]
    public void Test_ConstructUser() {
      
      //------------------------------------Setup--------------------------------------
      var user = new User("Alan");
      var tweetContent = "this is a tweet";
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var tweet = new Tweet(user, tweetContent);
      //------------------------------------Test PostCondition-------------------------
      Assert.IsNotNull(tweet);
      Assert.AreSame(user, tweet.User);
      Assert.AreEqual(tweetContent, tweet.TweetContent);
    }

  }
}
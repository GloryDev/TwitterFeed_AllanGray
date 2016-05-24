using System.Linq;
using NUnit.Framework;
using TwitterFeed_AllanGray;

namespace Test_TwitterFeed_AllanGray {
  [TestFixture]
  public class TestTweetLine {
    [Test]
    public void Test_ConstructUser() {
      var username = "UserName";
      var tweetContent = "This is my first tweet";
      //------------------------------------Setup--------------------------------------
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var user = new TweetLine(username, tweetContent);
      //------------------------------------Test PostCondition-------------------------
      Assert.IsNotNull(user);
      Assert.AreEqual(username, user.TweeterName);
      Assert.AreEqual(tweetContent, user.TweetContent);
    }   

  }
}
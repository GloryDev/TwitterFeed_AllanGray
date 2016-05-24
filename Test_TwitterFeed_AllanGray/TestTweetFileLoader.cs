using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestLilithClasses;
using TwitterFeed_AllanGray;

namespace Test_TwitterFeed_AllanGray {
  [TestFixture]
  public class TestTweetFileLoader
  {
    [Test]
    public void Test_ConstructUserLoader()
    {
      //------------------------------------Setup--------------------------------------
      var streamReader = new StreamReaderEmptyStub();
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var tweetFileLoader = new TweetFileLoader(streamReader);
      //------------------------------------Test PostCondition-------------------------
      Assert.IsNotNull(tweetFileLoader);
    }

    [Test]
    public void Test_LoadTweets_WhenNoTweets_ShouldReturnEmptyList() {
      //------------------------------------Setup--------------------------------------
      var streamReader = new StreamReaderEmptyStub();
      var tweetFileLoader = new TweetFileLoader(streamReader);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var tweets = tweetFileLoader.LoadTweets();
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsEmpty(tweets);
    }

    [Test]
    public void Test_LoadTweets_WhenOneTweets_ShouldReturnTweet()
    {
      //------------------------------------Setup--------------------------------------
      var streamReader = new StreamReaderFake(new List<string> {"Alan> This is my first tweet"});
      var tweetFileLoader = new TweetFileLoader(streamReader);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var tweets = tweetFileLoader.LoadTweets();
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsNotEmpty(tweets);
      var tweetLine = tweets.First();
      Assert.AreEqual("Alan", tweetLine.TweeterName);
      Assert.AreEqual("This is my first tweet", tweetLine.TweetContent);
    }

    [Test]
    public void Test_LoadTweets_WhenOneTweetsWithTweeterNameNoContent_ShouldReturnEmptyTweetContent()
    {
      //------------------------------------Setup--------------------------------------
      var streamReader = new StreamReaderFake(new List<string> {"Alan>"});
      var tweetFileLoader = new TweetFileLoader(streamReader);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var tweets = tweetFileLoader.LoadTweets();
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsNotEmpty(tweets);
      var tweetLine = tweets.First();
      Assert.AreEqual("Alan", tweetLine.TweeterName);
      Assert.AreEqual("", tweetLine.TweetContent);
    }

    [Test]
    public void Test_LoadTweets_WhenNoTweeterSeperator_ShouldThrowException()
    {
      //------------------------------------Setup--------------------------------------
      var streamReader = new StreamReaderFake(new List<string> {"Alan"});
      var tweetFileLoader = new TweetFileLoader(streamReader);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var fileLoadException = Assert.Throws<TwitterFileLoadException>(() => tweetFileLoader.LoadTweets());
      //------------------------------------Test PostCondition-------------------------
      StringAssert.Contains("An exception occured loading data from the tweet file", fileLoadException.Message);
      var exception = fileLoadException.InnerExceptions.First();
      StringAssert.Contains("tweet line 'Alan' has an invalid format", exception.Message);
    }
  }

}
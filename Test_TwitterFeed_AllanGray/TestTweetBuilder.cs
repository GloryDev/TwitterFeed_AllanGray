using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TestLilithClasses;
using TwitterFeed_AllanGray;

namespace Test_TwitterFeed_AllanGray {
  [TestFixture]
  public class TestTweetBuilder
  {
    [Test]
    public void Test_ConstructUserLoader()
    {
      //------------------------------------Setup--------------------------------------
      var streamReaderEmpty = new StreamReaderEmptyStub();
      var users = new List<User>();
      var tweetFileLoader = new TweetFileLoader(streamReaderEmpty);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var tweetBuilder = new TweetBuilder(tweetFileLoader, users);
      //------------------------------------Test PostCondition-------------------------
      Assert.IsNotNull(tweetBuilder);
    }
    [Test]
    public void Test_BuildTweets_WhenNoTweets_ShouldReturnEmptyList()
    {
      //------------------------------------Setup--------------------------------------
      var streamReaderEmpty = new StreamReaderEmptyStub();
      var users = new List<User>();
      var tweetFileLoader = new TweetFileLoader(streamReaderEmpty);
      var tweetBuilder = new TweetBuilder(tweetFileLoader, users);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var tweetList = tweetBuilder.BuildTweets();
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsEmpty(tweetList);
    }
    [Test]
    public void Test_BuildTweets_WhenOneTweet_ShouldReturnTweet()
    {
      //------------------------------------Setup--------------------------------------
      var streamReaderEmpty = new StreamReaderFake(new List<string> {"Alan> This is my first tweet"});
      var tweetFileLoader = new TweetFileLoader(streamReaderEmpty);
      var users = new List<User>(new List<User> { new User("Alan")});
      var tweetBuilder = new TweetBuilder(tweetFileLoader, users);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var tweetList = tweetBuilder.BuildTweets();
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsNotEmpty(tweetList);
      var tweet = tweetList.First();
      Assert.AreEqual("Alan", tweet.User.Name);
      Assert.AreEqual("This is my first tweet", tweet.TweetContent);
    }
    [Test]
    public void Test_BuildTweets_WhenTwoTweets_ShouldReturnTweets()
    {
      //------------------------------------Setup--------------------------------------
      var tweets = new List<string> {
        "Alan> This is my first tweet",
        "Alan> This is my second tweet"
      };

      var streamReaderEmpty = new StreamReaderFake(tweets);
      var tweetFileLoader = new TweetFileLoader(streamReaderEmpty);
      var users = new List<User>(new List<User> { new User("Alan")});
      var tweetBuilder = new TweetBuilder(tweetFileLoader, users);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var tweetList = tweetBuilder.BuildTweets();
      //------------------------------------Test PostCondition-------------------------
      Assert.AreEqual(2, tweetList.Count);
      var tweet = tweetList.First();
      Assert.AreEqual("Alan", tweet.User.Name);
      Assert.AreEqual("This is my first tweet", tweet.TweetContent);

      var tweet2 = tweetList[1];
      Assert.AreEqual("Alan", tweet2.User.Name);
      Assert.AreEqual("This is my second tweet", tweet2.TweetContent);
    }
    [Test]
    public void Test_BuildTweets_WhenTwoTweetsDifferentUsers_ShouldReturnTweets()
    {
      //------------------------------------Setup--------------------------------------
      var tweets = new List<string> {
        "Alan> This is my first tweet",
        "Brett> This is Brett's first tweet"
      };

      var userList = new List<User> {
        new User("Alan"),
        new User("Brett")
      };

      var streamReaderEmpty = new StreamReaderFake(tweets);
      var tweetFileLoader = new TweetFileLoader(streamReaderEmpty);
      var users = new List<User>(userList);
      var tweetBuilder = new TweetBuilder(tweetFileLoader, users);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var tweetList = tweetBuilder.BuildTweets();
      //------------------------------------Test PostCondition-------------------------
      Assert.AreEqual(2, tweetList.Count);
      var tweet = tweetList.First();
      Assert.AreEqual("Alan", tweet.User.Name);
      Assert.AreEqual("This is my first tweet", tweet.TweetContent);

      var tweet2 = tweetList[1];
      Assert.AreEqual("Brett", tweet2.User.Name);
      Assert.AreEqual("This is Brett's first tweet", tweet2.TweetContent);
    }
    [Test]
    public void Test_BuildTweets_WhenHasTweets_ShouldAddTweetsToTweets()
    {
      //------------------------------------Setup--------------------------------------
      var tweets = new List<string> {
        "Alan> This is my first tweet",
        "Brett> This is Brett's first tweet"
      };

      var brettUser = new User("Brett");
      var userList = new List<User> {
        new User("Alan"),
        brettUser
      };

      var streamReaderEmpty = new StreamReaderFake(tweets);
      var tweetFileLoader = new TweetFileLoader(streamReaderEmpty);
      var users = new List<User>(userList);
      var tweetBuilder = new TweetBuilder(tweetFileLoader, users);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var tweetList = tweetBuilder.BuildTweets();
      //------------------------------------Test PostCondition-------------------------
      Assert.AreEqual(2, tweetList.Count);
      var alansTweet = tweetList.First();
      var alan = alansTweet.User;
      CollectionAssert.IsNotEmpty(alan.Tweets);
      Assert.AreSame(alansTweet, alan.Tweets.First());

      var brettsTweet = tweetList[1];
      Assert.AreSame(brettsTweet, brettUser.Tweets.First());
    }
    [Test]
    public void Test_BuildTweets_WhenFollowsUserWithTweets_ShouldAddotherTweetsToTweets()
    {
      //------------------------------------Setup--------------------------------------
      var tweets = new List<string> {
        "Brett> This is Brett's first tweet"
      };

      var brettUser = new User("Brett");
      var alanUser = new User("Alan");
      var userList = new List<User> {
        alanUser,
        brettUser
      };

      alanUser.AddFollows(brettUser);

      var streamReaderEmpty = new StreamReaderFake(tweets);
      var tweetFileLoader = new TweetFileLoader(streamReaderEmpty);
      var users = new List<User>(userList);
      var tweetBuilder = new TweetBuilder(tweetFileLoader, users);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      tweetBuilder.BuildTweets();
      //------------------------------------Test PostCondition-------------------------
      Assert.AreEqual(1, brettUser.Tweets.Count);
      Assert.AreEqual(1, alanUser.Tweets.Count);
      var alansTwitterFeedFirstItem = alanUser.Tweets.First();
      Assert.AreEqual("Brett", alansTwitterFeedFirstItem.UserName);
      Assert.AreEqual("This is Brett's first tweet", alansTwitterFeedFirstItem.TweetContent);
    }
    [Test]
    public void Test_BuildTweets_WhenManyTweets_ShouldInterLaceAllFollowsTweetsWithFollowedByTweets()
    {
      //------------------------------------Setup--------------------------------------
      var tweets = new List<string> {
        "Brett> This is Brett's first tweet",
        "Alan> This is Alan's first tweet",
        "Brett> This is Brett's second tweet"
      };

      var brettUser = new User("Brett");
      var alanUser = new User("Alan");
      var userList = new List<User> {
        alanUser, 
        brettUser
      };

      brettUser.AddFollows(alanUser);

      var streamReaderEmpty = new StreamReaderFake(tweets);
      var tweetFileLoader = new TweetFileLoader(streamReaderEmpty);
      var users = new List<User>(userList);
      var tweetBuilder = new TweetBuilder(tweetFileLoader, users);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      tweetBuilder.BuildTweets();
      //------------------------------------Test PostCondition-------------------------
      Assert.AreEqual(3, brettUser.Tweets.Count);
      var brettFirstFeed = brettUser.Tweets.First();
      Assert.AreEqual("Brett", brettFirstFeed.UserName);
      Assert.AreEqual("This is Brett's first tweet", brettFirstFeed.TweetContent);
      Assert.AreEqual("This is Alan's first tweet", brettUser.Tweets[1].TweetContent);
      Assert.AreEqual("This is Brett's second tweet", brettUser.Tweets[2].TweetContent);

      Assert.AreEqual(1, alanUser.Tweets.Count);
    }

    [Test]
    public void Test_BuildTweets_WhenTweetNotAssociatedWithAUser_ShouldThrowException()
    {
      //------------------------------------Setup--------------------------------------
      var tweets = new List<string> {
        "UnknownUser> This is my first tweet"
      };

      var streamReaderEmpty = new StreamReaderFake(tweets);
      var tweetFileLoader = new TweetFileLoader(streamReaderEmpty);
      var users = new List<User>(new List<User> { new User("Alan") });
      var tweetBuilder = new TweetBuilder(tweetFileLoader, users);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var aggregateException = Assert.Throws<AggregateException>(() => tweetBuilder.BuildTweets());
      //------------------------------------Test PostCondition-------------------------
      var innerExceptions = aggregateException.InnerExceptions;
      Assert.AreEqual(1, innerExceptions.Count);
      var exception = innerExceptions.First();
      StringAssert.Contains("was not found in the user config file", exception.Message);
    }
  }
} 
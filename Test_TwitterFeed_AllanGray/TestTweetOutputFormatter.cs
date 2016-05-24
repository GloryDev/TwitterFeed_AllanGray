using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TwitterFeed_AllanGray;

namespace Test_TwitterFeed_AllanGray {
  [TestFixture]
  public class TestTweetOutputFormatter {

    [Test]
    public void Test_ConstructUser() {
      
      //------------------------------------Setup--------------------------------------
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var tweetOutputter = new TweetOutputFormatter(new TestingOutputter());
      //------------------------------------Test PostCondition-------------------------
      Assert.IsNotNull(tweetOutputter);
    }
    [Test]
    public void Test_Output_WhenNoUsers_ShouldEmpty() {
      
      //------------------------------------Setup--------------------------------------
      var users = new List<User>();
      var testingOutputter = new TestingOutputter();
      var tweetOutputter = new TweetOutputFormatter(testingOutputter);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      tweetOutputter.Output(users);
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsEmpty(testingOutputter.Output);
    }
    [Test]
    public void Test_Output_WhenOneUserNoTweets_ShouldOutputUserName() {
      
      //------------------------------------Setup--------------------------------------
      var username = "UserName";
      var users = new List<User> {new User(username)};
      var testingOutputter = new TestingOutputter();
      var tweetOutputter = new TweetOutputFormatter(testingOutputter);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      tweetOutputter.Output(users);
      //------------------------------------Test PostCondition-------------------------
      var output = testingOutputter.Output;
      CollectionAssert.IsNotEmpty(output);
      var outputLine = output.First();
      Assert.AreEqual(username, outputLine);
    }
    [Test]
    public void Test_Output_WhenOneUserHasATweet_ShouldOutputUserNameAndTweet() {
      
      //------------------------------------Setup--------------------------------------
      var username = "UserName";
      var user = new User(username);
      user.Tweets.Add(new Tweet(user, "My First Tweet"));
      var users = new List<User> {user};
      var testingOutputter = new TestingOutputter();
      var tweetOutputter = new TweetOutputFormatter(testingOutputter);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      tweetOutputter.Output(users);
      //------------------------------------Test PostCondition-------------------------
      var output = testingOutputter.Output;
      Assert.AreEqual(2, output.Count);
      var outputLine = output.First();
      Assert.AreEqual(username, outputLine);


      var outputLine2 = output[1];
      Assert.AreEqual("    @UserName: My First Tweet", outputLine2);
    }
    [Test]
    public void Test_Output_WhenOneUserHasTwoTweets_ShouldOutputUserNameAndTweets() {
      
      //------------------------------------Setup--------------------------------------
      var username = "UserName";
      var user = new User(username);
      user.Tweets.Add(new Tweet(user, "My First Tweet"));
      user.Tweets.Add(new Tweet(user, "My Second Tweet"));
      var users = new List<User> {user};
      var testingOutputter = new TestingOutputter();
      var tweetOutputter = new TweetOutputFormatter(testingOutputter);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      tweetOutputter.Output(users);
      //------------------------------------Test PostCondition-------------------------
      var output = testingOutputter.Output;
      Assert.AreEqual(3, output.Count);
      var outputLine = output.First();
      Assert.AreEqual(username, outputLine);


      var outputLine2 = output[1];
      Assert.AreEqual("    @UserName: My First Tweet", outputLine2);

      var outputLine3 = output[2];
      Assert.AreEqual("    @UserName: My Second Tweet", outputLine3);
    }
    [Test]
    public void Test_Output_WhenOneUserFollows_ShouldOutputUserNameAndFollowsTweets() {
      
      //------------------------------------Setup--------------------------------------
      var follower = new User("firstUser");
      var followed = new User("followedUser");

      var tweet = new Tweet(followed, "Followed Users First Tweet");
      followed.Tweets.Add(tweet);
      follower.Tweets.Add(tweet);

      var users = new List<User> {follower, followed};

      var testingOutputter = new TestingOutputter();
      var tweetOutputter = new TweetOutputFormatter(testingOutputter);
      //------------------------------------Test Precondition--------------------------
      Assert.AreEqual(1, follower.Tweets.Count);
      Assert.AreEqual(1, followed.Tweets.Count);
      //------------------------------------Execute------------------------------------
      tweetOutputter.Output(users);
      //------------------------------------Test PostCondition-------------------------
      var output = testingOutputter.Output;
      Assert.AreEqual(4, output.Count);
      var outputLine = output.First();
      Assert.AreEqual("firstUser", outputLine);


      var outputLine2 = output[1];
      Assert.AreEqual("    @followedUser: Followed Users First Tweet", outputLine2);

    }
  }

  class TestingOutputter : IOutputter {
    public List<string> Output { get; } = new List<string>();

    public void WriteLine(string line) {
      Output.Add(line);
    }
  }
}
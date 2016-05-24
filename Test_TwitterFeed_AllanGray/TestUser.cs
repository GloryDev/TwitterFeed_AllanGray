using System.Linq;
using NUnit.Framework;
using TwitterFeed_AllanGray;

namespace Test_TwitterFeed_AllanGray {
  [TestFixture]
  public class TestUser {
    [Test]
    public void Test_ConstructUser() {
      var username = "UserName";
      //------------------------------------Setup--------------------------------------
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var user = new User(username);
      //------------------------------------Test PostCondition-------------------------
      Assert.IsNotNull(user);
      Assert.AreEqual(username, user.Name);
    }

    [Test]
    public void Test_Follows_WhenDoesNotFollowAnyone_ShouldReturnEmptyList() {
      //------------------------------------Setup--------------------------------------
      var user = new User("Alan");
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var follows = user.Follows;
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsEmpty(follows);
    }
    [Test]
    public void Test_ToString_ShouldReturnName() {
      //------------------------------------Setup--------------------------------------
      var expectedName = "Alan";
      var user = new User(expectedName);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var actualName = user.ToString();
      //------------------------------------Test PostCondition-------------------------
      Assert.AreEqual(expectedName, actualName);
    }

    [Test]
    public void Test_AddFollows_WhenFollowOnePerson_ShouldAddPersonToFollows() {
      //------------------------------------Setup--------------------------------------
      var user = new User("Alan");
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------

      user.AddFollows(new User("Brett"));
      //------------------------------------Test PostCondition-------------------------
      var follows = user.Follows;
      CollectionAssert.IsNotEmpty(follows);
      Assert.AreEqual("Brett", follows.First().Name);
    }
    [Test]
    public void Test_AddFollows_WhenAlreadyFollows_ShouldNotAddFollowsOrFollowedBy() {
      //------------------------------------Setup--------------------------------------
      var user = new User("Alan");
      //------------------------------------Test Precondition--------------------------

      CollectionAssert.IsEmpty(user.FollowedBy);
      CollectionAssert.IsEmpty(user.Follows);
      //------------------------------------Execute------------------------------------
      var followedUser = new User("Brett");
      user.AddFollows(followedUser);
      user.AddFollows(followedUser);
      //------------------------------------Test Post Condition-------------------------
      var follows = user.Follows;
      Assert.AreEqual(1, follows.Count);
      Assert.AreEqual("Brett", follows.First().Name);
    }

    [Test]
    public void Test_FollowedBy_WhenDoesNotHaveFoloowers_ShouldReturnEmptyList() {
      //------------------------------------Setup--------------------------------------
      var user = new User("Alan");
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var followedBy = user.FollowedBy;
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsEmpty(followedBy);
    }

    [Test]
    public void Test_AddFollows_ShouldAddToOtherUsersFollowedBy() {
      //------------------------------------Setup--------------------------------------
      var user = new User("Alan");
      var userA = new User("A");
      var userB = new User("B");
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      userA.AddFollows(user);
      userB.AddFollows(user);
      //------------------------------------Test PostCondition-------------------------
      var followedBy = user.FollowedBy;
      CollectionAssert.IsNotEmpty(followedBy);
      Assert.AreEqual("A", followedBy.First().Name);
      Assert.AreEqual("B", followedBy.Last().Name);
    }

    [Test]
    public void Test_AddTweets_ShouldAddToTweetsForUser()
    {
      //------------------------------------Setup--------------------------------------
      var user = new User("Alan");
      var tweet = new Tweet(user, "A tweet");
      //------------------------------------Test Precondition--------------------------
      CollectionAssert.IsEmpty(user.Tweets);
      //------------------------------------Execute------------------------------------
      user.Tweets.Add(tweet);
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsNotEmpty(user.Tweets);
      var returnedTweet = user.Tweets.First();
      Assert.AreSame(tweet, returnedTweet);
    }

  }
}
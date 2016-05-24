using System.Collections.Generic;
using System.Data;
using System.Linq;
using NUnit.Framework;
using TwitterFeed_AllanGray;

namespace Test_TwitterFeed_AllanGray {
  [TestFixture]
  public class TestUserBuilder
  {
    [Test]
    public void Test_ConstructUserLoader()
    {
      //------------------------------------Setup--------------------------------------
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string>());
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userBuilder = new UserBuilder(userFileLoader);
      //------------------------------------Test PostCondition-------------------------
      Assert.IsNotNull(userBuilder);
    }
    [Test]
    public void Test_BuildUsers_WhenNoUsers_ShouldReturnEmptyList()
    {
      //------------------------------------Setup--------------------------------------
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string>());
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userBuilder = new UserBuilder(userFileLoader);
      var userList = userBuilder.BuildUsers();
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsEmpty(userList);
    }
    [Test]
    public void Test_BuildUsers_WhenOneUser_ShouldReturnUser()
    {
      //------------------------------------Setup--------------------------------------
      var userConfig = "Alan";
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string> {userConfig});
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userBuilder = new UserBuilder(userFileLoader);
      var userList = userBuilder.BuildUsers();
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsNotEmpty(userList);
      var user = userList.First();
      Assert.AreEqual(userConfig, user.Name);
    }
    [Test]
    public void Test_BuildUsers_WhenOneUserWithSpacesInName_ShouldReturnUser()
    {
      //------------------------------------Setup--------------------------------------
      var userConfig = "Alan Brett";
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string> {userConfig});
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userBuilder = new UserBuilder(userFileLoader);
      var userList = userBuilder.BuildUsers();
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsNotEmpty(userList);
      var user = userList.First();
      Assert.AreEqual(userConfig, user.Name);
    }

    [Test]
    public void Test_BuildUsers_WhenOneUserWithFollowsAndNousers_ShouldReturnUser()
    {
      //------------------------------------Setup--------------------------------------
      var userConfig = "Alan follows";
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string> { userConfig });
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userBuilder = new UserBuilder(userFileLoader);
      var userList = userBuilder.BuildUsers();
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsNotEmpty(userList);
      var user = userList.First();
      Assert.AreEqual("Alan", user.Name);
    }

    [Test]
    public void Test_BuildUsers_WhenTwoUsersNoFollows_ShouldReturnBothUser()
    {
      //------------------------------------Setup--------------------------------------
      var user1Config = "Alan";
      var user2Config = "Brett";
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string> {user1Config, user2Config});
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userBuilder = new UserBuilder(userFileLoader);
      var userList = userBuilder.BuildUsers();
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsNotEmpty(userList);
      var user1 = userList.First();
      Assert.AreEqual(user1Config, user1.Name);
      CollectionAssert.IsEmpty(user1.Follows);

      var user2 = userList.Last();
      Assert.AreEqual(user2Config, user2.Name);
      CollectionAssert.IsEmpty(user2.Follows);

    }

    [Test]
    public void Test_BuildUsers_WhenManyUsersNoFollows_ShouldReturnAllUsers()
    {
      //------------------------------------Setup--------------------------------------
      var user1Config = "Alan";
      var user2Config = "Brett";
      var user3Config = "Benny";
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string> {user1Config, user2Config, user3Config});
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userBuilder = new UserBuilder(userFileLoader);
      var userList = userBuilder.BuildUsers();
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsNotEmpty(userList);
      var user1 = userList.First();
      Assert.AreEqual(user1Config, user1.Name);
      var user2 = userList[1];
      Assert.AreEqual(user2Config, user2.Name);
      var user3 = userList[2];
      Assert.AreEqual(user3Config, user3.Name);
      
    }
    [Test]
    public void Test_BuildUsers_WhenTwoUsersOneFollows_ShouldReturnAllUsersWithFollows()
    {
      //------------------------------------Setup--------------------------------------
      var user1Config = "Alan";
      var user2Config = "Brett follows Alan";
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string> {user1Config, user2Config});
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userBuilder = new UserBuilder(userFileLoader);
      var userList = userBuilder.BuildUsers();
      //------------------------------------Test PostCondition-------------------------
      var user1 = userList.First();
      Assert.AreEqual(user1Config, user1.Name);
      CollectionAssert.IsEmpty(user1.Follows);

      var user2 = userList[1];
      Assert.AreEqual("Brett", user2.Name);
      CollectionAssert.IsNotEmpty(user2.Follows);
      var follows = user2.Follows;
      Assert.AreEqual("Alan", follows.First().Name);
    }

    [Test]
    public void Test_BuildUsers_WhenLastUserUserHasTwoFollows_ShouldReturnUserWithFollows()
    {
      //------------------------------------Setup--------------------------------------
      var user1Config = "Alan";
      var user2Config = "Brett";
      var user3Config = "Benny follows Alan, Brett";
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string> {user1Config, user2Config, user3Config});
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userBuilder = new UserBuilder(userFileLoader);
      var userList = userBuilder.BuildUsers();
      //------------------------------------Test PostCondition-------------------------
      var user3 = userList[2];
      Assert.AreEqual("Benny", user3.Name);
      CollectionAssert.IsNotEmpty(user3.Follows);
      var follows = user3.Follows;
      Assert.AreEqual("Alan", follows.First().Name);
      Assert.AreEqual("Brett", follows.Last().Name);
    }

    [Test]
    public void Test_BuildUsers_WhenFirstUserUserHasTwoFollows_ShouldReturnUserWithFollowers()
    {
      //------------------------------------Setup--------------------------------------
      var user1Config = "Alan follows Brett, Benny";
      var user2Config = "Brett";
      var user3Config = "Benny";
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string> {user1Config, user2Config, user3Config});
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userBuilder = new UserBuilder(userFileLoader);
      var userList = userBuilder.BuildUsers();
      //------------------------------------Test PostCondition-------------------------

      var user1 = userList[0];
      Assert.AreEqual("Alan", user1.Name);
      CollectionAssert.IsNotEmpty(user1.Follows);
      var follows = user1.Follows;
      Assert.AreEqual("Brett", follows.First().Name);
      Assert.AreEqual("Benny", follows.Last().Name);
    }
    [Test] public void Test_BuildUsers_WhenFirstUserUserHasOneFollows_ShouldReturnUserWithFollows()
    {
      //------------------------------------Setup--------------------------------------
      var user1Config = "Alan follows Benny";
      var user2Config = "Brett";
      var user3Config = "Benny";
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string> {user1Config, user2Config, user3Config});
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userBuilder = new UserBuilder(userFileLoader);
      var userList = userBuilder.BuildUsers();
      //------------------------------------Test PostCondition-------------------------

      var user1 = userList[0];
      Assert.AreEqual("Alan", user1.Name);
      CollectionAssert.IsNotEmpty(user1.Follows);
      var follows = user1.Follows;
      Assert.AreEqual("Benny", follows.First().Name);
    }
    [Test]
    [Ignore("Hypothesis as to whether this is needed or not")]
    public void Test_BuildUsers_WhenFirstUserUserHasOneFollows_ShouldSetFollowsUserAsFollowed()
    {
      //------------------------------------Setup--------------------------------------
      var user1Config = "Alan follows Benny";
      var user2Config = "Brett";
      var user3Config = "Benny";
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string> {user1Config, user2Config, user3Config});
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userBuilder = new UserBuilder(userFileLoader);
      var userList = userBuilder.BuildUsers();
      //------------------------------------Test PostCondition-------------------------
      var user1 = userList[0];
      Assert.AreEqual("Alan", user1.Name);
      CollectionAssert.IsNotEmpty(user1.Follows);
      var follows = user1.Follows;
      Assert.AreEqual("Benny", follows.First().Name); 
    }

    [Test]
    public void Test_BuildUsers_WhenFollowsNonExistentUser_ShouldCreateTheUser()
    {

      //------------------------------------Setup--------------------------------------
      var user1Config = "Alan follows UnknownUser";
      IUserFileLoader userFileLoader = new UserFileLoaderFake(new List<string> {user1Config});
      var userBuilder = new UserBuilder(userFileLoader);
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userList = userBuilder.BuildUsers();
      //------------------------------------Test PostCondition-------------------------
      Assert.AreEqual(2, userList.Count);
      Assert.AreEqual("Alan", userList[0].Name);
      Assert.AreEqual("UnknownUser", userList[1].Name);
    }

    //Note: This is a preferable way of working that the errors are logged and the entire file processed.
    // The errors can then all be reported at the end as compared to recording the errors on the fly.
    // For the purposes of this test however I am not going to implement a full logging framework or use
    // one of the existing frameworks.
    //Another option might be to throw the errors at the end of the BuildUsers method.
    // this would depend on convention to an extend. The advantage of throwing the error at the end
    //  with all the accumulated errors is that the error is then definitely handled. This method 
    //  of calling a seperate method would be less advisable.
    //  the other option would be just to log all the errors and not propagate to the calling method.
    //  This would be better for certain UI.
  }
}
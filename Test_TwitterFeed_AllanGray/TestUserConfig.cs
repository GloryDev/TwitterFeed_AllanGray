using System;
using NUnit.Framework;
using TwitterFeed_AllanGray;

namespace Test_TwitterFeed_AllanGray {
  [TestFixture]
  public class TestUserConfig {
    // ReSharper disable ObjectCreationAsStatement
    [Test]
    public void Test_ConstructUser_WithEmptyLine_ShouldRaiseError() {
      var userConfigLine = "";
      //------------------------------------Setup--------------------------------------
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      Assert.Throws<ArgumentException>(() => new UserConfig(userConfigLine));
      //------------------------------------Test PostCondition-------------------------
    }
    [Test]
    public void Test_ConstructUser_WithOnlyWhiteSpalceLine_ShouldRaiseError() {
      var userConfigLine = "     ";
      //------------------------------------Setup--------------------------------------
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      Assert.Throws<ArgumentException>(() => new UserConfig(userConfigLine));
      //------------------------------------Test PostCondition-------------------------
    }

    [Test]
    public void Test_ConstructUser_WithUserName_ShouldSetUserName()
    {
      var userConfigLine = "UserName";
      //------------------------------------Setup--------------------------------------
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userConfig = new UserConfig(userConfigLine);
      //------------------------------------Test PostCondition-------------------------
      Assert.IsNotNull(userConfig);
      Assert.AreEqual(userConfigLine, userConfig.UserName);
      Assert.IsNotNull(userConfig.Follows);
    }

    [Test]
    public void Test_ConstructUser_WithFollows_ShouldSetUserFollows()
    {
      var userConfigLine = "UserName1 follows UserName2";
      //------------------------------------Setup--------------------------------------
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userConfig = new UserConfig(userConfigLine);
      //------------------------------------Test PostCondition-------------------------
      CollectionAssert.IsNotEmpty(userConfig.Follows);
    }
  }
} 
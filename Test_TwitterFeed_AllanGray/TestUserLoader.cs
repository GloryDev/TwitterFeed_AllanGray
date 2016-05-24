using NUnit.Framework;
using TwitterFeed_AllanGray;

namespace Test_TwitterFeed_AllanGray
{

  [TestFixture]
  public class TestUserLoader
  {
    [Test]
    public void Test_ConstructUserLoader()
    {
      //------------------------------------Setup--------------------------------------
      const string userFileName = "User.txt";
      //------------------------------------Test Precondition--------------------------
      //------------------------------------Execute------------------------------------
      var userLoader = new UserFileLoader(userFileName);
      //------------------------------------Test PostCondition-------------------------
      Assert.IsNotNull(userLoader);
    }
  }
}

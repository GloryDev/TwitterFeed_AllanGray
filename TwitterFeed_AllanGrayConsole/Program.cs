using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LillithClasses;
using TwitterFeed_AllanGray;

namespace TwitterFeed_AllanGrayConsole {
  class Program {
    static void Main(string[] args) {
      string userFileName = "user.txt";
      string tweetFileName = "tweet.txt";
      if (args.Length > 0) {
        userFileName = args[0];
        tweetFileName = args[1];
      }

      try {
        var userBuilder = new UserBuilder(new UserFileLoader(userFileName));
        var users = userBuilder.BuildUsers();

        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        var fileInfo = new FileInfo($@"{baseDirectory}\" + tweetFileName);
        using (var filestream = new FileStream(fileInfo.FullName, FileMode.Open)) {
          var fileStream = new StreamReader(filestream);
          IStreamReader streamReader = new StreamReaderWrapper(fileStream);
          var tweetBuilder = new TweetBuilder(new TweetFileLoader(streamReader), users);
          tweetBuilder.BuildTweets();
        }

        var tweetOutputFormatter = new TweetOutputFormatter(new ConsoleOutputter());
        tweetOutputFormatter.Output(users);
        Console.ReadLine();
      }
      catch (Exception e) {
        Console.WriteLine(e);
        Console.ReadLine();
      }
    }
  }
}
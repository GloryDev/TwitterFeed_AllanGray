using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LillithClasses;

namespace TwitterFeed_AllanGray {
  public class TweetFileLoader {
    private readonly IStreamReader _tweetStream;
    private List<Exception> Exceptions { get; } = new List<Exception>();

    public TweetFileLoader(IStreamReader tweetStream) {
      _tweetStream = tweetStream;
    }

    public List<TweetLine> LoadTweets() {
      //Note:- I have done this one using a stream reader that has been injected.
      //This makes the loading of the tweet file a little more testable than the loading of the user file.
      //I have also changed to the another option of exception handling. Accumulating all the exceptions and
      // then throwing an aggregate exception with all the accumulated exceptions.
      // Another alternate of just logging all the exceptions has not been explored here.
      // The option of logging all the exceptions and sending the administrator/support site a message when an error is logged
      //  would probably be my preferred option in a user facing production application like this but I have not implemented it
      //  here due to the effort involved. I traditionaly wrapped log4net behind an interface. The logger then became injectable using 
      //  hand rolled dependency injection or one of the many DI frameworks. The logger could then be changed for test mode (Spit the exceptions
      // out to the console and deployed mode (log errors in a file and message support). This could also be configured for severity and 
      // all those fun things.
      var tweetsList = new List<TweetLine>();
      try {

        while (!_tweetStream.EndOfStream) {
          try {
            var tweetLine = _tweetStream.ReadLine();
            var tweetPieces = tweetLine.Split('>');
            if (tweetPieces.Length != 2) ThrowTweetLineException(tweetLine);

            var line = new TweetLine(tweetPieces.FirstOrDefault(), tweetPieces[1].Trim());
            tweetsList.Add(line);
          }
          catch (Exception e) {
            Exceptions.Add(e);
          }
        }
      }
      catch (Exception e) {
        Exceptions.Add(e);
      }
      if (this.Exceptions.Count != 0) {
        throw new TwitterFileLoadException("An exception occured loading data from the tweet file.", this.Exceptions);
      }
      return tweetsList;
    }

    private static void ThrowTweetLineException(string tweetLine) {
      throw new FileLoadException("The Tweet File has an error: tweet line '" + tweetLine +
                                  "' has an invalid format.");
    }
  }

  public class TwitterFileLoadException: AggregateException
  {

    public TwitterFileLoadException(string message, IEnumerable<Exception> exceptions) : base(message, exceptions) {
    }
  }
}
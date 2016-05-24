using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitterFeed_AllanGray {
  public class User {
    private List<User> _follows = new List<User>();
    private List<User> _followedBy = new List<User>();
    public string Name { get; }

    public IReadOnlyList<User> Follows => _follows;

    public IList<Tweet> Tweets { get; } = new List<Tweet>();
    public IReadOnlyList<User> FollowedBy => _followedBy;

    public User(string name) {
      Name = name;
    }

    public override string ToString() {
      return this.Name;
    }

    public void AddFollows(User user) {
      var foundUser = _follows.Find(user1 => user1.Name == user.Name);
      if (foundUser != null) return;
      _follows.Add(user);
      user.AddFollowedBy(this);
    }

    private void AddFollowedBy(User user) {
      _followedBy.Add(user);
    }
  }
}
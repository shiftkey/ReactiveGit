ReactiveGit
===========

This is an experiment in combining Rx and `LibGit2Sharp` to create an API to make git operations friendly for asynchronous situations.

`libgit2` operations are, by their nature, synchronous. This is reflected in the wrapper APIs such as `LibGit2Sharp`. This is great for simple interactions with repositories, but working with complex tasks against large repositories requires embracing asynchrony and pushing operations onto background threads.

Rather than create a duplicate set of asynchronous APIs for `LibGit2Sharp`, this project is intending to represent specific Git actions as a thing which can be observed. Many operations support cancellation, and this can be handled in `ReactiveGit` by simply disposing of the subscription to an observable action.

The other focus of this framework is around progress handlers, which `LibGit2Sharp` added support for recently. This enables a user to specify a callbacks to events in `libgit2`. By passing an observer to the operation in `ReactiveGit`, you can receive progress information in real-time to display in your application.

### Installation

Install the package from NuGet!

```
Install-Package ReactiveGit
```

### Examples

```
// many operations require authentication
CredentialsHandler credentials = (url, usernameFromUrl, types) =>
    new UsernamePasswordCredentials
    {
        Username = "shiftkey-tester",
        Password = "haha-password"
    };


// you can also pass an existing LibGit2Sharp repository here
var repository = new ObservableRepository(
  @"C:\Users\brendanforster\Documents\GìtHūb\testing-pushspecs",
  credentials);

// specify a progress observer to report progress
var pullObserver = new ReplaySubject<Tuple<string, int>>();
pullObserver.Subscribe(
  next => Console.WriteLine("Progress: " + next.Item2));

// execute the operation
var pullResult = await repository.Pull(pullObserver);
```

### Contributing

If you'd like to get involved, check out the [CONTRIBUTING.md](https://github.com/shiftkey/ReactiveGit/blob/master/CONTRIBUTING.md) docs for more information.

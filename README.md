ReactiveGit
===========

This is an experiment in combining Rx and LibGit2sharp (in particular the new progress handler APIs) to create an API surface for specific tasks which is more friendly for asynchronous applications.

I'm not sure if this will ever get into production usage. Actually, I'm kinda surprised this even works.

### Why?

`libgit2` operations are, by their nature, synchronous. This is reflected in the wrapper APIs such as `libgit2sharp`. This is great for simple interactions with repositories, but working with complex tasks against large repositories requires embracing asynchrony and pushing operations onto background threads.

Rather than build a whole set of complementary asynchronous APIs for `libgit2sharp`, this project is looking to represent specific Git actions as a thing which can be observed. Many operations support cancellation, and this is handled in `ReactiveGit` by simply disposing of the subscription to an observable action.

The other focus of this framework is around progress handlers, which `libgit2sharp` added support for recently. This enables a user to specify a callbacks to events in `libgit2`. By passing an observer to the operation in `ReactiveGit`, you can receive progress information in real-time to display in your application.

### What to do?

There's often long-running tasks when working with a git repository:

 - clone
 - fetch
 - checkout
 - pull
 - push

Being able to report on the progress of operations, as well as compose asynchronous operations together, is what I'm experimenting with here...  

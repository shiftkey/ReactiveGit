ReactiveGit
===========

This is an experiment in combining Rx and LibGit2sharp (in particular the new progress handler APIs) to create an API surface for specific tasks which is more friendly for asynchronous applications.

I'm not sure if this will ever get into production usage. Actually, I'm kinda surprised this even works.

### What?

There's often long-running tasks when working with a git repository:

 - clone
 - fetch
 - checkout
 - pull
 - push

Being able to report on the progress of operations, as well as compose asynchronous operations together, is what I'm experimenting with here...  
 

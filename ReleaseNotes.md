### New in 0.0.4 (Released 2014/10/14)
* clone messages improved to match up with Git behaviour
* clone no longer displays checkout file paths
* `IObservableRepository.Inner` is available for when you need to access the
 underlying LibGit2Sharp repository

### New in 0.0.3 (Released 2014/10/12)
* a proper build process, just like a real OSS project
* now targets LibGit2Sharp v0.19

**Breaking changes:**
 - The `Credentials` `ctor` have been deprecated upstream in favour of
 `CredentialsHandler` delegates.
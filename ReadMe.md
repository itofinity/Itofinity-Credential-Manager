# Overview

[Itofinity Credential Manager](https://github.com/itofinity/itofinity-credential-manager) (ICM) began life as an attempted fork of Microsoft's [Git Credential Manager for Windows](https://github.com/microsoft/git-credential-manager-for-windows) (GCMW).

I'm a great fan of the [GCMW](https://github.com/microsoft/git-credential-manager-for-windows), it means I don't need to muck around with SSH keys, especially if I'm switch workstations, it provides support for 2FA on specified hosts and its a GUI when it can be and CLI when it has to be.

However [ICM](https://github.com/itofinity/itofinity-credential-manager) attempts to address some limitations of the GCMW, namely:
1. It only runs on Windows
1. It only works for Git
1. Adding new Hosts, e.g. GitLab is complicated

## Attempt #1 
This was a straight fork of [GCMW](https://github.com/microsoft/git-credential-manager-for-windows) and then an attempt to extract all the GUI code into Windows specific projects and porting all the remaining code to Dotnet Standard/Core. 

The intention was that it would remain possible to build the existing .NET Framework dependent _git-credential-manager.exe_ and _git-askpass.exe_ executables, but then additionally create new mirror GUI projects, using [Avalonia](https://github.com/avaloniaui/avalonia), and add new Dotnet Core console application versions for _git-credential-manager.exe_ and _git-askpass.exe_.

I still believe there is some merit in that approach, evolution over revolution, but for me it foundered on 2 main issues:

1. [GCMW](https://github.com/microsoft/git-credential-manager-for-windows) is primarily a console application that can spin up GUI components as required.
    * A single [GCMW](https://github.com/microsoft/git-credential-manager-for-windows) flow, e.g. GET, can spawn 1 or more windows/dialogs, one at a time but essentially as independent windows.
    * [Avalonia](https://github.com/avaloniaui/avalonia) to my understanding doesn't allow this, you instantiate a single Avalonia _Application_ and windows chain from one to another. 
    * I could not reconcile these differences within the existing [GCMW](https://github.com/microsoft/git-credential-manager-for-windows) architecture
1. [GCMW](https://github.com/microsoft/git-credential-manager-for-windows) is a well controlled application, as part of the [Git for Windows](https://github.com/git-for-windows/git) distribution. As such its code is, quite rightly, well controlled. The code relies on hardcoding support of hosts such as [Bitbucket.org](https://bitbucket.org), [Github.com](https://github.com) and [Azure Devops](https://devops.azure.com) (There is a performance reason for this)
    * This makes it difficult to add new, especially smaller, hosts
    * porting to Dotnet Core is complex.

## Attempt #2

This current approach is a complete re-write, focusing on reproducing the behaviour of [GCMW](https://github.com/microsoft/git-credential-manager-for-windows) and by definition acting as a Git Credential Helper, see
* https://git-scm.com/docs/git-credential-store
* https://git-scm.com/book/en/v2/Git-Tools-Credential-Storage
* https://git-scm.com/docs/gitcredentials

# Misc

[Todo](docs/todo.md)
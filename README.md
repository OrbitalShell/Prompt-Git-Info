# Prompt Git Info
Module for Orbital Shell

Add custom text to the prompt of console showing the status of git repositories if the current directory is above a repository folder


![.NET](https://github.com/OrbitalShell/Prompt-Git-Info/workflows/.NET/badge.svg)
![last commit](https://img.shields.io/github/last-commit/orbitalshell/Prompt-Git-Info?style=plastic)
![version](https://img.shields.io/github/v/tag/orbitalshell/Prompt-Git-Info?style=plastic) 
<br>
![toplanguage](https://img.shields.io/github/languages/top/orbitalshell/Orbital-Shell)
![lngcount](https://img.shields.io/github/languages/count/orbitalshell/Orbital-Shell)

### Usage

install into the shell:
```shell
> module -i prompt-git-info
```

### Preview

up to date

<img src="assets/0.png"/>

worktree changed

<img src="assets/1.png"/>

index changed

<img src="assets/2.png"/>

ahead

<img src="assets/3.png"/>

behind

<img src="assets/4.png"/>

no repo

<img src="assets/norepo.png"/>
<br>
<br>

### Settings

namespace **env.com.git.promptInfo**

variable | type | value
-- | -- | --
aheadBackgroundColor               | string          | \e[48;5;136m
aheadTextTemplate                  | string          | %bgColor%(f=white) %repoName% ├ %branch% %sepSymbol%%errorMessage%\e[48;5;237m+%indexAdded% ~%indexChanges% -%indexDeleted% \| ~%worktreeChanges% -%worktreeDeleted% ?%untracked% \e[48;5;136m↑%ahead%(rdc)
behindBackgroundColor              | string          | (b=darkred)
behindTextTemplate                 | string          | %bgColor%(f=white) %repoName% ├ %branch% %sepSymbol%%errorMessage%\e[48;5;237m+%indexAdded% ~%indexChanges% -%indexDeleted% \| ~%worktreeChanges% -%worktreeDeleted% ?%untracked% (b=darkred)↓%behind%(rdc)
infoBackgroundColor                | string          | \e[48;5;237m
isEnabled                          | bool            | true
modifiedBackgroundColor            | string          | \e[48;5;130m
modifiedTextTemplate               | string          | %bgColor%(f=white) %repoName% ├ %branch% %sepSymbol%%errorMessage%\e[48;5;237m+%indexAdded% ~%indexChanges% -%indexDeleted% \| ~%worktreeChanges% -%worktreeDeleted% ?%untracked%(rdc)
modifiedUntrackedBackgroundColor   | string          | \e[48;5;166m
noDataTextTemplate                 | string          | %bgColor%(f=white) %repoName% ├ %branch% %errorMessage%(rdc)
templateNoRepository               | string          | (b=darkblue)(f=white) » %errorMessage%(rdc)
unknownBackgroundColor             | string          | (b=darkblue)
upToDateBackgroundColor            | string          | \e[48;5;22m

<br><br><br>

<hr>

<br>

<b>Orbital Shell</b> is a multi-plateform (**windows, linux, macos, arm**) command shell, inspired by <b><i>bash</i></b> and POSIX recommendations. It provides any usual bash shell feature (even if modernized) and nice syntaxes and features allowing to interact (get/set/call members) with C# objects. Developed using **C# 8, .NET Core 3.1/Net 5 and .NET Standard 2.1**


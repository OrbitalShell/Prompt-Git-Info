# Prompt Git Info
Module for Orbital Shell

Add custom text to the prompt of console showing the status of git repositories if the current directory is above a repository folder


![.NET](https://github.com/OrbitalShell/Prompt-Git-Info/workflows/.NET/badge.svg)
![last commit](https://img.shields.io/github/last-commit/orbitalshell/Prompt-Git-Info?style=plastic)
![releasever](https://img.shields.io/github/v/release/orbitalshell/Prompt-Git-Info?style=plastic) 
![releasedate](https://img.shields.io/github/release-date/orbitalshell/Prompt-Git-Info?style=plastic) 
<br>
![toplanguage](https://img.shields.io/github/languages/top/orbitalshell/Prompt-Git-Info)
![lngcount](https://img.shields.io/github/languages/count/orbitalshell/Prompt-Git-Info)
<br>
![codesize](https://img.shields.io/github/languages/code-size/orbitalshell/Prompt-Git-Info)
![reposize](https://img.shields.io/github/repo-size/orbitalshell/Prompt-Git-Info)


### Usage

install into the shell:
```shell
> module -i prompt-git-info
```
disable/enable:
```shell
> prompt-info -e false
> prompt-info -e true
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

behind + behind message

<img src="assets/6.png"/>

ahead/behind

<img src="assets/5.png"/>

ahead/behind + behind message

<img src="assets/7.png"/>

no repo

<img src="assets/norepo.png"/>
<br>
<br>

### Settings

namespace **env.com.git.promptInfo**

variable | type | value
-- | -- | --
isEnabled                          | bool                | true
isEnabledGetRemoteStatus           | bool                | true
runInBackgroundTask                | bool                | fals|e
infoBackgroundColor                | string              | \e[48;5;237|m
modifiedTextTemplate               | string              | %bgColor%(f=w|hite) %repoName% ├ %branch% %sepSymbol%%errorMessage%\e[48;5;237m+%indexAdded% ~%indexChanges% -%indexDeleted% \| ~%worktreeChanges% -%worktreeDeleted% ?%untracked%(rdc|)
behindTextTemplate                 | string              | %bgColor%(f=white) %repoName% ├ %branch% %sepSymbol%%errorMessage%\e[48;5;237m+%indexAdded% ~%indexChanges% -%indexDeleted% \| ~%worktreeChanges% -%worktreeDeleted% ?%untracked% (b=darkred)↓%behind%%behindMessage%(rdc)
aheadBehindTextTemplate            | string              | %bgColor%(f=white) %repoName% ├ %branch% %sepSymbol%%errorMessage%\e[48;5;237m+%indexAdded% ~%indexChanges% -%indexDeleted% \| ~%worktreeChanges% -%worktreeDeleted% ?%untracked% \e[48;5;136m↑%ahead%(b=darkred)↓%behind%%behindMessage%(rdc)
aheadTextTemplate                  | string              | %bgColor%(f=white) %repoName% ├ %branch% %sepSymbol%%errorMessage%\e[48;5;237m+%indexAdded% ~%indexChanges% -%indexDeleted% \| ~%worktreeChanges% -%worktreeDeleted% ?%untracked% \e[48;5;136m↑%ahead%(rdc)
noDataTextTemplate                 | string              | %bgColor%(f=white) %repoName% ├ %branch% %errorMessage%(rdc)
templateNoRepository               | string              | (b=darkblue)(f=white) » %errorMessage%(rdc)
behindBackgroundColor              | string              | (b=darkred)
aheadBackgroundColor               | string              | \e[48;5;136m
upToDateBackgroundColor            | string              | \e[48;5;22m
modifiedBackgroundColor            | string              | \e[48;5;130m
modifiedUntrackedBackgroundColor   | string              | \e[48;5;166m
unknownBackgroundColor             | string              | (b=darkblue)

<br>

### Settings example

change behin background color, for example within user **.profile** script :

```shell
set env.com.git.promptInfo.behindBackgroundColor (b=magenta)
```

<br><br><br>

<hr>

<br>

<b>Orbital Shell</b> is a multi-plateform (**windows, linux, macos, arm**) command shell, inspired by <b><i>bash</i></b> and POSIX recommendations. It provides any usual bash shell feature (even if modernized) and nice syntaxes and features allowing to interact (get/set/call members) with C# objects. Developed using **C# 8, .NET Core 3.1/Net 5 and .NET Standard 2.1**


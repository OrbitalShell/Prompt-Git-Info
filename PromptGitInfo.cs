using OrbitalShell.Component.CommandLine.CommandModel;
using OrbitalShell.Component.Shell.Module;
using OrbitalShell.Component.Shell.Hook;
using OrbitalShell.Component.Shell;
using OrbitalShell.Component.Shell.Variable;
using OrbitalShell.Component.CommandLine.Processor;
using System;
using System.IO;
using System.Linq;
using OrbitalShell.Component.Console;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrbitalShell.Module.PromptGitInfo
{
    /// <summary>
    /// module commands : prompt git infos
    /// </summary>
    [Commands("prompt git info module commands")]
    [CommandsNamespace(CommandNamespace.tools, ToolNamespace)]
    [Hooks]
    public class PromptGitInfo : ICommandsDeclaringType
    {
        #region attributes 

        public const string GitGetRemoteStatusCmd = "fetch --no-tags --no-auto-gc -q";
        public const string GitGetStatusCmd = "status -s -b -u -M --porcelain";
        public const string GitGetRemoteBranchStatusCmd = "branch -v -r";
        public const string GitGetLocalBranchStatusCmd = "branch -v";
        public const string GitFolder = ".git";
        public const string GitCmd = "git";

        public const string ToolNamespace = "git";
        public const string ToolVarSettingsName = "promptInfo";
        public const string VarIsEnabled = "isEnabled";
        public const string VarInfoBackgroundColor = "infoBackgroundColor";
        public const string VarBehindBackgroundColor = "behindBackgroundColor";
        public const string VarAheadBackgroundColor = "aheadBackgroundColor";
        public const string VarUpToDateBackgroundColor = "upToDateBackgroundColor";
        public const string VarModifiedBackgroundColor = "modifiedBackgroundColor";
        public const string VarModifiedUntrackedBackgroundColor = "modifiedUntrackedBackgroundColor";
        public const string VarUnknownBackgroundColor = "unknownBackgroundColor";
        public const string VarModifiedTextTemplate = "modifiedTextTemplate";
        public const string VarBehindTextTemplate = "behindTextTemplate";
        public const string VarAheadBehindTextTemplate = "aheadBehindTextTemplate";
        public const string VarAheadTextTemplate = "aheadTextTemplate";
        public const string VarTextTemplateNoData = "noDataTextTemplate";
        public const string VarTextTemplateNoRepository = "templateNoRepository";
        public const string VarIsEnabledGetRemoteStatus = "isEnabledGetRemoteStatus";
        public const string VarRunInBackgroundTask = "runInBackgroundTask";

        string _namespace => Variables.Nsp(ShellEnvironmentNamespace.com + "", ToolNamespace, ToolVarSettingsName);

        string Text;

        #endregion

        #region init

        /// <summary>
        /// init module hook
        /// </summary>
        [Hook(Hooks.ModuleInit)]
        public void Init(CommandEvaluationContext context)
        {
            // init settings
            var branchSymbol = Unicode.EdgeRowLeft;
            //var sepSymbol = Unicode.RightChevron;

            context.ShellEnv.AddNew(_namespace, VarIsEnabled, true, false);
            context.ShellEnv.AddNew(_namespace, VarIsEnabledGetRemoteStatus, true, false);
            context.ShellEnv.AddNew(_namespace, VarRunInBackgroundTask, false, false);

            var behindColor = "(b=darkred)";
            var aheadColor = ANSI.SGR_SetBackgroundColor8bits(136);
            var infoColor = ANSI.SGR_SetBackgroundColor8bits(237/*59*/);
            context.ShellEnv.AddNew(_namespace, VarInfoBackgroundColor, infoColor);

            context.ShellEnv.AddNew(
                _namespace,
                VarModifiedTextTemplate,
                $"%bgColor%(f=white) %repoName% {branchSymbol} %branch% %sepSymbol%%errorMessage%%infoColor%+%indexAdded% ~%indexChanges% -%indexDeleted% | ~%worktreeChanges% -%worktreeDeleted% ?%untracked%(rdc) ", false);

            context.ShellEnv.AddNew(
                _namespace,
                VarBehindTextTemplate,
                $"%bgColor%(f=white) %repoName% {branchSymbol} %branch% %sepSymbol%%errorMessage%%infoColor%+%indexAdded% ~%indexChanges% -%indexDeleted% | ~%worktreeChanges% -%worktreeDeleted% ?%untracked% %behindColor%{Unicode.ArrowDown}%behind%%behindMessage%(rdc) ", false);
            
            context.ShellEnv.AddNew(
                _namespace,
                VarAheadBehindTextTemplate,
                $"%bgColor%(f=white) %repoName% {branchSymbol} %branch% %sepSymbol%%errorMessage%%infoColor%+%indexAdded% ~%indexChanges% -%indexDeleted% | ~%worktreeChanges% -%worktreeDeleted% ?%untracked% %aheadColor%{Unicode.ArrowUp}%ahead%%behindColor%{Unicode.ArrowDown}%behind%%behindMessage%(rdc) ", false);

            context.ShellEnv.AddNew(
                _namespace,
                VarAheadTextTemplate,
                $"%bgColor%(f=white) %repoName% {branchSymbol} %branch% %sepSymbol%%errorMessage%%infoColor%+%indexAdded% ~%indexChanges% -%indexDeleted% | ~%worktreeChanges% -%worktreeDeleted% ?%untracked% %aheadColor%{Unicode.ArrowUp}%ahead%(rdc) ", false);

            context.ShellEnv.AddNew(
                _namespace,
                VarTextTemplateNoData,
                $"%bgColor%(f=white) %repoName% {branchSymbol} %branch% %errorMessage%(rdc) ", false);
            
            context.ShellEnv.AddNew(
                _namespace,
                VarTextTemplateNoRepository,
                $"%unknownBackgroundColor%(f=white) {branchSymbol} %errorMessage%(rdc) ", false);
            
            context.ShellEnv.AddNew(_namespace, VarBehindBackgroundColor, behindColor, false);
            context.ShellEnv.AddNew(_namespace, VarAheadBackgroundColor, aheadColor, false);
            context.ShellEnv.AddNew(_namespace, VarUpToDateBackgroundColor, ANSI.SGR_SetBackgroundColor8bits(22), false);
            context.ShellEnv.AddNew(_namespace, VarModifiedBackgroundColor, ANSI.SGR_SetBackgroundColor8bits(130), false);
            context.ShellEnv.AddNew(_namespace, VarModifiedUntrackedBackgroundColor, ANSI.SGR_SetBackgroundColor8bits(166), false);
            context.ShellEnv.AddNew(_namespace, VarUnknownBackgroundColor, "(b=darkblue)", false);
        }

        #endregion

        #region Command

        /// <summary>
        /// enable or disable prompt git info
        /// </summary>
        [Command("setup prompt git infos")]
        public CommandVoidResult PromptInfo(
            CommandEvaluationContext context,
            [Option("e", "enable", "if true enable the prompt customization, otherwise disable it", true, true)] bool isEnabled = true
        )
        {
            context.Variables.SetValue(Variables.Nsp(VariableNamespace.env + "", _namespace), VarIsEnabled, isEnabled);
            return CommandVoidResult.Instance;
        }

        #endregion

        #region prompt hook

        /// <summary>
        /// prompt begin hook
        /// </summary>
        [Hook(Hooks.PromptOutputBegin)]
        public void PromptOutputBegin(CommandEvaluationContext context)
        {
            void a() => PromptOutputBeginBody(context);
            if (context.ShellEnv.IsOptionSetted(_namespace, VarRunInBackgroundTask))
                Task.Run(a);
            else
                a();
            if (Text != null) context.Out.Echo(Text, false);
        }

        public void PromptOutputBeginBody(CommandEvaluationContext context)
        { 
            if (context.ShellEnv.IsOptionSetted(_namespace, VarIsEnabled))
            {
                var repoPath = DoesRepoPathExists(Environment.CurrentDirectory);
                var branch = (repoPath == null) ? "" : GetBranch(repoPath);
                var repo = GetRepoStatus(context, repoPath, branch,out var behindMessage);
                var repoName = Path.GetFileName(Path.GetDirectoryName(repoPath));

                var tpl = (repo.RepoStatus==RepoStatus.Unknown)?VarTextTemplateNoRepository: VarTextTemplateNoData;
                if (repo.Ahead > 0 && repo.Behind > 0) tpl = VarAheadBehindTextTemplate;
                else if (repo.Ahead > 0) tpl = VarAheadTextTemplate;
                else if (repo.Behind > 0) tpl = VarBehindTextTemplate;
                else if (repo.IsModified) tpl = VarModifiedTextTemplate;

                string text =
                     context.ShellEnv.GetValue<string>(
                         _namespace,
                         tpl
                     );

                var bgColor = context.ShellEnv.GetValue<string>(_namespace, VarUnknownBackgroundColor);
                switch (repo.RepoStatus)
                {
                    case RepoStatus.AheadBehind:
                    case RepoStatus.Behind:
                        bgColor = context.ShellEnv.GetValue<string>(_namespace, VarBehindBackgroundColor);
                        break;
                    case RepoStatus.Ahead:
                        bgColor = context.ShellEnv.GetValue<string>(_namespace, VarAheadBackgroundColor);
                        break;
                    case RepoStatus.Modified:
                        bgColor = context.ShellEnv.GetValue<string>(_namespace, VarModifiedBackgroundColor);
                        break;
                    case RepoStatus.ModifiedUntracked:
                        bgColor = context.ShellEnv.GetValue<string>(_namespace, VarModifiedUntrackedBackgroundColor);
                        break;
                    case RepoStatus.UpToDate:
                        bgColor = context.ShellEnv.GetValue<string>(_namespace, VarUpToDateBackgroundColor);
                        break;
                    case RepoStatus.Unknown:
                        bgColor = context.ShellEnv.GetValue<string>(_namespace, VarUnknownBackgroundColor);
                        break;
                }            

                var vars = new Dictionary<string, string>
                {
                    { "infoColor" , context.ShellEnv.GetValue<string>(_namespace,VarInfoBackgroundColor) },
                    { "behindColor" , context.ShellEnv.GetValue<string>(_namespace,VarBehindBackgroundColor) },
                    { "aheadColor" , context.ShellEnv.GetValue<string>(_namespace,VarAheadBackgroundColor) },
                    { "unknownBackgroundColor" , context.ShellEnv.GetValue<String>(_namespace,VarUnknownBackgroundColor) },

                    { "bgColor" , bgColor },
                    { "branch" , branch },
                    { "errorMessage" , string.IsNullOrWhiteSpace(repo.ErrorMessage)?"" : $"(f=red){repo.ErrorMessage}(rdc)" },
                    { "indexAdded" , repo.IndexAdded+"" },
                    { "indexChanges" , repo.IndexChanges+"" },
                    { "indexDeleted" , repo.IndexDeleted+"" },
                    { "worktreeChanges" , repo.WorktreeChanges+"" },
                    { "worktreeAdded" , repo.WorktreeAdded+"" },
                    { "worktreeDeleted" , repo.WorktreeDeleted+"" },
                    { "untracked" , repo.Untracked+"" },
                    { "repoName" , repoName },
                    { "behind" , repo.Behind+"" },
                    { "ahead" , repo.Ahead+"" },
                    { "sepSymbol" , "" },
                    { "behindMessage" , behindMessage==null?"":$" ({behindMessage})" }
                };
                Text = SetVars(context, text, vars);                                
            }
        }

        #endregion

        #region utils

        static string DoesRepoPathExists(string path)
        {
            while (true)
            {
                string repPath;
                if (Directory.Exists(repPath = Path.Combine(path, GitFolder)))
                    return repPath;
                var lastPath = path;
                path = Path.Combine(path, "..");
                var ppath = new DirectoryInfo(path);
                if (!ppath.Exists) break;
                path = ppath.FullName;
                if (path == lastPath) break;
            }
            return null;
        }

        RepoInfo GetRepoStatus(
            CommandEvaluationContext context,
            string repoPath,
            string branch,
            out string behindMessage
            )
        {
            behindMessage = null;
            if (string.IsNullOrWhiteSpace(repoPath)) return new RepoInfo { RepoStatus = RepoStatus.Unknown };

            var r = new RepoInfo { RepoStatus = RepoStatus.UpToDate };

            if (context.ShellEnv.IsOptionSetted(_namespace, VarIsEnabledGetRemoteStatus))
                try
                {
                    // fetch remote status
                    context.CommandLineProcessor.ShellExec(context, GitCmd, GitGetRemoteStatusCmd, out var r0, true, false);
                    if (r0 != null)
                    {
                        // local branches list
                        context.CommandLineProcessor.ShellExec(context, GitCmd, GitGetLocalBranchStatusCmd, out var r1, true, false);
                        if (r1 != null)
                        {
                            var lines = r1.Split(Environment.NewLine);
                            if (lines.Any())
                            {
                                var localBranchLine = lines.Where(x => x.StartsWith("*")).FirstOrDefault();
                                if (localBranchLine != null && 
                                    (
                                        localBranchLine.Contains("[behind")
                                        || localBranchLine.Contains(", behind")
                                    ))
                                {
                                    // get remote branch info
                                    context.CommandLineProcessor.ShellExec(context, GitCmd, GitGetRemoteBranchStatusCmd, out var r2, true, false);
                                    if (r2 != null)
                                    {
                                        var lines2 = r2.Split(Environment.NewLine);
                                        if (lines2.Any())
                                        {
                                            var remoteBranchLine = lines2.Where(x => x.Contains($"/{branch} ")).FirstOrDefault();
                                            if (remoteBranchLine != null)
                                            {
                                                var lc = lines2.Select(x => x.Trim().Split(" ")[0].Length).Max();
                                                behindMessage = remoteBranchLine[(lc +2 +7 +2)..];
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch { };

            try
            {
                context.CommandLineProcessor.ShellExec(context, "git", GitGetStatusCmd, out var output, true, false);
                
                if (output != null)
                {
                    var lines = output.Split(Environment.NewLine);
                    foreach (var line in lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line) && line.Length > 2)
                        {
                            var x = line[0];
                            var y = line[1];
                            r.Inc(x, y,line);
                        }
                    }
                    r.Update();
                }
            }
            catch (Exception ex)
            {
                return new RepoInfo { RepoStatus = RepoStatus.Unknown, ErrorMessage = ex.Message };
            }
            return r;
        }

        static string SetVars(CommandEvaluationContext context, string text, Dictionary<string, string> vars)
        {
            foreach (var kv in vars)
                text = text.Replace($"%{kv.Key}%", kv.Value);
            return text;
        }

        static string GetBranch(string repoPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(repoPath)) return null;
                var lines = File.ReadAllLines(Path.Combine(repoPath, "HEAD"));
                var txt = lines.Where(x => !string.IsNullOrWhiteSpace(x)).FirstOrDefault();
                if (txt == null) return "";
                var t = txt.Split("/");
                return t.Last();
            }
            catch (Exception ex)
            {
                return $"(f=red){ex.Message}(rdc)";
            }
        }

        #endregion
    }
}
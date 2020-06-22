using System.Collections.Generic;
using Wox.Plugin;
using System.Diagnostics;
using System.Linq;
using System;
using System.IO;
using Newtonsoft.Json;

namespace app
{
    public struct Settings {
        public string shell { get; set; }
        public string ghqCommand { get; set; }
        public string openCommand { get; set; }
    }

    public class Main : IPlugin
    {
        private Settings settings;
        void IPlugin.Init(PluginInitContext context)
        {
            var settingsFilePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "wox-plugin-ghq.json");
            if (File.Exists(settingsFilePath)) {
                this.settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsFilePath));
            } else {
                this.settings = new Settings() {
                    shell = "wsl.exe",
                    ghqCommand = "ghq",
                    openCommand = "explorer.exe",
                };
                File.WriteAllText(settingsFilePath, JsonConvert.SerializeObject(this.settings));
            }
        }

        List<Result> IPlugin.Query(Query query)
        {
            var output = runGhq($"list -p {query.Search}");

            var results = chompLines(output).Select(line => new Result() {
                Title = line,
                IcoPath = "logo.png",
                Action = e =>
                {
                    runCommand($"{this.settings.openCommand} {line}");
                    return true;
                }
            });

            results = results.Concat(
                new [] {
                    new Result() {
                        Title = $"Get {query.FirstSearch}",
                        IcoPath = "logo.png",
                        Action = e =>
                        {
                            runGhq($"get {query.FirstSearch}");
                            var dir = chompLines(runGhq($"list -p {query.FirstSearch}")).First();
                            runCommand($"{this.settings.openCommand} {dir}");
                            return true;
                        }
                    },
                    new Result() {
                        Title = $"Create {query.FirstSearch}",
                        IcoPath = "logo.png",
                        Action = e =>
                        {
                            var result = runGhq($"create {query.FirstSearch}");
                            var dir = chompLines(result).Last();
                            runCommand($"{this.settings.openCommand} {dir}");
                            return true;
                        }
                    }
                }
            );

            return results.ToList();
        }

        private string runGhq(string command) {
            return runCommand(this.settings.ghqCommand + " " + command);
        }

        private string runCommand(string command = "") {
            var cmd = new Process();
            cmd.StartInfo.FileName = this.settings.shell;
            cmd.StartInfo.Arguments = command;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();
            cmd.WaitForExit();

            return cmd.StandardOutput.ReadToEnd();
        }

        private string[] chompLines(string lines) {
            return lines.Split(new string [] {"\n"}, StringSplitOptions.RemoveEmptyEntries).Select(line => line.TrimEnd('\r', '\n')).ToArray();
        }
    }
}

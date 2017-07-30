using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;


namespace DependencyUpdater
{
    class Program
    {
        static int Main(string[] args)
        {
            try {
                if (args.Length == 0) {
                    Console.WriteLine("Error: please specify .nuspec file");
                    Console.WriteLine("Usage: DependencyUpdater.exe example.nuspec [packages.config]");
                    return 1;
                }

                var nuspec = args[0];
                if (!File.Exists(nuspec)) {
                    Console.WriteLine($"Error: '{nuspec}' doesn't exist");
                    return 1;
                }

                string conf = null;
                if (args.Length > 1) {
                    conf = args[1];
                    if (!File.Exists(conf)) {
                        Console.WriteLine($"Error: '{conf}' doesn't exist");
                        return 1;
                    }
                } else if (File.Exists("packages.config")) {
                    conf = "packages.config";
                } else if (File.Exists("../../packages.config")) {
                    conf = "../../packages.config";
                } else {
                    Console.WriteLine($"Error: can't find packages.config");
                    Console.WriteLine($"Please copy it to current dir or specify explicitly");
                    return 1;
                }

                ReplaceDependencies(nuspec, conf);

                Console.WriteLine($"Updated dependencies in {nuspec}");
                return 0;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex}");
                return 1;
            }
        }

        static void ReplaceDependencies(string nuspec, string conf)
        {
            var packages = XDocument.Load(conf).Descendants("package");
            var file = XDocument.Load(nuspec);

            foreach (var dep in file.Descendants("dependency")) {
                var package = packages.Where(p => p.Attribute("id").Value == dep.Attribute("id").Value).FirstOrDefault();
                if (package != null) {
                    dep.Attribute("version").Value = package.Attribute("version").Value;
                }
            }

            file.Save(nuspec);
        }
    }
}
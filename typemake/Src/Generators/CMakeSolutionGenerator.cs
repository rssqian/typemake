﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace TypeMake
{
    public class CMakeSolutionGenerator
    {
        private String SolutionName;
        private List<ProjectReference> ProjectReferences;
        private String OutputDirectory;

        public CMakeSolutionGenerator(String SolutionName, List<ProjectReference> ProjectReferences, String OutputDirectory)
        {
            this.SolutionName = SolutionName;
            this.ProjectReferences = ProjectReferences;
            this.OutputDirectory = OutputDirectory;
        }

        public void Generate(bool EnableRebuild)
        {
            var CMakeListsPath = Path.Combine(OutputDirectory, "CMakeLists.txt");

            var Lines = GenerateLines(CMakeListsPath).ToList();
            TextFile.WriteToFile(CMakeListsPath, String.Join("\n", Lines), new UTF8Encoding(false), !EnableRebuild);
        }

        private IEnumerable<String> GenerateLines(String CMakeListsPath)
        {
            yield return @"cmake_minimum_required(VERSION 3.0.2)";
            yield return $@"project({SolutionName})";
            foreach (var p in ProjectReferences)
            {
                yield return @"add_subdirectory(" + FileNameHandling.GetRelativePath(p.FilePath, OutputDirectory).Replace('\\', '/') + ")";
            }
            yield return "";
        }
    }
}

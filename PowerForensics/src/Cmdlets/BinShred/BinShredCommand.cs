using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Collections.Specialized;

namespace PowerForensics.Cmdlets
{
    [Cmdlet("Invoke", "BinShred")]
    [Alias("binshred")]
    public class BinShredCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Path { get; set;  }

        [Parameter(Mandatory = true, Position = 1)]
        public string TemplatePath { get; set; }

        protected override void BeginProcessing()
        {
            ProviderInfo provider = null;
            Collection<string> templatePaths = this.SessionState.Path.GetResolvedProviderPathFromPSPath(TemplatePath, out provider);
            if (
                (!String.Equals("FileSystem", provider.Name, StringComparison.OrdinalIgnoreCase)) ||
                (templatePaths.Count != 1)
                )
            {
                ThrowTerminatingError(
                    new ErrorRecord(
                        new ArgumentException(
                            String.Format("Could not load template {0}. The path must represent a single FileSystem path.", TemplatePath),
                            "TemplatePath"), "TemplateMustBeFileSystemPath", ErrorCategory.InvalidArgument, TemplatePath));
            }

            Collection<string> filePaths = this.SessionState.Path.GetResolvedProviderPathFromPSPath(Path, out provider);
            if (!String.Equals("FileSystem", provider.Name, StringComparison.OrdinalIgnoreCase))
            {
                ThrowTerminatingError(
                    new ErrorRecord(
                        new ArgumentException(
                            String.Format("Could not load file {0}. The path must represent a FileSystem path.", Path),
                            "Path"), "PathMustBeFileSystemPath", ErrorCategory.InvalidArgument, Path));
            }

            string templateContent = File.ReadAllText(templatePaths[0]);

            foreach (string currentPath in filePaths)
            {
                byte[] fileContent = File.ReadAllBytes(currentPath);

                try
                {
                    OrderedDictionary results = BinShred.Shred(fileContent, templateContent);
                    WriteObject(results);
                }
                catch (ParseException e)
                {
                    WriteError(new ErrorRecord(e, "ParseError", ErrorCategory.ParserError, currentPath));
                }
            }
        }
    }
}
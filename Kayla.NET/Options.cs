using CommandLine;

namespace Kayla.NET
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Set the files or folders to be converted.")]
        public string Input { get; set; }

        [Option('o', "output", Required = true, HelpText = "Set the path to save the converted files.")]
        public string Output { get; set; }

        [Option('f', "format", Required = false, HelpText = "Set the output format. The default value is SubRip. You can use following formats:\r\nMicroDVD (*.sub)\r\nSAMI (*.smi)\r\nSubStationAlpha (*.ass, *.ssa)\r\nSubViewer (*.sub)\r\nTimedText (*.xml, *.ttml)\r\nWebVTT (*.vtt)\r\nYtXml (*.xml)\r\nSubRip (*.srt)")]
        public string Format { get; set; }

        [Option('b', "batch", Required = false,
            HelpText = "Set if you want to convert all supported files in a folder.")]
        public bool BatchProcess { get; set; }
    }
}
using CommandLine;

namespace Kayla.NET
{
    public class Options
    {
        [Option('f', "format", Required = true,
            HelpText =
                "Set the output format. The default value is SubRip. You can use following formats:\r\nMicroDVD (*.sub)\r\nSAMI (*.smi)\r\nSubStationAlpha (*.ass)\r\nSubViewer (*.sub)\r\nTimedText (*.xml)\r\nWebVTT (*.vtt)\r\nSubRip (*.srt)")]
        public string Format { get; set; }

        [Option('i', "input", Required = true,
            HelpText = "Set the files or folders to be converted. It will detect a format automatically.")]
        public string Input { get; set; }

        [Option('o', "output", Required = true, HelpText = "Set the path to save the converted files.")]
        public string Output { get; set; }

        [Option('s', "sync", Required = false,
            HelpText = "Set seconds to adjust subtitle sync. e.g: 3 or -3")]
        public int AdjustSync { get; set; }

        [Option('b', "batch", Required = false,
            HelpText = "Set if you want to convert all supported files in a folder.")]
        public bool BatchProcess { get; set; }
    }
}
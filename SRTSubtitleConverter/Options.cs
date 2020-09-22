using CommandLine;

namespace SRTSubtitleConverter
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Set the files or folders to be converted.")]
        public string Input { get; set; }

        [Option('o', "output", Required = true, HelpText = "Set the path to save the converted files.")]
        public string Output { get; set; }

        [Option('b', "batch", Required = false,
            HelpText = "Set if you want to convert all supported files in a folder.")]
        public bool BatchProcess { get; set; }
    }
}
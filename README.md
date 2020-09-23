# Kayla.NET
<p>
    <a href="https://github.com/Cryental/SRTSubtitleConverter/blob/master/LICENSE" alt="License">
        <img src="https://img.shields.io/github/license/Cryental/SRTSubtitleConverter" /></a>
</p>

This tool can quickly convert subtitles to SubRip (SRT) or any supported format.

It removes all custom styles and comments from the original file so you will get a clean subtitle.

## Features

### Supported Subtitle File Format
- SAMI (Synchronized Accessible Media Interchange)
- SubStation Alpha (or ASS)
- MicroDVD
- SubViewer 2.0
- WebVTT (Web Video Text Tracks)
- Youtube Specific XML Format
- Timed Text Markup Language

### Automatic Encoding Converter 
You don't need to worry about encoding problem anymore such as EUC-KR.

This software will detect your subtitle encoding type and convert into UTF-8 automatically.

### Batch Conversion
You can convert all supported formats with one command line.

## Downloads

### Windows
Download `Kayla.NET-Win64.exe` from Releases and run.

### Linux
Download `Kayla.NET-Linux` from Releases and run following commands:
```
$ chmod 777 Kayla.NET-Linux
$ chmod +x Kayla.NET-Linux
$ ./Kayla.NET-Linux
```
Tested with Ubuntu 20.04.

### macOS
Download `Kayla.NET-macOS` from Releases and run following commands:
```
$ ./Kayla.NET-macOS
```
Tested with macOS 10.15.

## How To Use
```
Syntax:
  -i, --input     Required. Set the files or folders to be converted.

  -o, --output    Required. Set the path to save the converted files.

  -f, --format    Set the output format. The default value is SubRip. You can use following formats:
                  MicroDVD (*.sub)
                  SAMI (*.smi)
                  SubStationAlpha (*.ass)
                  SubViewer (*.sub)
                  TimedText (*.xml)
                  WebVTT (*.vtt)
                  SubRip (*.srt)

  -s, --sync      Set seconds to adjust subtitle sync. e.g: 3 or -3
  
  -b, --batch     Set if you want to convert all supported files in a folder.
  
  Batch Convert: $ Kayla.NET --batch --format [Format] --input "/path/to/folder/sub.smi" --output "/path/to/folder/output" 
  Single File Convert: $ Kayla.NET --format [Format] --input "/path/to/folder/sub.smi" --output "/path/to/folder/output"
Example:
  $ Kayla.NET --format SubRip --input "/path/to/folder/sub.smi" --output "/path/to/folder/output"
```

## Requirements
This software will work with Windows, Linux and macOS without any additional software.

## Disclaimer
This software is licensed by GPL-3.0 License.

Some code snippets are from https://github.com/AlexPoint/SubtitlesParser.

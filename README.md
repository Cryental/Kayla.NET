# SRTSubtitleConverter
This tool can quickly convert subtitles to SubRip (SRT) format.

It removes all custom styles and comments from the original file so you will get a clean subtitle.

## Features

### Support Subtitles File Format Conversion From:
- SAMI (Synchronized Accessible Media Interchange)
- SubStation Alpha (or ASS)
- MicroDVD
- SubViewer 2.0
- WebVTT (Web Video Text Tracks)
- Youtube Specific XML Format
- TTML

### Bundled Automatic Encoding Converter 
You don't worry about encoding problem. This software will detect your subtitle encoding type and convert into UTF-8.

### Batch Conversion
You can convert all supported formats with one command line.

## Downloads

### Windows:
Download `SRTSubtitleConverter-Win64.exe`.

### Linux:
Download `SRTSubtitleConverter-Linux` from Releases and run following commands:
```
$ chmod 777 SRTSubtitleConverter-Linux
$ chmod +x SRTSubtitleConverter-Linux
$ ./SRTSubtitleConverter-Linux
```
Tested with Ubuntu 20.04.

### macOS:
Download `SRTSubtitleConverter-macOS` from Releases and run following commands:
```
$ ./SRTSubtitleConverter-macOS
```
Tested with macOS 10.15.

## How To Use
```
-i, --input      Required. Set the files or folders to be converted.
-o, --output     Required. Set the path to save the converted files.
-b, --batch      Set if you want to convert all supported files in a folder.
```

## Requirements
This software will work with Windows, Linux and macOS.

## Disclaimer
This software is licensed by GPL-3.0 License.
Some code snippets are from https://github.com/AlexPoint/SubtitlesParser.

# SRTSubtitleConverter
This tool can quickly convert subtitles format from one format to SubRip (SRT) format. 

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

## Usages
```
-i, --input      Required. Set the files or folders to be converted.
-o, --output     Required. Set the path to save the converted files.
-b, --batch      Set if you want to convert all supported files in a folder.
```
### Example:
```
Single File Without File Name: SRTSubtitleConverter --input "/path/to/folder/file.smi" --output "/path/output/to/folder/"
Single File With File Name: SRTSubtitleConverter --input "/path/to/folder/file.smi" --output "/path/output/to/folder/file.srt"
Batch File Conversation: SRTSubtitleConverter --input "/path/to/folder/" --output "/path/output/to/folder/" --batch
```

## Requirements
This software will work with Windows, Linux and macOS.

## Disclaimer
This software is licensed by GPL-3.0 License.
Some code snippets are from https://github.com/AlexPoint/SubtitlesParser. Thank you for your great work.

This project is still under development. It's open for pull requests :)

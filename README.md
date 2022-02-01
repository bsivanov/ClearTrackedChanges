Clear Tracked Changes
===
This is a Visual Studio extension that installs a single command named 'Clear Tracked Changes' in the Tools menu. When invoked, the command clears the tracked changes (the yellow, green and orange bars in the left part of the Visual Studio code editor) from all files.

After installing, you can map a shortcut of your choise to the command - it should be present in Tools -> Options -> Environment -> Keyboard named as `Tools.ClearTrackedChanges`.

## Availability

The extension is available on the Visual Studio Marketplace here: [Clear Tracked Changes](https://marketplace.visualstudio.com/items?itemName=bsivanov.ClearTrackedChanges).

## Compatibility

The marketplace version currently supports Visual Studio 2022, 2019, and 2017.

The latest version supporting Visual Studio 2015 and below is [v1.1.0](https://github.com/bsivanov/ClearTrackedChanges/releases/tag/v1.1.0). It is available in [Releases](https://github.com/bsivanov/ClearTrackedChanges/releases), and could be manually installed using the .vsix file.

## Building

To build the extension yourself, you need Visual Studio 2022 with 'Visual Studio extension development' workload installed.

## History

The extenstion is creates as an exercise to [answer](https://stackoverflow.com/a/20084053/259206) this StackOverflow question: [How to reset track changes in Visual Studio?](https://stackoverflow.com/q/16768233/259206). 

It is more of a hack, as it just toggles the Visual Studio setting for tracking changes (Tools -> Options -> Text Editor -> Track Changes), relying that this would clear the tracked changes from the editor.

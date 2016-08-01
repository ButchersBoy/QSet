# Q-SET: MSMQ Development & Monitoring Tool

Several years ago this was a licensed product sold for a fee.  I no longer actively maintain it so have open sourced it.

It's a pretty comprehensive tool for working with MSMQ whether you are in development or admin phase.

The code is pretty clean and tidy but I wrote it a long time ago so make no apology for the lack of what might be considered common design patterns.

A big thank-you to https://github.com/dandrejvv for removing the licenced 3rd party libs and replacing with free versions, making this tool fully open source and entirely free to use.

This is a C#/WinForms project.

Features:

* Easily send (and repeat send) messages using a variety of formatters
* Inspect messages with hex, text, XML & custom HTML viewers
* Drag and drop messages between queues
* Search across a domain for queues
* Custom grouping of queues
* Monitoring of queue sizes

![Main Screen](./Graphics/QSetBeta1ScreenshotActual.jpg "Main Screen")
![New Message Screen](./Graphics/QSetB2NewMsg.gif "New Message Screen")
![Queue Search Screen](./Graphics/QSetSearch.gif "Queue Search Screen")



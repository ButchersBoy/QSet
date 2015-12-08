Tasks:

Phase 1
Estimated version: 0.1

TODO

1.  Formatting of XML in XmlViewer. - RETURN TO AND CHECK DOCTYPE XML
2.  Versioning policy.
3.	Handle permissions on queues.
4.	Additional floating message viewer windows.
5.	Post, and remove messages, purge message queues
6.	Administer message queues
7.	File associations - check if file association is already associated to another program.
9.	Find & Organise
10.	Exclude queues from monitor
------------------------------
BUGS

1.	Search, no results (message queuing not installed).  Press OK: unhandled exception.
2.	Bring back sender ID in column list saya System.Byte[]

----------------------------

KNOWLEDGE BASE
1.	"Unable to open or read queue.  Remote computer is not available."  1.3  Unable to view contents of a remote queue with said message
	This can occur of the full remote host name cannot be resolved.  Try "ping remotecomputer.mydomain.com"  If this dows not work an entry may be required in the DNS

-----------------------------

Message Browser Column Issues:

The following columns cause error:

AdministrationQueue
ResponseQueue	} have a play round with these two, see what comes back in message browser
SourceMachine	}

The following message browser columns need some sort of interpretation:

DestinationQueue
	System.Messaging.MessageQueue
DestinationSymmetricKey
	System.Byte[]
DigitalSignature
	System.Byte[]
Extension
	System.Byte[]
SenderCertificate
	System.Byte[]
SenderId
	System.Byte[]
TransactionStatusQueue
	System.Messageing.MessageQueue

	

---------
Add the manifest into the compiled exe with name and ID:
('Import' the resource)

RT_MANIFEST		ID 1


Build a release:

* Ensure QSet setup project is closed!
* Compile Release configuration
* Add manifest (see above)
* Run SignReleaseExe.bat
* Copy dll's and exe's from Bin\Release to D:\MulhollandSource\SetupProjects\QSet\ReleaseFiles
* Check you can run .exe in ReleaseFiles
* Ensure D:\MulhollandSource\SetupProjects\QSet\ReleaseFiles contains correct version number
* Update version number of setup project properties
* Build the QSet setup project (right click in Solution Explorer)

Product code from version 1.1: {3AAAC327-0624-479B-8E9C-F172BF7F749F} (must remain static throughout a major version)
Product code from version 1.0: {9932C463-E872-425C-99E1-58C0AEFD94B8} (broken as version was set to 0.2)
Upgrade code from version 1.0: {ED06B5D0-E227-415D-B1CA-FF3F4099CDDB} (must remain static through all versions)

-----




MSMQ Counters

MSMQ Queue Object

The MSMQ Queue performance object monitors message statistics for a selected queue. There are instances for each queue on the computer. In addition, if an Message Queuing–based application has a private queue open on another computer, an instance for that queue is also available. The computer queues instance represents the computer's source journal and dead letter queues.

Bytes in Journal - Queue Shows the total number of bytes that currently reside in the journal queue. For the computer queue instance, this represents the computer journal queue. - PERF_COUNTER_RAWCOUNT 
Bytes in Queue - Shows the total number of bytes that currently reside in the queue. For the computer queue instance, this represents the dead letter queue. - PERF_COUNTER_RAWCOUNT 
Messages in Journal Queue - Shows the total number of messages that currently reside in the journal queue. For the computer queue instance, this represents the computer journal queue. - PERF_COUNTER_RAWCOUNT 
Messages in Queue - Shows the total number of messages that currently reside in the queue. For the computer queue instance, this represents the dead letter queue. For queues residing on the local computer, this counter shows how many messages are waiting to be read. For queues residing on a different computer, this counter shows the number of messages waiting to be sent. This counter is useful in determining if message handling is stalled. - PERF_COUNTER_RAWCOUNT 

MSMQ Service Object

The MSMQ Service performance object monitors session and message statistics for a selected computer that is running Message Queuing.

Incoming Messages/sec - Shows the rate of incoming Message Queuing messages handled by the Message Queuing service. This counter is useful for determining if the local Message Queuing service is currently receiving messages. - PERF_COUNTER_COUNTER 
IP Sessions - Shows the number of open IP sessions. - PERF_COUNTER_RAWCOUNT 
IPX Sessions - Shows the number of open IPX sessions. - PERF_COUNTER_RAWCOUNT 
MSMQ Incoming Messages - Shows the total number of incoming messages handled by the Message Queuing service. - PERF_COUNTER_RAWCOUNT 
MSMQ Outgoing Messages - Shows the total number of outgoing messages handled by the Message Queuing service. - PERF_COUNTER_RAWCOUNT 
Outgoing Messages/sec - Shows the rate of outgoing Message Queuing messages handled by the Message Queuing service. This counter is useful for determining if the local Message Queuing service is currently sending messages. - PERF_COUNTER_COUNTER 
Sessions - Shows the total number of open network sessions. A session is opened whenever a message is to be sent or received and closed automatically after two to five minutes of inactivity. Windows 2000 Server closes a session after two minutes; Windows 2000 Professional or Windows 95 closes a session after five minutes. This counter indicates if messaging activity has occurred recently. - PERF_COUNTER_RAWCOUNT 
Total Bytes in All Queues - Shows the total number of bytes in all active queues under the Message Queuing service. - PERF_COUNTER_RAWCOUNT 
Total Messages in All Queues - Shows the total number of messages in all active queues under the Message Queuing service. This counter shows the total number of messages that are waiting to be received by local applications or sent to another destination. - PERF_COUNTER_RAWCOUNT 

MSMQ Session Object

The MSMQ Session performance object monitors statistics about active sessions between computers running Message Queuing. There can be an instance for each session.

Incoming Bytes - Shows the total number of bytes that were received through the selected session. PERF_COUNTER_RAWCOUNT 
Incoming Bytes/sec Shows the rate at which Message Queuing messages are entering through the selected session. PERF_COUNTER_RAWCOUNT 
Incoming Messages Shows the total number of messages that were received through the selected session. PERF_COUNTER_RAWCOUNT 
Incoming Messages/sec Shows the rate at which Message Queuing messages are entering through the selected session. PERF_COUNTER_RAWCOUNT 
Outgoing Bytes shows the total number of bytes that were sent through the selected session. PERF_COUNTER_RAWCOUNT 
Outgoing Bytes/sec Shows the rate at which Message Queuing messages are leaving through the selected session. PERF_COUNTER_RAWCOUNT 
Outgoing Messages Shows the total number of messages that were sent through the selected session. PERF_COUNTER_RAWCOUNT 
Outgoing Messages/sec Shows the rate at which Message Queuing messages are leaving through the selected session. PERF_COUNTER_RAWCOUNT 

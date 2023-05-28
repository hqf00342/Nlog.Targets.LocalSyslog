# Nlog.Targets.LocalSyslog

`Nlog.Targets.LocalSyslog` is a custom target for NLog that logs to the local syslog on Linux.  
This library makes use of the Linux syslog() system call.

This extension works in the following environment.

- Linux.
- NLog 5.0 or higher
- .net standard 2.1 (.NET5, 6, 7)

## Usage

Reference this DLL in your project, and include `nlog.config` as follows.
This example sets syslog priority to `local7.*`.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true">

	<extensions>
		<add assembly="Nlog.Targets.LocalSyslog"/>
	</extensions>

	<targets async="true">
		<target name="mysyslog" type="LocalSyslog" Facility="local7" layout="${message:withexception=true}"/>
	</targets>

	<rules>
		<logger name="*" minlevel="Info" writeTo="mysyslog" enabled="true" />
	</rules>
</nlog>
```

Then use it in your c# code:

```csharp
var logger = LogManager.GetCurrentClassLogger();
logger.Info("test");
```


## implicit value

If you omit some xml-attributes, the following default values will be used.
Even if you specify an invalid facility name, `local0` will be used.

```
Facility = "local0"
layout = "${message:withexception=true}"
```

## Supported facilities
The following names are available for the Facility attribute.

- kern
- user
- mail
- daemon
- auth
- syslog
- lpr
- news
- uucp
- cron
- authpriv
- ftp
- local0
- local1
- local2
- local3
- local4
- local5
- local6
- local7

## Correspondence between NLog and syslog
NLog's log-levels are converted to syslog levels according to the table below.

| NLog level     | syslog level |
| :-----------   | :----------- |
| LogLevel.Fatal | alert(1)     |
| LogLevel.Error | err(3)       |
| LogLevel.Warn  | warning(4)   |
| LogLevel.Info  | info(6)      |
| LogLevel.Debug | debug(7)     |
| LogLevel.Trace | debug(7)     |

## Licence

MIT
t.nagashima <hqf00342 at nifty.com> 

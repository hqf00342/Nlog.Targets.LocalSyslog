# Nlog.Targets.LocalSyslog

`Nlog.Targets.LocalSyslog` is a custom target for NLog that logs to the local syslog on Linux.  
This works only on Linux and is abailable with Nlog version 5.0 and later.

## Available environment
- Linux only.
- NLog 5.0 or higher
- .net standard 2.1 (.NET5, 6, 7)

## Assumed use

This custom target logs Nlog output to Linux local system log. 
The emmited logs can be contorolled using `rsyslog`, `journal`, or `logrotate`.

For example, use this library to set syslog priority to "local3.*".

```xml
	<target name="mysyslog" type="LocalSyslog" Facility="local3"/>
```

Then use `ryslog` to save the log to /var/log/local3.log.

**/etc/rsyslog.d/local3.conf**
```
local3.*  -/var/log/local3.log
```

Of course you can also use `logrotate` to do log-rotation.


## Usage

Reference this DLL in your project, and include `nlog.config` as follows.

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
Use local0 even if you specify an invalid facility name.
  
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
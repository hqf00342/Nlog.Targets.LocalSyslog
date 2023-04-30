/*!
Nlog.Targets.LocalSyslog
ver1.0

Copyright (c) 2023 t.nagashima <hqf00342@nifty.com>

Released under the MIT license.
see https://opensource.org/licenses/MIT
*/

using NLog;
using NLog.Targets;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Nlog.Targets.LocalSyslog;

[Target("LocalSyslog")]
public partial class LocalSyslogTarget : TargetWithLayout
{
    [DllImport("libc.so.6", CharSet = CharSet.Ansi)]
    private static extern void syslog(int priority, string s);

    //[LibraryImport("libc.so.6", StringMarshalling = StringMarshalling.Utf8)]
    //private static partial void syslog(int priority, string s);

    private readonly bool _isLinux;

    private readonly int LOCAL0 = 16;

    private readonly Dictionary<string, int> _facilities = new()
    {
        {"kern",0},    {"user",1},    {"mail",2},      {"daemon",3},
        {"auth",4},    {"syslog",5},  {"lpr",6},       {"news",7},
        {"uucp",8},    {"cron",9},    {"authpriv",10}, {"ftp",11},
        {"local0",16}, {"local1",17}, {"local2",18},   {"local3",19},
        {"local4",20}, {"local5",21}, {"local6",22},   {"local7",23},
    };

    private readonly Dictionary<LogLevel, int> _loglevels = new()
    {
        {LogLevel.Fatal, 1 }, // 1=alert
        {LogLevel.Error, 3 }, // 3=err
        {LogLevel.Warn , 4 }, // 4=warning
        {LogLevel.Info , 6 }, // 6=info
        {LogLevel.Debug, 7 }, // 7=debug
        {LogLevel.Trace, 7 }, // 7=debug
    };

    public string Facility { get; set; } = "local0";

    public LocalSyslogTarget()
    {
        _isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        this.Layout = "${message:withexception=true}";
    }

    protected override void Write(LogEventInfo logEvent)
    {
        if (_isLinux)
        {
            try
            {
                var pri = GetFacilityNumber(Facility) * 8 + _loglevels[logEvent.Level];
                var msg = RenderLogEvent(this.Layout, logEvent);
                syslog(pri, msg);
            }
            catch { }
        }
    }

    private int GetFacilityNumber(string facility) =>
        _facilities.TryGetValue(facility.ToLower(), out var num) ? num : LOCAL0;
}
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System;
using System.Web.UI;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PCCore")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("SDT")]
[assembly: AssemblyProduct("PCCore")]
[assembly: AssemblyCopyright("Copyright © SDT 2009")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("182ae9c4-756d-4aa5-837c-942f2fd7693a")]
[assembly: WebResource("PCCore.Control.PopNote.pop_ico.gif", "image/gif")]

[assembly: WebResource("PCCore.Control.PopNote.pop_note.css", "text/css")]
[assembly: WebResource("PCCore.Control.Label.Label.css", "text/css")]
[assembly: WebResource("PCCore.Control.Label.Label_zh.css", "text/css")]
[assembly: WebResource("PCCore.Control.Label.Label_tw.css", "text/css")]
[assembly: WebResource("PCCore.Control.Label.Label_jp.css", "text/css")]
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
//允许页面直接使用本工程里面的类
[assembly: TagPrefix("PCCore", "PC")]
﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.8784
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("PRRetentionExporter.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to PCMS.
        '''</summary>
        Friend ReadOnly Property ApplicationName() As String
            Get
                Return ResourceManager.GetString("ApplicationName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to compass2008.
        '''</summary>
        Friend ReadOnly Property BE_DBPassword() As String
            Get
                Return ResourceManager.GetString("BE_DBPassword", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to sa.
        '''</summary>
        Friend ReadOnly Property BE_DBUserName() As String
            Get
                Return ResourceManager.GetString("BE_DBUserName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 34.
        '''</summary>
        Friend ReadOnly Property ColorIndex_C1() As String
            Get
                Return ResourceManager.GetString("ColorIndex_C1", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 35.
        '''</summary>
        Friend ReadOnly Property ColorIndex_C2() As String
            Get
                Return ResourceManager.GetString("ColorIndex_C2", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 2.
        '''</summary>
        Friend ReadOnly Property ColorIndex_DEF() As String
            Get
                Return ResourceManager.GetString("ColorIndex_DEF", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 37.
        '''</summary>
        Friend ReadOnly Property ColorIndex_DLP() As String
            Get
                Return ResourceManager.GetString("ColorIndex_DLP", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 24.
        '''</summary>
        Friend ReadOnly Property ExportPeriod_Limit() As String
            Get
                Return ResourceManager.GetString("ExportPeriod_Limit", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 200.
        '''</summary>
        Friend ReadOnly Property ExportPrj_Limit() As String
            Get
                Return ResourceManager.GetString("ExportPrj_Limit", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to compass2009.
        '''</summary>
        Friend ReadOnly Property FE_DBPassword() As String
            Get
                Return ResourceManager.GetString("FE_DBPassword", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to sa.
        '''</summary>
        Friend ReadOnly Property FE_DBUserName() As String
            Get
                Return ResourceManager.GetString("FE_DBUserName", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to ret.history.
        '''</summary>
        Friend ReadOnly Property HistoryFile() As String
            Get
                Return ResourceManager.GetString("HistoryFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to V1.1.
        '''</summary>
        Friend ReadOnly Property HistoryVersion() As String
            Get
                Return ResourceManager.GetString("HistoryVersion", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 10.1.1.126.
        '''</summary>
        Friend ReadOnly Property PROD_BE() As String
            Get
                Return ResourceManager.GetString("PROD_BE", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to 10.1.1.125.
        '''</summary>
        Friend ReadOnly Property PROD_FE() As String
            Get
                Return ResourceManager.GetString("PROD_FE", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Require\.
        '''</summary>
        Friend ReadOnly Property RequireFolder() As String
            Get
                Return ResourceManager.GetString("RequireFolder", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Require\config.xml.
        '''</summary>
        Friend ReadOnly Property SettingFile() As String
            Get
                Return ResourceManager.GetString("SettingFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to sql.txt.
        '''</summary>
        Friend ReadOnly Property SQLFile() As String
            Get
                Return ResourceManager.GetString("SQLFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to dd-mmm-yyyy.
        '''</summary>
        Friend ReadOnly Property XLS_DateFormat() As String
            Get
                Return ResourceManager.GetString("XLS_DateFormat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to #,##0.00.
        '''</summary>
        Friend ReadOnly Property XLS_DecimalFormat() As String
            Get
                Return ResourceManager.GetString("XLS_DecimalFormat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to #,##0.
        '''</summary>
        Friend ReadOnly Property XLS_NumFormat() As String
            Get
                Return ResourceManager.GetString("XLS_NumFormat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to mm/yyyy.
        '''</summary>
        Friend ReadOnly Property XLS_PeriodFormat() As String
            Get
                Return ResourceManager.GetString("XLS_PeriodFormat", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to General.
        '''</summary>
        Friend ReadOnly Property XLS_StringFormat() As String
            Get
                Return ResourceManager.GetString("XLS_StringFormat", resourceCulture)
            End Get
        End Property
    End Module
End Namespace

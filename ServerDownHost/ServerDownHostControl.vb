﻿Imports System.IO
Imports System.ServiceModel
Imports System.Timers

Public Class ServerDownHostControl
#Region "Declarations"
#Region "Attributes"
    Private sh As ServiceHost
    Private Shared instance As ServerDownHostControl = Nothing
    Private Shared views As List(Of MainWindow) = Nothing
    Private isLogEnabled As Boolean = False
    Private isSaveEnabled As Boolean = False
    Private logText As New List(Of String)
    Private logTimer As Timer = Nothing
    Private saveTimer As Timer = Nothing
    Private defaultLogFileSavePath As String = Path.Combine(Directory.GetCurrentDirectory(), "logfile.log")
    Private logFileText As New List(Of String)
    Private statusMessage As String = ""
#End Region
#Region "Constants"
    Private Const logInterval As Double = 1000 * 1 * 10 'Critical Error after a while with low intervals; 10 seconds minimum!
    Private Const saveInterval As Double = 1000 * 1 * 30
    'Private Const stringSaveEnabled As String = "Automatic saving is enabled."
    'Private Const stringSaveDisabled As String = "Automatic saving is disabled."
    'Private Const stringLogFileSaveSuccess As String = "The logfile was saved."
    Private Const stringLogFileSaveFailure As String = "The logfile could not be saved."
    'Private Const stringLogEnabled As String = "Logging: Enabled."
    'Private Const stringLogDisabled As String = "Logging: Disabled."
    Private Const stringServiceReady As String = "Service ready."
    Private Const stringServiceStopped As String = "Service stopped."
    Private Const stringServiceStopFailure As String = "Error: Could not stop service."
    Private Const stringNewReportText As String = "New Report: "
#End Region
#Region "Properties"
    'Private ReadOnly Property LogState As String
    '    Get
    '        If isLogEnabled Then
    '            Return stringLogEnabled
    '        Else
    '            Return stringLogDisabled
    '        End If
    '    End Get
    'End Property
#End Region
#End Region
#Region "Singleton-Pattern"
    Public Shared Function GetInstance() As ServerDownHostControl
        If instance Is Nothing Then
            instance = New ServerDownHostControl()
            views = New List(Of MainWindow)()
        End If
        Return instance
    End Function
#End Region
#Region "Control"
    ''' <summary>
    ''' Adds a view to the list of controlled views.
    ''' </summary>
    ''' <param name="view"></param>
    Public Shared Sub AddView(ByRef view As MainWindow)
        GetInstance().InitializeWindow(view)
        views.Add(view)
    End Sub

    ''' <summary>
    ''' Initializes all views.
    ''' </summary>
    Public Sub InitializeWindow(ByRef view As MainWindow)
        view.ClearList()
        StartLogTimer()
        StartSaveTimer()
        UpdateView(view)
    End Sub

    ''' <summary>
    ''' Updates the ListBox's position to show the most recent entry.
    ''' </summary>
    ''' <param name="listBox"></param>
    Public Sub UpdateListBoxPosition(listBox As ListBox)
        If listBox IsNot Nothing Then
            Dim _border = TryCast(VisualTreeHelper.GetChild(listBox, 0), Border)
            Dim _scrollViewer = TryCast(VisualTreeHelper.GetChild(_border, 0), ScrollViewer)
            _scrollViewer.ScrollToBottom()
        End If
    End Sub

    ''' <summary>
    ''' Starts the hosting.
    ''' </summary>
    Public Sub InitializeHost()
        If sh Is Nothing Or sh?.State = CommunicationState.Closed Then
            sh = New ServiceHost(GetType(GetLog))
            sh.Open()
            statusMessage = stringServiceReady
            views.ForEach(Sub(view) view.StatusMessage = statusMessage)
            Console.WriteLine(stringServiceReady)
            Console.ReadLine()
        End If
    End Sub

    ''' <summary>
    ''' Stops the hosting.
    ''' </summary>
    Public Sub CloseHost()
        Try
            sh.Close()
            statusMessage = stringServiceStopped
            views.ForEach(Sub(view) view.StatusMessage = stringServiceStopped)
        Catch ex As Exception
            statusMessage = stringServiceStopFailure
            views.ForEach(Sub(view) view.StatusMessage = stringServiceStopFailure)
        End Try
    End Sub

    ''' <summary>
    ''' Returns the <c>LogText</c>.
    ''' </summary>
    ''' <returns></returns>
    Public Function GetLogText() As String()
        Return logText.ToArray()
    End Function
#End Region
#Region "Logging"
#Region "ToggleLogState"
    ''' <summary>
    ''' Turns the logging process on.
    ''' </summary>
    Public Sub SetLogOn()
        If isLogEnabled = False Then
            Try
                StartLog()
                'isLogEnabled = True
                'statusMessage = stringLogEnabled
                'views.ForEach(Sub(view) UpdateView(view))
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
                'isLogEnabled = False
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Turns the logging process off.
    ''' </summary>
    Public Sub SetLogOff()
        If isLogEnabled Then
            Try
                EndLog()
                'isLogEnabled = False
                'statusMessage = stringLogDisabled
                'views.ForEach(Sub(view) UpdateView(view))
            Catch ex As Exception
                Console.WriteLine(ex.StackTrace)
                'isLogEnabled = True
            End Try
        End If
    End Sub
#End Region
#Region "StartLogEndLog"
    ''' <summary>
    ''' Starts the logging process.
    ''' </summary>
    ''' <param name="isConsole"></param>
    Public Sub StartLog(Optional ByVal isConsole As Boolean = False)
        If isConsole Then
#Region "isConsole = True"
            Throw New NotImplementedException("isConsole = True")
            'Dim reply As String
            'Console.WriteLine("Do you want to start a test? Y/N")
            'reply = Console.ReadLine()
            'If reply.ToLower().First().Equals("y") Or reply.ToLower().First().Equals("j") Then
            '    TestServers()
            'ElseIf reply.ToLower().First().Equals("n") Then
            '    Console.WriteLine("Test aborted.")
            '    Console.ReadKey()
            '    Throw New Exception("Test aborted.")
            'Else
            '    Console.WriteLine("Invalid reply.")
            '    Console.ReadKey()
            '    Throw New Exception("Invalid reply.")
            'End If
#End Region
        Else
            StartLogTimer()
        End If
    End Sub

    ''' <summary>
    ''' Stops the logging process.
    ''' </summary>
    Public Sub EndLog()
        StopLogTimer()
    End Sub
#End Region
#Region "Update"
    ''' <summary>
    ''' Tests, if the specified servers are running and updates the views.
    ''' </summary>
    Public Sub TestServers()
        logText = ServerTest.GetServerStateStrings()
        logFileText.AddRange(logText)
        views.ForEach(Sub(view) view.Dispatcher.BeginInvoke(Threading.DispatcherPriority.Background, Sub() UpdateListBox(view)))
    End Sub

    Public Sub UpdateView(ByRef view As MainWindow)
        'view.checkBoxLog.IsChecked = isLogEnabled
        'view.checkBoxSave.IsChecked = isSaveEnabled
        view.StatusMessage = statusMessage
    End Sub
#End Region
#Region "SaveLog"
    ''' <summary>
    ''' Saves the log as a file at the default location.
    ''' </summary>
    Private Sub SaveLogFile()
        Dim sw As StreamWriter = StreamWriter.Null
        Try
            sw = New StreamWriter(path:=defaultLogFileSavePath, append:=True, encoding:=Text.Encoding.UTF8)
            logFileText.ForEach(Sub(line) sw.WriteLine(line))
            logFileText.Clear()
            sw.Flush()
            'statusMessage = stringLogFileSaveSuccess
            'isSaveEnabled = True
            'views.ForEach(Sub(view) view.Dispatcher.BeginInvoke(Threading.DispatcherPriority.Background, Sub() UpdateView(view)))
        Catch ex As Exception
            statusMessage = stringLogFileSaveFailure
            'isSaveEnabled = False
            views.ForEach(Sub(view) view.Dispatcher.BeginInvoke(Threading.DispatcherPriority.Background, Sub()
                                                                                                             UpdateView(view)
                                                                                                             view.ShowErrorMessage(stringLogFileSaveFailure, ServerDownClassLibrary.EErrorLevel.Warning)
                                                                                                         End Sub))
        Finally
            sw.Close()
        End Try
    End Sub
#End Region
#End Region
#Region "Timer"
    ''' <summary>
    ''' Starts the timer, that triggers an event after a specified amount of time has passed.
    ''' </summary>
    Private Sub StartLogTimer()
        If logTimer Is Nothing Or logTimer?.Enabled = False Then
            logTimer = New Timer(logInterval)
            AddHandler logTimer.Elapsed, Sub() TestServers()
            logTimer.AutoReset = True
            'isLogEnabled = True
            'statusMessage = stringLogEnabled
            'views.ForEach(Sub(view) UpdateView(view))
            logTimer.Start()
        End If
    End Sub

    ''' <summary>
    ''' Initializes and starts the <c>saveTimer</c>.
    ''' </summary>
    Public Sub StartSaveTimer()
        If saveTimer Is Nothing Or saveTimer?.Enabled = False Then
            saveTimer = New Timer(saveInterval)
            AddHandler saveTimer.Elapsed, Sub() SaveLogFile()
            saveTimer.AutoReset = True
            'isSaveEnabled = True
            'statusMessage = stringSaveEnabled
            'views.ForEach(Sub(view) UpdateView(view))
            saveTimer.Start()
        End If
    End Sub

    ''' <summary>
    ''' Stops the <c>logTimer</c>.
    ''' </summary>
    Private Sub StopLogTimer()
        If logTimer?.Enabled Then
            logTimer.Stop()
        End If
    End Sub

    ''' <summary>
    ''' Stops the <c>saveTimer</c>.
    ''' </summary>
    Public Sub StopSaveTimer()
        If saveTimer?.Enabled Then
            'isSaveEnabled = False
            'statusMessage = stringSaveDisabled
            'views.ForEach(Sub(view) UpdateView(view))
            saveTimer.Stop()
        End If
    End Sub
#End Region
#Region "Delegates"
    ''' <summary>
    ''' Updates the view's listBox.
    ''' </summary>
    ''' <param name="view"></param>
    Private Sub UpdateListBox(view As MainWindow)
        logText.ForEach(Sub(item) view.AddLogItem(item))
        UpdateListBoxPosition(view.listBoxLog)
        view.StatusMessage = stringNewReportText & Date.Now.ToString()
    End Sub
#End Region
End Class